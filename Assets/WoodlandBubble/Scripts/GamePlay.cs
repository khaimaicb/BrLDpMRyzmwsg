using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
//using GoogleMobileAds.Api;
//using ChartboostSDK;


public enum GameState
{
    MainMenu,
    Playing,
    Highscore,
    GameOver,
    Pause,
    BlockedGame
}

public class GamePlay : MonoBehaviour
{
	// Point to your Taco Package
	public GameObject MyTaco;

    public static GamePlay Instance;
    bool gameOverShow;
    private static GameState gameStatus;
    public GameObject gameOverMenu;
    public GameObject highScorePopup;
    public GameObject backHide;
    public GameObject pauseText;
    public GameObject MusicObject;

    public GameObject settingButton;
    GameObject pauseButton;
    GameObject flash;
    GameObject stage;
    bool highScoreShow;
    static int vipeCounter;
    public static bool ControlAllowed;
    //public float[] NextStageDistance = new float[];
    //	[HideInInspector]
    public float NextStageDistance = 1000;
//    public List<AdEvents> adsEvents = new List<AdEvents>();
    public GameObject mainMenu;
    static float AdTimer;
    public bool watchedVideo;

    public GameState GameStatus
    {
        get
        {
            return gameStatus;
        }

        set
        {
            Debug.Log(value);
            if (value == GameState.Playing)
            {
				if ((gameStatus == GameState.MainMenu || gameStatus == GameState.GameOver) && !watchedVideo) {

					print ("Game Started");

					creatorBall.Instance.InitGame ();

				}

                else if (gameStatus == GameState.GameOver && watchedVideo)
                {
                    mainscript.Instance.BurnRows(3);
                    watchedVideo = false;
                }

                gameOverMenu.SetActive(false);
                mainMenu.SetActive(false);
                pauseText.SetActive(false);
                settingButton.SetActive(true);
                CheckAdsEvents(value);
            }
            else if (value == GameState.BlockedGame)
            {
                CheckAdsEvents(value);
            }
            else if (value == GameState.GameOver)
            {
                gameOverMenu.SetActive(true);
                settingButton.SetActive(false);
                ShowGameOver();
                CheckAdsEvents(value);
            }
            else if (value == GameState.Highscore)
            {
                GameStatus = GameState.GameOver;
                settingButton.SetActive(false);
                CheckAdsEvents(value);
            }
            else if (value == GameState.Pause)
            {
                pauseText.SetActive(true);
                settingButton.SetActive(false);
                CheckAdsEvents(value);
            }
            if (value == GameState.MainMenu)
            {
                settingButton.SetActive(false);
                mainMenu.gameObject.SetActive(true);
                CheckAdsEvents(value);
            }


            gameStatus = value;
        }
    }

    void CheckAdsEvents(GameState state)
    {
        //bool checkBanner = false;
        //foreach (AdEvents item in adsEvents)
        //{
        //    if (item.gameEvent == state)
        //    {
        //        ShowAdByType(item.adType);
        //        if (item.adType == AdType.AdmobBanner)
        //            checkBanner = true;
        //    }
        //}
        //if (!checkBanner)
        //    AdmobIntegration.THIS.HideBanner();
    }

/*    void ShowAdByType(AdType adType)
    {
        //Debug.Log("show " + adType);
        //if (adType == AdType.AdmobInterstitial)
        //    AdmobIntegration.THIS.ShowInterstitial();
        //else if (adType == AdType.AdmobBanner)
        //    AdmobIntegration.THIS.ShowBanner();
        //else if (adType == AdType.UnityAdsRewardedVideo)
        //    UnityAdsIntegration.THIS.ShowRewardedVideo();
        //else if (adType == AdType.UnityAdsVideo)
        //    UnityAdsIntegration.THIS.ShowVideo();
        //else if (adType == AdType.ChartboostInterstitial)
        //    ChartboostIntegration.THIS.ShowInterstitial();
        
    }
*/
    void Awake()
    {
        //		Application.targetFrameRate = 60;
        Instance = this;
        if (AdmobIntegration.THIS == null)
            gameObject.AddComponent<AdmobIntegration>();
        ControlAllowed = true;
        vipeCounter++;
//		PlayerPrefs.SetFloat("highscore", 0);
//		PlayerPrefs.Save();

        if (InitScript.Inctance == null)
            gameObject.AddComponent<InitScript>();

        GameObject soundBase = GameObject.Find("SoundBase");
        if (soundBase == null)
        {
            soundBase = Instantiate(Resources.Load("SoundBase")) as GameObject;
            soundBase.transform.SetParent(Camera.main.transform);
            soundBase.transform.localPosition = Vector3.zero;
        }
        GameObject gameMusic = GameObject.Find("Music");
        if (gameMusic == null)
            gameMusic = Instantiate(Resources.Load("Music")) as GameObject;
        gameMusic.name = "Music";
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        //if (Application.loadedLevelName == "game")
        if (sceneName == "game")
            //DontDestroyOnLoad(gameMusic);
        if (!gameMusic.GetComponent<AudioSource>().isPlaying)
            gameMusic.GetComponent<AudioSource>().Play();
        //if (AdmobIntegration.THIS != null)
        //{
        //    AdmobIntegration.THIS.RequestBanner(AdPosition.Bottom);
        //    //if (AdmobIntegration.THIS.showBanner)
        //    //      StartCoroutine(InitScript.Inctance.ShowAdsBanner());
        //}
        //if (ChartboostIntegration.THIS != null)
        //{
        //    if (Chartboost.hasInterstitial(CBLocation.Default))
        //        Chartboost.cacheInterstitial(CBLocation.Default);
            
        //}
    }
    // Use this for initialization
    void Start()
    {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (sceneName == "game")
        //        if (Application.loadedLevelName == "game")
        {
            PlayerPrefs.SetInt("tutPassed", 1);
            PlayerPrefs.Save();
        }




        flash = GameObject.Find("flash");
        pauseButton = GameObject.Find("PauseButton");
        //if(gameStatus != GameState.BlockedGame) 
        //    gameStatus = GameState.WaitForPopup;
    }


    // Update is called once per frame
    void Update()
    {
		// TACO : Not sure what this was for, it was causing problem with text input
        //if (Input.GetKeyDown(KeyCode.L))
          //  GameStatus = GameState.GameOver;


        if (GameStatus == GameState.BlockedGame)
            return;
		
        if (GameStatus == GameState.Pause)
        {
            Time.timeScale = 0;
            ControlAllowed = false;
        }
        else
        {
            Time.timeScale = 1;
        }

    }

	public void TacoOpen()
	{
		MyTaco.BroadcastMessage ("OpenTacoFromGame");

	}

    public void ShowGameOver()
    {
        mainscript.Instance.CheckBoosts();
        float highScore = PlayerPrefs.GetFloat("highscoreArcade");
        if (Score.score > highScore)
        {
            Time.timeScale = 0;
            ControlAllowed = false;
            PlayerPrefs.SetFloat("highscoreArcade", Score.score);
            Highscore.Instance.UpdateManually();
            object[] modelInfo = new object[1];
            modelInfo[0] = Score.score;
            MyTaco.BroadcastMessage ("CheckWillGetFreeTokens", modelInfo);

        }
        else
        {
            mainscript.Instance.CheckBoosts();
        }
        if(Score.score > 0) {
            MyTaco.BroadcastMessage ("checkUserLoginToShowPopup");
        }
        //GUI.Box(new Rect(0,0,Screen.width / 2, Screen.height / 2), "Test box");

        
		// Sending end of game score to Taco :
		MyTaco.BroadcastMessage ("TacoPostScore", Score.score);

    }
    public void LoadMenu() {
        this.GameStatus = GameState.MainMenu;
    }
    public void ShowModalRemindSignup() {
        MyTaco.BroadcastMessage("TacoCloseAllModal");
        object[] modelInfo = new object[4];
        modelInfo[0] = "Taco Token Gift Free";
        modelInfo[1] = "Congrats on the very high score you just made there. It is converted into " + Score.score + " Taco Tokens which can be spent in Tournaments. Please sign in to redeem it!";
        modelInfo[2] = Score.score;
        modelInfo[3] = "TacoFreePlayGiftToken";
        GamePlay.Instance.MyTaco.BroadcastMessage("TacoOpenModel", modelInfo);
    }
    public void ShowModalEarningTokens() {
        var highScoreNow = PlayerPrefs.GetFloat("highscoreArcade", 0);
        MyTaco.BroadcastMessage("TacoCloseAllModal");
        object[] modelInfo = new object[4];
        modelInfo[0] = "Congratulations!";
        modelInfo[1] = "You break new high score, you will receive: " + highScoreNow + " G.";
        modelInfo[2] = highScoreNow;
        modelInfo[3] = "";
        MyTaco.BroadcastMessage("TacoOpenModel", modelInfo);
        UpdateHighScore();
	}
    public void ChangeHighScoreToLocal() {
        GamePlay.Instance.MyTaco.BroadcastMessage("ChangeHighScoreToLocal");
    }
    public void UpdateHighScore() {
        PlayerPrefs.SetFloat("highscoreArcade", Score.score);
        PlayerPrefs.Save();
        GamePlay.Instance.GameStatus = GameState.Highscore;
        Highscore.Instance.highscore = Score.score;
        Highscore.Instance.UpdateManually();
        InitScript.Inctance.UpdateScores();
    }
}
