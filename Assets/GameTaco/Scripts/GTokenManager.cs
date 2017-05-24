using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.Networking;

namespace GameTaco
{
	public class GTokenManager : MonoBehaviour {
		public static GTokenManager Instance = null;
		public GameObject AddGTokensText = null;
        public GameObject AddGTokensButton = null;

		// Use this for initialization
		void Start () {
            AddGTokensText.GetComponent<Text>().text = "";
		}
		
		/// <summary>
		/// Awake is called when the script instance is being loaded.
		/// </summary>
		void Awake()
		{
			Instance = this;
            AddGTokensButton = GameObject.Find("AddGTokensButton");
			AddGTokensText = GameObject.Find("AddGTokensText");
            AddGTokensText.GetComponent<Text>().text = "";
		}
		void destroy() {

		}
		public void convertFundsToGTokens(int fee) {
        
            AddGTokensText.GetComponent<Text>().text = TacoConfig.AddGTokensAdding;
            AddGTokensButton.GetComponent<Button>().interactable = false;

            Action<string> success = (string data) => {
                TacoConfig.print("Purchase GTokens complete - " + data);
                var r = JsonUtility.FromJson<AddGTokesResult>(data);
                if (r.success) {
                    TacoManager.UpdateFundsWithToken(r.funds, r.gTokens);
                   	string returnMessage = TacoConfig.AddFundsSuccessMessage + TacoManager.FormatGTokens(double.Parse(r.gTokens));

                    AddGTokensText.GetComponent<Text>().text = "";
                    
                    // show a dialog to indicate success : then return to tournaments
                    TacoManager.OpenModal( TacoConfig.AddGTokensTitleSuccessMessage ,returnMessage, null, ModalFunctions.ReturnToTournaments,null,null, false );
                    
                } else {
                AddGTokensText.GetComponent<Text>().text = TacoConfig.Error +" : " + r.message;
            }
            AddGTokensButton.GetComponent<Button>().interactable = true;
        };

        Action<string, string> fail = (string data, string error) => {
 
            if (!string.IsNullOrEmpty(error)) {
                TacoConfig.print("Error : " + error);
            }
            TacoManager.CloseMessage();
            string msg = data + (string.IsNullOrEmpty(error) ? "" : " : " + error);
            TacoConfig.print("Error adding gTokens : " + msg);
            if(!string.IsNullOrEmpty(data)) {
                ErrorResult errorResult = JsonUtility.FromJson<ErrorResult>(data);
                if(!errorResult.success) {
                    msg = errorResult.message;
                }
            }
            AddGTokensText.GetComponent<Text>().text = TacoConfig.Error +" : " + msg;
            AddGTokensButton.GetComponent<Button>().interactable = true;
            //TacoManager.OpenModal(TacoConfig.TacoLoginErrorHeader, msg);
        };

        StartCoroutine(ApiManager.Instance.AddGTokens( fee, fee * TacoConfig.gTokenExchange,success, fail));
    }
	}

}