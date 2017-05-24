using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using System.Linq;
using System.Globalization;

namespace GameTaco
{

	public class TournamentManager : MonoBehaviour {

		public static TournamentManager Instance;

		public GameObject TournamentDetailsPrefab;
		public GameObject TournamentInvitePrefab;
		public GameObject TournamentCreateInvitePrefab;

	// panels
		private GameObject myPrivatePanel = null;
		private GameObject myActivePanel = null;
		private GameObject myPublicPanel = null;
		private GameObject myLeaderboardPanel = null;

	// text
		private Text tournamentDescriptionText = null;
		private Text createStatusText = null;
		private GameObject TacoCreateFee = null;
		private Text TacoCurrencyText = null;
		private GameObject TacoCreateGToken = null;

		private Text gTokensWillHaveText = null;
		private GameObject opponentsText = null;
		private GameObject prizeText = null;
		private Text addGTokensText = null;

	// misc
		private GameObject feeToggle = null;
		private GameObject structureToggle = null;
		private GameObject playersToggle = null;
		private GameObject opponentsToggle = null;
		private GameObject typeCurrencyToggle = null;
		private GameObject createGTokenToggle = null;
		private GameObject gameToggle = null;
		private GameObject currencyToConvertToggle = null;
		private GameObject timeRemainingToggle = null;
		List<string> invitedFriends = new List<string>();

	// TODO Find a better way to do this
	// using these to set the Toggle Group's state
		private GameObject myActiveTournamentsButton = null;
		private GameObject myPrivateTournamentsButton = null;
		private GameObject myPublicTournamentsButton = null;
		

		// buttons
		private GameObject CreateTournamentButton = null;
		private GameObject InviteFriendsButton = null;
		private GameObject addGTokensButton = null;

	// modes
		private String CurrentSubPanel;

	// Use this for initialization
		void Start () {
			
			Instance = this;

		//panels
			myPrivatePanel = GameObject.Find(PanelNames.MyPrivatePanel);
			myActivePanel = GameObject.Find(PanelNames.MyActivePanel);
			myLeaderboardPanel = GameObject.Find(PanelNames.MyLeaderboardPanel);
			myPublicPanel = GameObject.Find(PanelNames.MyPublicPanel);

		//text
			createStatusText = GameObject.Find("CreateStatusText").GetComponent<Text> ();
			tournamentDescriptionText = GameObject.Find("TournamentDescriptionText").GetComponent<Text> ();
			prizeText = GameObject.Find("PrizeText");
			opponentsText = GameObject.Find("OpponentsText");
			gTokensWillHaveText = GameObject.Find("GTokensWillHaveText").GetComponent<Text>();
			TacoCreateFee = GameObject.Find("TacoCreateFee");
			TacoCurrencyText = GameObject.Find("TacoCurrencyText").GetComponent<Text>();
			TacoCreateGToken  = GameObject.Find("TacoCreateGToken");
			addGTokensText = GameObject.Find("AddGTokensText").GetComponent<Text>();
		// misc
			playersToggle = GameObject.Find("PlayersToggle");
			feeToggle = GameObject.Find("FeeToggle");
			structureToggle = GameObject.Find("StructureToggle");
			opponentsToggle = GameObject.Find("OpponentsToggle");
			gameToggle = GameObject.Find("GameToggle");
			timeRemainingToggle = GameObject.Find("TimeRemainingToggle");
			InviteFriendsButton = GameObject.Find("InviteFriendsButton");
			currencyToConvertToggle = GameObject.Find("CurrencyToConvertToggle");
			myActiveTournamentsButton = GameObject.Find("MyActiveTournamentsButton");
			myPrivateTournamentsButton = GameObject.Find("MyPrivateTournamentsButton");
			myPublicTournamentsButton = GameObject.Find("MyPublicTournamentsButton");
			typeCurrencyToggle = GameObject.Find("TypeCurrencyToggle");
			createGTokenToggle = GameObject.Find("CreateGTokenToggle");
			addGTokensButton = GameObject.Find("AddGTokensButton");

			CreateTournamentButton = GameObject.Find("CreateNewTournamentButton");
			activeTypeCurrency(0);
		}
		
	// Update is called once per frame
		void Update () {


			if (this.isActiveAndEnabled && !TacoManager.CheckModalsOpen() ) {

				if (Input.GetKeyDown (KeyCode.Return)) {

					// TODO find a better way - give the button focus and let the button handle the keypress
					if ( TacoManager.CreatePublicPanel.activeInHierarchy ) {

						StartCreate ();
					}

				}
			}

		}

	// called to open tournaments to default state
		public void Open(){

			if (CurrentSubPanel == null) {

				// sets the toggle group to the right button - need a better way to do this
				// this also fires the button and activates the listview
				myPublicTournamentsButton.GetComponent<Toggle> ().isOn = true;

				//ShowTournamentPanel (PanelNames.MyOpenPanel);

			} else {
				
				ShowTournamentPanel (CurrentSubPanel);

			}

		}

	// Unity Toggles only have onValueChanged exposed, so when tapped - you get events for the one turning on and also the one turning off
	// don't want multiple events, so detect only the events that are different
		public void PanelToggle(string panelName = null) {

			if (panelName != CurrentSubPanel) {

				ShowTournamentPanel (panelName);

			}

		}

		public string ReturnSubPanel(string panelName){

			if (panelName == null) {

			// do we already have a current panel?
				return CurrentSubPanel;

			} else {

			// if not use the one passed in
				return panelName;
			}


		}

		public void ShowTournamentPanel(string panelName = null) {


		// if passed in with null don't reset everything - show the subpanel
			string switchPanelName = ReturnSubPanel(panelName);

			SetAllTournamentPanels (false);

			// open the one you want and dependent stuff
			switch (switchPanelName) {

				case PanelNames.MyActivePanel:

				myActivePanel.SetActive (true);

				getActiveTournaments ();
				UpdateHeaderText (TacoConfig.TacoTournamentActive);

			// set our current panel name for next time
				CurrentSubPanel = switchPanelName;

				break;

				case PanelNames.MyPrivatePanel:

				myPrivatePanel.SetActive (true);

				getPrivateUserTournaments ();
				UpdateHeaderText (TacoConfig.TacoTournamentPrivate);


			// set our current panel name for next time
				CurrentSubPanel = switchPanelName;

				break;

				case PanelNames.MyPublicPanel:

				myPublicPanel.SetActive (true);

				getPublicTournaments ();
				UpdateHeaderText (TacoConfig.TacoTournamentPublic);

			// set our current panel name for next time
				CurrentSubPanel = switchPanelName;

				break;

				case PanelNames.MyLeaderboardPanel:
				
				myLeaderboardPanel.SetActive (true);
				LeaderboardList.Instance.LoadLeaderboard (TacoManager.Target);

				break;

			}

		}

		public void SetAllTournamentPanels(bool active){

			myPublicPanel.SetActive(active);
			myPrivatePanel.SetActive(active);
			myActivePanel.SetActive(active);
			myLeaderboardPanel.SetActive (active);

		}

	// TODO : find a better way to work with the toggle control
		public void ShowPanelByToggle ( string panelName ){

			myActiveTournamentsButton.GetComponent<Toggle> ().isOn = false;
			myPrivateTournamentsButton.GetComponent<Toggle> ().isOn = false;
			myPublicTournamentsButton.GetComponent<Toggle> ().isOn = false;

			switch (panelName) {

				case PanelNames.MyActivePanel:

			// sets the toggle group to the right button - need a better way to do this
			// this also fires the button and activates the listview
				myActiveTournamentsButton.GetComponent<Toggle> ().isOn = true;

				break;


				case PanelNames.MyPrivatePanel:

			// sets the toggle group to the right button - need a better way to do this
			// this also fires the button and activates the listview
				myPrivateTournamentsButton.GetComponent<Toggle> ().isOn = true;

				break;

				case PanelNames.MyPublicPanel:

			// sets the toggle group to the right button - need a better way to do this
			// this also fires the button and activates the listview
				myPublicTournamentsButton.GetComponent<Toggle> ().isOn = true;

				break;
			}


		}

		public int GetGameFromToggles(){

			int gameOption = TacoManager.GetTogglActiveName ( gameToggle.GetComponent<ToggleGroup> ()  );

			int game = 0;

			switch (gameOption) {

				case 0:
				game = 3;
				break;

				case 1:
				game = 2;
				break;

			}

			return game;

		}

	

		public string GetOpponentsFromToggles(){


			int opponentsOption = TacoManager.GetTogglActiveName ( opponentsToggle.GetComponent<ToggleGroup> ()  );

			string opponents = "";

			switch (opponentsOption) {

				case 0:
				opponents = TacoConfig.TacoTournamentOpponentsPublic;
				break;

				case 1:
				opponents = TacoConfig.TacoTournamentOpponentsPrivate;
				break;

				case 2:
				opponents = TacoConfig.TacoTournamentOpponentsChallenge;
				break;

			}

			return opponents;

		}

		public int GetCurrentFeeFromToggles(){

			int feeAmount = TacoManager.GetTogglActiveName (  feeToggle.GetComponent<ToggleGroup> ()  );

			return feeAmount;
		}

		public int GetCurrentGTokenFromToggles() {
			int gTokensAmount = TacoManager.GetTogglActiveName (  createGTokenToggle.GetComponent<ToggleGroup> ()  );

			return gTokensAmount;
		}
		public int GetCurrentCurrencyToConvertFromToggles() {
			int gTokensAmount = TacoManager.GetTogglActiveName (  currencyToConvertToggle.GetComponent<ToggleGroup> ()  );
			return gTokensAmount;
		}
		public void UpdateHeaderText(string text) {
			
			tournamentDescriptionText.text = text;

		}
		
		public void UpdatePrizeText() {
			
			int playersOption = TacoManager.GetTogglActiveName ( playersToggle.GetComponent<ToggleGroup> ()  );
			ToggleGroup abc = structureToggle.GetComponent<ToggleGroup> () ;
			Toggle[] prizeStrucList = abc.GetComponentsInChildren<Toggle>();

			int typeCurrencyChoice =  TacoManager.GetTogglActiveName ( typeCurrencyToggle.GetComponent<ToggleGroup> ()  );
			ToggleGroup typeCurrencyGroup = typeCurrencyToggle.GetComponent<ToggleGroup>();
			var typeCurrencyList =  typeCurrencyGroup.GetComponentsInChildren<Toggle>();
			activeTypeCurrency(typeCurrencyChoice);

			Debug.Log("hear ne");
			for(var i = 0 ; i < prizeStrucList.Length; i++){
					prizeStrucList[i].enabled = true;
				}
			if(playersOption == 2){
				//prizeStrucList[0].isOn = true;
				for(var i = 1 ; i < prizeStrucList.Length; i++){
					if(prizeStrucList[i].isOn){
						prizeStrucList[0].isOn = true;
						prizeStrucList[i].GetComponentInChildren<Text>().color = Color.white;
						prizeStrucList[i].GetComponentInChildren<Text>().GetComponentInChildren<Image>().color = Color.white;
					}
					prizeStrucList[i].isOn = false;
					prizeStrucList[i].enabled = false;
					prizeStrucList[i].GetComponentInChildren<Text>().color = Color.gray;
					prizeStrucList[i].GetComponentInChildren<Text>().GetComponentInChildren<Image>().color = Color.gray;
				}
				for(var i = 0 ; i < 1; i++){
					prizeStrucList[i].enabled = true;
					prizeStrucList[i].GetComponentInChildren<Text>().color = Color.white;
					prizeStrucList[i].GetComponentInChildren<Text>().GetComponentInChildren<Image>().color = Color.white;
				}
				//PrizeText =""+     GetCurrentPrizeFromToggles () ;
			}else if(playersOption == 3){
				//prizeStrucList[0].isOn = true;
				for(var i = 2 ; i < prizeStrucList.Length; i++){
					if(prizeStrucList[i].isOn){
						prizeStrucList[0].isOn = true;
						prizeStrucList[i].GetComponentInChildren<Text>().color = Color.white;
						prizeStrucList[i].GetComponentInChildren<Text>().GetComponentInChildren<Image>().color = Color.white;
					}
					prizeStrucList[i].isOn = false;
					prizeStrucList[i].enabled = false;
					prizeStrucList[i].GetComponentInChildren<Text>().color = Color.gray;
					prizeStrucList[i].GetComponentInChildren<Text>().GetComponentInChildren<Image>().color = Color.gray;
				}
				for(var i = 0 ; i < 2; i++){
					prizeStrucList[i].enabled = true;
					prizeStrucList[i].GetComponentInChildren<Text>().color = Color.white;
					prizeStrucList[i].GetComponentInChildren<Text>().GetComponentInChildren<Image>().color = Color.white;
				}
				//PrizeText =""+     GetCurrentPrizeFromToggles () ;
			}else if(playersOption == 5){
				//prizeStrucList[0].isOn = true;
				for(var i = 3 ; i < prizeStrucList.Length; i++){
					if(prizeStrucList[i].isOn){
						prizeStrucList[0].isOn = true;
						prizeStrucList[i].GetComponentInChildren<Text>().color = Color.white;
						prizeStrucList[i].GetComponentInChildren<Text>().GetComponentInChildren<Image>().color = Color.white;
						
					}
					prizeStrucList[i].isOn = false;
					prizeStrucList[i].enabled = false;
					prizeStrucList[i].GetComponentInChildren<Text>().color = Color.gray;
					prizeStrucList[i].GetComponentInChildren<Text>().GetComponentInChildren<Image>().color = Color.gray;
				}
				for(var i = 0 ; i < 3; i++){
					prizeStrucList[i].enabled = true;
					prizeStrucList[i].GetComponentInChildren<Text>().color = Color.white;
					prizeStrucList[i].GetComponentInChildren<Text>().GetComponentInChildren<Image>().color = Color.white;
				}
				//PrizeText =""+     GetCurrentPrizeFromToggles () ;
			}else if (playersOption == 10){
				//prizeStrucList[0].isOn = true;
				for(var i = 4 ; i < prizeStrucList.Length; i++){
					if(prizeStrucList[i].isOn){
						prizeStrucList[0].isOn = true;
						prizeStrucList[i].GetComponentInChildren<Text>().color = Color.white;
						prizeStrucList[i].GetComponentInChildren<Text>().GetComponentInChildren<Image>().color = Color.white;
					}
					prizeStrucList[i].isOn = false;
					prizeStrucList[i].enabled = false;
					prizeStrucList[i].GetComponentInChildren<Text>().color = Color.gray;
					prizeStrucList[i].GetComponentInChildren<Text>().GetComponentInChildren<Image>().color = Color.gray;
				}
				for(var i = 0 ; i < 4; i++){
					prizeStrucList[i].enabled = true;
					prizeStrucList[i].GetComponentInChildren<Text>().color = Color.white;
					prizeStrucList[i].GetComponentInChildren<Text>().GetComponentInChildren<Image>().color = Color.white;
				}
				//PrizeText =""+     GetCurrentPrizeFromToggles () ;
			}
			else{
				for(var i = 0 ; i < prizeStrucList.Length; i++){
					prizeStrucList[i].enabled = true;
					prizeStrucList[i].GetComponentInChildren<Text>().color = Color.white;
					prizeStrucList[i].GetComponentInChildren<Text>().GetComponentInChildren<Image>().color = Color.white;
				}
				//PrizeText =""+     GetCurrentPrizeFromToggles () ;
			}
			string OpponentsText = GetOpponentsFromToggles ();
			
			opponentsText.GetComponent<Text> ().text = OpponentsText;

	
			if (opponentsText.GetComponent<Text>().text == TacoConfig.TacoTournamentOpponentsPrivate )
			{
				InviteFriendsButton.SetActive(true);
			}
			else
			{
				InviteFriendsButton.SetActive(false);
			}
			Button createButton = CreateTournamentButton.GetComponent<Button>();

			if(typeCurrencyChoice == 0) {
				string PrizeText = TacoConfig.TournamentFeePrefix +" "+GetCurrentPrizeFromToggles ();
				prizeText.GetComponent<Text> ().text = PrizeText;

				TacoConfig.print (TacoManager.User.funds + "  >= " + GetCurrentFeeFromToggles ());

				// set the button on/off depending on enough funds
				if(  TacoManager.User.funds >= GetCurrentFeeFromToggles ()) {

					createButton.interactable = true;
					createStatusText.text = TacoConfig.TournamentCreateMessage;

				}else{
					createButton.interactable = false;
					createStatusText.text = TacoConfig.TournamentCreateNotEnoughFunds;
				}
			}
			else {
				string PrizeText = TacoConfig.TournamentFeePrefix +" "+CalculateGTokenFromToggles ();
				prizeText.GetComponent<Text> ().text = PrizeText;
				TacoConfig.print (TacoManager.User.gToken + "  >= " + CalculateGTokenFromToggles ());

				double gTokenOption = GetCurrentGTokenFromToggles () * TacoConfig.gTokenExchange;
				double userGTokens = double.Parse(TacoManager.User.gToken, NumberStyles.Number);
				TacoConfig.print ("GTokenOption: " + gTokenOption + " " + "userGtokens: " + userGTokens);
				if(  userGTokens >= gTokenOption) {
					createButton.interactable = true;
					createStatusText.text = TacoConfig.TournamentCreateMessage;
				}else{
					createButton.interactable = false;
					createStatusText.text = TacoConfig.TournamentCreateNotEnoughGTokens;
				}
			}

			

		}

		public void UpdateGTokenToBuy() {
			int gTokenchoise =  TacoManager.GetTogglActiveName ( currencyToConvertToggle.GetComponent<ToggleGroup> ()  );
			ToggleGroup currencyGroup = currencyToConvertToggle.GetComponent<ToggleGroup>();
			var currencyList =  currencyGroup.GetComponentsInChildren<Toggle>();
			gTokensWillHaveText.text = "GTokens you will have: " + (gTokenchoise * TacoConfig.gTokenExchange).ToString("0,0.00");
			addGTokensText.text = "";
		}

		public void activeTypeCurrency(int typeCurrency) {
			if(typeCurrency == 0) {
				TacoCreateFee.SetActive(true);
				feeToggle.SetActive(true);
				
				createGTokenToggle.SetActive(false);
				TacoCreateGToken.SetActive(false);
			}
			else {
				TacoCreateFee.SetActive(false);
				feeToggle.SetActive(false);
				createGTokenToggle.SetActive(true);
				TacoCreateGToken.SetActive(true);
			}
		}
		
		public string GetCurrentPrizeFromToggles(){
			int feeAmount = GetCurrentFeeFromToggles();
			int playersOption = TacoManager.GetTogglActiveName ( playersToggle.GetComponent<ToggleGroup> ()  );
			int prizeStructure = TacoManager.GetTogglActiveName ( structureToggle.GetComponent<ToggleGroup> ()  );
			
			int total = feeAmount * playersOption;
			decimal amount = (decimal)((total - (.1 * total))/prizeStructure);
			Debug.Log("fee " + feeAmount + "players "+playersOption+ "prize "+prizeStructure+  "total "	+total);
			return amount.ToString("C");
		}

		public string CalculateGTokenFromToggles(){
			int gTokenOption = GetCurrentGTokenFromToggles() * TacoConfig.gTokenExchange;
			int playersOption = TacoManager.GetTogglActiveName ( playersToggle.GetComponent<ToggleGroup> ()  );
			int prizeStructure = TacoManager.GetTogglActiveName ( structureToggle.GetComponent<ToggleGroup> ()  );
			
			int total = gTokenOption * playersOption;
			decimal amount = (decimal)((total - (.1 * total))/prizeStructure);
			Debug.Log("GToken " + gTokenOption + "players "+playersOption+ "prize "+prizeStructure+  "total "	+total);
			return "G " + amount.ToString(TacoConfig.GTokenFormat);
		}
		
		public string GetFeeAmount (){

			string amount = TacoManager.GetTogglActiveName (feeToggle.GetComponent<ToggleGroup> ()).ToString();
			return  amount;
		}

		public void InviteFriendsFromCreate(){


			GameObject invitePreFab = TacoModalPanel.Instance.GetPreFab ();

			TournamentCreateInvite  _tournamentCreateInvite = invitePreFab.GetComponent<TournamentCreateInvite>();

			invitedFriends.Add(_tournamentCreateInvite.EmailInput.text);


		}

		public void InviteFriend(){

			TacoManager.OpenMessage ( TacoConfig.TacoSending );

			Action<string> success = (string data) => {

				TacoManager.CloseMessage();

			};

			Action<string, string> fail = (string data, string error) => {

				TacoConfig.print("Error getting open tournaments : " + data);
				if (!string.IsNullOrEmpty(error)) {
					TacoConfig.print("Error : " + error);
				}
			};


			string fromEmail = TacoManager.User.email;
			string baseUrl = "baysidegames.com";

			GameObject invitePreFab = TacoModalPanel.Instance.GetPreFab ();

			TournamentInvite  _TournamentInvite = invitePreFab.GetComponent<TournamentInvite>();

			string emails =  _TournamentInvite.GetEmail ();
			int tournamentId = TacoManager.Target.id;

			TacoConfig.print("emails : "+  emails );

			StartCoroutine(ApiManager.Instance.InviteFriend( fromEmail , baseUrl, emails, tournamentId , success, fail ));


			}


		public void TappedInviteFromList ( Tournament t ){

			// TODO : move tournament Target to this class
			TacoManager.SetTarget(t);

			TacoManager.OpenModalWithPreFab (TacoConfig.TacoInviteFriendsModalHeader,TournamentInvitePrefab, TacoConfig.InviteSprite, ModalFunctions.InviteFriends,  TacoConfig.CloseSprite, null);

			if ( t.played  ) {
				TournamentManager.Instance.ShowTournamentPanel (PanelNames.MyLeaderboardPanel);

			}else if(t.memberlength == t.size){
				Double prize = double.Parse(  t.prize.ToString()  );

				string replacedString = TacoConfig.TacoSurePlayModalBody.Replace ("&prize",  TacoManager.FormatMoney(prize) );

				TacoManager.OpenModal (TacoConfig.TacoSurePlayModalHeader,replacedString, TacoConfig.PlaySprite, ModalFunctions.StartPlay, TacoConfig.CloseSprite, null);
			}else{
				TacoManager.OpenModalWithPreFab (TacoConfig.TacoInviteFriendsModalHeader,TournamentInvitePrefab, TacoConfig.InviteSprite, ModalFunctions.InviteFriends,  TacoConfig.CloseSprite, null);

				TacoModalPanel.Instance.SetModalButtonEnabled (false);

				TournamentInvite tournamentInvite = TacoModalPanel.Instance.GetPreFab ().GetComponent<TournamentInvite> ();
			}
		}

		public void TappedRemovedEmailFromCreate ( int index ){

			invitedFriends.RemoveAt(index);

			TacoManager.CloseAllModals();

			TappedInviteFromCreate();
		}


		public void TappedInviteFromCreate (  ){


			TacoManager.OpenModalWithPreFab (TacoConfig.TacoInviteFriendsModalHeader,TournamentCreateInvitePrefab, TacoConfig.InviteSprite, ModalFunctions.InviteFriendsFromCreate,  TacoConfig.CloseSprite, null);

			TacoModalPanel.Instance.SetModalButtonEnabled (false);

			TournamentCreateInvite _tournamentCreateInvite = TacoModalPanel.Instance.GetPreFab ().GetComponent<TournamentCreateInvite> ();


			for ( var i = 0; i < invitedFriends.Count; i++ )
			{

				_tournamentCreateInvite.AddInvite(invitedFriends[i], i );

				/*
				_tournamentCreateInvite.AddInvite(invitedFriends[0] , 1);
				_tournamentCreateInvite.AddInvite(invitedFriends[0], 2);
				_tournamentCreateInvite.AddInvite(invitedFriends[0] , 3);
				_tournamentCreateInvite.AddInvite(invitedFriends[0], 4);
				_tournamentCreateInvite.AddInvite(invitedFriends[0] , 5);
				_tournamentCreateInvite.AddInvite(invitedFriends[0], 0);
				_tournamentCreateInvite.AddInvite(invitedFriends[0] , 0);
				_tournamentCreateInvite.AddInvite(invitedFriends[0], 0);
				_tournamentCreateInvite.AddInvite(invitedFriends[0] , 0);
				_tournamentCreateInvite.AddInvite(invitedFriends[0], 0);
				_tournamentCreateInvite.AddInvite(invitedFriends[0] , 0);
				_tournamentCreateInvite.AddInvite(invitedFriends[0], 0);
				*/

			}


		}

		public void TappedJoinFromList ( Tournament t ){

		// TODO : move tournament Target to this class
			TacoManager.SetTarget(t);
			TacoConfig.print("join");
			TacoConfig.print(t);
			TacoConfig.print(t.typeCurrency);
			Double prize =   t.prize;
			Double entryFee =  t.entryFee;
			int typeCurrency = t.typeCurrency;
			string players = t.size.ToString () +" "+ TacoConfig.Players;

			TacoManager.OpenModalWithPreFab (TacoConfig.TacoSureJoinModalHeader,TournamentDetailsPrefab, TacoConfig.JoinSprite, ModalFunctions.JoinTournament,  TacoConfig.CloseSprite, null);

			TournamentDetails tournamentDetails = TacoModalPanel.Instance.GetPreFab ().GetComponent<TournamentDetails> ();

			if(typeCurrency == 0){
				string replacedString = TacoConfig.TacoSureJoinModalBody.Replace ("&entryFee",  TacoManager.FormatMoney(entryFee) );
				replacedString = replacedString.Replace ("&prize",  TacoManager.FormatMoney(prize) );
				tournamentDetails.UpdateDetails(replacedString, players, TacoManager.FormatMoney(prize) ) ;
			}else{
				string replacedString = TacoConfig.TacoSureJoinModalBody.Replace ("&entryFee",  TacoManager.FormatGTokens(entryFee) );
				replacedString = replacedString.Replace ("&prize",  TacoManager.FormatGTokens(prize) );
				tournamentDetails.UpdateDetails(replacedString, players, TacoManager.FormatGTokens(prize) ) ;
			}
			
		}

		public void TappedGameFromList ( Tournament t ){

			TacoManager.SetTarget(t);

			if ( t.played  ) {

				TournamentManager.Instance.ShowTournamentPanel (PanelNames.MyLeaderboardPanel);

			}else{

				Double prize =   t.prize;
				int typeCurrency = t.typeCurrency;
				string replacedString = string.Empty;

				if(typeCurrency == 0){
					replacedString = TacoConfig.TacoSurePlayModalBody.Replace ("&prize",  TacoManager.FormatMoney(prize) );
				} else {
					replacedString = TacoConfig.TacoSurePlayModalBody.Replace ("&prize",  TacoManager.FormatGTokens(prize) );
				}
				TacoManager.OpenModal (TacoConfig.TacoSurePlayModalHeader,replacedString, TacoConfig.PlaySprite, ModalFunctions.StartPlay, TacoConfig.CloseSprite, null);
			
			}

		}

	#region Get tournaments

	// for public
		public void getPublicTournaments() {

			TacoManager.OpenMessage ( TacoConfig.TacoRefreshing );

			Action<string> success = (string data) => {


                PublicTournamentsResult r = JsonUtility.FromJson<PublicTournamentsResult>(data);
			
                PublicTournamentsList.Instance.Reload(r.tournaments);

				TacoManager.CloseMessage();

			};

			Action<string, string> fail = (string data, string error) => {
				
				TacoConfig.print("Error getting open tournaments : " + data);
				if (!string.IsNullOrEmpty(error)) {
					TacoConfig.print("Error : " + error);
				}
			};

            //+ TacoConfig.SiteId
			StartCoroutine(ApiManager.Instance.GetWithToken("api/tournament/public/"+ TacoConfig.SiteId, success, fail));


		}

		public void getActiveTournaments() {

			TacoConfig.print ("Getting user t");
			TacoManager.OpenMessage ( TacoConfig.TacoRefreshing );

			Action<string> success = (string data) => {
                
                ActiveTournamentsResult r = JsonUtility.FromJson<ActiveTournamentsResult>(data);
			//TacoConfig.print("Result = " + r.success + " - " + r.tournaments.Count + " - " + data);
                ActiveTournamentsList.Instance.Reload(r.started, r.ended);

				TacoManager.CloseMessage();

			};

			Action<string, string> fail = (string data, string error) => {

				TacoConfig.print("Error getting open tournaments : " + data);
				if (!string.IsNullOrEmpty(error)) {
					TacoConfig.print("Error : " + error);
				}
			};

			StartCoroutine(ApiManager.Instance.GetWithToken("api/tournament/user/" + TacoConfig.SiteId, success, fail));

		}

		/*
		public void getPublicTournaments() {

			TacoConfig.print ("Getting user t");
			TacoManager.OpenMessage ( TacoConfig.TacoRefreshing );

			Action<string> success = (string data) => {
				MyCurrentTournamentsResult r = JsonUtility.FromJson<MyCurrentTournamentsResult>(data);
				//TacoConfig.print("Result = " + r.success + " - " + r.tournaments.Count + " - " + data);


				MyTournamentsList.Instance.Reload(r.tournaments);
			
				TacoManager.CloseMessage();

			};

			Action<string, string> fail = (string data, string error) => {

				TacoConfig.print("Error getting open tournaments : " + data);
				if (!string.IsNullOrEmpty(error)) {
					TacoConfig.print("Error : " + error);
				}
			};

			StartCoroutine(ApiManager.Instance.GetWithToken("api/tournament/public/" + TacoConfig.SiteId, success, fail));

		}
		*/

		public void getPrivateUserTournaments() {

			TacoConfig.print ("Getting user t");
			TacoManager.OpenMessage ( TacoConfig.TacoRefreshing );

			Action<string> success = (string data) => {
                
                PrivateTournamentsResult r = JsonUtility.FromJson<PrivateTournamentsResult>(data);
				//TacoConfig.print("Result = " + r.success + " - " + r.tournaments.Count + " - " + data);

                PrivateTournamentsList.Instance.Reload(r.tournaments);

				TacoManager.CloseMessage();

			};

			Action<string, string> fail = (string data, string error) => {

				TacoConfig.print("Error getting open tournaments : " + data);
				if (!string.IsNullOrEmpty(error)) {
					TacoConfig.print("Error : " + error);
				}
			};

			StartCoroutine(ApiManager.Instance.GetWithToken("api/tournament/private/" + TacoConfig.SiteId, success, fail));

		}

	#endregion


	#region Create tournament
		public void CreateTournament() {

			// disable the button during the creation process : it holds the keyboard focus
			// TODO : find a way to release the focus
			Button createButton = CreateTournamentButton.GetComponent<Button>();
			createButton.interactable = false;
			int typeCurrency = TacoManager.GetTogglActiveName (typeCurrencyToggle.GetComponent<ToggleGroup> ());
			string feeAmount = string.Empty;
			string currencyIcon = string.Empty;
			if(typeCurrency == 0) {
				feeAmount = GetFeeAmount ();
				currencyIcon = "$";
			} else {
				feeAmount = (GetCurrentGTokenFromToggles () * TacoConfig.gTokenExchange ).ToString();
				currencyIcon = "G";
			}
			

			int playersOption = TacoManager.GetTogglActiveName (playersToggle.GetComponent<ToggleGroup> ());
			
			int prizeStructure = TacoManager.GetTogglActiveName ( structureToggle.GetComponent<ToggleGroup> ()  );
			Debug.Log("prizeStructure" + prizeStructure);
			string name = string.Format(TacoManager.GameName + " - " + currencyIcon + "{0} Entry - {1} Players", feeAmount, playersOption );

			int opponentsOption = TacoManager.GetTogglActiveName ( opponentsToggle.GetComponent<ToggleGroup> ()  );

			string opponents = "";

			string invitedFriendsString = "";

			for( int i = 0; i < invitedFriends.Count; i++ ) {

				invitedFriendsString = invitedFriendsString + "," +invitedFriends[i].ToString();
			}

			if(!string.IsNullOrEmpty(invitedFriendsString)) {
				invitedFriendsString = invitedFriendsString.Substring(1);
			}

			switch (opponentsOption) {

				case 0:
				opponents = "public";
				break;

				case 1:
				opponents = "private";
				break;


			}
			int timeRemaining = 0;
			timeRemaining = TacoManager.GetTogglActiveName (timeRemainingToggle.GetComponent<ToggleGroup> ());
		 
			Action<string> success = (string data) => {
				TacoConfig.print("Create Tournament complete - " + data);

				TournamentSubmitResult(TacoConfig.TacoTournamentCreated);

				var r = JsonUtility.FromJson<CreateTournamentResult>(data);

				if (r.tournament != null) {
					TacoConfig.print("New tournament = " + r.tournament.id + " - " + r.tournament.name);
					
					double val = 0;
					if(r.typeCurrency == "real") {
						if (double.TryParse(r.userFunds, out val)) {
							TacoManager.UpdateFundsOnly(val);
						}
					}
					else {
						TacoManager.UpdateTokenOnly(r.userFunds);
					}
					

					TacoManager.SetTarget(r.tournament);
				}

				
					
			};

			Action<string, string> fail = (string data, string error) => {
				var msg = data + (string.IsNullOrEmpty(error) ? "" : " : " + error);
				TacoConfig.print("Error create tournamet : " + msg);

				if (!string.IsNullOrEmpty(data)) {
					var r = JsonUtility.FromJson<CreateTournamentResult>(data);
					if (!string.IsNullOrEmpty(r.message)) {

						TacoConfig.print("r.message: " + r.message);
						TournamentSubmitError(r.message);

					}
				}
			};

			StartCoroutine(ApiManager.Instance.CreateTournament( timeRemaining, typeCurrency, prizeStructure, invitedFriendsString , name, feeAmount, playersOption, opponents, TacoManager.GameId, TacoManager.User.token, success, fail));
			
		}

		private IEnumerator postCreate(string name, string fee, string players, string opponents, string gameInt) {
			WWWForm form = new WWWForm();

			form.AddField("name", name);
			form.AddField("fee", fee);
			form.AddField("size", players);

		//form.AddField("gameId", TacoManager.Instance.GameId);
			form.AddField("gameId", gameInt);

			form.AddField("siteId", TacoConfig.SiteId);
			form.AddField("opponents", opponents);


			TacoConfig.print("Form Post : Fee =" + fee + " players = " + players + " name =" + name);

			TacoConfig.print("Create Tournament complete - " + fee);

			UnityWebRequest www = UnityWebRequest.Post(Constants.BaseUrl + "api/tournament", form);
			www.SetRequestHeader("x-access-token", TacoManager.User.token);
			yield return www.Send();

			if (www.isError || www.responseCode == 500) {

				TacoConfig.print("DL Handler = " + www.downloadHandler.text);
				TacoConfig.print("URL = " + www.url);
				TacoConfig.print("Error downloading: " + www.error);

				if (!string.IsNullOrEmpty(www.downloadHandler.text)) {

					var r = JsonUtility.FromJson<CreateTournamentResult>(www.downloadHandler.text);
					if (!string.IsNullOrEmpty(r.message)) {

						TacoConfig.print("r.message: " +  r.message);

						TournamentSubmitError (r.message);

					}
				}
			} else {
			// show the highscores
				TacoConfig.print("Create Tournament complete - " + www.downloadHandler.text);


				TournamentSubmitResult ( TacoConfig.TacoTournamentCreated);

				var r = JsonUtility.FromJson<CreateTournamentResult>(www.downloadHandler.text);

				if(r.tournament != null) {
					TacoConfig.print("New tournament = " + r.tournament.id + " - " + r.tournament.name);
				}
				double val = 0;

				if (double.TryParse(r.userFunds, out val)) {
					TacoManager.UpdateFundsOnly(val);

				}

				TacoManager.SetTarget(r.tournament);
			}
		}

		public void StartCreate( ){

			string bodyString = TacoConfig.TournamentCreateModalBody;

		//GetCurrentFeeFromToggles
		//string feeString = GetFeeAmount();
			int typeCurrencyOption = TacoManager.GetTogglActiveName (typeCurrencyToggle.GetComponent<ToggleGroup> ());
			Double feeString = 0;
			string replacedString = string.Empty;
			if(typeCurrencyOption == 0) {
				feeString = double.Parse( GetFeeAmount()  );
				replacedString = bodyString.Replace ("&fee", TacoManager.FormatMoney( feeString ) );
			}
			else {
				feeString = GetCurrentGTokenFromToggles () * TacoConfig.gTokenExchange;
				replacedString = bodyString.Replace ("&fee", TacoManager.FormatGTokens( feeString ) );
			}

		// pop a dialog to make sure they want to create the tournament
			TacoManager.OpenModal (TacoConfig.TournamentCreateModalHeader, replacedString, TacoConfig.PlaySprite,  ModalFunctions.TournamentSubmit );

		}

		public void TournamentSubmitResult( string results){

			// clear invites
			invitedFriends.Clear();

			// turn back on create button
			Button createButton = CreateTournamentButton.GetComponent<Button>();
			createButton.interactable = true;

			TacoManager.CloseMessage();
		// open modal but don't let it be closed, they have to hit okay

			TacoManager.OpenModal ("", results, null, ModalFunctions.TournamentSubmitComplete , null,null,false);
		}


		public void TournamentSubmitError( string results){


			TacoConfig.print ("TournamentSubmitError");
			TacoManager.CloseMessage();
			TacoManager.OpenModal ("", results);

		}

		public void TournamentSubmit(){

		// pop the message panel while submitting
			TacoManager.OpenMessage( TacoConfig.TacoTournamentSubmittingMessage );

			CreateTournament();
			
		}

	#endregion

	#region Join Tournament
		public void Join() {

			TacoManager.OpenMessage (TacoConfig.TacoPublicJoining);

			var t = TacoManager.Target;


			Action<string> success = (string data) => {
            // show the highscores
				TacoConfig.print("Join Tournament complete - " + data);
            //createResultsText.GetComponent<Text>().text = "Tournament created, click the 'Play' button below to play your round now!";

				var r = JsonUtility.FromJson<JoinTournamentResult>(data);


				if (r.success) {

					TacoManager.SetTarget(t);

					Decimal prize = decimal.Parse(  r.tournament.prize.ToString("F2")  );
					Decimal entryFee = decimal.Parse(  r.tournament.entryFee.ToString("F2")  );
					
					string replacedString = "";
					if(r.tournament.typeCurrency == 0){
						replacedString = TacoConfig.TacoJoinPublicSuccessBody.Replace ("&userFunds", r.userFunds);
						replacedString = replacedString.Replace ("&entryFee", entryFee.ToString());
						replacedString = replacedString.Replace ("&prize", prize.ToString());
					}
					else{
						replacedString = TacoConfig.TacoJoinPublicSuccessBody.Replace ("&userFunds"," G "+ r.userFunds);
						replacedString = replacedString.Replace ("&entryFee"," G "+ entryFee.ToString());
						replacedString = replacedString.Replace ("&prize"," G "+ prize.ToString());
					}
					TacoManager.CloseMessage ();

					TacoManager.OpenModal(TacoConfig.TacoJoinPublicSuccessHead, replacedString, null, ModalFunctions.JoinPublicSuccess );

					double val = 0;
					if(r.tournament.typeCurrency == 0){
						//real money
						if (double.TryParse(r.userFunds, out val)) {
							TacoManager.UpdateFundsWithToken(val,r.currencyValue);
						}
					}else{
						//userFunds is 'gtoken' now
						if (double.TryParse(r.currencyValue, out val)) {
							TacoManager.UpdateFundsWithToken(val, r.userFunds);
						}
					}	
					

				} else {

					TacoManager.CloseMessage ();

					TacoManager.OpenModal(TacoConfig.TacoJoinPublicErrorHead, TacoConfig.TacoJoinPublicErrorMessage00 + r.message);

				}
			};

			Action<string, string> fail = (string data, string error) => {
				var msg = data + (string.IsNullOrEmpty(error) ? "" : " : " + error);
				TacoConfig.print("Error adding funds : " + msg);

				if (!string.IsNullOrEmpty(data)) {
					var r = JsonUtility.FromJson<CreateTournamentResult>(data);
					if (!string.IsNullOrEmpty(r.message)) {
						TacoConfig.print("r.message: " + r.message);
						TacoManager.CloseMessage ();
						TacoManager.OpenModal(TacoConfig.TacoJoinPublicErrorHead, r.message);
					}
				}
			};

			
			if (t != null) {
				StartCoroutine(ApiManager.Instance.JoinTournament(t.typeCurrency ,t.id, TacoConfig.SiteId, TacoManager.GameId, TacoManager.User.token, success, fail));
			}

		}

		private IEnumerator postJoin(Tournament t) {
			WWWForm form = new WWWForm();
			form.AddField("type", t.typeCurrency);
			form.AddField("tournamentId", t.id);
			form.AddField("siteId", TacoConfig.SiteId);
			form.AddField("gameId", TacoManager.GameId);

			UnityWebRequest www = UnityWebRequest.Post(Constants.BaseUrl + "api/tournament/join", form);
			www.SetRequestHeader("x-access-token", TacoManager.User.token);
			yield return www.Send();

			if (www.isError || www.responseCode == 500) {

				TacoConfig.print("DL Handler = " + www.downloadHandler.text);
				TacoConfig.print("URL = " + www.url);
				TacoConfig.print("Error downloading: " + www.error);


				var r = JsonUtility.FromJson<CreateTournamentResult>(www.downloadHandler.text);


				if (!string.IsNullOrEmpty(r.message)) {

					TacoManager.CloseMessage ();

					TacoManager.OpenModal (TacoConfig.TacoJoinPublicErrorHead,r.message);


				}

			} else {
			// show the highscores
				TacoConfig.print("Join Tournament complete - " + www.downloadHandler.text);
			//createResultsText.GetComponent<Text>().text = "Tournament created, click the 'Play' button below to play your round now!";

				var r = JsonUtility.FromJson<JoinTournamentResult>(www.downloadHandler.text);
				TacoConfig.print(r);

				if (r.success) {

					TacoManager.SetTarget(t);

					Decimal prize = decimal.Parse(  r.tournament.prize.ToString("F2")  );
					Decimal entryFee = decimal.Parse(  r.tournament.entryFee.ToString("F2")  );

					string replacedString = TacoConfig.TacoJoinPublicSuccessBody.Replace ("&userFunds", r.userFunds);
					replacedString = replacedString.Replace ("&entryFee", entryFee.ToString());

					replacedString = replacedString.Replace ("&prize", prize.ToString());

					TacoManager.CloseMessage ();


					TacoManager.OpenModal (TacoConfig.TacoJoinPublicSuccessHead ,replacedString, null, ModalFunctions.JoinPublicSuccess  );

					double val = 0;
					if(double.TryParse(r.userFunds, out val)) {
						TacoManager.UpdateFundsOnly(val);

					}



				} else {
					TacoManager.CloseMessage ();
					TacoManager.OpenModal (TacoConfig.TacoJoinPublicErrorHead ,TacoConfig.TacoJoinPublicErrorMessage00 + r.message);

				}


			}
		}

	#endregion
	}


}
