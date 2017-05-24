using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameTaco {
	
// Const values that probably won't change per game or would be customized or localized

    #region Helper Models

    public static class Constants {

       public const string BaseUrlUnsecure = "http:s//www.gametaco.com/";
       public static readonly string BaseUrl = "http://localhost:3000/";//Debug.isDebugBuild ? "https://taco-webapp-pr-8.herokuapp.com/" : "https://taco-webapp.herokuapp.com/";
       public const string StripeTokenUrl = "https://api.stripe.com/v1/tokens";
		public const string StripePublicKey = "pk_test_mIhrqMhTL13eRBG4CTOBLj9A";
       public const string localUrl = "http://localhost:3000/";

       public const string AesIV256 = @"G8G22#I2197QX^AV";
        public const string AesKey256 = @"5DON70%B*6A71EG&3JD^%1F*E#7JSW@^";

   }

	public class TacoFont {
		
		public int Size;
		public FontStyle FontStyle;
		public Color32 FontColor;

		public TacoFont ( int size ){

			Size = size;
		}
	
	}

   public class TacoUser {
    public int userId;
    public string email;
    public double funds;
    public string token;
    public int avatar;

    public string gToken;
    public int autoLogin;
    public int highScoreUser;

    public int ticket;

}

public static class CanvasNames {

  public const string TacoBlockingCanvas = "TacoBlockingCanvas";
  public const string TacoCommonCanvas = "TacoCommonCanvas";
  public const string TacoAuthCanvas = "TacoAuthCanvas";
  public const string TacoTournamentsCanvas = "TacoTournamentsCanvas";
  public const string TacoFundsCanvas = "TacoFundsCanvas";
  public const string TacoOurGamesCanvas = "TacoOurGamesCanvas";
}

public static class PanelNames {

   public const string Modal = "TacoModalPanel";
   public const string Message = "TacoMessagePanel";
   public const string Foldout = "TacoFoldout";

   public const string MyTournamentsPanel = "MyTournamentsPanel";
   public const string MyPrivatePanel = "MyPrivatePanel";
   public const string MyActivePanel = "MyActivePanel";
   public const string MyLeaderboardPanel = "MyLeaderboardPanel";
   public const string MyPublicPanel = "MyPublicPanel";
   public const string CreatePublicPanel = "CreateTournamentPanel";
   public const string JoinPublicPanel = "JoinPublicPanel";

   public const string AddFundsPanel = "AddFundsPanel";

   public const string PrizesPanel = "PrizeItemsPanel";

   public const string AddGTokensPanel = "AddGTokensPanel";
   public const string WithdrawFundsPanel = "WithdrawFundsPanel";

   public const string RegisterPanel = "RegisterPanel";
   public const string LoginPanel = "LoginPanel";

   public const string FeaturedGamesPanel = "FeaturedGamesPanel";

}

public static class ModalFunctions {

   public const string TournamentSubmit = "TournamentSubmit";
   public const string TournamentSubmitResult = "TournamentSubmitResult";
   public const string ForfeitTournamentGame = "ForfeitTournamentGame";

   public const string SawIntro = "SawIntro";
   public const string StartPlay = "StartPlay";

   public const string JoinPublicSuccess = "TacoJoinPublicSuccess";

   public const string JoinTournament = "JoinTournament";
    public const string InviteFriends = "InviteFriends";
    public const string InviteFriendsFromCreate = "InviteFriendsFromCreate";


   public const string LogoutUser = "LogoutUser";
   public const string TournamentSubmitComplete = "TournamentSubmitComplete";
   public const string RegisterResult = "RegisterResult";
   public const string TournamentGamePosted = "TournamentGamePosted";
	public const string ReturnToTournaments = "ReturnToTournaments";

   public const string ReturnToGame = "ReturnToGame";
    public const string ReturnToMenu = "ReturnToMenu";

    public const string TacoEndTournament = "TacoEndTournament";

    public const string TacoFreePlayGiftToken = "TacoFreePlayGiftToken";
}

public static class UserPreferences {

   public const string sawIntro = "sawIntro";
   public const string autoLogin = "autoLogin";
   public const string userToken = "userToken";

}
    #endregion

    #region Result Models

[System.Serializable]
public class CreateTournamentResult : System.Object {

    public bool success;
    public Tournament tournament;
    public string message;
    public string userFunds;

    public string typeCurrency;
    public CreateTournamentResult() { }
}

[System.Serializable]
public class LeaderboardResult : System.Object {

    public bool success;
    public Tournament tournament;
    public string winner;
    public string message;
    public string status;

    public List<LeaderboardRow> leaderboard;

    public LeaderboardResult() { }
}




	    /* - recorded JSON for LeaderboardResult
	    www = {"success":true,
		    "winner":"o@1.com",
		    "tournament":{"id":138,"creatorId":1,"name":"Bubbles - 2 Entry - 2","gameId":1,"type":null,"accessType":"public","size":2,"entryFee":"2.00","prize":"3.60",
		    "startDate":null,"endDate":null,"createdAt":"2016-08-31T07:42:20.487Z","updatedAt":"2016-09-07T06:56:49.011Z",
		    "featured":true,"status":"ended","created_at":"2016-08-31T07:42:20.487Z","updated_at":"2016-09-07T06:56:49.011Z"},

		    "leaderboard":[{"rank":"1","score":0,"email":"o@1.com"},{"rank":"1","score":0,"email":"dfkasljfdalk@sdfkjasdflkjsa.com"}]}
		    UnityEngine.Debug:Log(Object)
		    GameTaco.TacoConfig:print(String) (at Assets/GameTaco/Scripts/TacoConfig.cs:345)
		    GameTaco.<GetWithToken>c__Iterator0:MoveNext() (at Assets/GameTaco/Scripts/ApiManager.cs:64)
		    UnityEngine.SetupCoroutine:InvokeMoveNext(IEnumerator, IntPtr)

    */

            [System.Serializable]
            public class LeaderboardRow {
                public string rank;
                public int score;
                public string email;

                public LeaderboardRow() { }
            }

    public class PrivateRow {
		
		public bool success;
		public List<Tournament> started;
		public List<Tournament> ended;
		public int userId;
		public int gameId;
		public string gameName;
		public string email;
		public double funds;
		public string token;
		public string message;
		public int avatar;

        public PrivateRow() { }
    }

            [System.Serializable]
            public class ActiveTournamentsResult : System.Object {

                public List<Tournament> started;
                public List<Tournament> ended;
                public string message;

                public ActiveTournamentsResult() { }
            }

			[System.Serializable]
			public class PublicTournamentsResult : System.Object {

				public List<Tournament> tournaments;
				public string message;

                public PublicTournamentsResult() { }
			}

	        [System.Serializable]
	        public class PrivateTournamentsResult : System.Object {

		        public List<Tournament> tournaments;
		        public string message;

		        public PrivateTournamentsResult() { }
	        }

			[System.Serializable]
			public class MyPastTournamentsResult : System.Object {

				public List<Tournament> tournaments;
				public string message;

				public MyPastTournamentsResult() { }
			}

            [System.Serializable]
            public class JoinTournamentResult : System.Object {

                public bool success;
                public Tournament tournament;
                public string message;
                public string userFunds;
                public string currencyValue;
                public JoinTournamentResult() { }
            }

	    /* - recorded JSON for JoinTournamentResult
	    Join Tournament complete -
	    {"success":true,
		    "tournament":{"id":192,"creatorId":1,"name":"Bubbles - $5 Entry - 5 Players","gameId":1,"type":null,"accessType":"public","size":5,"entryFee":"5.00","prize":"22.50","startDate":null,"endDate":null,"createdAt":"2016-09-07T02:34:18.884Z","updatedAt":"2016-09-07T02:34:18.884Z","featured":true,"status":"started","created_at":"2016-09-07T02:34:18.884Z","updated_at":"2016-09-07T02:34:18.884Z"}
		    ,"userFunds":"-1891.20"}

    */



            [System.Serializable]
            public class StartGameResult : System.Object {

                public bool success;
                public int tournamentId;
                public string token;
                public int highScore;
                public string message;

                public StartGameResult() { }
            }

            [System.Serializable]
            public class ScoreResult : System.Object {

                public bool success;
                public int score;
                public string message;
                public double funds;
                public double gTokens;
                public bool updated;
                public ScoreResult() { }
        //public bool success {
        //    get { return this._success; }
        //    set { this._success = value; }
        //}
        //public int score {
        //    get { return this._score; }
        //    set { this._score = value; }
        //}
            }

            [System.Serializable]
            public class TournamentList : System.Object {

                public List<Tournament> tournaments;

                public TournamentList() { }

            }

            public static class TournamentStatus {
                public const string Started = "started";
                public const string Ended = "ended";
                public const string Processed = "processed";
            }

            [System.Serializable]
            public class Tournament : System.Object {

                public string name;
                public double prize;
                public double entryFee;
                public int id;
                public int gameId;
                public string status;
                public bool played;
                public int size;
                public int prize_structure;
				public string endDate;
                public int memberlength;
                public int typeCurrency;
                public Tournament() { }

            }

            [System.Serializable]
            public class TournamentResult : System.Object {

                public bool success;
                public List<Tournament> tournaments;

                public TournamentResult() { }

            }

            [System.Serializable]
            public class Prize : System.Object {
                public string name;
                public int ticket;
                public string description;
                public string image;
            }

            [System.Serializable]
            public class LoginResult : System.Object {

                public bool success;
                public List<Tournament> started;
                public List<Tournament> ended;
                public int userId;
                public int gameId;
                public string gameName;
                public string mail;
                public double funds;
                public string token;
                public string message;
                public string avatar;
                public string userName;
                public string value;
                public string msg;

                public string gToken;
                public int highScoreUser;
                public int ticket;
                public LoginResult() { }

            }

            [System.Serializable]
            public class SessionResult : System.Object {

                public bool success;

                public string message;

                public UserDetail user;
                public GameDetail game;

                public string avatar;
                public string value;
                public string msg;
                public string gToken;
                public int highScoreUser;
                public int ticket;
                public SessionResult() { }

            }


            [System.Serializable]
            public class GameDetail : System.Object {
                public int id;
                public string name;
                public string category;

                public GameDetail() { }
            }

            [System.Serializable]
            public class UserDetailResult : System.Object {

               public bool success;

               public UserDetail user;

               public UserDetailResult() { }

           }

           [System.Serializable]
           public class UserDetail : System.Object {

               public int id;
               public string userName;
               public string email;
               public double funds;

               public string gToken;

               public UserDetail() { }

           }

           [System.Serializable]
           public class GameFeaturedResult : System.Object {

               public bool success;
               public List<Game> games;

               public GameFeaturedResult() { }

           }

           [System.Serializable]
           public class Game : System.Object {

               public int id;
               public string name;
               public string category;
               public string type;
               public string imageUrl;
               public string downloadUrl;
               public string siteId;
               public string status;
               public List<GameLinks> links;

               public Game() { }

           }

           [System.Serializable]
           public class GameLinks : System.Object {

               public string name;
               public string value;


               public GameLinks() { }

           }

           [System.Serializable]
           public class AddGTokesResult : System.Object {

               public bool success;
               public string message;

               public double funds;
               public string gTokens;

           }

           [System.Serializable]
           public class ErrorResult : System.Object {

               public bool success;
               public string message;

           }
        public class UpdateHighScoreAndTokensResult : System.Object {

               public bool success;
               public string message;

               public string gtokens;

           }
    #endregion

       }
