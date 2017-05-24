using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using UnityEngine.Networking;

namespace GameTaco
{
    public class ApiManager : MonoBehaviour {
        #region Singleton
        private static ApiManager mInstance;
        public static ApiManager Instance {
            get {
                if (mInstance == null) {
                    mInstance = new GameObject().AddComponent<ApiManager>();
                }
                return mInstance;
            }
        }
        #endregion

        #region MonoBehaviour functions
        // Use this for initialization
        void Start() {
        }

        // Update is called once per frame
        void Update() {
        }
        #endregion

        #region Generic Api Calls
        public IEnumerator GetWithToken(string url, Action<string> onSuccess, Action<string, string> onFail = null) {
            UnityWebRequest www = UnityWebRequest.Get(Constants.BaseUrl + url);

            string currentToken = null;

            if (TacoManager.User == null) {
                currentToken = TacoManager.GetPreferenceString (UserPreferences.userToken);
            } else {
                currentToken =  TacoManager.User.token;
            }

            www.SetRequestHeader("x-access-token", currentToken);

            yield return www.Send();

            TacoConfig.print (Constants.BaseUrl + url);
            TacoConfig.print("www = " + www.downloadHandler.text);

            if (www.isError || www.responseCode == 500) {
                TacoConfig.print (  "www.isError =" + www.error);

                if (onFail != null) {
                    onFail(www.downloadHandler.text, www.error);
                }
            } else {
                onSuccess(www.downloadHandler.text);
            }
        }

        public void GetSession(string token) {
            TacoManager.OpenMessage(TacoConfig.TacoLoginStatusMessage00);

            TacoConfig.print("Attempting auto login with token = " + token);

            Action<string> success = (string data) => {
                TacoConfig.print("data= " + data);
                SessionResult r = JsonUtility.FromJson<SessionResult>(data);
                if (r.success) {

                    TacoConfig.print("Success = " + r);

                    TacoManager.OpenMessage(TacoConfig.TacoLoginStatusMessage01);
                    TacoManager.UpdateUser(r, token);
                     if(r.msg == "in"){
                        TacoManager.OpenModal("Login success","Welcome back to game taco! ");
                    }else{
                        TacoManager.OpenModal("Login success","You get "+r.value+" taco token for login today! ");
                    }
                } else {
                    //TacoConfig.print("Error = " + r.message );

                    TacoManager.CloseMessage();
                    
                    TacoManager.SetPreferenceString (UserPreferences.userToken, null);
                    TacoManager.SetPreference(UserPreferences.autoLogin , 0);
                    TacoManager.ShowPanel(PanelNames.LoginPanel);

                    if(!string.IsNullOrEmpty(r.message)) {
                        TacoManager.OpenModal( TacoConfig.TacoLoginErrorHeader , r.message );
                    }
                }
            };

            Action<string, string> fail = (string data, string error) => {
                TacoConfig.print("Error on Login : " + data);

                if (!string.IsNullOrEmpty(error)) {
                    TacoConfig.print("Error : " + error);
                }

                TacoManager.CloseMessage();

                string msg = data + (string.IsNullOrEmpty(error) ? "" : " : " + error);

                // TODO : have the message 'Cannot Reach Destination Host' - read 'No Internet Connection'  -- something more clear?
                TacoManager.OpenModal(TacoConfig.TacoLoginErrorHeader, TacoConfig.TacoLoginErrorMessage01 + "\n\n" + msg);
            };
            string url = "api/session/detail/" + TacoConfig.SiteId;
            StartCoroutine(ApiManager.Instance.GetWithToken(url, success, fail));
        }


        #endregion

        #region Authentication
        public IEnumerator Login(string email, string password, Action<string> onSuccess, Action<string, string> onFail = null) {
            WWWForm form = new WWWForm();

            form.AddField("email", email);
            form.AddField("password", password);
            form.AddField("siteId", TacoConfig.SiteId);

            TacoConfig.print ("login form= " + form);

            UnityWebRequest www = UnityWebRequest.Post(Constants.BaseUrl + "api/login", form);
            yield return www.Send();

            if (www.isError || www.responseCode == 500) {
                TacoConfig.print ("login www.responseCode = " + www.responseCode);

                if (onFail != null) { onFail(www.downloadHandler.text, www.error); }
            } else {
                TacoConfig.print ("login www.downloadHandler.text = " + www.downloadHandler.text);

                onSuccess(www.downloadHandler.text);
            }
        }

        public IEnumerator LoginWithFacebook(Action<string> onSuccess, Action<string, string> onFail = null) {

            UnityWebRequest www = UnityWebRequest.Get(Constants.BaseUrl + "/auth/facebook");
            yield return www.Send();

            if (www.isError || www.responseCode == 500) {
                TacoConfig.print ("login www.responseCode = " + www.responseCode);

                if (onFail != null) { onFail(www.downloadHandler.text, www.error); }
            } else {
                TacoConfig.print ("login www.downloadHandler.text = " + www.downloadHandler.text);

                onSuccess(www.downloadHandler.text);
            }
        }

        //public void LoginByToken( string token) {

        //  TacoManager.Instance.OpenMessage ( TacoConfig.TacoLoginStatusMessage00);

        //  TacoConfig.print("Attempting auto login with token = " + token);

        //  Action<string> success = (string data) => {

        //          TacoConfig.print("data= " + data);

        //      UserDetailResult r = JsonUtility.FromJson<UserDetailResult>(data);

        //      if (r.success) {

        //          TacoConfig.print("Success = " + r);

        //          TacoManager.Instance.OpenMessage ( TacoConfig.TacoLoginStatusMessage01);
        //          TacoManager.Instance.UpdateUser(r, token);


        //      } else {

        //          //TacoConfig.print("Error = " + r.message );

        //          TacoManager.Instance.CloseMessage();
        //          TacoManager.Instance.OpenModal ( TacoConfig.TacoLoginErrorHeader , TacoConfig.TacoLoginErrorMessage01  );
        //      }
        //  };

        //  Action<string, string> fail = (string data, string error) => {

        //      TacoConfig.print("Error on Login : " + data);

        //      if (!string.IsNullOrEmpty(error)) {
        //          TacoConfig.print("Error : " + error);
        //      }

        //      TacoManager.Instance.CloseMessage();

        //      string msg = data + (string.IsNullOrEmpty(error) ? "" : " : "  + error);

        //          // TODO : have the message 'Cannot Reach Destination Host' - read 'No Internet Connection'  -- something more clear?
        //      TacoManager.Instance.OpenModal(TacoConfig.TacoLoginErrorHeader, TacoConfig.TacoLoginErrorMessage01 + "\n\n" + msg );

        //  };

        //  string url = "/api/user/detail";

        //  StartCoroutine(ApiManager.Instance.GetWithToken(url, success, fail));


        //}

        public IEnumerator Register(string userName, string email, string password, bool ageCheck, Action<string> onSuccess, Action<string, string> onFail = null) {
            WWWForm form = new WWWForm();

            form.AddField("userName", userName);
            form.AddField("email", email.ToLower());
            form.AddField("password", password);
            form.AddField("confirmPassword", password);
            form.AddField("ageCheck", ageCheck.ToString().ToLower());
            form.AddField("siteId", TacoConfig.SiteId);

            var gtoken = PlayerPrefs.GetString("gTokenSignUpGift","");
            form.AddField("gtoken", gtoken);
            TacoConfig.print(gtoken);
            UnityWebRequest www = UnityWebRequest.Post(Constants.BaseUrl + "api/register", form);
            yield return www.Send();

            if (www.isError || www.responseCode == 500) {
                if (onFail != null) { onFail(www.downloadHandler.text, www.error); }
            } else {
                onSuccess(www.downloadHandler.text);
            }
        }
        #endregion

        #region Funds
        public IEnumerator AddFunds(string number, string month, string year, string cvc, int amount, Action<string> onSuccess, Action<string, string> onFail = null) {
            WWWForm form = new WWWForm();

            form.AddField("card[number]", number);
            form.AddField("card[exp_month]", month);
            form.AddField("card[exp_year]", year);
            form.AddField("card[cvc]", cvc);

            UnityWebRequest www = UnityWebRequest.Post(Constants.StripeTokenUrl, form);
            www.SetRequestHeader("Authorization", "Bearer " + Constants.StripePublicKey);

            yield return www.Send();

            if (www.isError || www.responseCode == 500) {
				// Check for HTTP failure
                if (onFail != null) { 
					onFail(www.downloadHandler.text, www.error); 
				}
                TacoConfig.print("GetStripeToken Failed - " + www.downloadHandler.text);
            } else {
				// Check for response for success / failure
				var r = JsonUtility.FromJson<StripeTokenResult>(www.downloadHandler.text);

				if (r.error.message != null) {
					// API returned with error
					Debug.LogWarning("Stripe error: " + r.error.message);
					if (onFail != null) { 
						onFail(r.error.message, null); 
					}

				} else {
					// Success
					TacoConfig.print ("GetStripeToken success - " + www.downloadHandler.text);
					TacoConfig.print ("Token = " + r.id);

					StartCoroutine (chargeUser (r.id, amount, onSuccess, onFail));
				}
            }
        }


        public IEnumerator AddGTokens(int fee, int gTokens, Action<string> onSuccess, Action<string, string> onFail = null) {
            WWWForm form = new WWWForm();

            form.AddField("fee", fee);
            form.AddField("amount", gTokens.ToString());

            UnityWebRequest www = UnityWebRequest.Post(Constants.BaseUrl + "api/tokens", form);
            www.SetRequestHeader("x-access-token", TacoManager.User.token);
            yield return www.Send();

            if (www.isError || www.responseCode == 500) {
                if (onFail != null) { onFail(www.downloadHandler.text, www.error); }
            } else {
                onSuccess(www.downloadHandler.text);
            }
        }

        public IEnumerator UpdateHighScoreAndGTokens(int score, string userToken ,Action<string> onSuccess, Action<string, string> onFail = null) {
            WWWForm form = new WWWForm();

            form.AddField("newHighScore", score);
            form.AddField("siteId", TacoConfig.SiteId);

            UnityWebRequest www = UnityWebRequest.Post(Constants.BaseUrl + "api/updatehighscoreandtokens", form);
            www.SetRequestHeader("x-access-token", userToken);
            yield return www.Send();

            if (www.isError || www.responseCode == 500) {
                if (onFail != null) { onFail(www.downloadHandler.text, www.error); }
            } else {
                onSuccess(www.downloadHandler.text);
            }
        }

        //NOTE: Add funds first calls stripe to get the proper token, this calls our backend to actually charge the user
        private IEnumerator chargeUser(string token, int amount, Action<string> onSuccess, Action<string, string> onFail = null) {
            WWWForm form = new WWWForm();
            form.AddField("token", token);
            form.AddField("amount", amount);

            UnityWebRequest www = UnityWebRequest.Post(Constants.BaseUrl + "api/funds/stripe", form);
            www.SetRequestHeader("x-access-token", TacoManager.User.token);
            //www.SetRequestHeader("Authorization", "Bearer " + stripeToken );
            yield return www.Send();

            if (www.isError || www.responseCode == 500) {
                if (onFail != null) { onFail(www.downloadHandler.text, www.error); }
            } else {
                onSuccess(www.downloadHandler.text);
            }
        }

        public IEnumerator WithdrawFunds(string amount, string name, string address1, string address2, string city, string zip, string state, Action<string> onSuccess, Action<string, string> onFail = null) {
            WWWForm form = new WWWForm();
            form.AddField("amount", amount);
            form.AddField("name", name);
            form.AddField("address1", address1);
            form.AddField("address2", address2);
            form.AddField("city", city);
            form.AddField("state", state);
            form.AddField("zip", zip);

            UnityWebRequest www = UnityWebRequest.Post(Constants.BaseUrl + "api/funds/withdraw", form);
            www.SetRequestHeader("x-access-token", TacoManager.User.token);

            yield return www.Send();

            if (www.isError || www.responseCode == 500) {
                if (onFail != null) { onFail(www.downloadHandler.text, www.error); }
            } else {
                onSuccess(www.downloadHandler.text);
            }
        }
        #endregion

        #region Game
        public IEnumerator StartGame(int tournamentId, string userToken, Action<string> onSuccess, Action<string, string> onFail = null) {
            WWWForm form = new WWWForm();

            form.AddField("siteId", TacoConfig.SiteId);
            form.AddField("tournamentId", tournamentId);

            UnityWebRequest www = UnityWebRequest.Post(Constants.BaseUrl + "api/game/start", form);
            www.SetRequestHeader("x-access-token", userToken);
            yield return www.Send();

            if (www.isError || www.responseCode == 500) {
                if (onFail != null) { onFail(www.downloadHandler.text, www.error); }
            } else {
                onSuccess(www.downloadHandler.text);
            }
        }

        /*
        public IEnumerator ForfeitTournamentGame(int score, int tournamentId, int gameId, string gameToken, string userToken, Action<string> onSuccess, Action<string, string> onFail = null) {
            WWWForm form = new WWWForm();

            form.AddField("token", gameToken);
            form.AddField("tournamentId", tournamentId);
            form.AddField("gameId", gameId);
            form.AddField("score", score);

            UnityWebRequest www = UnityWebRequest.Post(Constants.BaseUrl + "api/game/quit", form);
            www.SetRequestHeader("x-access-token", userToken);

            TacoManager.Instance.OpenMessage (TacoConfig.TacoPlayEndedMessage);

            yield return www.Send();

            if (www.isError) {
                if (onFail != null) { onFail(www.downloadHandler.text, www.error); }
            } else {
                onSuccess(www.downloadHandler.text);
            }
        }

        */

        public IEnumerator EndGame(int score, int tournamentId, int gameId, string gameToken, string userToken, Action<string> onSuccess, Action<string, string> onFail = null) {
            
            WWWForm form = new WWWForm();
            
            form.AddField("token", string.IsNullOrEmpty(gameToken) ? string.Empty : gameToken);
            form.AddField("tournamentId", tournamentId);
            form.AddField("gameId", gameId);
            form.AddField("score", score);

            UnityWebRequest www = UnityWebRequest.Post(Constants.BaseUrl + "api/game/end", form);
            www.SetRequestHeader("x-access-token", userToken);

            string message = string.IsNullOrEmpty(gameToken) ? TacoConfig.TacoPlayAgainEndedMessage : TacoConfig.TacoPlayEndedMessage;
            TacoManager.OpenMessage (message);

            yield return www.Send();

            if (www.isError) {
                if (onFail != null) { onFail(www.downloadHandler.text, www.error); }
            } else {
                onSuccess(www.downloadHandler.text);
            }
        }

        #endregion

        #region Tournament
        public IEnumerator CreateTournament( int timeRemaining, int typeCurrency, int prizeStruc, string invited, string name, string fee, int players, string opponents, int gameId, string userToken, Action<string> onSuccess, Action<string, string> onFail = null) {
            WWWForm form = new WWWForm();
            form.AddField("prize", prizeStruc);
            form.AddField("name", name);
            form.AddField("fee", fee);
            form.AddField("size", players);
            form.AddField("gameId", gameId);
            form.AddField("siteId", TacoConfig.SiteId);
            form.AddField("opponents", opponents);
            form.AddField("timeRemaining", timeRemaining);
            if(typeCurrency == 0) {
                form.AddField("typeCurrency", "real");
            }
            else {
                form.AddField("typeCurrency", "gToken");
            }
            TacoConfig.print("invited friends = "+ invited);

            form.AddField("invites", invited);

            TacoConfig.print("Form Post : Fee =" + fee + " players = " + players + " name =" + name+ " prizrSt =" + prizeStruc);

            TacoConfig.print("Create Tournament complete - " + fee);

            UnityWebRequest www = UnityWebRequest.Post(Constants.BaseUrl + "api/tournament", form);
            www.SetRequestHeader("x-access-token", userToken);
            yield return www.Send();

            if (www.isError || www.responseCode == 500) {
                if (onFail != null) { onFail(www.downloadHandler.text, www.error); }
            } else {
                onSuccess(www.downloadHandler.text);
            }
        }

        public IEnumerator JoinTournament(int type, int tournamentId, string siteId, int gameId, string userToken, Action<string> onSuccess, Action<string, string> onFail = null) {
            WWWForm form = new WWWForm();
            form.AddField("type", type);
            form.AddField("tournamentId", tournamentId);
            form.AddField("siteId", siteId);
            form.AddField("gameId", gameId);

            UnityWebRequest www = UnityWebRequest.Post(Constants.BaseUrl + "api/tournament/join", form);
            www.SetRequestHeader("x-access-token", userToken);
            yield return www.Send();

            if (www.isError || www.responseCode == 500) {
                if (onFail != null) { onFail(www.downloadHandler.text, www.error); }
            } else {
                onSuccess(www.downloadHandler.text);
            }
        }


		public IEnumerator InviteFriend(string fromEmail, string baseUrl, string emails, int tournamentId, Action<string> onSuccess, Action<string, string> onFail = null) {
			WWWForm form = new WWWForm();

			string currentToken = null;

			if (TacoManager.User == null) {
				currentToken = TacoManager.GetPreferenceString (UserPreferences.userToken);
			} else {
				currentToken =  TacoManager.User.token;
			}

            form.AddField("fromUserId",TacoManager.User.userId);
			form.AddField("fromEmail", fromEmail);
			form.AddField("baseUrl", baseUrl);
			form.AddField("emails", emails);
			form.AddField("tournamentId", tournamentId);

			UnityWebRequest www = UnityWebRequest.Post(Constants.BaseUrl + "api/friends/invite/tournament", form);
			www.SetRequestHeader("x-access-token", currentToken);

			yield return www.Send();

			if (www.isError || www.responseCode == 500) {
				if (onFail != null) { onFail(www.downloadHandler.text, www.error); }
			} else {
				onSuccess(www.downloadHandler.text);
			}
		}

        #endregion

        #region Cert pin tests
        public void CertTest() {
            //var client = new X509Certificate2("Assets/GameTaco/dev.baysidegames.com.crt");
            var client = new X509Certificate2("Assets/GameTaco/pokedb.io.crt");

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidationCallback);
            print("callback set in cert test");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Constants.BaseUrl + "api/certtest");

            print("client public key string = " + client.GetPublicKeyString());

            request.Headers["x-access-token"] = TacoManager.User.token;
            request.Headers.Add("x-access-token", TacoManager.User.token);
            request.AuthenticationLevel = AuthenticationLevel.MutualAuthRequired;
            request.ClientCertificates.Add(client);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            print("response received - " + response.StatusCode);
        }

        public void CertLogin(string email, string password, Action<string> onSuccess) {
            var client = new X509Certificate2("Assets/GameTaco/dev.baysidegames.com.crt");
            //var client = new X509Certificate2("Assets/GameTaco/pokedb.io.crt");

            var postData = "email=" + email;
            postData += "&password=" + password;
            postData += "&siteId=" + TacoConfig.SiteId;
            var data = System.Text.Encoding.ASCII.GetBytes(postData);

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidationCallback);
            print("callback set in cert login");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Constants.BaseUrl + "api/login");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream()) {
                stream.Write(data, 0, data.Length);
            }

            //request.Headers.Add("x-access-token", TacoManager.Instance.User.token);
            request.AuthenticationLevel = AuthenticationLevel.MutualAuthRequired;
            request.ClientCertificates.Add(client);

            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //request.BeginGetResponse(testCallback, request);
            var instance = UnityMainThreadDispatcher.Instance;
            request.BeginGetResponse(new AsyncCallback((result) => {

                HttpWebResponse asyncResponse = (result.AsyncState as HttpWebRequest).EndGetResponse(result) as HttpWebResponse;
                print("response received - " + asyncResponse.StatusCode);
                var body = new System.IO.StreamReader(asyncResponse.GetResponseStream()).ReadToEnd();

                instance.Enqueue(() => onSuccess(body));
            }), request);
        }

        public static bool ValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors) {
            //SEE http://stackoverflow.com/questions/24221974/force-httpclient-to-trust-single-certificate

            //var client = new X509Certificate2("Assets/GameTaco/pokedb.io.crt");
            var client = new X509Certificate2("Assets/GameTaco/dev.baysidegames.com.crt");
            var clientPublicKey = client.GetPublicKeyString();

            var serverPublicKey = certificate.GetPublicKeyString();
            print("Validation callback " + clientPublicKey == serverPublicKey);

            return clientPublicKey == serverPublicKey;
            //if (policyErrors != SslPolicyErrors.None) {
            //    print("NO POLICY ERRORS FOUND");
            //    return true;
            //}
            //print("POLICY ERRORS FOUND");
            //return false;

        }
        #endregion


        #region Load Image From WWW
        public IEnumerator WWWImageLoad( string url, Image imageToReplace) {

            string imageURL = Constants.BaseUrl + url;

            WWW www = new WWW(imageURL  );

            yield return www;

            imageToReplace.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));

            www.Dispose();
            www = null;

        }

        public IEnumerator WWWAvatarSocial( string url, Image imageToReplace) {

            WWW www = new WWW(url );
            yield return www;
            
            imageToReplace.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));

            imageToReplace.rectTransform.sizeDelta = new Vector2(120, 125);
            imageToReplace.GetComponent<RectTransform>().localPosition = new Vector3(90,0,0);

            www.Dispose();
            www = null;

        }
        #endregion
    }
}
