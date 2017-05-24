using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.Networking;

namespace GameTaco
{

    [Serializable]
    public class WithdrawFundsResult {
        public bool success;
        public string message;
        public string funds;
        public WithdrawFundsResult() { }
    }

    public class StripeWithdrawFundsManager : MonoBehaviour {

        public static StripeWithdrawFundsManager Instance = null;

        public GameObject NameInput = null;
        public GameObject AmountInput = null;
        public GameObject AddressLine1Input = null;
        public GameObject AddressLine2Input = null;
        public GameObject CityInput = null;
        public GameObject StateInput = null;
        public GameObject ZipInput = null;
        public GameObject WithdrawText = null;

        void Awake() {
            Instance = this;

            AmountInput = GameObject.Find("AmountInputField");
            AddressLine1Input = GameObject.Find("AddressLine1InputField");
            AddressLine2Input = GameObject.Find("AddressLine2InputField");
            CityInput = GameObject.Find("CityInputField");
            NameInput = GameObject.Find("NameInputField");
            StateInput = GameObject.Find("StateDropdown");
            ZipInput = GameObject.Find("ZipInputField");

            WithdrawText = GameObject.Find("WithdrawFundsText");

        }

        // Use this for initialization
        void Start () {
           
        }
        
	// Update is called once per frame
        void Update () {
            if (this.isActiveAndEnabled) {
                if (Input.GetKeyDown(KeyCode.Tab)) {
                    if (AmountInput.GetComponent<InputField>().isFocused) {
                        NameInput.GetComponent<InputField>().ActivateInputField();
                    } else if (NameInput.GetComponent<InputField>().isFocused) {
                        AddressLine1Input.GetComponent<InputField>().ActivateInputField();
                    } else if (AddressLine1Input.GetComponent<InputField>().isFocused) {
                        AddressLine2Input.GetComponent<InputField>().ActivateInputField();
                    } else if (AddressLine2Input.GetComponent<InputField>().isFocused) {
                        CityInput.GetComponent<InputField>().ActivateInputField();
                    } else if (CityInput.GetComponent<InputField>().isFocused) {
                        StateInput.GetComponent<Dropdown>().Show();
                    }
                //else if (StateInput.GetComponent<Dropdown>().isActiveAndEnabled) {
                //    ZipInput.GetComponent<InputField>().ActivateInputField();
                //}
                    else {
                        AmountInput.GetComponent<InputField>().ActivateInputField();
                    }
                    
                } else if (Input.GetKeyDown(KeyCode.Return)) {
                    RequestWithdraw();
                }
            }
        }

        public void RequestWithdraw() {
         
            var amount = AmountInput.GetComponent<InputField>().text;

            if (amount == string.Empty) {
                WithdrawText.GetComponent<Text>().text = TacoConfig.WithdrawFundsError00;
                return;
            }
            double amountDecimal = 0;

            if(!Double.TryParse(amount, out amountDecimal)) {
                WithdrawText.GetComponent<Text>().text = TacoConfig.WithdrawFundsError01;
                return;
            }

            if(amountDecimal > TacoManager.User.funds) {
                WithdrawText.GetComponent<Text>().text = TacoConfig.WithdrawFundsError02;
                return;
            }


            var name = NameInput.GetComponent<InputField>().text;
            if (name == string.Empty) {
                WithdrawText.GetComponent<Text>().text = TacoConfig.WithdrawFundsError03;
                return;
            }

            var address1 = AddressLine1Input.GetComponent<InputField>().text;
            if (address1 == string.Empty) {
                WithdrawText.GetComponent<Text>().text = TacoConfig.WithdrawFundsError04;
                return;
            }

            var address2 = AddressLine2Input.GetComponent<InputField>().text;

            var city = CityInput.GetComponent<InputField>().text;
            if (city == string.Empty) {
                WithdrawText.GetComponent<Text>().text = TacoConfig.WithdrawFundsError05;
                return;
            }

            var zip = ZipInput.GetComponent<InputField>().text;
            if (zip == string.Empty) {
                WithdrawText.GetComponent<Text>().text = TacoConfig.WithdrawFundsError06;
                return;
            }

        //TODO: Check if the state allows real money
            var stateDropdown = StateInput.GetComponent<Dropdown>();
            var state = stateDropdown.options[stateDropdown.value].text;

        //StartCoroutine(withdrawRequest(amount, name, address1, address2, city, zip, state));


            Action<string> success = (string data) => {
                TacoConfig.print("Withdrawa funds complete - " + data);
                var r = JsonUtility.FromJson<WithdrawFundsResult>(data);
                if (r.success) {
                    if (r.funds != null && r.funds != string.Empty) {
                        TacoConfig.print("funds = " + r.funds);

                        var funds = double.Parse(r.funds);

                        TacoManager.UpdateFundsOnly(funds);
                        WithdrawText.GetComponent<Text>().text = TacoConfig.WithdrawFundsSuccessMessage;
                    }
                } else {
                   WithdrawText.GetComponent<Text>().text = TacoConfig.Error +": " + r.message;
               }
           };

           Action<string, string> fail = (string data, string error) => {
            string msg = data + ((string.IsNullOrEmpty(error)) ? "" : " : " + error);


            WithdrawText.GetComponent<Text>().text = TacoConfig.Error +": " + msg;
        };

        StartCoroutine(ApiManager.Instance.WithdrawFunds(amount, name, address1, address2, city, zip, state, success, fail));

    }


    public IEnumerator withdrawRequest(string amount, string name, string address1, string address2, string city, string zip, string state) {
        WWWForm form = new WWWForm();
        form.AddField("amount", amount);
        form.AddField("name", name);
        form.AddField("address1", address1);
        form.AddField("address2", address2);
        form.AddField("city", city);
        form.AddField("state", state);
        form.AddField("zip", zip);


        UnityWebRequest www = UnityWebRequest.Post(Constants.BaseUrl + "api/funds/withdraw", form);
        www.SetRequestHeader("x-access-token", TacoManager.User.token);

        yield return www.Send();

        if (www.isError) {
            WithdrawText.GetComponent<Text>().text = TacoConfig.Error;
        } else {
            TacoConfig.print("Withdrawa funds complete - " + www.downloadHandler.text);
            var r = JsonUtility.FromJson<WithdrawFundsResult>(www.downloadHandler.text);
            if (r.success) {
                if (r.funds != null && r.funds != string.Empty) {
                    TacoConfig.print("funds = " + r.funds);

                    var funds = double.Parse(r.funds);

                    TacoManager.UpdateFundsOnly(funds);
                    WithdrawText.GetComponent<Text>().text = TacoConfig.WithdrawFundsSuccessMessage;
                }
            } else {
               WithdrawText.GetComponent<Text>().text = TacoConfig.Error +": " + r.message;
           }
       }

   }
}

}
