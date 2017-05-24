// Const values that will probably change per game implementation or might be customized or localized 

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace GameTaco
{
	public class TacoConfig : MonoBehaviour {
		public static TacoConfig Instance;

		// This game - change for each game
		public const string SiteId = "bubbles";

		// static sprites that will be loaded from resources folder
		public static Sprite TacoIntroGraphic;
		public static Sprite ModalSprite;
		public static Sprite OptionalModalSprite;
		public static Sprite CloseSprite;
		public static Sprite PlaySprite;
		public static Sprite JoinSprite;
		public static Sprite InviteSprite;
		public static Sprite FirstPlaceSprite;
		public static Sprite SecondPlaceSprite;
		public static Sprite ThirdPlaceSprite;
		public static Sprite NotSelected;
		public static Sprite Avatar00;
		public static Sprite Avatar01;
		public static Sprite Avatar02;
		public static Sprite Avatar03;
		public static Sprite Avatar04;
		public static Sprite Avatar05;
		public static Sprite Avatar06;
		public static Sprite Avatar07;

		// to hold strings
		public static Hashtable TacoStrings = new Hashtable();

		// allows developers to squelch our debug spam 
		public static bool showDebugLog = true;

		// text format strings

		public const string MoneyFormat = "C"; //= "{0:#,###,###.##}";
		public const string GTokenFormat = "0,0.00";

		public const int gTokenExchange = 1000;
		// meta words 
		public const string  Players = "players";
		public const string  Prize = "prize";
		public const string  Played = "Played";
		public const string  Finished = "Finished";
		public const string  Open = "Open";

		public const string  TacoGameName00 = "Bubbles";
		public const string  TacoGameName01 = "2048";
		public const string  TacoGameName02 = "Holy Guacamole";

		// general

		public const string  NoResults = "No Tournaments";
		public const string  GamePlayingMessage = "Playing Tournament Game!";

		public const string  TacoIntroHeader = "Welcome to Game Taco, Real Money Tournaments";
		public const string  TacoIntroBody = "Sign-Up, Add Funds using Stripe and Play or Create Tournaments!";

		public const string TacoSending = "Sending...";
		public const string TacoRefreshing = "Refreshing...";
		public const string Error = "Sorry, Something Went Wrong";

		public const string  TacoQuitGameHeader = "Quit Tournament game?";
		public const string  TacoQuitGameBody = "Are you sure you want to quit this tournament game and forfeit your round?";

		// help
		public const string TacoHelpLoginTitle = "Sign-Up and Join Today";
		public const string TacoHelpLoginBody = "GameTaco let's you bring your skills to battle! To battle your friends for real money!\n\nAll it takes is a quick Sign-Up and withdraw some cash to get started. Withdrawls are done with Stripe.";

		// headers
		public const string TacoFundsHeader = "Add or Withdraw Funds";
		public const string TacoOpenButtonMessage = "Play Tournaments Today!";

		public const string TacoOurGamesHeader = "Game Taco Games!";
		public const string TacoOurGamesMessage = "Take a look at all of our games.";

		// login
		public const string TacoLoginMessage = "Practice, Compete - Win $$";
		public const string TacoLoginEmail = "ENTER EMAIL";
		public const string TacoLoginPassword = "ENTER PASSWORD";
		public const string TacoLoginConfirm = "CONFIRM PASSWORD";
		public const string TacoLoginUser = "ENTER USERNAME";

		public const string TacoLoginErrorHeader = "Login Failed";
		public const string TacoLoginErrorMessage00 = "An email and password is required";
		public const string TacoLoginErrorMessage01 = "Email/Password incorrect";
		public const string TacoLoginErrorMessage02 = "Please confirm your password";
		public const string TacoLoginErrorMessage03 = "Password and Confirm Password must match";
		public const string TacoLoginErrorMessage04 = "Please confirm age to continue";
		public const string TacoLoginErrorMessage05 = "An unexpected error occurred";

		public const string TacoLoginStatusMessage00 = "Logging in...";
		public const string TacoLoginStatusMessage01 = "Login Complete";

		public const string TacoRegisteredStatusMessage00 = "Registering...";

		public const string TacoRegisteredModalTitle = "Account Created";

		public const string TacoRegisteredModalBody = "Your account has been created.";
		public const string TacoRegisteredErrorHeader = "Account Creation Error";
		public const string TacoRegisteredErrorMessage06 = "Enter your email.";
		public const string TacoRegisteredErrorMessage05 = "An unexpected error occurred";
		public const string TacoRegisteredErrorMessage00 = "Password and Confirm Password must match.";
		public const string TacoRegisteredErrorMessage01 = "Not a valid email.";
		public const string TacoRegisteredErrorMessage02 = "Please verify if you are over 18.";
		public const string TacoRegisteredErrorMessage03 = "Please enter a username.";
		public const string TacoRegisteredErrorMessage04 = "Please enter a password and confirm that password.";

		public const string TacoRegisteredErrorMessage07 = "Password must be at least 4 characters long.";
		public const string TacoRegisteredAllOkay = "Looks good, tap or click to register.";

		// our games

		public const string TacoOurGamesLoadingMessage = "Loading Games...";
		public const string TacoHighScoresPerhapsText = "highscore";
		public const string TacoHighScoresType = "highscoretype";

		//tournaments 

		// leaderboard 
		public const string LeaderboardResults = "Name : &name Prize : &prize Entry Fee : &fee";
		public const string CurrentLeader = "Current Leader";

		// List view header rows
		public static readonly List<string> PastTournamentsColumns = new List<string>()  { "Name", "Prize", "Winner", "Entry Fee","Status","View Results" };
		public static readonly List<string> CurrentTournamentsColumns = new List<string>()  { "Name", "Prize","Winner", "Entry Fee","Status","Play/View Results" };
		public static readonly List<string> PublicTournamentsColumns = new List<string>()  { "Name", "Prize", "Winner","Entry Fee","Status","Join" };
		public static readonly List<string> PrivateTournamentsColumns = new List<string>()  { "Name", "Prize", "Winner","Entry Fee","Status","Play/Invite Friends" };
		public static readonly List<string> LeaderboardTournamentsColumns = new List<string>()  {"", "Player","Username", "Score", "Rank",""};

		public const string TacoTournamentError = "Error getting Your Tournaments ";

		public const string TacoTournamentJoinOrCreate = "Join an existing tournament or create a new one below";
		public const string TacoTournamentCreated = "Tournament created and added to your current Tournaments.";

		public const string TournamentCreateModalHeader = "Create Tournament?";
		public const string TournamentCreateModalBody = "Are you sure you would like to create this tournament? If so a fee of &fee will be debited from your account?";
		public const string TournamentFeePrefix = "Prize Total :"; 
		public const string TournamentCreateNotEnoughFunds = "You don't have enough funds to create this Tournament."; 
		public const string TournamentCreateNotEnoughGTokens = "You don't have enough GTokens to create this Tournament."; 
		public const string TournamentCreateMessage = "Select the Entry Fee, Number of Players and type of opponents.";

		public const string TacoTournamentSubmittingMessage = "Creating Your Tournament";  

		public const string TacoTournamentOpponentsPublic = "Public - Anyone can join";  
		public const string TacoTournamentOpponentsPrivate = "Private - Only friends can join";  
		public const string TacoTournamentOpponentsChallenge = "Challenge - Directly challenge other players";  

		public const string TacoTournamentActive = "TOURNAMENTS YOU'VE JOINED. TOURNAMENTS END WHEN ALL MEMBERS HAVE PLAYED.";  
		public const string TacoTournamentPublic = "PUBLIC TOURNAMENTS YOU CAN JOIN.";  
		public const string TacoTournamentPrivate = "YOUR PRIVATE TOURNAMENTS.";  

		public const string TacoPublicJoin = "Join >";

		public const string TacoPublicJoining = "Joining...";

		public const string TacoLeaderboardSeeResults = "Results >";

		public const string TacoSureLogoutModalHeader = "Logout?";
		public const string TacoSureLogoutModalBody = "Are you sure you want to logout?";

		public const string TacoSurePlayModalHeader = "Play This Tournament?";
		public const string TacoSurePlayModalBody = "Are you sure you are ready to play this game with a prize of &prize?";

		public const string TacoPlayStarting = "Starting Tournament Game, good luck!";
		public const string TacoPlayError = "Tournament Game failed.";

		public const string TacoPlayActiveHeader = "Tournament Game Active!";
		public const string TacoPlayActiveBody = "Game is currently paused, but active.\n\nPlease finish your game and Good Luck!";


		public const string TacoPlayEndedMessage = "Your Round Finished : Posting results!";
		public const string TacoPlayAgainEndedMessage = "Your Round Finished!";
		public const string TacoPlayEndedModalHeader = "Tournament Round Finished!";
		public const string TacoPlayEndedModalBody = "Your game ended, we have posted your score of &gameEndScore to the tournament.\n\n Good Luck!  ";

		public const string TacoPlayEndedAgainModalBody = "Your game ended. Good Luck!  ";
		// join public 

		public const string TacoSureJoinModalHeader = "Join This Tournament?";
		public const string TacoSureJoinModalBody = "Are you sure you would like to join this Public Tournament with a prize of &prize? \nIf so a fee of &entryFee will be debited from your account?";

		public const string TacoJoinPublicSuccessHead = "Tournament Joined!";
		public const string TacoJoinPublicSuccessBody = "You can Play your round now. \n\n You joined a tournament with a prize of &prize and a entry fee of &entryFee . \n You now have : &userFunds available. ";

		public const string TacoJoinPublicErrorHead = "Join Tournament Error";
		public const string TacoJoinPublicErrorMessage00 = "Unable to join, please ensure you have enough funds to join the tournament.";

		// funds 

		// invite friends 

		public const string TacoInviteFriendsModalHeader = "Invite Your Friends";

		// withdraw
		public const string WithdrawFundsError00 = "An amount is required";
		public const string WithdrawFundsError01 = "Amount must be a number";
		public const string WithdrawFundsError02 = "You don't have that many funds available. Nice try!";
		public const string WithdrawFundsError03 = "A name is required";
		public const string WithdrawFundsError04 = "An address is required";
		public const string WithdrawFundsError05 = "A city is required";
		public const string WithdrawFundsError06 = "A zip code is required";
		public const string WithdrawFundsSuccessMessage = "Your request has been submitted. A check should be sent within 2 weeks.";

		// add 
		public const string AddFundsError00 = "A credit card number is required";
		public const string AddFundsError01 = "An expiration month is required";
		public const string AddFundsError02 = "An expiration year is required";
		public const string AddFundsError03 = "A CVC field is required";
		public const string AddFundsError04 = "An amount must be selected.";
		public const string AddFundsSuccessMessage = "Charge complete, you now have ";
		public const string AddFundsTitleSuccessMessage = "Funds Added";

		public const string AddGTokensTitleSuccessMessage = "GTokens Added";
		public const string AddFundsAdding = "Adding specified funds....";
		public const string AddGTokensAdding = "Adding specified GTokens....";

		//tabs
		public const string TacoTournamentCurrentTab = "Current Tournaments";

		public const string TournamentListViewView = "Results >";
		public const string TournamentListViewPlay = "Play Now >";

		public const string TournamentListViewPlayed = "Played";
		public const string TournamentListViewReady = "Ready";

		public const int ListViewButtonHeight = 90;
		public const int ListViewTournamentsButtonHeight = 150;

		public Color32 ListViewHighlightColor = hexToColor ("f8c477ff");
		public Color32 ListViewHeaderColor = hexToColor ("950606ff");
		public Color32 ListViewTextBrightColor = hexToColor ("f8c477ff");

		// fonts

		public const int ListViewHeaderHeight = 64;
		public const int ListViewItemFontSize = 36;
		public const int ListViewHeaderFontSize = 30;

		public TacoFont HeaderFont = new TacoFont(52);
		public TacoFont BodyFont = new TacoFont(42);
		public TacoFont InputFont = new TacoFont(42);
		public TacoFont MessageFont = new TacoFont(50);

		// Developers can easily turn on/off our debug messages
		public static void print(string message, Object context = null) {
			if (showDebugLog) {
				if (TacoManager.tacoDebugConsole != null) {
					TacoManager.tacoDebugConsole.Log(message);
				}
				
				if (context != null) {
					Debug.Log(message);
				} else {
					Debug.Log(message, context);
				}
			}
		}

		void Awake() {
			Instance = this;

			// remember these have to live under the Resources folder
			TacoIntroGraphic = Resources.Load("TacoIntroGraphic",typeof(Sprite)) as Sprite;
			ModalSprite = Resources.Load("TacoOkay",typeof(Sprite)) as Sprite;
			OptionalModalSprite = Resources.Load("TacoBack",typeof(Sprite)) as Sprite;
			CloseSprite = Resources.Load("TacoCloseCircle",typeof(Sprite)) as Sprite;
			PlaySprite = Resources.Load("TacoPlay",typeof(Sprite)) as Sprite;
			JoinSprite = Resources.Load("TacoJoin",typeof(Sprite)) as Sprite;
			InviteSprite = Resources.Load("TacoInvite",typeof(Sprite)) as Sprite;
			NotSelected = Resources.Load("TacoNotSelected",typeof(Sprite)) as Sprite;
			FirstPlaceSprite = Resources.Load("TacoFirstPlace",typeof(Sprite)) as Sprite;
			SecondPlaceSprite = Resources.Load("TacoSecondPlace",typeof(Sprite)) as Sprite;
			ThirdPlaceSprite = Resources.Load("TacoThirdPlace",typeof(Sprite)) as Sprite;

			Avatar00 = Resources.Load("TacoAvatar_00",typeof(Sprite)) as Sprite;
			Avatar01 = Resources.Load("TacoAvatar_01",typeof(Sprite)) as Sprite;
			Avatar02 = Resources.Load("TacoAvatar_02",typeof(Sprite)) as Sprite;
			Avatar03 = Resources.Load("TacoAvatar_03",typeof(Sprite)) as Sprite;
			Avatar04 = Resources.Load("TacoAvatar_04",typeof(Sprite)) as Sprite;
			Avatar05 = Resources.Load("TacoAvatar_05",typeof(Sprite)) as Sprite;
			Avatar06 = Resources.Load("TacoAvatar_06",typeof(Sprite)) as Sprite;
			Avatar07 = Resources.Load("TacoAvatar_07",typeof(Sprite)) as Sprite;

			// TODO : Move all of these to const like the other below - don't want two lists of strings. Did it this way so I can match the key using the associative array

			// device specific 

			#if UNITY_EDITOR
			TacoStrings ["DeviceName"] = "Computer";
			#endif

			#if UNITY_IOS
			TacoStrings ["DeviceName"] = "device";
			#endif

			#if UNITY_STANDALONE_OSX
			TacoStrings ["DeviceName"] = "Mac";
			#endif

			#if UNITY_STANDALONE_WIN
			TacoStrings ["DeviceName"] = "Computer";
			#endif

			
			// styles 

			// buttons

			TacoStrings ["CreateTournamentButton"] = "Create Tournament";
			TacoStrings ["TournamentsButton"] = "Tournaments";
			TacoStrings ["AddFundsButton"] = "Add Funds";
			TacoStrings ["AddGTokensButton"] = "Add GTokens";
			TacoStrings ["AddGTokenButton"] = "Add GToken";
			TacoStrings ["LogoutButton"] = "Logout";
			TacoStrings ["OtherGamesButton"] = "Our Games";
			TacoStrings ["WithdrawFundsButton"] = "Withdraw Funds";

			TacoStrings ["CreateNewTournamentButton"] = "Create Tournament";
			TacoStrings ["TacoStageButton"] = "Play Tournaments Today!";

			TacoStrings ["TacoModalButton"] = "Close";

			TacoStrings ["WithdrawFundsButton"] = "Withdraw Funds";

			TacoStrings ["CurrentLeaderboardButton"] = "Leaderboard";
			TacoStrings ["CurrentPlayButton"] = "Play";

			TacoStrings ["NavCreateTournamentButton"] = "Create New Tournament!";
			TacoStrings ["MyActiveTournamentsButton"] = "My Tournaments";
			TacoStrings ["MyLeaderboardTournamentsButton"] = "Leaderboards";
			TacoStrings ["MyPublicTournamentsButton"] = "Public";
			TacoStrings ["MyPrivateTournamentsButton"] = "Browse";

			TacoStrings ["LoginRegisterButton"] = "Skip For Now";
			TacoStrings ["LoginButton"] = "Sign-In";
			TacoStrings ["LoginSkipButton"] = "Skip For Now";
			TacoStrings ["LoginRegisterButton"] = "Create";

			TacoStrings ["RegisterAgeToggle"] = "By checking this box, I confirm that I am at least 18 years of age.";
			TacoStrings ["AutoLoginToggle"] = "Remember my login on this " + TacoStrings ["DeviceName"];

			TacoStrings ["TacoFundsSkipButton"] = "Skip For Now";
			TacoStrings ["JoinTournamentButton"] = "Join";


			TacoStrings ["InviteFriendsButton"] = "Invite Friends";

			TacoStrings ["TacoCreateTime"] = "Time Remaining :";
			// labels & text


			TacoStrings ["TacoInvitedText"] = "Friends To Invite:";
			TacoStrings ["TacoOurGamesHeader"] = "Other Taco Games";
			TacoStrings ["TacoOurGamesMessage"] = "New Challenges!";

			TacoStrings ["TacoEmail"] = "Enter your email or username";
			TacoStrings ["TacoPassword"] = "Enter your password";

			TacoStrings ["TacoCreateHeader"] = TacoStrings ["CreateTournament"];
			TacoStrings ["TacoCreateMessage"] = "Configure a new tournament below.";
			TacoStrings ["TacoCurrencyText"] = "Type Currency :";
			TacoStrings ["TacoCreatePlayers"] = "Tournament Size :";
			TacoStrings ["TacoCreateFee"] = "Entry Fee :";
			TacoStrings ["TacoPrizeStructure"] = "Winner :";
			TacoStrings ["TacoCreateGToken"] = "GToken :";
			// our games 
			TacoStrings ["TacoGames"] = "Featured Games with Taco Power:";
			TacoStrings ["TacoGamesMessage"] = "Tap a game for installation details.";

			// funds 
			TacoStrings ["AddFundsPanelHeader"] = "Enter your credit card details to add funds from Stripe.";
			TacoStrings ["WithdrawFundsPanelHeader"] = "Enter your details and the amount that you would like to withdraw.";

			//GTokens
			TacoStrings ["AddGTokensPanelHeader"] = "Choice price to convert GTokens";

			TacoStrings ["TogglePublic"] = "Public";
			TacoStrings ["TogglePrivate"] = "Private";
			TacoStrings ["TacoCreateAccess"] = "Opponents :";

			TacoStrings ["ToggleRealCurrency"] = "Real";
			TacoStrings ["ToggleGTokenCurrency"] = "GToken";

			TacoStrings ["StatusText"] = TacoLoginMessage;
			TacoStrings ["TacoOpenButtonMessage"] = TacoOpenButtonMessage;

			TacoStrings ["TacoJoinMessage"] = "Would you like to join this Public Tournament?";
			TacoStrings ["TacoJoinHeader"] = "Join Tournament?";

			// leaderboard

			TacoStrings ["LeaderboardPanelHeader"] = "Tournament Results";

			// modals 

			TacoStrings ["TacoIntroHeader"] = "Practice, Compete - Win $$";
			TacoStrings ["TacoIntroBody"] = "Start challenging your friends for real money. Login or register to challenge today.";

            TacoStrings ["TacoInviteEmail"] =  "Enter Your Friend's Email or Username:";
		}


		public static string GetValue( string key){
			string value = (string)TacoStrings [key];
			return value;
		}

		public static Color hexToColor(string hex) {
			hex = hex.Replace ("0x", "");//in case the string is formatted 0xFFFFFF
			hex = hex.Replace ("#", "");//in case the string is formatted #FFFFFF
			byte a = 255;//assume fully visible unless specified in hex
			byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
			//Only use alpha if the string has enough characters
			if(hex.Length == 8){
				a = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
			}
			return new Color32(r,g,b,a);
		}

		public Sprite GetAvatarSprite( int avatar){
			Sprite avatarSprite = Avatar00;

			switch (avatar) {
				case 0:
				avatarSprite = Avatar00;
				break;
				case 1:
				avatarSprite = Avatar01;
				break;
				case 2:
				avatarSprite = Avatar02;
				break;
				case 3:
				avatarSprite = Avatar03;
				break;
				case 4:
				avatarSprite = Avatar04;
				break;
				case 5:
				avatarSprite = Avatar05;
				break;
				case 6:
				avatarSprite = Avatar06;
				break;
				case 7:
				avatarSprite = Avatar07;
				break;
			}

			return avatarSprite;
		}
	}
}
