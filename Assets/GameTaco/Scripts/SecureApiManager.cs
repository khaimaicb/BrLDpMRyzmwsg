using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace GameTaco {

    public class SecureApiManager : MonoBehaviour {

        #region Singleton 
        private static SecureApiManager mInstance;
        public static SecureApiManager Instance {
            get {
                if (mInstance == null) {
                    mInstance = new GameObject().AddComponent<SecureApiManager>();
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

        #region Authentication
        public void Login(string email, string password, Action<string> onSuccess, Action<string, string> onFail = null) {
            string postData = String.Format("email={0}&password={1}&siteId={2}", email, password, TacoConfig.SiteId);
            var data = System.Text.Encoding.ASCII.GetBytes(postData);

            post(Constants.BaseUrl + "api/login", data, onSuccess, onFail);
        }

        public void Register(string userName, string email, string password, bool ageCheck, Action<string> onSuccess, Action<string, string> onFail = null) {
            string postData = String.Format("userName={0}&email={1}&password={2}&confirmPassword={3}&ageCheck={4]&siteId={5}", userName, email, password, password, ageCheck.ToString().ToLower(), TacoConfig.SiteId);
            var data = System.Text.Encoding.ASCII.GetBytes(postData);

            post(Constants.BaseUrl + "api/register", data, onSuccess, onFail);
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
                //if (onFail != null) { onFail(www.downloadHandler.text, www.error); }
                TacoConfig.print("GetStripeToken Failed - " + www.downloadHandler.text);
            } else {
                TacoConfig.print("GetStripeToken success - " + www.downloadHandler.text);
                var r = JsonUtility.FromJson<StripeTokenResult>(www.downloadHandler.text);
                TacoConfig.print("Token = " + r.id);

                chargeUser(r.id, amount, onSuccess, onFail);
            }
        }

        //NOTE: Add funds first calls stripe to get the proper token, this calls our backend to actually charge the user
        private void chargeUser(string token, int amount, Action<string> onSuccess, Action<string, string> onFail = null) {
            string postData = String.Format("token={0}&amount={1}", token, amount);
            var data = System.Text.Encoding.ASCII.GetBytes(postData);

            post(Constants.BaseUrl + "api/funds/stripe", data, onSuccess, onFail);
        }


        public void WithdrawFunds(string amount, string name, string address1, string address2, string city, string zip, string state, Action<string> onSuccess, Action<string, string> onFail = null) {
            string postData = String.Format("amount={0}&name={1}&address1={2}&address2={3}&city={4}&state={5}&zip={6}", amount, name, address1, address2, city, state, zip);
            var data = System.Text.Encoding.ASCII.GetBytes(postData);

            post(Constants.BaseUrl + "api/funds/withdraw", data, onSuccess, onFail);
        }
        #endregion

        #region Game
        public void StartGame(int tournamentId, Action<string> onSuccess, Action<string, string> onFail = null) {
            string postData = String.Format("siteId={0}&tournamentId={1}", TacoConfig.SiteId, tournamentId);
            var data = System.Text.Encoding.ASCII.GetBytes(postData);

            post(Constants.BaseUrl + "api/game/start", data, onSuccess, onFail);
        }

        public void EndGame(int score, int tournamentId, int gameId, string gameToken, Action<string> onSuccess, Action<string, string> onFail = null) {
            string postData = String.Format("token={0}&tournamentId={1}&gameId={2}&score={3}", gameToken, tournamentId, gameId, score);
            var data = System.Text.Encoding.ASCII.GetBytes(postData);

            post(Constants.BaseUrl + "api/game/end", data, onSuccess, onFail);
        }


        #endregion

        #region Tournament
        public void CreateTournament(string name, string fee, int players, string opponents, int gameId, Action<string> onSuccess, Action<string, string> onFail = null) {
            string postData = String.Format("name={0}&fee={1}&size={2}&gameId={3}&siteId={4}&opponents={5}", name, fee, players, gameId, TacoConfig.SiteId, opponents);
            var data = System.Text.Encoding.ASCII.GetBytes(postData);

            post(Constants.BaseUrl + "api/tournament", data, onSuccess, onFail);
        }

        public void JoinTournament(int tournamentId, string siteId, int gameId, Action<string> onSuccess, Action<string, string> onFail = null) {
            string postData = String.Format("tournamentId={0}&siteId={1}&gameId={2}", tournamentId, TacoConfig.SiteId, gameId);
            var data = System.Text.Encoding.ASCII.GetBytes(postData);

            post(Constants.BaseUrl + "api/tournament/join", data, onSuccess, onFail);
        }
        #endregion

        #region Generic Api Calls

        public void Get(string url, Action<string> onSuccess, Action<string, string> onFail = null) {
            var client = new X509Certificate2("Assets/GameTaco/Security/dev.baysidegames.com.crt");
            //var client = new X509Certificate2("Assets/GameTaco/Security/pokedb.io.crt");

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidationCallback);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            //TODO: Update token to a property
            if (TacoManager.User != null && !string.IsNullOrEmpty(TacoManager.User.token)) {
                request.Headers.Add("x-access-token", TacoManager.User.token);
            }

            request.AuthenticationLevel = AuthenticationLevel.MutualAuthRequired;
            request.ClientCertificates.Add(client);

            var instance = UnityMainThreadDispatcher.Instance;
            request.BeginGetResponse(new AsyncCallback((result) => {

                HttpWebResponse asyncResponse = (result.AsyncState as HttpWebRequest).EndGetResponse(result) as HttpWebResponse;
                print("response received - " + asyncResponse.StatusCode);

                if (asyncResponse.StatusCode != HttpStatusCode.OK) {
                    if (onFail != null) {
                        var body = new System.IO.StreamReader(asyncResponse.GetResponseStream()).ReadToEnd();
                        instance.Enqueue(() => onFail(body, null));
                    }
                } else {
                    var body = new System.IO.StreamReader(asyncResponse.GetResponseStream()).ReadToEnd();
                    instance.Enqueue(() => onSuccess(body));
                }
            }), request);
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


                } else {

                    //TacoConfig.print("Error = " + r.message );

                    TacoManager.CloseMessage();
                    TacoManager.OpenModal(TacoConfig.TacoLoginErrorHeader, TacoConfig.TacoLoginErrorMessage01);
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

            this.Get(url, success, fail);
        }

        #endregion

        #region Helper Functions
        private void post(string url, byte[] data, Action<string> onSuccess, Action<string, string> onFail = null) {
            var client = new X509Certificate2("Assets/GameTaco/Security/dev.baysidegames.com.crt");
            //var client = new X509Certificate2("Assets/GameTaco/Security/pokedb.io.crt");

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidationCallback);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            //TODO: Update token to a property
            if (TacoManager.User != null && !string.IsNullOrEmpty(TacoManager.User.token)) {
                request.Headers.Add("x-access-token", TacoManager.User.token);
            }

            using (var stream = request.GetRequestStream()) {
                stream.Write(data, 0, data.Length);
            }

            request.AuthenticationLevel = AuthenticationLevel.MutualAuthRequired;
            request.ClientCertificates.Add(client);

            var instance = UnityMainThreadDispatcher.Instance;
            request.BeginGetResponse(new AsyncCallback((result) => {
                HttpWebResponse asyncResponse = (result.AsyncState as HttpWebRequest).EndGetResponse(result) as HttpWebResponse;
                print("response received - " + asyncResponse.StatusCode);
                if (asyncResponse.StatusCode != HttpStatusCode.OK) {
                    if (onFail != null) {
                        var body = new System.IO.StreamReader(asyncResponse.GetResponseStream()).ReadToEnd();
                        instance.Enqueue(() => onFail(body, null));
                    }
                } else {
                    var body = new System.IO.StreamReader(asyncResponse.GetResponseStream()).ReadToEnd();
                    instance.Enqueue(() => onSuccess(body));
                }
            }), request);
        }

        #endregion

        #region Certificate Validation Callback
        public static bool ValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors) {
            //SEE http://stackoverflow.com/questions/24221974/force-httpclient-to-trust-single-certificate

            //var client = new X509Certificate2("Assets/GameTaco/Security/pokedb.io.crt");
            var client = new X509Certificate2("Assets/GameTaco/Security/dev.baysidegames.com.crt");
            var clientPublicKey = client.GetPublicKeyString();

            var serverPublicKey = certificate.GetPublicKeyString();
            print("Validation callback " + clientPublicKey == serverPublicKey);

            return clientPublicKey == serverPublicKey;
        }
        #endregion

    }
}