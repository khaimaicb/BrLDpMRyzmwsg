using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Linq;
using UnityEngine.Networking;

namespace GameTaco
{

    [Serializable]
    public class AddFundsResult {
        public bool success;
        public string message;
        public string funds;
        public AddFundsResult() { }
    }

	[Serializable]
	public class StripeTokenResultError {
		public string message;
		public string type;
		public string param;
		public string code;
	}

    [Serializable]
    public class StripeTokenResult {
        public string id;
		public StripeTokenResultError error;

        public StripeTokenResult() { }
    }

    public class StripeAddFundsManager : MonoBehaviour {

    //private string tokenUrl = "https://api.stripe.com/v1/tokens";
    //private string chargeUrl = "https://api.stripe.com/v1/charges";
    //NOTE: This is a test token

        public static StripeAddFundsManager Instance = null;

        public GameObject CardNumberInput = null;
        public GameObject ExpMonthInput = null;
        public GameObject ExpYearInput = null;
        public GameObject CVCInput = null;

        public GameObject Toggle5 = null;
        public GameObject Toggle10 = null;
        public GameObject Toggle25 = null;
        public GameObject Toggle50 = null;
        public GameObject Toggle100 = null;

        public GameObject AddStatusText = null;
        public GameObject AddGTokensText = null;
        public GameObject AddFundsButton = null;
        public GameObject AddGTokensButton = null;
        public GameObject FundsGroup = null;
        private bool isAdding = false;

        void Awake() {
            Instance = this;
        }

	// Use this for initialization
        void Start () {

            CardNumberInput = GameObject.Find("CardNumberInputField");
            AddStatusText = GameObject.Find("AddFundsText");
            AddGTokensText = GameObject.Find("AddGTokensText");
            ExpMonthInput = GameObject.Find("ExpMonthInputField");
            ExpYearInput = GameObject.Find("ExpYearInputField");
            CVCInput = GameObject.Find("CVCInputField");

            Toggle5 = GameObject.Find("Toggle5");
        //Toggle5.tag = "5";
            Toggle10 = GameObject.Find("Toggle10");
        //Toggle10.tag = "10";
            Toggle25 = GameObject.Find("Toggle25");
        //Toggle25.tag = "25";
            Toggle50 = GameObject.Find("Toggle50");
        //Toggle50.tag = "50";
            Toggle100 = GameObject.Find("Toggle100");
        //Toggle100.tag = "100";

            FundsGroup = GameObject.Find("TacoFundsToggleGroup");
            AddGTokensButton = GameObject.Find("AddGTokensButton");
            AddFundsButton = GameObject.FindGameObjectWithTag("AddFundsPanelButton");
        }
        public void Destroy() {
            CardNumberInput.GetComponent<InputField>().text = string.Empty;
            ExpMonthInput.GetComponent<InputField>().text = string.Empty;
            ExpYearInput.GetComponent<InputField>().text = string.Empty;
            CVCInput.GetComponent<InputField>().text = string.Empty;
            AddStatusText.GetComponent<Text>().text = string.Empty;
            Toggle5.GetComponent<Toggle>().isOn = true;
        }
        
	// Update is called once per frame
        void Update () {
            if (this.isActiveAndEnabled) {
                if (Input.GetKeyDown(KeyCode.Tab)) {
                    
                    if (CardNumberInput.GetComponent<InputField>().isFocused) {
                     ExpMonthInput.GetComponent<InputField>().ActivateInputField();
                 } else if (ExpMonthInput.GetComponent<InputField>().isFocused) {
                    ExpYearInput.GetComponent<InputField>().ActivateInputField();
                } else if (ExpYearInput.GetComponent<InputField>().isFocused) {
                    CVCInput.GetComponent<InputField>().ActivateInputField();
                } else {
                    CardNumberInput.GetComponent<InputField>().ActivateInputField();
                }
                
            } else if (Input.GetKeyDown(KeyCode.Return)) {
                
                AddFunds();
            }
        }
    }

    public void AddFunds() {
        var num = CardNumberInput.GetComponent<InputField>().text;
        if(num == string.Empty) {
            AddStatusText.GetComponent<Text>().text = TacoConfig.AddFundsError00;
            return;
        }

        var month = ExpMonthInput.GetComponent<InputField>().text;
        if (month == string.Empty) {
            AddStatusText.GetComponent<Text>().text = TacoConfig.AddFundsError01;
            return;
        }

        var year = ExpYearInput.GetComponent<InputField>().text;
        if (year == string.Empty) {
            AddStatusText.GetComponent<Text>().text = TacoConfig.AddFundsError02;
            return;
        }

        var cvc = CVCInput.GetComponent<InputField>().text;
        if (cvc == string.Empty) {
            AddStatusText.GetComponent<Text>().text = TacoConfig.AddFundsError03;
            return;
        }

        var group = FundsGroup.GetComponent<ToggleGroup>();
        var toggle = group.ActiveToggles().FirstOrDefault();
        if(toggle == null) {
            AddStatusText.GetComponent<Text>().text = TacoConfig.AddFundsError04;
            return;
        }
        int amount = 0;

        switch (toggle.name) {
            case "Toggle5":
            amount = 5 * 100;
            break;
            case "Toggle10":
            amount = 10 * 100;
            break;
            case "Toggle25":
            amount = 25 * 100;
            break;
            case "Toggle50":
            amount = 50 * 100;
            break;
            case "Toggle100":
            amount = 100 * 100;
            break;
            default:
            break;
        }
        AddFundsButton.GetComponent<Button>().interactable = false;
        AddStatusText.GetComponent<Text>().text = TacoConfig.AddFundsAdding;
        isAdding = true;

        //StartCoroutine(purchaseFunds(num, Int32.Parse(month), Int32.Parse(year), Int32.Parse(cvc), amount));
        //StartCoroutine(GetStripeToken(num, month, year, cvc, amount));


        Action<string> success = (string data) => {
            TacoConfig.print("Purchase funds complete - " + data);
            var r = JsonUtility.FromJson<AddFundsResult>(data);
            if (r.success) {
                if (r.funds != null && r.funds != string.Empty) {
                  
                    TacoConfig.print("funds = " + r.funds);

                    var funds = double.Parse(r.funds);

					// funds added : return to Tournaments view

					TacoManager.UpdateFundsOnly(funds);
					string returnMessage = TacoConfig.AddFundsSuccessMessage + TacoManager.FormatMoney(  funds);
                    AddStatusText.GetComponent<Text>().text = "";
                    Destroy();
					// show a dialog to indicate success : then return to tournaments
                    TacoManager.OpenModal( TacoConfig.AddFundsTitleSuccessMessage ,returnMessage, null, ModalFunctions.ReturnToTournaments,null,null, false );

                }
            } else {
               AddStatusText.GetComponent<Text>().text = TacoConfig.Error +" : " + r.message;
           }
           AddFundsButton.GetComponent<Button>().interactable = true;
       };

       Action<string, string> fail = (string data, string error) => {
        var msg = data + (string.IsNullOrEmpty(error) ? "" : " : " + error);
        TacoConfig.print("Error adding funds : " + msg);
        AddStatusText.GetComponent<Text>().text = TacoConfig.Error +" : " + msg;
        AddFundsButton.GetComponent<Button>().interactable = true;
    };

    StartCoroutine(ApiManager.Instance.AddFunds(num, month, year, cvc, amount, success, fail));
}

    public IEnumerator GetStripeToken(string number, string month, string year, string cvc, int amount) { //, Action<string> onSuccess, Action<string, string> onFail = null) {

        WWWForm form = new WWWForm();

        form.AddField("card[number]", number);
        form.AddField("card[exp_month]", month);
        form.AddField("card[exp_year]", year);
        form.AddField("card[cvc]", cvc);

        UnityWebRequest www = UnityWebRequest.Post(Constants.StripeTokenUrl, form);
        www.SetRequestHeader("Authorization", "Bearer " + Constants.StripePublicKey);

        yield return www.Send();

        if (www.isError) {
            //if (onFail != null) { onFail(www.downloadHandler.text, www.error); }
            TacoConfig.print("GetStripeToken Failed - " + www.downloadHandler.text);
        } else {
            TacoConfig.print("GetStripeToken success - " + www.downloadHandler.text);
            var r = JsonUtility.FromJson<StripeTokenResult>(www.downloadHandler.text);
            TacoConfig.print("Token = " + r.id);

            StartCoroutine(purchaseFundsWithToken(r.id, amount));
            //onSuccess(www.downloadHandler.text);
        }
    }

    private IEnumerator purchaseFundsWithToken(string token, int amount) {
        WWWForm form = new WWWForm();
        form.AddField("token", token);
        form.AddField("amount", amount);

        UnityWebRequest www = UnityWebRequest.Post(Constants.BaseUrl + "api/funds/stripe", form);
        www.SetRequestHeader("x-access-token", TacoManager.User.token);
        //www.SetRequestHeader("Authorization", "Bearer " + stripeToken );
        yield return www.Send();

        if (www.isError || www.responseCode == 500) {
            TacoConfig.print("URL = " + www.url);
            TacoConfig.print("Error adding funds: " + www.error);
            TacoConfig.print(www.downloadHandler.text);
            AddStatusText.GetComponent<Text>().text = TacoConfig.Error +" : " + www.downloadHandler.text;
        } else {
            TacoConfig.print("Purchase funds complete - " + www.downloadHandler.text);
            var r = JsonUtility.FromJson<AddFundsResult>(www.downloadHandler.text);
            if (r.success) {
                if (r.funds != null && r.funds != string.Empty) {
                    TacoConfig.print("funds = " + r.funds);

                    var funds = double.Parse(r.funds);

                    TacoManager.UpdateFundsOnly(funds);
                    AddStatusText.GetComponent<Text>().text = TacoConfig.AddFundsSuccessMessage + " " + funds.ToString("C") + "";
                }
            } else {
               AddStatusText.GetComponent<Text>().text = TacoConfig.Error +" : " +  r.message;
           }
            //AddStatusText.GetComponent<Text>().text = "An amount must be selected.";
       }

   }

   private IEnumerator purchaseFunds(string number, int month, int year, int cvc, int amount) {
    WWWForm form = new WWWForm();
    form.AddField("number", number);
    form.AddField("month", month);
    form.AddField("year", year);
    form.AddField("cvc", cvc);
    form.AddField("amount", amount);


    UnityWebRequest www = UnityWebRequest.Post(Constants.BaseUrl + "api/funds", form);
    www.SetRequestHeader("x-access-token", TacoManager.User.token);
        //www.SetRequestHeader("Authorization", "Bearer " + stripeToken );
    yield return www.Send();

    if(www.isError) {
        AddStatusText.GetComponent<Text>().text = TacoConfig.Error;
    } else {
        TacoConfig.print("Purchase funds complete - " + www.downloadHandler.text);
        var r = JsonUtility.FromJson<AddFundsResult>(www.downloadHandler.text);
        if(r.success) {
            if (r.funds != null && r.funds != string.Empty) {
                TacoConfig.print("funds = " + r.funds);

                var funds = double.Parse(r.funds);

                TacoManager.UpdateFundsOnly(funds);
                AddStatusText.GetComponent<Text>().text = TacoConfig.AddFundsSuccessMessage + " " + funds.ToString("C") + "";
            }
        } else {
           AddStatusText.GetComponent<Text>().text = TacoConfig.Error +" : " + r.message;
       }
            //AddStatusText.GetComponent<Text>().text = "An amount must be selected.";
   }

}

}

}
