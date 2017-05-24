using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class mainscript : MonoBehaviour
{
    public static mainscript Instance;
    GameObject ball;
    Vector2 speed = // Star movement speed / second
        new Vector2(250, 250);
    GameObject PauseDialogLD;
    GameObject OverDialogLD;
    GameObject PauseDialogHD;
    GameObject OverDialogHD;
    GameObject UI_LD;
    GameObject UI_HD;
    GameObject PauseDialog;
    GameObject OverDialog;
    GameObject FadeLD;
    GameObject FadeHD;
    GameObject AppearLevel;
    Vector2 target;
    Vector2 worldPos;
    Vector2 startPos;
    float startTime;
    float duration = 1.0f;
    bool setTarget;
    float mTouchOffsetX;
    float mTouchOffsetY;
    float xOffset;
    float yOffset;
    public int bounceCounter = 0;
    GameObject[] fixedBalls;
    public Vector2[][] meshArray;
    int offset;
    public GameObject checkBall;
    float waitTime = 0f;
    int revertButterFly = 1;
    public static int score;
    public static int stage = 1;
    const int STAGE_1 = 0;
    const int STAGE_2 = 300;
    const int STAGE_3 = 750;
    const int STAGE_4 = 1400;
    const int STAGE_5 = 2850;
    const int STAGE_6 = 4100;
    const int STAGE_7 = 5500;
    const int STAGE_8 = 6900;
    const int STAGE_9 = 8500;
    public int arraycounter = 0;
    public ArrayList controlArray = new ArrayList();
    bool destringAloneBall;
    public bool dropingDown;
    public float dropDownTime = 0f;
    public bool isPaused;
    public bool noSound;
    public bool gameOver;
    public bool arcadeMode;
    public float bottomBorder;
    public float topBorder;
    public float leftBorder;
    public float rightBorder;
    public float gameOverBorder;
    public float ArcadedropDownTime;
    public bool hd;
    public GameObject Fade;
    public int highScore;
    public AudioClip pops;
    public AudioClip click;
    public AudioClip levelBells;
    float appearLevelTime;
    public GameObject boxCatapult;
    public GameObject boxFirst;
    public GameObject boxSecond;
    public GameObject ElectricLiana;
    public GameObject BonusLiana;
    public GameObject BonusScore;
    public static bool ElectricBoost;
    bool BonusLianaCounter;
    bool gameOverShown;
    public static bool StopControl;
    public GameObject finger;

    public GameObject BoostChanging;

    int stageTemp;
    public Hashtable animTable = new Hashtable();
    public bool notAllowDropDown;
    public GameObject popupScore;
    public Canvas canvasSetting;
    public bool startCheckForDropDown;
    public Transform targetSquare;
    //	public int[][] meshMatrix = new int[15][17];
    // Use this for initialization
	
    void Awake()
    {
        animTable.Clear();
        notAllowDropDown = false;
        stage = 1;
        mainscript.StopControl = false;
        arcadeMode = InitScript.Arcade;
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            //SwitchLianaBoost();
            //arcadeMode = true;
        }
//		if(DisplayMetricsAndroid.WidthPixels>700){
//			hd = true;
//		}
        //	AdMobUnityPlugin.ShowAds();
        StartCoroutine(startButterfly());
//		StartCoroutine(startBonusScore());
//		StartCoroutine(startBonusLiana());

//		AdMobUnityPlugin.HideAds();
    }

    void ShowAd()
    {
//        AdmobAd.Instance().ShowInterstitialAd();
    }

    public void CheckBoosts()
    {
        if (InitScript.Cherry)
        {
            Score.Instance.addMultiplyer();
        }
        if (InitScript.Electric)
        {
            if (!mainscript.ElectricBoost)
                mainscript.Instance.SwitchLianaBoost();
        }
        if (InitScript.Changing)
        {
            BoostChanging.SetActive(true);
            finger.SetActive(true);
        }
        if (GamePlay.Instance.GameStatus == GameState.GameOver)
        {
            InitScript.Cherry = false;
            InitScript.Electric = false;
            InitScript.Changing = false;

        }
    }

    public void SwitchLianaBoost()
    {
        if (!ElectricBoost)
        {
            ElectricBoost = true;
            ElectricLiana.SetActive(true);
        }
        else
        {
            ElectricBoost = false;
            ElectricLiana.SetActive(false);
        }
    }

    void Start()
    {
        Instance = this;
//        AdmobAd.Instance().LoadInterstitialAd(true);

        if (AdmobIntegration.THIS != null)
            AdmobIntegration.THIS.RequestInterstitial();
        //	AdmobAd.Instance().LoadInterstitialAd(true);
//		audio.PlayClipAtPoint(ambient, new Vector3(5, 1, 2));
        //audio.loop = true;
//		if(DisplayMetricsAndroid.WidthPixels>700){
//			hd = true;
//		}
        stageTemp = 1;
        RandomizeWaitTime();
        score = 0;
        if (PlayerPrefs.GetInt("noSound") == 1)
            noSound = true;
//		if(PlayerPrefs.GetInt("arcade")==1){ arcadeMode = true; 		highScore = PlayerPrefs.GetInt("scoreArcade");}
//		if(PlayerPrefs.GetInt("arcade")==0){ arcadeMode = false;		highScore = PlayerPrefs.GetInt("score");}

        GamePlay.Instance.GameStatus = GameState.MainMenu;	
//		GameObject.Find("GUIHighscore").GetComponent<GUIText>().text = "High Score: " + highScore+"";
        CheckBoosts();
    }
	
    // Update is called once per frame
    void Update()
    {
        if (noSound)
            GetComponent<AudioSource>().volume = 0;
        if (!noSound)
            GetComponent<AudioSource>().volume = 0.5f;
//		if (Input.GetKeyDown(KeyCode.Escape)) { 
////			GameObject.Find("PauseButton").GetComponent<clickButton>().OnMouseDown();
//		}


        //if(gameOver && !gameOverShown){
        //	gameOverShown = true;
        //	GamePlay.Instance.ShowGameOver();
			
        //	return;
        //}
		
        if (score < STAGE_2)
            stage = 1;
        if (score >= STAGE_2 && score < STAGE_3)
        {
            stage = 2;
        }
        if (score >= STAGE_3 && score < STAGE_4)
        {
            stage = 3;
        }
        if (score >= STAGE_4 && score < STAGE_5)
        {
            stage = 4;
        }
        if (score >= STAGE_5 && score < STAGE_6)
        {
            stage = 5;
        }
        if (score >= STAGE_6 && score < STAGE_7)
        {
            stage = 6;
        }
        if (score >= STAGE_7 && score < STAGE_8)
        {
            stage = 7;
        }
        if (score >= STAGE_8 && score < STAGE_9)
        {
            stage = 8;
        }
        if (score >= STAGE_9)
        {
            stage = 9;
        }
		
        if (stage > stageTemp)
        {
            stageTemp = stage;
            //	AppearLevel.SetActiveRecursively(true); 
            appearLevelTime = Time.time + 2f; 
        }
		
			
        if (checkBall != null)
        {
            if (checkBall.tag != "Pearl")
                checkBall.GetComponent<ball>().checkNearestColor();
            else
                checkBall.GetComponent<ball>().changeNearestColor();
            Destroy(checkBall.GetComponent<Rigidbody>());

            checkBall = null;
            startCheckForDropDown = true;
        }
        if (startCheckForDropDown)
        {
            if (notAllowDropDown)
            {
                if (IsAnimationStoped())
                {
                    notAllowDropDown = false;
                }
            }
            if (!notAllowDropDown)
            {
                startCheckForDropDown = false;
                //connectNearBallsGlobal();
                int missCount = 1;
                if (stage >= 3)
                    missCount = 2;
                if (stage >= 9)
                    missCount = 1;
                Invoke("destroyAloneBall", 0.5f);
                if (!arcadeMode)
                {
                    if (bounceCounter >= missCount)
                    {
                        bounceCounter = 0;
                        dropDownTime = Time.time + 0.5f;
                        Invoke("dropDown", 0.1f);
                    }
                    else
                    {
                        if (!destringAloneBall && !dropingDown)
                        {
                            connectNearBallsGlobal();
                            destringAloneBall = true;
                            //StartCoroutine(destroyAloneBall());
                            //	dropingDown = true;

                        }
                    }

                }
            }
        }

        if (arcadeMode && Time.time > ArcadedropDownTime && GamePlay.Instance.GameStatus == GameState.Playing)
        {
            bounceCounter = 0;
            ArcadedropDownTime = Time.time + 10f;
            dropDownTime = Time.time + 0.2f;
            dropDown();
        }


        if (Time.time > waitTime && waitTime != 0f)  //// random butterfly
        {
            /*if(arcadeMode){
				GameObject gm = GameObject.Find ("Creator");
				gm.GetComponent<creatorButterFly>().createButterFly(revertButterFly);
		        waitTime = 0f;
				RandomizeWaitTime();
				revertButterFly *=-1;
			}*/
        }
		
        if (Time.time > dropDownTime && dropDownTime != 0f)
        {
            //	dropingDown = false;
            CheckLosing();
            dropDownTime = 0;
            StartCoroutine(getBallsForMesh());
        }
    }

    public void PopupScore(int value, Vector3 pos)
    {
        pos += Vector3.right * 0.5f;
        score += value * Score.Instance.multiplyer;
        Transform parent = GameObject.Find("CanvasScore").transform;
        GameObject poptxt = Instantiate(popupScore, pos, Quaternion.identity) as GameObject;
        poptxt.transform.GetComponentInChildren<Text>().text = "" + value;
        poptxt.transform.SetParent(parent);
        poptxt.transform.localScale = Vector3.one;
        Destroy(poptxt, 1);
    }

    public bool IsAnimationStoped()
    {
        fixedBalls = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject obj in fixedBalls)
        {
            if (obj.layer == 9)
            {
                if (obj.GetComponent<ball>().animStarted)
                    return false;
            }
        }
        return true;
    }

    public void CheckLosing()
    {
        fixedBalls = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject obj in fixedBalls)
        {
            if (obj.layer == 9)
            {
                if (transform.position.y <= gameOverBorder)
                {
                    if (!mainscript.ElectricBoost)
                        GamePlay.Instance.GameStatus = GameState.GameOver;
                    else
                    {
                        mainscript.Instance.BurnRows(3);
                    }
                }
            }
        }
    }

    IEnumerator startBonusLiana()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(30, 120));
            Instantiate(BonusLiana);
        }
    }

    IEnumerator startBonusScore()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5, 20));
            Instantiate(BonusScore);
        }
    }

    IEnumerator startButterfly()
    {
        while (GamePlay.Instance.GameStatus == GameState.Playing)
        {
            yield return new WaitForSeconds(Random.Range(5, 10));
            GameObject gm = GameObject.Find("Creator");
            revertButterFly *= -1;
            gm.GetComponent<creatorButterFly>().createButterFly(revertButterFly);
        }

    }

    IEnumerator getBallsForMesh()
    {
        GameObject[] meshes = GameObject.FindGameObjectsWithTag("Mesh");
        foreach (GameObject obj1 in meshes)
        {    											
            Collider2D[] fixedBalls = Physics2D.OverlapCircleAll(obj1.transform.position, 0.1f, 1 << 9);  //balls
            foreach (Collider2D obj in fixedBalls)
            {
                obj1.GetComponent<Grid>().busy = obj.gameObject;
                obj.GetComponent<bouncer>().offset = obj1.GetComponent<Grid>().offset;
            }
        }
        yield return new WaitForSeconds(0.2f);
    }

	
    public GameObject createFirstBall(Vector3 vector3)
    {
        GameObject gm = GameObject.Find("Creator");
        return gm.GetComponent<creatorBall>().createFirstBall(vector3);
    }

    public void connectNearBallsGlobal()
    {
        ///connect near balls
        fixedBalls = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject obj in fixedBalls)
        {
            if (obj.layer == 9)
                obj.GetComponent<ball>().connectNearBalls();
        }
        mainscript.StopControl = false;
    }

    public void dropDown()
    {
        creatorBall.Instance.EnableGridColliders();

        dropingDown = true;
        fixedBalls = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject obj in fixedBalls)
        {
            if (obj.layer == 9)
                obj.GetComponent<bouncer>().dropDown();
        }
        GameObject gm = GameObject.Find("Creator");
        gm.GetComponent<creatorBall>().createRow(0);
        creatorBall.Instance.OffGridColliders();

        	Invoke("destroyAloneBall", 1f);
        	destroyAloneBall();
    }

    public void explode(GameObject gameObject)
    {
        //gameObject.GetComponent<Detonator>().Explode();
    }

    void RandomizeWaitTime()
    {
        const float minimumWaitTime = 5f;
        const float maximumWaitTime = 10f;
        waitTime = Time.time + Random.Range(minimumWaitTime, maximumWaitTime);
    }

    public void destroyAloneBallDelayed()
    {
        Invoke("destroyAloneBall", 0.5f);
    }

    public void destroyAloneBall()
    {
        int i;
        //	while(true){
        connectNearBallsGlobal();
        i = 0;
        destringAloneBall = true;
        Camera.main.GetComponent<mainscript>().arraycounter = 0;
        GameObject[] fixedBalls = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];			// detect alone balls
        Camera.main.GetComponent<mainscript>().controlArray.Clear();
        foreach (GameObject obj in fixedBalls)
        {
            if (obj != null)
            {
                if (obj.layer == 9)
                { 
                    if (!findInArray(Camera.main.GetComponent<mainscript>().controlArray, obj.gameObject) && !obj.GetComponent<ball>().destroyed)
                    {
                        if (obj.GetComponent<ball>().nearBalls.Count < 7 && obj.GetComponent<ball>().nearBalls.Count > 0)
                        {
                            i++;
                            //	if(i>5){ i = 0; yield return new WaitForSeconds(0.5f); yield return new WaitForSeconds(0.5f);}
                            //		if(dropingDown) yield return new WaitForSeconds(1f);
                            ArrayList b = new ArrayList();
                            obj.GetComponent<ball>().checkNearestBall(b);
                            if (b.Count > 0)
                            {
                                destroy(b);
                            }
                        }
                    }
                }	
            }
        }
        destringAloneBall = false;
        StartCoroutine(getBallsForMesh());
        dropingDown = false;
        //	Debug.Log(i);
        //		yield return new WaitForSeconds(2f);
        //	}
    }

    public bool findInArray(ArrayList b, GameObject destObj)
    {
        foreach (GameObject obj in b)
        {
			
            if (obj == destObj)
                return true;
        }
        return false;
    }

    public void destroy(GameObject obj)
    {
        if (obj.name.IndexOf("ball") == 0)
            obj.layer = 0;
        Camera.main.GetComponent<mainscript>().bounceCounter = 0;
        //	obj.GetComponent<OTSprite>().collidable = false;
        //	Destroy(obj);
        obj.GetComponent<ball>().destroyed = true;
        obj.GetComponent<ball>().growUp();
        Camera.main.GetComponent<mainscript>().explode(obj.gameObject);
        //      mainscript.Instance.PopupScore(3, transform.position);

    }

    void playPop()
    {
        if (!Camera.main.GetComponent<mainscript>().noSound)
            SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.Pops);
        //AudioSource.PlayClipAtPoint(pops, transform.position);
    }

    public void destroy(ArrayList b)
    {
        Camera.main.GetComponent<mainscript>().bounceCounter = 0;
        int scoreCounter = 0;
        int rate = 0;
        int soundPool = 0;

        foreach (GameObject obj in b)
        {
//			obj.GetComponent<OTSprite>().collidable = false;
            if (obj.name.IndexOf("ball") == 0)
                obj.layer = 0;
            if (!obj.GetComponent<ball>().destroyed)
            {
                if (soundPool < 5)
                    obj.GetComponent<ball>().growUpPlaySound();
                else
                    obj.GetComponent<ball>().growUp();
                soundPool++;
                if (scoreCounter > 3)
                {
                    rate += 3;
                    scoreCounter += rate;
                }
                scoreCounter++;
                //	Destroy(obj);
			
                obj.GetComponent<ball>().destroyed = true;
                Camera.main.GetComponent<mainscript>().explode(obj.gameObject);
            }
        }
        mainscript.Instance.PopupScore(scoreCounter, transform.position);

    }

    public void BurnRows(int columns)
    {
        
        GameObject[] grids = GameObject.FindGameObjectsWithTag("Mesh");
        foreach (GameObject item in grids)
        {

            if (item.transform.position.y < 0f)
            {
                
                GameObject ball = item.GetComponent<Grid>().busy;
                if (ball != null)
                    ball.GetComponent<ball>().destroy();
            }

        }
//		SwitchLianaBoost();
    }

    public void ChangeBoost()
    {
        Grid.waitForAnim = true;
        GameObject ball1 = boxSecond.GetComponent<Grid>().busy;
        boxCatapult.GetComponent<Grid>().busy.GetComponent<ball>().newBall = false;
        iTween.MoveTo(boxSecond.GetComponent<Grid>().busy, iTween.Hash("position", boxCatapult.transform.position, "time", 0.3, "easetype", iTween.EaseType.linear, "onComplete", "newBall"));
        iTween.MoveTo(boxCatapult.GetComponent<Grid>().busy, iTween.Hash("position", boxSecond.transform.position, "time", 0.3, "easetype", iTween.EaseType.linear));
        boxSecond.GetComponent<Grid>().busy = boxCatapult.GetComponent<Grid>().busy;
        boxCatapult.GetComponent<Grid>().busy = ball1;
        boxFirst.GetComponent<Grid>().busy = boxSecond.GetComponent<Grid>().busy;
        //	boxCatapult.GetComponent<Grid>().BounceFrom(boxFirst);
    }


	// Game Taco stuff : Integration 



	public void TacoStartTournamentGame()
	{
		
		print ( "Tournament Game Started" );
		mainscript.Instance.arcadeMode = false;
		GamePlay.Instance.GameStatus = GameState.Playing;
        creatorBall.Instance.InitGame ();
		print ( "GamePlay.Instance.GameStatus = "  + GamePlay.Instance.GameStatus );

	}

	public void TacoPauseGame()
	{
		
		print ( "Game Pause" );
		if (GamePlay.Instance.GameStatus == GameState.Playing) {
			
			GamePlay.Instance.GameStatus = GameState.Pause;
		}
		print ( "GamePlay.Instance.GameStatus = "  + GamePlay.Instance.GameStatus );
	}
		
	public void TacoUnPauseGame()
	{
		
		print ( "Game Pause" );

		if (GamePlay.Instance.GameStatus != GameState.MainMenu) {
			
			GamePlay.Instance.GameStatus = GameState.Playing;

		}
		print ( "GamePlay.Instance.GameStatus = "  + GamePlay.Instance.GameStatus );
	}
		
}
	

