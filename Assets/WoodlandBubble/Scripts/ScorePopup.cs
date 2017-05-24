using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScorePopup : MonoBehaviour {
	
	public AudioClip sound;
	Hashtable ht = new Hashtable();
	Text font;
	Vector2 currentScale;
	bool isText;
	
	void Awake(){
		ht.Add("x",3);
		ht.Add("y",3);
		ht.Add("time",2);
		ht.Add("delay",1);
		ht.Add("onupdate","myUpdateFunction");
		ht.Add("looptype",iTween.LoopType.pingPong);
        font = GetComponent<Text>();
	}
	
	// Use this for initialization
	void Start () {
		iTween.MoveAdd(gameObject,iTween.Hash("y", 0.2f, "time",1f,"easetype",iTween.EaseType.easeInOutSine));
		Invoke( "Destroy", 1f);
	//	currentScale = font.scale;
		//	audio.PlayOneShot(sound, MainMenu.sound);
		//	iTween.ScaleTo(gameObject, ht);
	}

	public void SetScore(int score){
		font.text = score.ToString();
	}
	
//	public void SetScore(int score, int Combo = 0){
//		if(Combo < 4){
//			font.scale *= 1.5f;
//			GamePlay.score += score * GamePlay.scoreMultiplier;
//			font.text = (score * GamePlay.scoreMultiplier).ToString();
//		}
//		else{
//			SetBonusScore(score, Combo );
//		}
//		Invoke( "Destroy", 0.5f);
//	}
	
//	public void SetBonusScore(int score, int Combo = 0){
//		
//		GamePlay.score += score * GamePlay.scoreMultiplier + Combo*10;
//		iTween.MoveAdd (gameObject, iTween.Hash ("y", 50 , "loopType", iTween.LoopType.none, "time", 1,  "easeType", iTween.EaseType.linear)); 
//		font.text = (score * GamePlay.scoreMultiplier + Combo*10).ToString();
//		font.topColor = Color.yellow;
//		font.botColor = Color.yellow;
//		font.scale *= 2f;
//		if(Combo == 4){
//			GameObject inst = (GameObject)Instantiate(Resources.Load("Prefabs/Bonuses/Combo_bunner"), transform.position+Vector3.back, transform.rotation);
//			iTween.ScaleTo (inst, iTween.Hash ("scale", Vector3.one*1.2f , "time", 0.5f,  "easeType", iTween.EaseType.easeOutBack)); 
//			iTween.ScaleTo (inst, iTween.Hash ("delay", 0.5f ,"scale", Vector3.one/2, "time", 0.5,  "easeType", iTween.EaseType.linear)); 
//			iTween.MoveAdd (inst, iTween.Hash ("y", 100 , "loopType", iTween.LoopType.none, "time", 1,  "easeType", iTween.EaseType.linear)); 
//			Destroy(inst, 1);
//		}
//		Invoke( "Destroy", 0.5f);
//	}
//	
//	public void SetText(string text){
//		isText = true;
//		font.text = text;
//		font.topColor = Color.yellow;
//		font.botColor = Color.yellow;
//		iTween.MoveAdd (gameObject, iTween.Hash ("y", 200 , "loopType", iTween.LoopType.none, "time", 2,  "easeType", iTween.EaseType.linear)); 
//		//		iTween.FadeTo(gameObject, iTween.Hash ("alpha", 0 , "loopType", iTween.LoopType.none, "time", 1,  "easeType", iTween.EaseType.linear)); 
//		Invoke( "OuntLineOff", 1f);
//		Invoke( "Destroy", 2f);
//	}
//	
//	void OuntLineOff(){
//		font.useOutline = false;
//		iTween.ValueTo(gameObject,iTween.Hash("from", 1f, "to", 0f,  "time",0.5, "easetype",iTween.EaseType.linear, "onupdate","changeTopColor"));
//		iTween.ValueTo(gameObject,iTween.Hash("from", 1f, "to", 0f,  "time",0.5, "easetype",iTween.EaseType.linear, "onupdate","changeBotColor"));
//	}
//	
//	void changeTopColor(float alpha){
//		font.topColor = new Color(font.topColor.r, font.topColor.g, font.topColor.b, alpha);
//	}
//	
//	void changeBotColor(float alpha){
//		font.botColor = new Color(font.botColor.r, font.botColor.g, font.botColor.b, alpha);
//	}
//	
//	public void SetScoreRed(int score){
//		iTween.MoveAdd(gameObject, iTween.Hash ("y", 50 , "time", 0.5f,  "easeType", iTween.EaseType.linear)); 
//		GamePlay.score += score;// * GamePlay.scoreMultiplier;
//		font.text = (score ).ToString();
//		font.topColor = Color.red;
//		font.botColor = Color.red;
//		font.scale *= 1.5f;
//		Invoke( "Destroy", 1f);
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		font.scale = Vector2.Lerp(font.scale, currentScale*3f, Time.deltaTime*20);
//		//		gameObject.transform.Translate(Vector3.up * Time.deltaTime*100);
//		//		transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one*4, Time.deltaTime);
//		//		if(!gameObject.animation.isPlaying)
//		//			Destroy(gameObject);
//	}
//	
	void Destroy(){
		Destroy (gameObject);
	}
}
