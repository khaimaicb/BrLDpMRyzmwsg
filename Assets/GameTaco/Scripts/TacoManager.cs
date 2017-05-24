using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography;

namespace GameTaco {

	public class TacoManager : MonoBehaviour {
		// Temporary solution for debugging canvases with ease
		// We shouldn't have to disable all canvases that are blocking the canvas we want to work on
		// then have to reenable them all again before playing
		[Header("Canvases")]
		public GameObject tacoBlockingCanvas;
		public GameObject tacoCommonCanvas;
		public GameObject tacoAuthCanvas;
		public GameObject tacoTournamentsCanvas;
		public GameObject tacoFundsCanvas;
		public GameObject tacoOurGamesCanvas;
		public GameObject tacoDebugCanvas;

		// canvas
		public static GameObject TacoBlockingCanvas = null;
		public static GameObject TacoCommonCanvas = null;
		public static GameObject TacoAuthCanvas = null;
		public static GameObject TacoTournamentsCanvas = null;
		public static GameObject TacoFundsCanvas = null;
		public static GameObject TacoOurGamesCanvas = null;
		public static TacoDebugConsole tacoDebugConsole = null;
		public static GameObject TacoPrizes = null;

		// panels
		public static GameObject MyTournamentsPanel = null;
		public static GameObject JoinPublicPanel = null;
		public static GameObject CreatePublicPanel = null;

		// panels common
		public static GameObject ModalPanel = null;
		public static GameObject MessagePanel = null;
		public static GameObject TacoFoldoutPanel = null;
		public static GameObject FeaturedGamesPanel = null;

		// panels funds
		public static GameObject AddFundsPanel = null;

		// Panels Prizes
		public static GameObject PrizesPanel = null;
		public static GameObject AddGTokensPanel = null;
		public static GameObject WithdrawFundsPanel = null;

		// panels auth
		public static GameObject LoginPanel = null;
		public static GameObject RegisterPanel = null;

		public static string GameName;
		public static int GameId;
		public static Image MyAvatarImage;
		public static TacoUser User;
		public static Tournament Target;
		public static Text StatusText;

		// if not null - a game is being played
		public static string GameToken = null;

		// TODO make this a list of panels in a stack, making it just the last panel now
		private static List<GameObject> panelStack  = new List<GameObject>();

		//button and text
		private static GameObject tacoOpenButton = null;
		private static GameObject tacoStageButton = null;
		private static GameObject tacoCloseButton = null;

		private static Text fundsText;
		private static Text usernameText;

		private static GameObject tacoHeader = null;

		private static int currentUserId = 0;

		void Start() {
			panelStack.Clear ();

			// TODO: MM: Temp solution
			// Find will not actually find disabled children
			if (tacoBlockingCanvas == null) {
				TacoBlockingCanvas = GameObject.Find(CanvasNames.TacoBlockingCanvas);
			} else {
				TacoBlockingCanvas = tacoBlockingCanvas;
			}
			
			if (tacoCommonCanvas == null) {
				TacoCommonCanvas = GameObject.Find(CanvasNames.TacoCommonCanvas);
			} else {
				TacoCommonCanvas = tacoCommonCanvas;
			}

			if (tacoAuthCanvas == null) {
				TacoAuthCanvas = GameObject.Find(CanvasNames.TacoAuthCanvas);
			} else {
				TacoAuthCanvas = tacoAuthCanvas;
			}

			if (tacoTournamentsCanvas == null) {
				TacoTournamentsCanvas = GameObject.Find(CanvasNames.TacoTournamentsCanvas);
			} else {
				TacoTournamentsCanvas = tacoTournamentsCanvas;
			}

			if (tacoFundsCanvas == null) {
				TacoFundsCanvas = GameObject.Find(CanvasNames.TacoFundsCanvas);
			} else {
				TacoFundsCanvas = tacoFundsCanvas;
			}

			if (tacoOurGamesCanvas == null) {
				TacoOurGamesCanvas = GameObject.Find(CanvasNames.TacoOurGamesCanvas);
			} else {
				TacoOurGamesCanvas = tacoOurGamesCanvas;
			}

			if (tacoDebugCanvas != null) {
				tacoDebugConsole = tacoDebugCanvas.GetComponent<TacoDebugConsole>();
				// Initially hide debug console canvas
				tacoDebugCanvas.SetActive(false);
			}
			
			// set canvas elements to on for finding by names to work
			TacoBlockingCanvas.SetActive(true);
			TacoCommonCanvas.SetActive(true);
			TacoAuthCanvas.SetActive(true);
			TacoTournamentsCanvas.SetActive(true);
			TacoFundsCanvas.SetActive(true);

			// panels
			MyTournamentsPanel = GameObject.Find(PanelNames.MyTournamentsPanel);
			CreatePublicPanel = GameObject.Find(PanelNames.CreatePublicPanel);
			JoinPublicPanel = GameObject.Find(PanelNames.JoinPublicPanel);
			PrizesPanel = GameObject.Find(PanelNames.PrizesPanel);

			// common
			ModalPanel = GameObject.Find(PanelNames.Modal);
			MessagePanel = GameObject.Find(PanelNames.Message);
			TacoFoldoutPanel = GameObject.Find(PanelNames.Foldout);
			FeaturedGamesPanel = GameObject.Find(PanelNames.FeaturedGamesPanel);

			AddFundsPanel = GameObject.Find(PanelNames.AddFundsPanel);
			AddGTokensPanel = GameObject.Find(PanelNames.AddGTokensPanel);
			WithdrawFundsPanel = GameObject.Find(PanelNames.WithdrawFundsPanel);

			RegisterPanel = GameObject.Find(PanelNames.RegisterPanel);
			LoginPanel = GameObject.Find(PanelNames.LoginPanel);

			// text
			fundsText = GameObject.Find("FundsText").GetComponent<Text> ();
			usernameText = GameObject.Find("UsernameText").GetComponent<Text> ();
			StatusText = GameObject.Find("StatusText").GetComponent<Text> ();

			// buttons
			tacoOpenButton = GameObject.Find("TacoOpenButton");
			tacoStageButton = GameObject.Find("TacoStageButton");
			tacoCloseButton = GameObject.Find("TacoCloseButton");
			tacoCloseButton.SetActive (false);

			// misc
			tacoHeader = GameObject.Find("TacoHeader");
			tacoHeader.SetActive (false);

			MyAvatarImage = GameObject.Find("MyAvatarImage").GetComponent<Image>();

			Init();
		}


		// Update is called once per frame
		void Update() {
			if (!CheckModalsOpen()) {
				if (Input.GetKeyDown (KeyCode.Escape)) {
					// close the last panel in the stack
					ClosePanel();
				}
			}

			if (ModalPanel.activeInHierarchy) {
				if (Input.GetKeyDown (KeyCode.Escape)) {
					DismissModal();
				}
			}

			if (TacoFoldoutPanel.activeInHierarchy) {
				if (Input.GetKeyDown (KeyCode.Escape)) {
					CloseFoldout();
				}
			}

			if (Input.GetKeyDown("`") && Debug.isDebugBuild) {
				tacoDebugCanvas.SetActive(!tacoDebugCanvas.active);
			}
		}

		public static void Init() {
			SetAllPanels(false);
			SetAllCanvas (false);

			HideAvatar();
			OpenTaco();
		}

		public static bool CheckForActiveGame() {
			// game is playing - alert the user that the game is paused
			if (GameToken != null) {
				TacoSetup.Instance.PauseMyGame();

				CloseAllModals();
				OpenModal (TacoConfig.TacoPlayActiveHeader, TacoConfig.TacoPlayActiveBody, null, ModalFunctions.ReturnToGame);

				return true;
			} else {
				return false;
			}
		}

		public static void OpenTaco() {
			bool activeGame = CheckForActiveGame();
			if ( !activeGame ) {
				
				if (TacoSetup.Instance.StartTacoOnLaunch == true) {
					// Open Game Taco By Default
					// TO DO : this could happen after a splash screen
					Setup();
				}

			}
		}
		// called when game wants to open taco for tournament play
		public static void OpenTacoFromGame() {

			bool activeGame = CheckForActiveGame();

			if ( !activeGame ) {
				
					Setup();

			}
		}

		public static void Setup(){

			if (UserLoggedIn()) {
				ShowPanel (PanelNames.MyTournamentsPanel);
			} else {
				ShowPanel (PanelNames.LoginPanel);
			}

		}

		public static bool UserLoggedIn() {
			if (currentUserId == 0) {
				return false;
			} else {
				return true;
			}
		}

		public static void HideAvatar() {
			tacoOpenButton.SetActive (false);
			tacoStageButton.SetActive (false);
		}

		public static void ShowAvatar() {
			tacoOpenButton.SetActive (false);
			tacoStageButton.SetActive (false);

			if (!UserLoggedIn()) {
				tacoStageButton.SetActive (true);
			} else {
				tacoOpenButton.SetActive (true);
			}
		}
		public static void UpdateAvatar( string username , double funds, string gToken ) {
			usernameText.text = username;

			// TODO get avatar from user
			int avatar = UnityEngine.Random.Range (0, 7);
			MyAvatarImage.sprite = TacoConfig.Instance.GetAvatarSprite (avatar);

			UpdateFundsWithTokenAndTicket(funds, gToken, User.ticket);
		}

		public static void UpdateAvatarOnline( string username , double funds , string avatarBase) {
			usernameText.text = username;

			int avatar = UnityEngine.Random.Range (0, 7);
			MyAvatarImage.sprite = TacoConfig.Instance.GetAvatarSprite (avatar);

			if(!string.IsNullOrEmpty(avatarBase)) {
				if(avatarBase.StartsWith("http")) {
					TacoSetup.Instance.StartCoroutine(ApiManager.Instance.WWWAvatarSocial(avatarBase, MyAvatarImage));
				}
				else {
					avatarBase = avatarBase.Split(';')[1];
					avatarBase = avatarBase.Split(',')[1];
					byte[] decodedBytes = Convert.FromBase64String (avatarBase);
					
					Texture2D avatarImage = MyAvatarImage.sprite.texture;
					avatarImage.LoadImage(decodedBytes);
					Sprite sprite = Sprite.Create (avatarImage, new Rect(0,0, avatarImage.width, avatarImage.height), new Vector2(.5f,.5f));
					MyAvatarImage.sprite = sprite; 

					MyAvatarImage.rectTransform.sizeDelta = new Vector2(120, 125);
					MyAvatarImage.GetComponent<RectTransform>().localPosition = new Vector3(90,0,0);
				}
			}
			
			UpdateFundsOnly (funds);
		}

		public static void CloseTaco() {
			tacoCloseButton.SetActive (false);

			// clear the panel stack
			panelStack.Clear();

			SetAllCanvas (false);
			TacoSetup.Instance.UnPauseMyGame();

			tacoHeader.SetActive (false);

			// keep common open
			TacoCommonCanvas.SetActive(true);

			ShowAvatar();
		}

		public static void SetAllCanvas(bool active) {
			// Never turn off >>>> TacoCommonCanvas.SetActive(active);
			TacoBlockingCanvas.SetActive(active);
			TacoAuthCanvas.SetActive(active);
			TacoTournamentsCanvas.SetActive(active);
			TacoFundsCanvas.SetActive(active);
			TacoOurGamesCanvas.SetActive(active);

		}
		public static void UpdateFundsWithToken(double funds, string gToken) {
			User.funds = funds;
			User.gToken = gToken;
			fundsText.text = FormatMoney(funds)  + "- G: " + double.Parse(gToken).ToString(TacoConfig.GTokenFormat);
		}
		public static void UpdateTokenOnly(string gToken) {
			User.gToken = gToken;
			fundsText.text = FormatMoney(User.funds)  + "- G: " + double.Parse(gToken).ToString(TacoConfig.GTokenFormat);
		}
		public static void UpdateFundsOnly(double funds) {
			User.funds = funds;
			fundsText.text = FormatMoney(User.funds)  + "- G: " + double.Parse(User.gToken).ToString(TacoConfig.GTokenFormat);
		}

		public static void UpdateFundsWithTokenAndTicket(double funds, string gToken, int ticket) {
			User.funds = funds;
			User.gToken = gToken;
			fundsText.text = FormatMoney(funds)  + "- G: " + double.Parse(gToken).ToString(TacoConfig.GTokenFormat) + " - T: " + ticket;
		}

		public static bool ValidateEmail( string email)
		{	
			return Regex.IsMatch(  email , @"^[a-zA-Z0-9]+((\_|\-|\.|\+){0,1}[a-zA-Z0-9]+)*@[a-zA-Z0-9]+((\_|\-){0,1}[a-zA-Z0-9]+)*(\.[a-zA-Z0-9]+)+$", RegexOptions.IgnoreCase);

		}

		public static string FormatMoney(double funds) {
			//NOTE: The reason r.funds is a string is that the JsonUtility seems to have issues with converting ints to decimals
			//if (funds != null && funds != string.Empty) {

			//}
			string formattedFunds = funds.ToString (TacoConfig.MoneyFormat);  //String.Format ( TacoConfig.MoneyFormat, funds);

			return formattedFunds;
		}

		public static string FormatGTokens(double funds) {

			string formattedFunds = 'G' + funds.ToString (TacoConfig.GTokenFormat);  //String.Format ( TacoConfig.MoneyFormat, funds);

			return formattedFunds;
		}

		public static void UpdateUser(SessionResult result, string token) {
			panelStack.Clear();

			TacoUser user = new TacoUser {

				email = result.user.email,    //.email,
				userId = result.user.id,
				funds = result.user.funds,
				token = token,
				gToken = result.gToken,
				highScoreUser = result.highScoreUser,
				ticket = result.ticket,
				//avatar = result.avatar,
			};

			User = user;
			PlayerPrefs.SetInt("highscoretype", 0);
			PlayerPrefs.SetFloat ("highscoreArcade", result.highScoreUser);

			UpdateAvatar (result.user.userName, result.user.funds, result.gToken);

			GameId = result.game.id;
			GameName = result.game.name;
			currentUserId = result.user.id;

			Setup();
			CloseMessage();
		}

		//TODO make sure result returns autoLogin - don't pass it in
		public static void CreateUser(LoginResult result ) {
			panelStack.Clear();

			TacoUser user = new TacoUser {
				email = result.mail,
				userId = result.userId,
				funds = 0.0f,
				token = result.token,
				gToken = result.gToken,
				highScoreUser = result.highScoreUser,
				ticket = result.ticket,
				//avatar = result.avatar,
			};
			User = user;
			PlayerPrefs.SetInt("highscoretype", 0);
			PlayerPrefs.SetFloat ("highscoreArcade", result.highScoreUser);

			UpdateAvatar(result.userName, result.funds, result.gToken);

			GameId = result.gameId;
			GameName = result.gameName;
			currentUserId = result.userId;

			print(GameId + " - " + GameName + " - " + currentUserId);

			ShowPanel(PanelNames.MyTournamentsPanel);
			CloseMessage();
		}

		public static void AskToLogoutUser() {
			TacoManager.CloseAllModals();
			OpenModal (  TacoConfig.TacoSureLogoutModalHeader , TacoConfig.TacoSureLogoutModalBody, null, "LogoutUser", TacoConfig.CloseSprite, null);
		}

		public static void LogoutUser() {
			User = null;

			ActiveTournamentsList.Instance.Destroy();
			PrivateTournamentsList.Instance.Destroy();
			PublicTournamentsList.Instance.Destroy();
			StripeAddFundsManager.Instance.Destroy();

			GamePlay.Instance.GameStatus = GameState.MainMenu;
			PlayerPrefs.SetInt(TacoConfig.TacoHighScoresType, 0);
			PlayerPrefs.SetFloat(TacoConfig.TacoHighScoresPerhapsText, 0);
			PlayerPrefs.SetFloat("highscoreArcade", 0);
			PlayerPrefs.Save();
			Highscore.Instance.highscore = PlayerPrefs.GetFloat("highscoreArcade");
			Highscore.Instance.UpdateManually();

			GameId = 0;
			currentUserId = 0;

			// clear the user preferences that were for this user
			SetPreferenceString (UserPreferences.userToken, null);
			SetPreference (UserPreferences.autoLogin, 0);

			TacoAuthCanvas.SetActive(true);

			LoginPanel.GetComponent<LoginPanel>().Init();
			RegisterPanel.GetComponent<RegisterPanel>().Init();

			CloseTaco();
		}
		
		/// <summary>
		/// Callback sent to all game objects before the application is quit.
		/// </summary>
		void OnApplicationQuit()
		{
			if(PlayerPrefs.GetInt(UserPreferences.autoLogin) != 1) {
				PlayerPrefs.SetInt(TacoConfig.TacoHighScoresType, 0);
				PlayerPrefs.SetFloat(TacoConfig.TacoHighScoresPerhapsText, 0);
				PlayerPrefs.SetFloat("highscoreArcade", 0);
				PlayerPrefs.Save();
				Highscore.Instance.highscore = PlayerPrefs.GetFloat("highscoreArcade");
				Highscore.Instance.UpdateManually();
			}
			PlayerPrefs.SetString("gTokenSignUpGift", "");
		}

		
		public static int GetTogglActiveName( ToggleGroup toggleGroup ) {
			Toggle activeToggle = toggleGroup.ActiveToggles().FirstOrDefault();
			if (activeToggle) {
				int activeInt = Int32.Parse (activeToggle.name);
				return activeInt;
			}
			return 0;
		}

		public static void AddPanelToStack(GameObject panel) {
			panel.SetActive (true);
			if (!panelStack.Contains(panel)) {
				// add the gameobject to the stack
				panelStack.Add (panel);
			}
		}

		public static void ShowPanel(string panelName) {
			// don't hold any panel state, you can show any panel - any time
			SetAllCanvas (false);
			SetAllPanels (false);

			// opening a Taco panel : so show the close button
			tacoCloseButton.SetActive (true);

			// open the one you want and dependent stuff
			switch (panelName) {

			case PanelNames.WithdrawFundsPanel:
				SetAllPanels (false);

				TacoBlockingCanvas.SetActive (true);
				TacoFundsCanvas.SetActive (true);

				TacoTournamentsCanvas.SetActive (false);
				MyTournamentsPanel.SetActive (false);

				// adds the panel and set the panel active
				AddPanelToStack (WithdrawFundsPanel);

				tacoHeader.SetActive (false);
				ShowAvatar();
				break;

			case PanelNames.AddFundsPanel:
				SetAllPanels (false);

				TacoBlockingCanvas.SetActive (true);
				TacoFundsCanvas.SetActive (true);

				TacoTournamentsCanvas.SetActive (false);
				MyTournamentsPanel.SetActive (false);
				// adds the panel and set the panel active
				AddPanelToStack (AddFundsPanel);

				tacoHeader.SetActive (false);
				ShowAvatar();
				break;

			case PanelNames.PrizesPanel:
				SetAllPanels (false);

				TacoBlockingCanvas.SetActive (true);
				TacoPrizes.SetActive (true);

				TacoTournamentsCanvas.SetActive (false);
				MyTournamentsPanel.SetActive (false);
				// adds the panel and set the panel active
				AddPanelToStack (PrizesPanel);

				tacoHeader.SetActive (false);
				ShowAvatar();
				break;	

			case PanelNames.AddGTokensPanel:
				SetAllPanels (false);

				TacoBlockingCanvas.SetActive (true);
				TacoFundsCanvas.SetActive (true);

				TacoTournamentsCanvas.SetActive (false);
				MyTournamentsPanel.SetActive (false);
				// adds the panel and set the panel active
				AddPanelToStack (AddGTokensPanel);

				tacoHeader.SetActive (false);
				ShowAvatar();
				TournamentManager.Instance.UpdateGTokenToBuy();
				break;	

			case PanelNames.RegisterPanel:
				SetAllPanels(false);

				TacoBlockingCanvas.SetActive(true);
				TacoAuthCanvas.SetActive(true);

				// adds the panel and set the panel active
				AddPanelToStack (RegisterPanel);

				tacoHeader.SetActive(true);
				HideAvatar();
				break;

			case PanelNames.LoginPanel:
				SetAllPanels (false);

				tacoHeader.SetActive (false);
				HideAvatar();

				if (PlayerPrefs.GetInt(UserPreferences.autoLogin) == 1) {
					string userToken = PlayerPrefs.GetString(UserPreferences.userToken);
					//ApiManager.Instance.LoginByToken( userToken );
					ApiManager.Instance.GetSession(userToken);
				}

				TacoBlockingCanvas.SetActive (true);
				TacoAuthCanvas.SetActive (true);

				// adds the panel and set the panel active
				AddPanelToStack (LoginPanel);

				if (PlayerPrefs.GetInt(UserPreferences.sawIntro) == 0 ) {
					OpenModalWithImage(TacoConfig.TacoIntroHeader, TacoConfig.TacoIntroBody, TacoConfig.TacoIntroGraphic, null, ModalFunctions.SawIntro);
				} else {
					// already saw the intro panel - don't show it
					SetPreference(UserPreferences.sawIntro,1);
				}

				break;

			case PanelNames.FeaturedGamesPanel:
				SetAllPanels(false);

				TacoBlockingCanvas.SetActive(true);
				TacoOurGamesCanvas.SetActive(true);

				// adds the panel and set the panel active
				AddPanelToStack (FeaturedGamesPanel);


				FeaturedGamesPanel featuredGamesPanel = TacoOurGamesCanvas.GetComponentInChildren<FeaturedGamesPanel> ();
				featuredGamesPanel.GetOurGames();

				tacoHeader.SetActive (false);
				ShowAvatar();
				break;
			// tournaments

			case PanelNames.MyTournamentsPanel:
				SetAllPanels(false);

				TacoBlockingCanvas.SetActive (true);
				TacoTournamentsCanvas.SetActive (true);

				MyTournamentsPanel.SetActive (true);

				// adds the panel and set the panel active
				AddPanelToStack (MyTournamentsPanel);

				TournamentManager.Instance.Open();

				tacoHeader.SetActive (true);
				ShowAvatar();
				break;

			case PanelNames.JoinPublicPanel:
				SetAllPanels(false);

				TacoBlockingCanvas.SetActive(true);
				TacoTournamentsCanvas.SetActive(true);

				MyTournamentsPanel.SetActive(true);

				// adds the panel and set the panel active
				AddPanelToStack (JoinPublicPanel);


				tacoHeader.SetActive(false);
				ShowAvatar();
				break;

			case PanelNames.CreatePublicPanel:
				SetAllPanels(false);

				TacoBlockingCanvas.SetActive (true);
				TacoTournamentsCanvas.SetActive (true);

				MyTournamentsPanel.SetActive (true);

				// adds the panel and set the panel active
				AddPanelToStack (CreatePublicPanel);

				TournamentManager.Instance.UpdatePrizeText();

				tacoHeader.SetActive (false);
				ShowAvatar();
				break;
			}
		}

		public static void SetPanelActiveByName(string panelName, bool active) {
			switch (panelName) {
			case PanelNames.WithdrawFundsPanel:
				WithdrawFundsPanel.SetActive(active);
				break;

			case PanelNames.AddFundsPanel:
				AddFundsPanel.SetActive(active);
				break;

			case PanelNames.AddGTokensPanel:
				AddGTokensPanel.SetActive(active);
				break;

			case PanelNames.RegisterPanel:
				RegisterPanel.SetActive(active);
				break;

			case PanelNames.LoginPanel:
				LoginPanel.SetActive(active);
				break;

			case PanelNames.FeaturedGamesPanel:
				FeaturedGamesPanel.SetActive(active);
				break;

			case PanelNames.MyTournamentsPanel:
				MyTournamentsPanel.SetActive(active);
				break;

			case PanelNames.JoinPublicPanel:
				JoinPublicPanel.SetActive(active);
				break;

			case PanelNames.CreatePublicPanel:
				CreatePublicPanel.SetActive(active);
				break;
			}
		}

		public static void ClosePanel() {
			// close the panel, but also check for the previous one from the stack
			if (panelStack.Count > 0) {
				GameObject lastPanelInList = panelStack [panelStack.Count - 1];
				lastPanelInList.SetActive (false);

				panelStack.RemoveAt(panelStack.Count - 1);

				if (panelStack.Count > 0) {
					GameObject panelToShow = panelStack[panelStack.Count - 1];
					// get the panelname of the panel to show, and show that
					ShowPanel (panelToShow.name);
				}

			}

			// nothing in the stack, close the taco to the stage
			if (panelStack.Count == 0) {
				CloseTaco();
			}
		}

		public static void OpenMessage (String title) {
			TacoCommonCanvas.SetActive(true);

			MessagePanel.GetComponent<TacoMessagePanel>().Open (title);

			//AddPanelToStack (MessagePanel);
			MessagePanel.SetActive(true);
		}

		public static void OpenFoldout() {
			bool activeGame = CheckForActiveGame();

			// if active game, don't open this
			if (!activeGame) {
				TacoCommonCanvas.SetActive(true);

				TacoFoldoutPanel.SetActive(true);

				HideAvatar();
			}
		}

		public static void CloseFoldout() {
			TacoFoldoutPanel.SetActive(false);
			ShowAvatar();
		}

		public static void CloseMessage() {
			MessagePanel.SetActive(false);
		}

		public static void CloseModal() {
			ModalPanel.SetActive(false);
		}

		public static void DismissModal() {
			TacoModalPanel.Instance.DismissModal();
		}
		public static void OpenModalWithPreFab(String title, GameObject preFab, Sprite buttonImage = null, string modalCallback = null, Sprite optionalButtonImage = null, string optionalModalCallback = null, bool allowClose = true) {
			TacoCommonCanvas.SetActive(true);

			ModalPanel.GetComponent<TacoModalPanel>().OpenWithPreFab(title, preFab, buttonImage, modalCallback, optionalButtonImage, optionalModalCallback, allowClose);

			//AddPanelToStack (ModalPanel);
			ModalPanel.SetActive(true);
		}

		public static void OpenModalWithImage (String title, String body , Sprite imageSprite , Sprite buttonImage = null , string modalCallback = null, Sprite optionalButtonImage = null, string optionalModalCallback = null , bool allowClose = true ) {
			TacoCommonCanvas.SetActive(true);

			ModalPanel.GetComponent<TacoModalPanel>().OpenWithImage(title, body, imageSprite, buttonImage, modalCallback, optionalButtonImage, optionalModalCallback, allowClose);

			//AddPanelToStack (ModalPanel);
			ModalPanel.SetActive(true);
		}

		public static void OpenModal (String title, String body , Sprite buttonImage = null , string modalCallback = null, Sprite optionalButtonImage = null, string optionalModalCallback = null , bool allowClose = true ) {
			TacoCommonCanvas.SetActive(true);

			ModalPanel.GetComponent<TacoModalPanel>().OpenDefault (title, body, buttonImage, modalCallback, optionalButtonImage, optionalModalCallback, allowClose);

			//AddPanelToStack (ModalPanel);
			ModalPanel.SetActive(true);
		}

		public static void CloseAllModals() {
			CloseMessage();
			DismissModal();
			CloseFoldout();
		}

		public static bool CheckModalsOpen() {
			bool open = false;

			if (MessagePanel.activeInHierarchy) {
				open = true;
			}

			if (ModalPanel.activeInHierarchy) {
				open = true;
			}

			if (TacoFoldoutPanel.activeInHierarchy) {
				open = true;
			}

			return open;
		}

		private static void SetAllPanels(bool state ) {
			LoginPanel.SetActive(state);
			RegisterPanel.SetActive(state);
			ModalPanel.SetActive(state);
			MessagePanel.SetActive(state);
			TacoFoldoutPanel.SetActive(state);
			WithdrawFundsPanel.SetActive(state);
			AddFundsPanel.SetActive(state);
			AddGTokensPanel.SetActive(state);
			MyTournamentsPanel.SetActive (state);
			CreatePublicPanel.SetActive (state);
			JoinPublicPanel.SetActive (state);
			PrizesPanel.SetActive (state);
		}

		public static int GetPreference(string key ) {
			int preference = PlayerPrefs.GetInt(key);
			return preference;
		}

		public static void SetPreference(string key, int value) {
			PlayerPrefs.SetInt(key, value);
		}

		public static string GetPreferenceString(string key) {
			string preference = PlayerPrefs.GetString(key);

			return preference;
		}

		public static void SetPreferenceString (string key, string value) {
			PlayerPrefs.SetString(key, value);
		}

		public static void ShowTacoButton(bool show) {
			tacoOpenButton.SetActive (show);
		}

		public static void SetTarget(Tournament target) {
			Target = target;
			//UpdateCurrentText();
		}

		public static string EncryptStringToBytes(string toEncrypt)
		{
			RijndaelManaged aes = new RijndaelManaged();
			aes.BlockSize = 128;
			aes.KeySize = 256;
			aes.IV = Encoding.UTF8.GetBytes(Constants.AesIV256);
			aes.Key = Encoding.UTF8.GetBytes(Constants.AesKey256);
			aes.Mode = CipherMode.CBC;
			aes.Padding = PaddingMode.PKCS7;

			byte[] src = Encoding.UTF8.GetBytes(toEncrypt);

			using (ICryptoTransform encrypt = aes.CreateEncryptor())
			{
				byte[] dest = encrypt.TransformFinalBlock(src, 0, src.Length);

				return Convert.ToBase64String(dest);
			}

		}

		/*
		public void PromptToQuitGame(int score) {

			OpenModal (  TacoConfig.TacoQuitGameHeader , TacoConfig.TacoQuitGameHeader, null, "forfeitTournamentGame", TacoConfig.Instance.closeSprite, null);

		}

		public void UnityDidExit(int score) {


			//TODO - A call to let the server know the game didn't finish

		}
		*/

		
	}
}
