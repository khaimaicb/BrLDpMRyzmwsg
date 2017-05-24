using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;


namespace GameTaco
{

	public class RegisterPanel : MonoBehaviour {

		public static RegisterPanel Instance;

		public Sprite Okay = null;
		public Sprite NotOkay = null;
		public Image RegisterButtonImage = null;
		
		public GameObject UserInput = null;
		public GameObject UserOkay = null;

		public GameObject EmailInput = null;
		public GameObject EmailOkay = null;
		public GameObject PasswordInput = null;
		public GameObject ConfirmInput = null;
		public GameObject PasswordOkay = null;
		public GameObject ConfirmOkay = null;
		public GameObject AgeToggle = null;
		public GameObject StatusText = null;

		void Awake() {
			Instance = this;
			AgeToggle = GameObject.Find("RegisterAgeToggle");

			UserInput.GetComponent<InputField> ().ActivateInputField ();

			Init ();
		}

    // Use this for initialization
		public void Init () {
			
			TacoConfig.print ("Taco : reset register");

			PasswordInput.SetActive (true);
			ConfirmInput.SetActive (true);
			EmailInput.SetActive (true);
			UserInput.SetActive (true);

			PasswordInput.GetComponent<InputField>().placeholder.GetComponent<Text>().text = TacoConfig.TacoLoginPassword;
			EmailInput.GetComponent<InputField>().placeholder.GetComponent<Text>().text = TacoConfig.TacoLoginEmail;
			ConfirmInput.GetComponent<InputField>().placeholder.GetComponent<Text>().text = TacoConfig.TacoLoginConfirm;
			UserInput.GetComponent<InputField>().placeholder.GetComponent<Text>().text = TacoConfig.TacoLoginUser;

			PasswordInput.GetComponent<InputField> ().text = "";
			EmailInput.GetComponent<InputField>().text = "";
			ConfirmInput.GetComponent<InputField>().text = "";
			UserInput.GetComponent<InputField>().text = "";

			PasswordOkay.SetActive (false);
			ConfirmOkay.SetActive (false);
			EmailOkay.SetActive (false);
			UserOkay.SetActive (false);
		}

	

	// Update is called once per frame
		void Update () {

			if (this.isActiveAndEnabled & !TacoManager.CheckModalsOpen ()) {

			// validate requirements
				RegisterButtonImage.sprite = NotOkay;
				bool emailValidated = false;
				bool userValidated = false;
				bool passwordValidated = false;



				if (PasswordInput.GetComponent<InputField> ().text == "" || ConfirmInput.GetComponent<InputField> ().text == "") {

					
					StatusText.GetComponent<Text> ().text = TacoConfig.TacoRegisteredErrorMessage04;

				}


				

				if (UserInput.GetComponent<InputField> ().text == "") {
					StatusText.GetComponent<Text> ().text = TacoConfig.TacoRegisteredErrorMessage03;

				}


				if (UserInput.GetComponent<InputField> ().text != "") {

					userValidated = true;
					UserOkay.SetActive (true);

				} else {
					
					userValidated = false;
					StatusText.GetComponent<Text> ().text = TacoConfig.TacoRegisteredErrorMessage03;
					UserOkay.SetActive (false);
				}

				if (string.IsNullOrEmpty(EmailInput.GetComponent<InputField> ().text)) {
					emailValidated = false;
					EmailOkay.SetActive (false);
					StatusText.GetComponent<Text> ().text = TacoConfig.TacoRegisteredErrorMessage06;
				}
				else {
					var textEmail = EmailInput.GetComponent<InputField> ().text;
					bool isEmail = TacoManager.ValidateEmail (textEmail);
					StatusText.GetComponent<Text> ().text = TacoConfig.TacoRegisteredErrorMessage01;
					if (isEmail == true) {
						emailValidated = true;
						
						StatusText.GetComponent<Text> ().text = TacoConfig.TacoRegisteredErrorMessage04;
						EmailOkay.SetActive (true);

					} else {
						emailValidated = false;
						StatusText.GetComponent<Text> ().text = TacoConfig.TacoRegisteredErrorMessage01;
						EmailOkay.SetActive (false);
					}
				}


				if (PasswordInput.GetComponent<InputField> ().text != "" && ConfirmInput.GetComponent<InputField> ().text != "") {

					if (ConfirmInput.GetComponent<InputField> ().text == PasswordInput.GetComponent<InputField> ().text) {
						
						if(PasswordInput.GetComponent<InputField> ().text.Length > 3 ) {
							passwordValidated = true;
							//StatusText.GetComponent<Text> ().text = "";

							PasswordOkay.SetActive (true);
							ConfirmOkay.SetActive (true);
						} else {
							passwordValidated = false;
							StatusText.GetComponent<Text> ().text = TacoConfig.TacoRegisteredErrorMessage07;

							PasswordOkay.SetActive (false);
							ConfirmOkay.SetActive (false);
						}

					} else {
						passwordValidated = false;
						StatusText.GetComponent<Text> ().text = TacoConfig.TacoRegisteredErrorMessage00;

						PasswordOkay.SetActive (false);
						ConfirmOkay.SetActive (false);
					}

				} else {
					//StatusText.GetComponent<Text> ().text = TacoConfig.TacoRegisteredErrorMessage04;
					passwordValidated = false;
					PasswordOkay.SetActive (false);
					ConfirmOkay.SetActive (false);
				}

			// test all conditions
				if (userValidated && passwordValidated && emailValidated && AgeToggle.GetComponent<Toggle> ().isOn) {
					
					RegisterButtonImage.sprite = Okay;

					StatusText.GetComponent<Text> ().text = TacoConfig.TacoRegisteredAllOkay;

				} else if (userValidated && passwordValidated && emailValidated && !AgeToggle.GetComponent<Toggle> ().isOn) {

					StatusText.GetComponent<Text> ().text = TacoConfig.TacoRegisteredErrorMessage02;

				} else {
					RegisterButtonImage.sprite = NotOkay;

				}



			// turn off placeholders on focus
				if (this.isActiveAndEnabled & !TacoManager.CheckModalsOpen ()) {

				// tab through inputs
					if (Input.GetKeyDown (KeyCode.Tab)) {

						if (UserInput.GetComponent<InputField> ().isFocused) {


							EmailInput.GetComponent<InputField> ().ActivateInputField ();


						} else if (EmailInput.GetComponent<InputField> ().isFocused) {
							

							PasswordInput.GetComponent<InputField> ().ActivateInputField ();

						} else if (PasswordInput.GetComponent<InputField> ().isFocused) {
							


							ConfirmInput.GetComponent<InputField> ().ActivateInputField ();

						} else if (ConfirmInput.GetComponent<InputField> ().isFocused) {



							UserInput.GetComponent<InputField> ().ActivateInputField ();



						} else {

							UserInput.GetComponent<InputField> ().ActivateInputField ();

						}

					} else if (Input.GetKeyDown (KeyCode.Return)) {
						Register ();
					}
					
				}


			}
		}

		private bool validateInput() {

			var email = EmailInput.GetComponent<InputField>().text;
			var password = PasswordInput.GetComponent<InputField>().text;
			var confirm = ConfirmInput.GetComponent<InputField>().text;
			var age = AgeToggle.GetComponent<Toggle>().isOn;
			var user = UserInput.GetComponent<InputField>().text;
			bool validated = true;

			if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirm)) {
				validated = false;
				StatusText.GetComponent<Text> ().text = TacoConfig.TacoRegisteredErrorMessage04;
			}

			if (string.IsNullOrEmpty(email)) {
				validated = false;
				StatusText.GetComponent<Text> ().text = TacoConfig.TacoRegisteredErrorMessage06;
			}
			else {
				bool isEmail = TacoManager.ValidateEmail (email);

				if (!isEmail) {
					validated = false;
					StatusText.GetComponent<Text> ().text = TacoConfig.TacoRegisteredErrorMessage01;
				}
			}

			if (string.IsNullOrEmpty(user)) {
				validated = false;
				StatusText.GetComponent<Text> ().text = TacoConfig.TacoRegisteredErrorMessage03;
			}

			if (!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(confirm)) {

				StatusText.GetComponent<Text> ().text = TacoConfig.TacoRegisteredErrorMessage04;

				if (password == confirm) {
					
					if(password.Length <= 3) {
						validated = false;
						StatusText.GetComponent<Text> ().text = TacoConfig.TacoRegisteredErrorMessage07;
					}
				}
				else {
					validated = false;
					StatusText.GetComponent<Text> ().text = TacoConfig.TacoRegisteredErrorMessage00;
				} 

			} else {
				validated = false;
				StatusText.GetComponent<Text> ().text = TacoConfig.TacoRegisteredErrorMessage04;
			}
			if(!age) {
				validated = false;
				StatusText.GetComponent<Text> ().text = TacoConfig.TacoRegisteredErrorMessage02;
			}
			return validated;
		}	
		public void Register() {

			TacoConfig.print ("Register Pressed");

			var email = EmailInput.GetComponent<InputField>().text;
			var password = PasswordInput.GetComponent<InputField>().text;
			var age = AgeToggle.GetComponent<Toggle>().isOn;
			var user = UserInput.GetComponent<InputField>().text;

			if(validateInput()){

				int avatar = 0;

            //StartCoroutine(postRegister(email, password, age, avatar, user));

				TacoManager.OpenMessage ( TacoConfig.TacoRegisteredStatusMessage00);

				Action<string> success = (string data) => {
					TacoConfig.print(data);
					LoginResult r = JsonUtility.FromJson<LoginResult>(data);
					TacoConfig.print("Result = " + r.success + " - " + r.started.Count + " - " + r.ended.Count + " - " + r.userId + " - " + r.funds);

					if (r.success) {
						TacoManager.CloseMessage();
                    // email sent, have to wait for them to verify.
						TacoManager.OpenModal(TacoConfig.TacoRegisteredModalTitle, r.message, null, "TacoRegisterResult");

                    // clean up the registerpanel
						Init();
					} else {

                    // an error returned
						TacoManager.CloseMessage();
						TacoManager.OpenModal(TacoConfig.TacoRegisteredErrorHeader, r.message);
					}
				};

				Action<string, string> fail = (string data, string error) => {
					TacoConfig.print("Error on register : " + data);
					if (!string.IsNullOrEmpty(error)) {
						TacoConfig.print("Error : " + error);
					}
					TacoManager.CloseMessage();
					string msg = data + (string.IsNullOrEmpty(error) ? "" : " : " + error);
					if(!string.IsNullOrEmpty(data)) {
						ErrorResult errorResult = JsonUtility.FromJson<ErrorResult>(data);
						if(!errorResult.success) {
							msg = errorResult.message;
						}
					}
					
					TacoManager.OpenModal(TacoConfig.TacoLoginErrorHeader, msg);
				};

				StartCoroutine(ApiManager.Instance.Register(user, email, password, age, success, fail));

			}
		}

	//private IEnumerator postRegister(string email, string password, bool ageCheck, int avatar, string user) {
 //       WWWForm form = new WWWForm();

	//	TacoConfig.print ("postRegister");

 //      // var split = email.Split('@');
 //      // var name = split.Length > 1 ? split[0] : "";

 //       form.AddField("userName", user);
 //       form.AddField("email", email);
 //       form.AddField("password", password);
 //       form.AddField("confirmPassword", password);
 //       form.AddField("ageCheck", ageCheck.ToString().ToLower());
 //       form.AddField("siteId", Constants.SiteId);
	//	form.AddField("avatar", avatar);

 //       UnityWebRequest www = UnityWebRequest.Post(Constants.BaseUrl + "api/register", form);
 //       yield return www.Send();

 //       if (www.isError) {
		
	//		TacoConfig.print("Error logging in: " + www.error);

	//		TacoManager.Instance.ClosePanel ( PanelNames.Message );
	//		TacoManager.Instance.OpenModal ( TacoConfig.TacoRegisteredErrorHeader , TacoConfig.TacoRegisteredErrorMessage05  );


 //       } else {
 //           TacoConfig.print(www.downloadHandler.text);
 //           LoginResult r = JsonUtility.FromJson<LoginResult>(www.downloadHandler.text);
 //           TacoConfig.print("Result = " + r.success + " - " + r.started.Count + " - " + r.ended.Count + " - " + r.userId + " - " + r.funds);

 //           if (r.success) {
		

	//			TacoManager.Instance.ClosePanel ( PanelNames.Message );
	//			// email sent, have to wait for them to verify.
	//			TacoManager.Instance.OpenModal (TacoConfig.TacoRegisteredModalTitle, TacoConfig.TacoRegisteredModalBody, null, "TacoRegisterResult" );

	//			// clean up the registerpanel
	//			Reset();


 //           } else {

	//			// an error returned
	//			TacoManager.Instance.ClosePanel ( PanelNames.Message );
	//			TacoManager.Instance.OpenModal (TacoConfig.TacoRegisteredErrorHeader, r.message);



 //           }
 //       }
 //   }

	}

}
