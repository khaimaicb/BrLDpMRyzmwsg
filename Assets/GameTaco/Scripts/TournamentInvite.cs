
	using System;
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections;
	using UnityEngine.Networking;
	using UnityEngine.EventSystems;

namespace GameTaco
{
	public class TournamentInvite : MonoBehaviour {

		public InputField EmailInput;
		public GameObject EmailOkay = null;

		//private bool emailValidated = false;

		public string GetEmail(){

			return EmailInput.text; 

		}

		public void Start(){

			EmailInput.onValueChange.AddListener (delegate {ValueChangeCheck ();});

		}

		public void ValueChangeCheck()
		{
			//if (emailValidated) {
				
				ValidateUserOrEmail (GetEmail());
			//}
		}

		public void ValidateUserOrEmail( string emailToCheck) {
			
				EmailOkay.SetActive(false);
				TacoModalPanel.Instance.SetModalButtonEnabled (false);

				Action<string> success = (string data) => {
				
					GameFeaturedResult r = JsonUtility.FromJson<GameFeaturedResult> (data);

					if (r.success) {
					
					if( data.Contains("true")  ){
						
						EmailOkay.SetActive(true);
						TacoModalPanel.Instance.SetModalButtonEnabled (true);

					};
						
					};
				};

				Action<string, string> fail = (string errorData, string error) => {
					TacoConfig.print ("Error on get : " + errorData);
					if (!string.IsNullOrEmpty (error)) {
						TacoConfig.print ("Error : " + error);
					}

					TacoManager.CloseMessage();
					string msg = errorData + (string.IsNullOrEmpty (error) ? "" : " : " + error);
					TacoManager.OpenModal (TacoConfig.TacoLoginErrorHeader, TacoConfig.TacoLoginErrorMessage01);
				};


			  string url = "api/user/verify?u=" + emailToCheck;
				StartCoroutine (ApiManager.Instance.GetWithToken (url, success, fail));
				
		}

		public void awake(){

			EmailInput.GetComponent<InputField> ().ActivateInputField ();

		}

		public void Update(){

			/*
			if (EmailInput.text != "") {

				bool isEmail = TacoManager.ValidateEmail (EmailInput.GetComponent<InputField> ().text);


				if (isEmail == true) {
					emailValidated = true;

				} else {
					emailValidated = false;

				};

			} else {
				emailValidated = false;

			}
			*/

		}

			
	}
}
