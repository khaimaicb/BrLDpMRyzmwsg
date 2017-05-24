// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score : MonoBehaviour {
	public static Score Instance;
	
	//we have a sound when game over happens so we just play it from the score since it manages when the score stops and such.
	public AudioClip gameOverSound;

	public GameObject scorePopup;
	
	//private variables we use to keep track of score
	public static int score = 0;
	private float scoreCounter = 0.0f;
	//we check to see if the player is null or not so we use it
	private GameObject player;
	
	private bool checkPlayer= true;
	public int multiplyer = 1;

	Text font;
	
	void  Start (){
		Instance = this;
        font = GetComponent<Text>();
		//we find the player and apply it to the variable player
	//	score = 0;
//		if(name != "scoreGameOver") 
//			player = GameObject.Find("Balloon").transform.FindChild("Sphere").gameObject.GetComponent<BalloonControl>();

	//	addBigStar();
	}

	void  Update (){
		if (GamePlay.Instance.GameStatus == GameState.BlockedGame){ Destroy( gameObject); return;}
		if(name == "scoreGameOver"){
			font.text = score.ToString();
			return;
		}
		score = mainscript.score;
		//if the player is not null, keep track of score
		if (GamePlay.Instance.GameStatus == GameState.Playing){
			//here we add score based on time, multiplyed by the multiplyer amount
			//scoreCounter += mainscript.score; //(1*Time.deltaTime)*multiplyer;
			//then we round the score so it has no decibel amounts
			score = mainscript.score;

			//if the player has a multiplyer, we show it with an X
			if(multiplyer > 1){
				font.text = "" + score.ToString() + " X" + multiplyer.ToString();
				//if the player doesn't have a multiplyer or lost it, we don't show the X and the multiplyer
			}else{
				font.text = "" + score.ToString();
			}
			//if the player is null (he got deleted in playercontrols.js) we do the gameover stuff from here.
		}
//		else{
//			if(checkPlayer == true){
//				//turn check player off because we only want to do gameover stuff once
//				checkPlayer = false;
//				//play the gameover sound
//		//		audio.PlayOneShot(gameOverSound);
//				//change the score so to normal so no multiplyer counter is there if the player had one
//		//		guiText.text = "SCORE: " + score.ToString();
//				//now we check to see if the score was higher than the last high score
//				if(name == "scoreGameOver") font.text = score.ToString();
//				if(score > PlayerPrefs.GetFloat("highscore")){
//					//if it is, we set the highscore playerpref to what the score was this round
//					PlayerPrefs.SetFloat("highscore", score);
//					//now we find the highscore guitext and send it a message to update the score to what it is now.
//			//		GameObject highScore= GameObject.Find("GUI/gameover/highscore");
//			//		highScore.SendMessage("updateScore", SendMessageOptions.DontRequireReceiver);
//				}
//			}
//		}
		
		
		
	}
	void  addBigStar (){
		score += 50;
		GameObject popupScore = Instantiate(scorePopup) as GameObject;
		popupScore.transform.parent = transform.parent;
		popupScore.transform.localScale = Vector3.one*50;
		popupScore.GetComponent<ScorePopup>().SetScore( 50);
	}
	//if the player picks up a pickup, we get a message to add one.
    public	void  addScore (int scores){
        Score.Instance.ShowPopup(scores);

		mainscript.score += scores * multiplyer;
//		if(StarsBarScript.Instance.IsFull()){
//			StarsBarScript.Instance.ResetBar();
			//			InitScript.Inctance.AddLife(1);
//		}
//		
//		StarsBarScript.Instance.AddValue(0.01f);
		//multiplyer += 1;
	}

	public void  ShowPopup (int scores){

		GameObject popupScore = Instantiate(scorePopup) as GameObject;
		popupScore.transform.parent = transform.parent;
		popupScore.transform.localScale = Vector3.one*50;
		popupScore.GetComponent<ScorePopup>().SetScore( scores);
	}

	public void  addMultiplyer (){
		multiplyer += 1;
		Invoke("lostMultiplyer",60);
	}
	
	//if the player misses a pickup, we reset it to one.
	void  lostMultiplyer (){
		multiplyer = 1;
	}
}