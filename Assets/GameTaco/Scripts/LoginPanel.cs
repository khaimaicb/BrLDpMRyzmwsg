
	using System;
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections;
	using UnityEngine.Networking;
	using UnityEngine.EventSystems;

// for testing 
// email: o1
// password: password1!

namespace GameTaco
{
	public class LoginPanel : MonoBehaviour {
		public static LoginPanel Instance;

		public GameObject EmailInput = null;
		public GameObject PasswordInput = null;
    //public GameObject TacoStatusText = null;

		public GameObject AutoLoginToggle = null;

		void Awake() {
			Instance = this;
			Init ();
			EmailInput.GetComponent<InputField> ().ActivateInputField ();
		}

	// Use this for initialization
		public void Init () {
			EventSystem.current.SetSelectedGameObject(EmailInput, null);

		//TacoStatusText.GetComponent<Text> ().text = TacoConfig.TacoLoginMessage;

			PasswordInput.SetActive (true);
			EmailInput.SetActive (true);

			PasswordInput.GetComponent<InputField>().placeholder.GetComponent<Text>().text = TacoConfig.TacoLoginPassword;
			EmailInput.GetComponent<InputField>().placeholder.GetComponent<Text>().text = TacoConfig.TacoLoginEmail;

			PasswordInput.GetComponent<InputField> ().text = "";
			EmailInput.GetComponent<InputField>().text = "";
		}
		
	// Update is called once per frame
		void Update() {
			if (this.isActiveAndEnabled & !TacoManager.CheckModalsOpen() ) {
				if (Input.GetKeyDown(KeyCode.Tab)) {
					if (EmailInput.GetComponent<InputField>().isFocused) {
						PasswordInput.GetComponent<InputField>().ActivateInputField();
					} 

					if (PasswordInput.GetComponent<InputField>().isFocused) {
						EmailInput.GetComponent<InputField>().ActivateInputField();
					} else if ( !EmailInput.GetComponent<InputField>().isFocused && !PasswordInput.GetComponent<InputField> ().isFocused) {
						EmailInput.GetComponent<InputField>().ActivateInputField();
					}
				} else if (Input.GetKeyDown(KeyCode.Return)) {
					Login();
				}
			}
		}

		public void Login() {
			var email = EmailInput.GetComponent<InputField>().text;
			var password = PasswordInput.GetComponent<InputField>().text;

			TacoConfig.print("Login : email =  " + email);

		// toggle returns a bool
			bool autoLoginBool = AutoLoginToggle.GetComponent<Toggle>().isOn;

		// API wants a string for the form post
			string autoLoginString = autoLoginBool.ToString();

		// Unity doesn't allow Bool as a preference type, using int
			int autoLoginInt = 0;

			if (autoLoginBool == true) {
				autoLoginInt = 1;
			} 

			TacoManager.OpenMessage ( TacoConfig.TacoLoginStatusMessage00);

        //TODO: Verify valid email syntax
			if (email == string.Empty || password == string.Empty) {
				TacoManager.CloseMessage();
				TacoManager.OpenModal( TacoConfig.TacoLoginErrorHeader, TacoConfig.TacoLoginErrorMessage00);
			} else {
				Action<string> success = (string data) => {
					LoginResult r = JsonUtility.FromJson<LoginResult>(data);
					if (r.success) {
						TacoManager.SetPreference(UserPreferences.autoLogin , autoLoginInt);
						TacoManager.SetPreferenceString(UserPreferences.userToken, r.token);

						TacoManager.OpenMessage(TacoConfig.TacoLoginStatusMessage01);
						TacoManager.CreateUser(r);
						if(r.msg == "in"){
                            TacoManager.OpenModal("Login success","Welcome back to game taco! ");
                        }else{
                            TacoManager.OpenModal("Login success","You get "+r.value+" taco token for login today! ");
                        }

					// clean up the login panel
						Init();

					} else {
						TacoManager.CloseMessage();
						TacoManager.OpenModal(TacoConfig.TacoLoginErrorHeader , TacoConfig.TacoLoginErrorMessage01);
					}
				};

				Action<string, string> fail = (string data, string error) => {
					TacoConfig.print("Error on Login : " + data);

					if (!string.IsNullOrEmpty(error)) {
						TacoConfig.print("Error : " + error);
					}

					TacoManager.CloseMessage();
					
					string msg = data + (string.IsNullOrEmpty(error) ? "" : " : "  + error);
					TacoConfig.print("login msg : " + msg);

					TacoManager.OpenModal(TacoConfig.TacoLoginErrorHeader, TacoConfig.TacoLoginErrorMessage01);
				};

				TacoConfig.print("Login calling API");
				//SecureApiManager.Instance.Login(email, password, success, fail);
                StartCoroutine(ApiManager.Instance.Login(email, password, success, fail));
                //StartCoroutine(postLogin(email, password));
			}
		}

		public void LoginWithFacebook() {
			TacoManager.OpenMessage ( TacoConfig.TacoLoginStatusMessage00);

			Action<string> success = (string data) => {
				LoginResult r = JsonUtility.FromJson<LoginResult>(data);
				if (r.success) {
					//TacoManager.SetPreference(UserPreferences.autoLogin , autoLoginInt);
					TacoManager.SetPreferenceString(UserPreferences.userToken, r.token);

					TacoManager.OpenMessage(TacoConfig.TacoLoginStatusMessage01);
					TacoManager.CreateUser(r);
					if(r.msg == "in"){
						TacoManager.OpenModal("Login success","Welcome back to game taco! ");
					}else{
						TacoManager.OpenModal("Login success","You get "+r.value+" taco token for login today! ");
					}

				// clean up the login panel
					Init();

				} else {
					TacoManager.CloseMessage();
					TacoManager.OpenModal(TacoConfig.TacoLoginErrorHeader , TacoConfig.TacoLoginErrorMessage01);
				}
			};

			Action<string, string> fail = (string data, string error) => {
				TacoConfig.print("Error on Login : " + data);

				if (!string.IsNullOrEmpty(error)) {
					TacoConfig.print("Error : " + error);
				}

				TacoManager.CloseMessage();
				
				string msg = data + (string.IsNullOrEmpty(error) ? "" : " : "  + error);
				TacoConfig.print("login msg : " + msg);

				TacoManager.OpenModal(TacoConfig.TacoLoginErrorHeader, TacoConfig.TacoLoginErrorMessage01);
			};

			TacoConfig.print("Login calling API");
			//SecureApiManager.Instance.Login(email, password, success, fail);
			StartCoroutine(ApiManager.Instance.LoginWithFacebook( success, fail));
			//StartCoroutine(postLogin(email, password));
			
		}

    //private IEnumerator postLogin(string email, string password) {
    //    WWWForm form = new WWWForm();

    //    form.AddField("email", email);
    //    form.AddField("password", password);
    //    form.AddField("siteId", Constants.SiteId);

    //    UnityWebRequest www = UnityWebRequest.Post(Constants.BaseUrl + "api/login", form);
    //    yield return www.Send();

    //    if (www.isError) {

    //        TacoConfig.print("Error logging in: " + www.error);
    //        LoginStatusText.GetComponent<Text>().text = "An unexpected error occurred";
    //    } else {

    //        LoginResult r = JsonUtility.FromJson<LoginResult>(www.downloadHandler.text);
    //        TacoConfig.print("Result = " + r.success + " - " + r.started.Count + " - " + r.ended.Count + " - " + r.userId + " - " + r.funds);

    //        if (r.success) {
    //            SkillsManager.Instance.Setup(r);
    //            LoginStatusText.GetComponent<Text>().text = "Login Complete";

    //        } else {
    //            LoginStatusText.GetComponent<Text>().text = "Email/Password incorrect";
    //        }
    //    }
    //}
	}
}
