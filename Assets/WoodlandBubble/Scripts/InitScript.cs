using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
//using GoogleMobileAds.Api;



public class InitScript : MonoBehaviour {
	public GameObject UpdatePopup;
	public GameObject RatePopup;
	public static InitScript Inctance;
	public static bool adReceived;
	public static bool adChartReceived;
	public static bool adChartMoreAppsReceived;
	string ver;
	public string myVer;
	bool FirstTime;
	public GameObject[] animatedButtons;
	public GameObject LoginButton;
	public GameObject InviteButton;
	public GameObject EMAIL;
	public GameObject MessagesBox;


	public static bool tutPassed;
	public static bool[] Skin = new bool[]{false,false,false,false,false};
	public static int Gems;
	public static bool Hat;
	public static bool InfinityLife;
	public static bool StickyStars;
	public static bool WasInApp;
	public static int CapOfLife = 10;
	public static bool sound = true;
	public static bool music;
	/// <summary>
	/// Dates and Times
	public static float RestLifeTimer;
	public static string DateOfExit;
	public static DateTime today;
	public static DateTime DateOfRestLife;
	public static string timeForReps;
	public static int waitedPurchaseGems;
	public static List<string> selectedFriends; 
	public static bool Lauched;
	public static int scoresForLeadboardSharing;
	public static int lastPlace;
	public static int savelastPlace;
	public static bool beaten;
	public static List<string> Beatedfriends;
	public static bool Arcade;
	public static bool Cherry;
	public static bool Electric;
	public static bool Changing;
	public static bool ShowedHardAd;

	int messCount;
	// Use this for initialization
	void Awake() {

		if(Application.platform == RuntimePlatform.WindowsEditor){
			Application.targetFrameRate = 60;
		//	PlayerPrefs.DeleteAll();
		//	Hat = true;
		//	InfinityLife = true;
		//	StickyStars = true;
			tutPassed = true;
//			RestScores();
		}
		Beatedfriends =  new List<string>();
		Beatedfriends.Clear();

		//		if( !InitScript.WasInApp)
//			ChartboostAd.Instance().LoadInterstitialAd(false);
		RestLifeTimer = PlayerPrefs.GetFloat("RestLifeTimer");
		DateOfExit = PlayerPrefs.GetString("DateOfExit");
		if(PlayerPrefs.GetInt("WasInApp") == 1)
			InitScript.WasInApp = true;

		if(PlayerPrefs.GetInt("InfinityLife") == 1)
			InitScript.InfinityLife = true;

		if(PlayerPrefs.GetInt("StickyStars") == 1)
			InitScript.StickyStars = true;

		Inctance = this;
		Gems = PlayerPrefs.GetInt("Gems");
//		if(PlayerPrefs.GetInt("tutPassed") == 1)
//			tutPassed = true;

		if(PlayerPrefs.GetInt("Lauched") == 0){    //First lauching
			FirstTime = true;
			Gems = 10;
			PlayerPrefs.SetInt("Gems",Gems);
			PlayerPrefs.SetInt("Lauched", 1);
			PlayerPrefs.Save();
		}

 
        //GameObject.Find("SkillsCanvas").gameObject.SetActive(false);
        //this.Skills(false);

        // see if we've got game music still playing
        GameObject gameMusic = GameObject.Find("Music");
//		if (gameMusic) {
//			// kill game music
//			Destroy(gameMusic);
//		}
		// make sure we survive going to different scenes
	//	DontDestroyOnLoad(gameMusic);

		Invoke("DelayStart",0.001f);


	}

	void DelayStart(){
		if(PlayerPrefs.GetInt("SoundOff")==0) Sound(true);
		else Sound(false);
		if(PlayerPrefs.GetInt("MusicOff")==0) Music(true);
		else Music(false);

	}

	public void HideFaceBookButton(){
		LoginButton.SetActive(false);
		InviteButton.SetActive(true);
	}

    void Start () {
		PlayerPrefs.SetInt("EnterCounter",PlayerPrefs.GetInt("EnterCounter")+1);
		PlayerPrefs.Save();
//		Debug.Log(Application.temporaryCachePath);

		if(PlayerPrefs.GetInt("EnterCounter") % 3 == 0 && PlayerPrefs.GetInt("Rated")==0){
			try {
				RatePopup.SetActive(true);
			} catch (System.Exception ex) {
				
			}
		}


        Debug.Log("Init is attached to " + this.gameObject);






//		myVer = PlayerSettings.bundleVersion;
//		ParseQuery<ParseObject> query = ParseObject.GetQuery("Version");
//		query.GetAsync("df8nSz06ZW").ContinueWith(t =>
//		                                          {
//			ParseObject gameScore = t.Result;
//			ver = gameScore.Get<string>("Number");
//		});
//		#if UNITY_ANDROID
//			Advertisement.Initialize ("e128f9998e7e081e", "d752564072385bdd");
//		#endif
		if( !InitScript.WasInApp){
  //          AdmobIntegration.THIS.RequestInterstitial();
//            AdmobAd.Instance().LoadInterstitialAd(true);
            //ChartboostAd.Instance().LoadInterstitialAd(true);
		}
	}

    //soomla

	public void AddGems(int count){
		Gems += count;
		SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.Cash);
		PlayerPrefs.SetInt("Gems", Gems);
		PlayerPrefs.Save();
	}

	public void SpendGems(int count){
		Gems -= count;
		PlayerPrefs.SetInt("Gems", Gems);
		PlayerPrefs.Save();
	}


	public void PurchaseSucceded(){
		InitScript.Inctance.AddGems(waitedPurchaseGems);
		waitedPurchaseGems = 0;
	}


	//public IEnumerator ShowAdsBanner(){
 //       AdmobIntegration.THIS.RequestBanner(AdPosition.Bottom);

 //       while (true && AdmobIntegration.THIS != null){
	//		yield return new WaitForSeconds(UnityEngine.Random.Range(10f, 20f));
 //           AdmobIntegration.THIS.ShowBanner();
	//		yield return new WaitForSeconds(UnityEngine.Random.Range(10f, 20f));
 //           AdmobIntegration.THIS.HideBanner();
	//	}
	//}

	public IEnumerator ShowAds(string location){
		yield return new WaitForSeconds(0.5f);
        //if(adChartReceived ){
        //    ChartboostAd.Instance().ShowInterstitialAd();
        //    adChartReceived = false;
        //}
        //else{
        //    ChartboostAd.Instance().LoadInterstitialAd(false);
        //}
        //ChartboostAd.Instance().LoadInterstitialAd(true);
        AdmobIntegration.THIS.ShowInterstitial();
//        AdmobAd.Instance().ShowInterstitialAd();

	}
     
	public IEnumerator ShowAdsAdmob(){
		yield return new WaitForSeconds(0.5f);
		if(adReceived ){
            AdmobIntegration.THIS.ShowInterstitial();
   //         AdmobAd.Instance().ShowInterstitialAd();
			adReceived = false;
			ShowedHardAd = true;
		}
		else{
            AdmobIntegration.THIS.RequestInterstitial();
      //      AdmobAd.Instance().LoadInterstitialAd(true);

			ShowedHardAd = true;
		}

	//	AdmobAd.Instance().LoadInterstitialAd(true);
		
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.Escape)){
			if(Application.loadedLevelName == "game"){
				if (GamePlay.Instance.GameStatus == GameState.Playing){
					GamePlay.ControlAllowed = false;
                    mainscript.StopControl = true;
                    GamePlay.Instance.GameStatus = GameState.Pause;
				}
			}
			else {
				//if(!InitScript.WasInApp)
				//	StartCoroutine(ShowAds("OnExit"));
				GameObject.Find("Camera").transform.Find("ExitPanel").gameObject.SetActive(true);

			}
		}
		if(ver != null){
			if(float.Parse(myVer) < float.Parse(ver) && !UpdatePopup.activeSelf) UpdatePopup.SetActive(true);
		}


	}

    public GameObject skillCanvas = null;
    public void Skills(bool show) {

        if(skillCanvas == null) {
            skillCanvas = GameObject.Find("SkillsCanvas");
        }

        //var canvas = GameObject.FindGameObjectWithTag("SkillsCanvasTag");// Find("SkillsCanvas");
        var canvas = skillCanvas;
        if (canvas == null) {
            Debug.Log("Canvas not found");
        } else {
            Debug.Log("Canvas found");
        }

        if(show) {
            canvas.SetActive(true);
        } else {
            canvas.SetActive(false);
        }
    }

	public void Sound(bool on){
		sound = on;
        Music(on);
		if(!sound){
			GameObject.Find("MusicAmb").GetComponent<AudioSource>().volume = 0;
			SoundBase.Instance.GetComponent<AudioSource>().volume = 0;
			PlayerPrefs.SetInt("SoundOff",1);
		}
		else{
			GameObject.Find("MusicAmb").GetComponent<AudioSource>().volume = 1;
			SoundBase.Instance.GetComponent<AudioSource>().volume = 1;
			PlayerPrefs.SetInt("SoundOff",0);
		}
		PlayerPrefs.Save();
		//		SendMessage("Sound", on);
	}
	
	public void Music(bool on){
		music = on;
		if(!music){
			if(Application.loadedLevelName == "game")
				GameObject.Find("Music").GetComponent<AudioSource>().volume = 0;
			PlayerPrefs.SetInt("MusicOff",1);
		}
		else{
			if(Application.loadedLevelName == "game")
				GameObject.Find("Music").GetComponent<AudioSource>().volume = 1;
			PlayerPrefs.SetInt("MusicOff",0);
		}
		PlayerPrefs.Save();
		//		SendMessage("Music", on);
	}


	public void UpdateScores(){
	}
	






}
