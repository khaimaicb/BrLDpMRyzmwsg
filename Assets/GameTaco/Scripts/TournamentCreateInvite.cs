
	using System;
	using UnityEngine;
	using UnityEngine.UI;
	using System.Collections;
	using UnityEngine.Networking;
	using UnityEngine.EventSystems;

namespace GameTaco
{
	public class TournamentCreateInvite : MonoBehaviour {

		public GameObject EmailInputGameObject;
		public InputField EmailInput;
		public GameObject EmailOkay = null;
		public GameObject Invited;
		public GameObject InviteInputField;


		private bool emailValidated = false;

		public string GetEmail(){

			return EmailInput.text; 

		}

		public void Start(){

			EmailInput.onValueChange.AddListener (delegate {ValueChangeCheck ();});

		}

		public void AddInvite( string email, int index) {

			GameObject invitedEntry = Instantiate(InviteInputField, Invited.transform) as GameObject;

			InputField invitedInputField = invitedEntry.GetComponent<InputField>();


			//invitedEntry.transform.position = new Vector3(10.0f, 10.0f, 0.0f);

			invitedInputField.enabled = false;
			invitedInputField.text = email;
			invitedInputField.textComponent.fontSize = 24;

			TacoButton tacoButton = invitedEntry.GetComponentInChildren<TacoButton>();
			tacoButton.SetCallBackDataInt(index);

		}

		public void ValueChangeCheck()
		{
			ValidateUserOrEmail(GetEmail());
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
			
		public void Awake(){


			InputField emailInputField = EmailInput.GetComponent<InputField>();

			emailInputField.ActivateInputField ();

			EventSystem.current.SetSelectedGameObject( EmailInputGameObject , null);

		}

		public void Update(){

			/*
			if ( !TacoManager.CheckModalsOpen() )
			{

				if (Input.GetKeyDown(KeyCode.Return))
				{

					//TacoModalPanel.

				} 

			}
			


			if (EmailInput.text != "") {

				bool isEmail = TacoManager.ValidateEmail (EmailInput.GetComponent<InputField> ().text);


				if (isEmail == true) {
					emailValidated = true;



				} else {
					emailValidated = false;

				};

			} else {
				emailValidated = false;

			}*/


		}

			
	}
}
