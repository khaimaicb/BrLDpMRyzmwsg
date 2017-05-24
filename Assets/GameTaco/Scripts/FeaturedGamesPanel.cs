using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

namespace GameTaco
{
	public class FeaturedGamesPanel : MonoBehaviour {
		public GameObject featuredGamePreFab;
		public ToggleGroup gamesToggleGroup;
		private bool loadedGames = false;

		// Update is called once per frame
		void Update() {
			if (this.isActiveAndEnabled & !TacoManager.CheckModalsOpen() ) {
				if (Input.GetKeyDown(KeyCode.Return) ) {
					TacoManager.ShowPanel (PanelNames.MyPublicPanel);
				}
			}
		}

		// Use this for initialization
		public void UpdateGames( GameFeaturedResult data) {
			var games = data.games;

			foreach (Game featuredGames in data.games) {
				GameObject currentGame = Instantiate (featuredGamePreFab, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;

				currentGame.transform.parent = gamesToggleGroup.transform;

				Text nameText = currentGame.GetComponentInChildren<Text> ();
				nameText.text = featuredGames.name;

				Transform background = currentGame.transform.Find ("Background");

				Image gameImage = background.GetComponent<Image> ();
				nameText.text = featuredGames.name;

				TacoButton tacoButton = currentGame.GetComponentInChildren<TacoButton> ();

				if (  featuredGames.links.Count > 0 ) {
					// using Name to store the URL - versus adding a new member variable to all buttons
					tacoButton.Name = featuredGames.links[0].value;
				}


				string url = featuredGames.imageUrl;


				print (url);

				StartCoroutine (ApiManager.Instance.WWWImageLoad (url, gameImage));
			}

		}

		public void GetOurGames() {
			if (!loadedGames) {
				TacoManager.OpenMessage (TacoConfig.TacoOurGamesLoadingMessage);

				Action<string> success = (string data) => {
					GameFeaturedResult r = JsonUtility.FromJson<GameFeaturedResult> (data);
					if (r.success) {
						TacoConfig.print (" GetOurGames Success = " + JsonUtility.ToJson (r));

						TacoManager.CloseMessage();
						UpdateGames (r);
					}
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

				string url = "api/game/featured";
				StartCoroutine (ApiManager.Instance.GetWithToken (url, success, fail));
				loadedGames = true;
			}
		}
	}
}
