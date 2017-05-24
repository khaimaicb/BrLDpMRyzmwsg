using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BubbleButton : MonoBehaviour
{

    GameObject On;
    GameObject Off;
    GameObject cam;
    System.Collections.Generic.Dictionary<string,string> parameters;
    bool WaitForPickupFriends;
    bool WaitForAksFriends;
    bool WaitForLoginToLeadboard;
    // Use this for initialization
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        if (name == "Sound")
        { 
            On = transform.FindChild("Background").gameObject;
            Off = transform.FindChild("Background1").gameObject;
            if (!InitScript.sound)
            {
                On.SetActive(false);
                Off.SetActive(true);
            }
        }
        if (name == "Music")
        { 
            On = transform.FindChild("Background").gameObject;
            Off = transform.FindChild("Background1").gameObject;
            if (!InitScript.music)
            {
                On.SetActive(false);
                Off.SetActive(true);
            }
        }
        if (InitScript.InfinityLife)
        {
            if (transform.parent.name == "LifePanel")
            {
                transform.parent.gameObject.SetActive(false);
            }
        }
        if (name == "Changing")
        {
            if (!InitScript.Changing)
            {
                transform.FindChild("Background").gameObject.SetActive(true);
                transform.FindChild("Background1").gameObject.SetActive(false);
                transform.Find("Price").GetComponent<Text>().text = "8";
                if (PlayerPrefs.GetInt("Changing") == 0)
                    transform.Find("Price").GetComponent<Text>().text = "1";
                transform.Find("Price").gameObject.SetActive(true);
            }
            else
            {
                transform.FindChild("Background").gameObject.SetActive(false);
                transform.FindChild("Background1").gameObject.SetActive(true);
                transform.Find("Price").gameObject.SetActive(false);
            }
        }
        if (name == "Cherry")
        {
            if (!InitScript.Cherry)
            {
                transform.FindChild("Background").gameObject.SetActive(true);
                transform.FindChild("Background1").gameObject.SetActive(false);
                transform.Find("Price").GetComponent<Text>().text = "5";
                if (PlayerPrefs.GetInt("Cherry") == 0)
                    transform.Find("Price").GetComponent<Text>().text = "1";
                transform.Find("Price").gameObject.SetActive(true);
            }
            else
            {
                transform.FindChild("Background").gameObject.SetActive(false);
                transform.FindChild("Background1").gameObject.SetActive(true);
                transform.Find("Price").gameObject.SetActive(false);
            }
        }
        if (name == "Electric")
        {
            if (!InitScript.Electric)
            {
                transform.FindChild("Background").gameObject.SetActive(true);
                transform.FindChild("Background1").gameObject.SetActive(false);
                transform.Find("Price").gameObject.SetActive(true);
                transform.Find("Price").GetComponent<Text>().text = "8";
                if (PlayerPrefs.GetInt("Electric") == 0)
                    transform.Find("Price").GetComponent<Text>().text = "1";
            }
            else
            {
                transform.FindChild("Background").gameObject.SetActive(false);
                transform.FindChild("Background1").gameObject.SetActive(true);
                transform.Find("Price").gameObject.SetActive(false);
            }
        }
    }

    void OnEnable()
    {
        if (name == "Video")
        {
//            if (Advertisement.IsReady("rewardedVideo"))
//                GetComponent<Image>().enabled = false;
//            else
//                GetComponent<Image>().enabled = true;                
        }
    }

    void returnControl()
    {
        mainscript.StopControl = false;
        GamePlay.ControlAllowed = true;
    }

    void offBlock()
    {
        InitScript.ShowedHardAd = true;
    }

    public void OnPress(bool isDown)
    {
        if (!isDown)
        {

            if (!InitScript.ShowedHardAd && transform.parent.name == "GameOver" && (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.WP8Player))
            {
                Invoke("offBlock", 3);
                return;
            }


            SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.click);

			if (name == "ChangeSceneButton")
			{
				SceneManager.LoadScene (1);
			}


            if (name == "Video")
            {
                //UnityAdsIntegration.THIS.ShowRewardedVideo();
            }

            if (name == "PauseButton")
            {
                if (GamePlay.Instance.GameStatus != GameState.Pause && GamePlay.Instance.GameStatus != GameState.GameOver)
                {
                    GamePlay.ControlAllowed = false;
                    mainscript.StopControl = true;
                    GamePlay.Instance.GameStatus = GameState.Pause;
                }
                else if (GamePlay.Instance.GameStatus == GameState.Pause)
                {
                    //BalloonControl.Instance.ChangeSkin(PlayerPrefs.GetInt("ActiveSkin"));
                    GamePlay.Instance.GameStatus = GameState.Playing;
                    mainscript.Instance.CheckBoosts();
                    Invoke("returnControl", 0.5f);
                }
            }
            if (name == "Play")
            {
                GamePlay.Instance.GameStatus = GameState.Playing;
            }
            if (name == "TutorialButton")
            {
                GameObject.Find("MainMenuCanvas").transform.Find("Tutorial").gameObject.SetActive(true);
            }
            if (name == "SettingButton")
            {
                if(GamePlay.Instance.GameStatus == GameState.Playing)
                    GamePlay.Instance.GameStatus = GameState.Pause;
                mainscript.Instance.canvasSetting.enabled = true;
            }
            if (name == "CloseSettingButton")
            {
                if(GamePlay.Instance.GameStatus == GameState.Pause)
                    GamePlay.Instance.GameStatus = GameState.Playing;
                mainscript.Instance.canvasSetting.enabled = false;
            }
            if (name == "Settings")
            {
                transform.parent.parent.Find("Options").gameObject.SetActive(true);
            }
            if (name == "AddLife")
            {
                GameObject.Find("Camera").transform.Find("Refill").gameObject.SetActive(true);
            }
            if (name == "AddGem")
            {
                GameObject.Find("Camera").transform.Find("GemsShop").gameObject.SetActive(true);
            }
            if (name == "Changing")
            {
                if (!InitScript.Changing)
                {
                    if (InitScript.Gems >= int.Parse(transform.Find("Price").GetComponent<Text>().text))
                    {
                        InitScript.Changing = true;
                        InitScript.Inctance.SpendGems(int.Parse(transform.Find("Price").GetComponent<Text>().text));
                        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.Cash);
                        InitScript.Changing = true;
                        mainscript.Instance.CheckBoosts();
                        transform.FindChild("Background").gameObject.SetActive(false);
                        transform.FindChild("Background1").gameObject.SetActive(true);
                        transform.Find("Price").gameObject.SetActive(false);
                        //PlayerPrefs.SetInt("Changing", 1);
                        //PlayerPrefs.Save();
                    }
                    else
                    {
                        GameObject.Find("Camera").transform.Find("GemsShop").gameObject.SetActive(true);
                    }
                }
            }
            if (name == "Cherry")
            {
                if (!InitScript.Cherry)
                {
                    if (InitScript.Gems >= int.Parse(transform.Find("Price").GetComponent<Text>().text))
                    {
                        InitScript.Cherry = true;
                        InitScript.Inctance.SpendGems(int.Parse(transform.Find("Price").GetComponent<Text>().text));
                        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.Cash);
                        InitScript.Cherry = true;
                        mainscript.Instance.CheckBoosts();
                        transform.FindChild("Background").gameObject.SetActive(false);
                        transform.FindChild("Background1").gameObject.SetActive(true);
                        transform.Find("Price").gameObject.SetActive(false);
                        //PlayerPrefs.SetInt("Cherry", 1);
                        //PlayerPrefs.Save();
                    }
                    else
                    {
                        GameObject.Find("Camera").transform.Find("GemsShop").gameObject.SetActive(true);
                    }
                }
            }
            if (name == "Electric")
            {
                if (!InitScript.Electric)
                {
                    if (InitScript.Gems >= int.Parse(transform.Find("Price").GetComponent<Text>().text))
                    {
                        InitScript.Electric = true;
                        InitScript.Inctance.SpendGems(int.Parse(transform.Find("Price").GetComponent<Text>().text));
                        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.Cash);
                        InitScript.Electric = true;
                        mainscript.Instance.CheckBoosts();

                        transform.FindChild("Background").gameObject.SetActive(false);
                        transform.FindChild("Background1").gameObject.SetActive(true);
                        transform.Find("Price").gameObject.SetActive(false);
                        //PlayerPrefs.SetInt("Electric", 1);
                        //PlayerPrefs.Save();
                    }
                    else
                    {
                        GameObject.Find("Camera").transform.Find("GemsShop").gameObject.SetActive(true);
                    }
                }
            }
            if (name == "Shop")
            {
                GameObject.Find("Camera").transform.Find("Shop").gameObject.SetActive(true);
            }
            if (name == "Close")
            {
                transform.parent.gameObject.SetActive(false);
            }
            if (name == "Back")
            {
                transform.parent.parent.gameObject.SetActive(false);
            }
            if (name == "Later")
            {
                transform.parent.gameObject.SetActive(false);
            }
            if (name == "Never")
            {
                PlayerPrefs.SetInt("Rated", 1);
                PlayerPrefs.Save();
                transform.parent.gameObject.SetActive(false);
            }
            if (name == "HowToPlay")
            {
                GameObject.Find("Camera").transform.Find("HelpInfo").gameObject.SetActive(true);
            }
            if (name == "BoostInfo")
            {
                GameObject.Find("Camera").transform.Find("BoostInfo").gameObject.SetActive(true);
            }
            if (name == "StartGame")
            {
                mainscript.Instance.arcadeMode = false;
                Invoke("StartGame", 0.2f);
                //GameObject.Find("Camera").transform.Find("MenuPlay").gameObject.SetActive(true);
            }
            if (name == "StartArcadeGame")
            {
                PlayerPrefs.SetString("gTokenSignUpGift","");
                mainscript.Instance.arcadeMode = true;
                GamePlay.Instance.ChangeHighScoreToLocal();
                Invoke("StartGame", 0.2f);

                //	GameObject.Find("Camera").transform.Find("MenuPlay").gameObject.SetActive(true);
            }
            if (name == "ExitGame")
            {
               Application.Quit();
            }

			if (name == "OpenTaco")
			{
				GamePlay.Instance.TacoOpen ();
			}

            if (name == "Menu")
            {
                string high = "highscoreArcade";
                if (Score.score > PlayerPrefs.GetFloat(high))
                {
                    PlayerPrefs.SetFloat(high, Score.score);
                    PlayerPrefs.Save();
                    Highscore.Instance.highscore = Score.score;
                    Highscore.Instance.UpdateManually();
                }

                if(mainscript.Instance.arcadeMode) {
                    GamePlay.Instance.GameStatus = GameState.MainMenu;
                }
                else {
                    if(GamePlay.Instance.GameStatus == GameState.Pause) {
                        GamePlay.Instance.MyTaco.BroadcastMessage("TacoCloseAllModal");
                        object[] modelInfo = new object[4];
                        modelInfo[0] = "End Tournament";
                        modelInfo[1] = "Are you sure you want to quit tournament and submit the score currently?";
                        modelInfo[2] = Score.score;
                        modelInfo[3] = "TacoEndTournament";
                        GamePlay.Instance.MyTaco.BroadcastMessage("TacoOpenModel", modelInfo);
                    }
                    else if(GamePlay.Instance.GameStatus == GameState.GameOver){
                        GamePlay.Instance.GameStatus = GameState.MainMenu;
                    }
                }
            }
            if (name == "BuyInfinityLife")
            {
                if (InitScript.Gems >= int.Parse(transform.Find("Count").GetComponent<Text>().text))
                {
                    SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.Cash);
                    InitScript.Inctance.SpendGems(int.Parse(transform.Find("Count").GetComponent<Text>().text));
                    InitScript.InfinityLife = true; 
                    PlayerPrefs.SetInt("InfinityLife", 1); 
                    PlayerPrefs.Save();
                    gameObject.SetActive(false);
                    transform.parent.transform.Find("LifePanel").gameObject.SetActive(false);
                    GameObject.Find("Camera").transform.Find("LifePanel").gameObject.SetActive(false);
                }
                else
                    GameObject.Find("Camera").transform.Find("GemsShop").gameObject.SetActive(true);

            }
            if (name == "Again")
            {
                //GamePlay.Instance.GameStatus = GameState.Playing;
                PlayerPrefs.SetString("gTokenSignUpGift","");
                mainscript.Instance.arcadeMode = true;
                Invoke("StartGame", 0.2f);
            }
            if (name == "Exit")
            {
                Application.Quit();
            }


			//MV : seeing if this breaks anything
            //if (name == "TacoButton") {
              //  _Manager.Instance.Open();
            //}


            if (name == "Sound" || name == "Music")
            {
                if (On.activeSelf)
                {
                    
                    if (name == "Sound")
                        InitScript.Inctance.Sound(false);
                    if (name == "Music")
                        InitScript.Inctance.Music(false);

                    On.SetActive(false);
                    Off.SetActive(true);
                }
                else
                {
                    if (name == "Sound")
                        InitScript.Inctance.Sound(true);
                    if (name == "Music")
                        InitScript.Inctance.Music(true);

                    On.SetActive(true);
                    Off.SetActive(false);
                }
            }

            if (name == "Boost_change")
            {
                mainscript.Instance.ChangeBoost();

            }
        }
    }

    void StartGame()
    {
        GamePlay.Instance.GameStatus = GameState.Playing;
    }

}
