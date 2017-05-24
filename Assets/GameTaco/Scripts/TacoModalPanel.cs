using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

namespace GameTaco
{

	public class TacoModalPanel : MonoBehaviour {

		public static TacoModalPanel Instance;

		public Text ImageText = null;
		public Image BodyImage = null;
		public GameObject PreFab = null;

		public GameObject AttachedPreFab = null;

		public GameObject ModalImage = null;

		public Text BodyText = null;
		public Text TitleText = null;
		public Image TacoModalButton;
		public Sprite NotOkay = null;

		public Image TacoModalOptionalButton;
		public GameObject TacoModalCloseButton;
		public string Callback = null;
		public string OptionalCallback = null;

		private bool ModalOpen = false;

		private bool ModalButtonEnabled = true;
		private bool AllowClose = true;

		private Sprite CurrentCustomTacoModalButton = null;

    // Use this for initialization
		void Start () {
			Instance = this;

		}

		void Update () {

			if (this.ModalOpen ) {

				if (Input.GetKeyDown (KeyCode.Return)  ) {

					ModalClicked ();

				} 

			}
		}

		public GameObject GetPreFab (){

			if(AttachedPreFab)
			{

				return AttachedPreFab;

			}
			return null;
		}

		public void AttachPreFab ( GameObject preFab ){

			PreFab.SetActive (true);
			Destroy (AttachedPreFab);

			AttachedPreFab = Instantiate (preFab, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;

			//AttachedPreFab = GameObject.Instantiate (preFab) as GameObject;
			AttachedPreFab.transform.parent = PreFab.transform;

			AttachedPreFab.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
			AttachedPreFab.transform.localPosition = new Vector3 (0.0f, 0.0f, 0.0f);
		}

		public void OpenWithPreFab (String title,  GameObject preFab, Sprite buttonImage = null , string modalCallback = null , Sprite optionalButtonImage = null, string optionalModalCallback = null , bool AllowClose = true ) 
		{

			ModalImage.SetActive (false);	

            BodyText.text = "";
			TitleText.text = title;

			BodyImage.sprite = null;

			AttachPreFab (preFab);

			// sometimes you don't want the user to be able to close the modal - they have to tap the main 'Okay' button
			SetAllowClose( AllowClose );

			Open ( buttonImage, modalCallback,  optionalButtonImage, optionalModalCallback);

		}

		public void OpenWithImage (String title, String body , Sprite imageSprite, Sprite buttonImage = null , string modalCallback = null , Sprite optionalButtonImage = null, string optionalModalCallback = null , bool AllowClose = true ) 
		{


			BodyText.text = "";
			TitleText.text = title;

			ImageText.text = body;
			ModalImage.SetActive (true);

			BodyImage.sprite = imageSprite;

		// sometimes you don't want the user to be able to close the modal - they have to tap the main 'Okay' button
			SetAllowClose( AllowClose );

			Open ( buttonImage, modalCallback,  optionalButtonImage, optionalModalCallback);
		}

		

		public void OpenDefault (String title, String body , Sprite buttonImage = null , string modalCallback = null , Sprite optionalButtonImage = null, string optionalModalCallback = null , bool AllowClose = true ) 
		{
			ModalImage.SetActive (false);	

			BodyText.text = body;
			TitleText.text = title;
			

		// sometimes you don't want the user to be able to close the modal - they have to tap the main 'Okay' button
			SetAllowClose( AllowClose );
			
			Open ( buttonImage, modalCallback,  optionalButtonImage, optionalModalCallback);

		}
		
		public void Open( Sprite buttonImage = null , string modalCallback = null, Sprite optionalButtonImage = null, string optionalModalCallback = null){

			TacoConfig.print ("modalCallback = " + modalCallback); 

			ModalOpen = true;

			SetImage (buttonImage);
			SetCallback (modalCallback);

			SetOptionalImage (optionalButtonImage);
			SetOptionalCallback (optionalModalCallback);

			// TODO Need to kill focus on other stuff when Modal is up - trying to find a way to do that
			GUI.FocusControl("");

		}

		public void SetAllowClose (bool value ){

			AllowClose = value;
			TacoModalCloseButton.SetActive(value);
			
		}


		public void SetModalButtonEnabled (bool active ){

			ModalButtonEnabled = active;

			if (active) {

				SetImage (CurrentCustomTacoModalButton);

			} else {
				TacoModalButton.sprite = NotOkay;
			}

		}


		public void SetImage (Sprite buttonImage ){


			if (buttonImage != null) {
				CurrentCustomTacoModalButton = buttonImage;
				TacoModalButton.sprite = buttonImage;
			} else {


				TacoModalButton.sprite = TacoConfig.ModalSprite;
			}
	
		}

		public void SetOptionalImage (Sprite buttonImage ){

			if (buttonImage != null) {

				TacoModalOptionalButton.gameObject.SetActive (true);
				TacoModalOptionalButton.sprite = buttonImage;

			} else {

				TacoModalOptionalButton.gameObject.SetActive (false);
				TacoModalOptionalButton.sprite = TacoConfig.OptionalModalSprite;
			}

		}

		public void SetCallback ( string callback ){


			Callback = callback;

		}

		public void SetOptionalCallback ( string callback ){


			OptionalCallback = callback;

		}

		public void Reset () {

			ModalOpen = false;

			ModalButtonEnabled = true;

			BodyText.text = "";
			TitleText.text = "";

			Destroy (AttachedPreFab);
			PreFab.SetActive (false);

			ImageText.text = "";

			ModalImage.SetActive (false);	

			Callback = null;
			OptionalCallback = null;
			AllowClose = true;
			TacoModalCloseButton.SetActive(AllowClose);

			TacoModalButton.sprite = TacoConfig.ModalSprite;

			TacoModalOptionalButton.gameObject.SetActive (false);
			TacoModalOptionalButton.sprite = TacoConfig.OptionalModalSprite;

			TacoManager.CloseModal ();

			if (TacoManager.GameToken != null) {
				TacoSetup.Instance.PauseMyGame();
			}

		}


		public void CloseModal () {


			if (AllowClose) {

				Reset ();
			}

		}

		public void DismissModal () {


			Reset ();

		}

	//TODO put these strings in TacoModels
		public void ModalClicked (){


			if (ModalButtonEnabled) {
			
				if (Callback != null) {

					switch (Callback) {

					case ModalFunctions.ReturnToGame:

						TacoManager.CloseTaco ();

						break;
					

					case ModalFunctions.TournamentGamePosted:

				// They finished the game, after modal closed - show past tournaments
						TacoManager.ShowPanel (PanelNames.MyPrivatePanel);

						break;

					case ModalFunctions.RegisterResult:

				// when they close the modal, show login
						TacoManager.ShowPanel (PanelNames.LoginPanel);

						break;
					
					case ModalFunctions.TournamentSubmit:
					
						TournamentManager.Instance.TournamentSubmit ();

						break;

					case ModalFunctions.TournamentSubmitComplete:
					
					// close the creation panel - remove from stack
						TacoManager.ClosePanel ();

					// refresh and show the current tournaments
						TacoManager.ShowPanel (PanelNames.MyTournamentsPanel);

						break;

					case ModalFunctions.LogoutUser:


						TacoManager.LogoutUser ();


						break;

					case ModalFunctions.JoinTournament:


						TournamentManager.Instance.Join ();


						break;

					case ModalFunctions.InviteFriends:


						TournamentManager.Instance.InviteFriend ();

						break;

						case ModalFunctions.InviteFriendsFromCreate:


							TournamentManager.Instance.InviteFriendsFromCreate ();

							break;

					case ModalFunctions.JoinPublicSuccess:

					// close the join panel - remove from stack
						TacoManager.ClosePanel ();

					// refresh and show the current tournaments
						TacoManager.ShowPanel (PanelNames.MyTournamentsPanel);


						break;

					case ModalFunctions.StartPlay:
						GameManager.Instance.StartPlay (TacoManager.Target);
						break;

					case ModalFunctions.SawIntro:

						TacoManager.SetPreference (UserPreferences.sawIntro, 1);
						TacoManager.ShowPanel (PanelNames.LoginPanel);
						break;

					case ModalFunctions.ForfeitTournamentGame:


				//TacoManager.Instance.ForfeitTournamentGame();
					

						break;


					case ModalFunctions.ReturnToTournaments:


					// refresh and show the current tournaments
						TacoManager.ShowPanel (PanelNames.MyTournamentsPanel);


						break;
					case ModalFunctions.ReturnToMenu :

						TacoSetup.Instance.ReturnMenu();
						break;

					case ModalFunctions.TacoEndTournament : {
						TacoSetup.Instance.TacoEndTournament();
						break;
					}

					case ModalFunctions.TacoFreePlayGiftToken : {
						TacoSetup.Instance.TacoSigninWithTokenFree();
						break;
					}

				}

			} 

				TacoConfig.print ("DismissModals"); 

				DismissModal ();


			}

		}

		public void OptionalModalClicked (){



			if (OptionalCallback != null) {

				switch (OptionalCallback) {

					case ModalFunctions.TournamentSubmit:

					

					TournamentManager.Instance.TournamentSubmit ();
					break;

					case ModalFunctions.TournamentSubmitResult:

					TournamentManager.Instance.TournamentSubmitResult( TacoConfig.TacoTournamentCreated);
					break;


				}

			}

			

			DismissModal ();
		}

	}

}