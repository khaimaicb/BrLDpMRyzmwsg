using UnityEngine;
using System.Collections;

namespace GameTaco {
	public class TacoSetup : MonoBehaviour {
		// point this to your main game script 
		public GameObject MyGameScript;
		private int scoreNow = 0;


		// do you want Taco to start when your game launches?
		public bool StartTacoOnLaunch = true;

		// do you want Taco to start when your game launches?
		public bool DontDestroyTacoOnload = true;

		public static TacoSetup Instance;

		// Use this for initialization
		void Start () {
			Instance = this;

			if (DontDestroyTacoOnload) {

				DontDestroyOnLoad(this.transform.gameObject);
			}
		}
		
		// Update is called once per frame
		void Update () {
			
		}
		public void TacoCloseAllModal() {
			TacoManager.CloseAllModals();
		}
		public void OpenTacoFromGame() {

			TacoManager.OpenTacoFromGame ();
		}
		public void TacoOpenModel(object[] contentModal) {
			scoreNow = int.Parse(contentModal[2].ToString());
			TacoManager.OpenModal(contentModal[0].ToString(), contentModal[1].ToString(), null, contentModal[3].ToString());
		}

		public void TacoEndTournament() {
			TacoPostScoreImmediately(scoreNow);
		}


		public void SetupMyGame() {

		}

		public void StartMyGame() {
			SendMessage ("TacoUnPauseGame");
			SendMessage ("TacoStartTournamentGame");
		}

		public void PauseMyGame() {
			SendMessage ("TacoPauseGame");
		}

		public void UnPauseMyGame() {
			SendMessage ("TacoUnPauseGame");
		}

		public void SendMessage (string message) {
			if (MyGameScript) {
				TacoConfig.print ("Taco Message sent to game :" + message);
				MyGameScript.BroadcastMessage (message);

			} else {
				TacoConfig.print ("TACO TIME to connect your game to our Start Game Broadcast message");
			}
		}

		public void TacoPostScore(int score) {
			if (TacoManager.Target != null && TacoManager.Target.id > 0) {
				GameManager.Instance.PostScore (score, TacoManager.Target);
			}
		}
		public void TacoPostScoreImmediately(int score) {
			if (TacoManager.Target != null && TacoManager.Target.id > 0) {
				GameManager.Instance.PostScoreImmediately (score, TacoManager.Target);
			}
		}
		public void ReturnMenu() {
			SendMessage ("LoadMenu");
		}

		public void checkUserLoginToShowPopup() {
			if(!TacoManager.UserLoggedIn()) {
				SendMessage ("ShowModalRemindSignup");
			}
		}
		
		public void TacoSigninWithTokenFree() {
			var guid = System.Guid.NewGuid().ToString().Substring(0, 1);
			var random = UnityEngine.Random.Range(-999,999);
			var ramdomString = System.DateTime.Now.Day + guid + System.DateTime.Now.Month + random ;
			var scoreToEncrypt = scoreNow + "|" + ramdomString;
			TacoConfig.print(scoreToEncrypt);
			var gToken = TacoManager.EncryptStringToBytes(scoreToEncrypt);
			TacoConfig.print(gToken);
			PlayerPrefs.SetString("gTokenSignUpGift", gToken);
			//Open Register
			TacoManager.ShowPanel(PanelNames.RegisterPanel);

		}
		public void CallMessage(string message) {
			SendMessage (message);
		}
		public void CheckWillGetFreeTokens(object[] contentModal) {
			var currentScore = int.Parse(contentModal[0].ToString());
			if(TacoManager.UserLoggedIn() && currentScore > TacoManager.User.highScoreUser) {
				//Update HighScoreToServer
				GameManager.Instance.UpdateHighScoreAndGtokens (currentScore);
			} else {
				CallMessage("CallGameOverSence");
			}

		}

		public void ChangeHighScoreToLocal() {
			if(TacoManager.UserLoggedIn()) {
				PlayerPrefs.SetInt(TacoConfig.TacoHighScoresType, 0);
				Highscore.Instance.highscore = PlayerPrefs.GetFloat("highscoreArcade");
				Highscore.Instance.UpdateManually();
			}
		}
	/*
	public void PromptToQuitGame( int score )
	{

		TacoManager.Instance.PromptToQuitGame (score);
	}


	public void UnityDidExit( int score )
	{

		TacoManager.Instance.UnityDidExit (score);
	}
	*/

}

}
