using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


namespace GameTaco
{

	public class GameManager : MonoBehaviour {

		#region Singleton 
		private static GameManager mInstance;
		public static GameManager Instance {
			get {
				if (mInstance == null) {
					mInstance = new GameObject().AddComponent<GameManager>();
				}
				return mInstance;
			}
		}
		#endregion

		// Use this for initialization
		void Start () {
	
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		#region Play Game
		public void StartPlay(Tournament target) {

			if (target != null && target.id > 0) {

				TacoManager.OpenMessage(TacoConfig.TacoPlayStarting);
				//StartCoroutine(postStart());

				Action<string> success = (string data) => {
					var r = JsonUtility.FromJson<StartGameResult>(data);
					if (r.success) {

						TacoManager.GameToken = r.token;

						GameManager.Instance.GetHighScore(r.highScore);

						TacoManager.CloseMessage();

						// delegate to your game 
						TacoSetup.Instance.StartMyGame();

						// set the status text 
						TacoManager.StatusText.text = TacoConfig.GamePlayingMessage;

						// close taco to play the game
						TacoManager.CloseTaco();

					} else {
						TacoManager.CloseAllModals();
						TacoManager.OpenModal(TacoConfig.TacoPlayError, r.message);
					}
				};


				Action<string, string> fail = (string data, string error) => {
					var msg = data + ((string.IsNullOrEmpty(error)) ? "" : " : " + error);
					TacoConfig.print("Error starting game - " + msg);

					var r = JsonUtility.FromJson<StartGameResult>(data);

					TacoManager.CloseAllModals();
					TacoManager.OpenModal(TacoConfig.TacoPlayError, r.message);
				};

				StartCoroutine(ApiManager.Instance.StartGame(target.id, TacoManager.User.token, success, fail));

			}
		}

		public void GetHighScore(int score) {
			PlayerPrefs.SetInt(TacoConfig.TacoHighScoresType, 1);
			PlayerPrefs.SetFloat(TacoConfig.TacoHighScoresPerhapsText, score);
			PlayerPrefs.Save();
			Highscore.Instance.highscore = score;
			Highscore.Instance.UpdateManually();
		}

		#endregion


		public void PostScore(int score, Tournament target) {

			TacoConfig.print(" Game Ended with score : " + score);
			
			PlayerPrefs.SetInt(TacoConfig.TacoHighScoresType, 0);
			PlayerPrefs.SetFloat(TacoConfig.TacoHighScoresPerhapsText, 0);
			Highscore.Instance.highscore = PlayerPrefs.GetFloat("highscoreArcade");
			Highscore.Instance.UpdateManually();

			if (target != null &&  TacoManager.UserLoggedIn() ) {
				//StartCoroutine(endGame(score));

				Action<string> success = (string data) => {

					ScoreResult r = JsonUtility.FromJson<ScoreResult>(data);
					TacoConfig.print("Result complete = " + r.success + " - " + r.score + " - " + r.message);
					string modalBody = string.Empty;
					//If user play again so token can't verify
					if(r.success) {
						modalBody= TacoConfig.TacoPlayEndedModalBody;

						modalBody = modalBody.Replace ("&gameEndScore", r.score.ToString());
					}
					else {
						modalBody = TacoConfig.TacoPlayEndedAgainModalBody;
					}

					TacoManager.CloseAllModals();
					TacoManager.OpenModal(TacoConfig.TacoPlayEndedModalHeader,modalBody,null, ModalFunctions.TournamentGamePosted );

					// set the status text 
					TacoManager.StatusText.text = "";

					// game is over : release token
					TacoManager.GameToken = null;
					if(r.updated) {
						TacoManager.UpdateFundsWithToken(r.funds, r.gTokens.ToString());
					}
				};

				Action<string, string> fail = (string data, string error) => {
					var msg = data + ((string.IsNullOrEmpty(error)) ? "" : " : " + error);
					TacoConfig.print("Error starting game - " + msg);

					var r = JsonUtility.FromJson<StartGameResult>(data);

					TacoManager.CloseAllModals();
					TacoManager.OpenModal(TacoConfig.TacoPlayError, r.message);
				};

				StartCoroutine(ApiManager.Instance.EndGame(score, target.id, target.gameId, TacoManager.GameToken, TacoManager.User.token, success, fail));
			}


		}
		
		public void PostScoreImmediately(int score, Tournament target) {

			TacoConfig.print(" Game Ended with score : " + score);

			PlayerPrefs.SetInt(TacoConfig.TacoHighScoresType, 0);
			PlayerPrefs.SetFloat(TacoConfig.TacoHighScoresPerhapsText, 0);
			Highscore.Instance.highscore = PlayerPrefs.GetFloat("highscoreArcade");
			Highscore.Instance.UpdateManually();

			if (target != null &&  TacoManager.UserLoggedIn() ) {
				//StartCoroutine(endGame(score));

				Action<string> success = (string data) => {

					ScoreResult r = JsonUtility.FromJson<ScoreResult>(data);
					TacoConfig.print("Result complete = " + r.success + " - " + r.score + " - " + r.message);
					string modalBody = string.Empty;
					//If user play again so token can't verify
					if(r.success) {
						modalBody= TacoConfig.TacoPlayEndedModalBody;

						modalBody = modalBody.Replace ("&gameEndScore", r.score.ToString());
					}
					else {
						modalBody = TacoConfig.TacoPlayEndedAgainModalBody;
					}

					TacoManager.CloseAllModals();
					TacoManager.OpenModal(TacoConfig.TacoPlayEndedModalHeader,modalBody,null, ModalFunctions.ReturnToMenu );

					// set the status text 
					TacoManager.StatusText.text = "";

					// game is over : release token
					TacoManager.GameToken = null;
					if(r.updated) {
						TacoManager.UpdateFundsWithToken(r.funds, r.gTokens.ToString());
					}
				};

				Action<string, string> fail = (string data, string error) => {
					var msg = data + ((string.IsNullOrEmpty(error)) ? "" : " : " + error);
					TacoConfig.print("Error starting game - " + msg);

					var r = JsonUtility.FromJson<StartGameResult>(data);

					TacoManager.CloseAllModals();
					TacoManager.OpenModal(TacoConfig.TacoPlayError, r.message);
				};

				StartCoroutine(ApiManager.Instance.EndGame(score, target.id, target.gameId, TacoManager.GameToken, TacoManager.User.token, success, fail));
			}


		}
		public void UpdateHighScoreAndGtokens(int newHighScore) {
			TacoConfig.print(newHighScore);
			TacoManager.OpenMessage("Sending High Score");
			Action<string> success = (string data) => {
				var r = JsonUtility.FromJson<UpdateHighScoreAndTokensResult>(data);
				if(r.success) {
					TacoConfig.print("Update highscore for user success" + r);
					TacoManager.UpdateTokenOnly(r.gtokens);
					TacoSetup.Instance.CallMessage("ShowModalEarningTokens");
				}
			};

			Action<string, string> fail = (string data, string error) => {
				TacoManager.CloseAllModals();
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
				
				TacoConfig.print(msg);

			};

			StartCoroutine(ApiManager.Instance.UpdateHighScoreAndGTokens(newHighScore, TacoManager.User.token,success, fail));
		}

		/*
		public void ForfeitTournamentGame() {

			TacoConfig.print(" Forfeiting game");

			if (Target != null && CurrentUserId != 0) {
				

				Action<string> success = (string data) => {

					ScoreResult r = JsonUtility.FromJson<ScoreResult>(data);
					TacoConfig.print("Result complete = " + r.success + " - " + r.score + " - " + r.message);

					string modalBody = TacoConfig.TacoPlayEndedModalBody;

					modalBody = modalBody.Replace ("&gameEndScore", r.score.ToString());

					CloseAllModals();
					OpenModal(TacoConfig.TacoPlayEndedModalHeader,modalBody,null, "TournamentGamePosted" );

				};

				Action<string, string> fail = (string data, string error) => {
					var msg = data + ((string.IsNullOrEmpty(error)) ? "" : " : " + error);
					TacoConfig.print("Error starting game - " + msg);

					var r = JsonUtility.FromJson<StartGameResult>(data);

					CloseAllModals();
					OpenModal(TacoConfig.TacoPlayError, r.message);
				};


				StartCoroutine(TacoApiManager.Instance.ForfeitTournamentGame(0, Target.id, Target.gameId, gameToken, User.token, success, fail));

			}

		}
		*/

	}


}