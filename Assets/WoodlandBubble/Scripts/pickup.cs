// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class pickup : MonoBehaviour {
	
	
	//we use these 8 textures to animate the pickup object
	Texture txtr1;
	Texture txtr2;
	Texture txtr3;
	Texture txtr4;
	Texture txtr5;
	Texture txtr6;
	Texture txtr7;
	Texture txtr8;
	
	//we have 2 sounds for when its picked up or missed
	public AudioClip pickupSound;
	public AudioClip lostSound;

	public GameObject Explosion;
	public GameObject TutrialComplete;

	public bool random = true;
	
	//private variables that we use to help animate and keep track of when its picked up or missed
	private float counter = 0.0f;
	private GameObject player;
	private GameObject scoreGUI;
	private bool lostMultiplyer= false;
	private bool grabbedPickup= false;
	
	void  Start (){
		if(Application.loadedLevelName != "game")
			TutrialComplete = GameObject.Find("Camera").transform.Find("Anchor").transform.Find("TutorialComplete").gameObject;
		//we find the player and apply it to the variable player to keep track of where he is
		player = GameObject.Find("Sphere");
		//we find the scoreGUI object to send him messages based on picking it up or missing it for showing multiplyers and changing how fast the score counts up
		scoreGUI = GameObject.Find("score");
		transform.position = new Vector3(transform.position.x, transform.position.y, 4.6f);

		if(name.Contains("superStar") || name.Contains("cherry") ){
			if(name.Contains("superStar"))
				SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.Chpok);
			StartCoroutine(flyUp());
		}
		if(name.Contains("cherry") && random){
			if(Random.Range(0,30)!=1) Destroy(gameObject);
		}
	}

	IEnumerator flyUp(){
		while(true){
			iTween.MoveAdd(gameObject,iTween.Hash("y", 1f, "time",3f,"easetype",iTween.EaseType.easeInOutSine));
			yield return new WaitForSeconds(1);
			iTween.MoveAdd(gameObject,iTween.Hash("y", -1f, "time",3,"easetype",iTween.EaseType.easeInOutSine));
			yield return new WaitForSeconds(1);
		}
	}

	void  Update (){
		if(InitScript.StickyStars && name == "pickup"){
			if(player != null)
				if(Vector3.Distance(transform.position, player.transform.position) < 8) 
					transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime*0.3f);

		}

		if(name.Contains("superStar") && Application.loadedLevelName == "game"){
			transform.Translate(Time.deltaTime*1,0,0);
		}
		//if the player is not null, we check to see where he is
		if(player != null){
			//if the player is past the pickup, then we send a message to the scoreGUI that it was missed and multiplyer needs to be reset
//			if(player.transform.position.x > transform.position.x + 3 && lostMultiplyer == false && grabbedPickup == false){
//				scoreGUI.SendMessage("lostMultiplyer", SendMessageOptions.DontRequireReceiver);
//				//we play the lost sound
////				if(GamePlay.sound)
////					audio.PlayOneShot(lostSound);
//				//and set lostmultiplyer to true so that this can't happen again.
//				lostMultiplyer = true;
//			}
		}
		
	}


	//if we hit a trigger we check to see what hit it.
	void  OnTriggerEnter ( Collider other  ){
		//if the player hit the pickup, then we do stuff
		if(other.tag == "Player" && grabbedPickup == false){
			//we set grabbedpickup to true so it can't happen more than once
			grabbedPickup = true;
			//then start the getmultiplyer function
			StartCoroutine( getMultiplyer());
		}
	}
	
	IEnumerator  getMultiplyer (){
		if(Explosion != null)
			Explosion.SetActive(true);
		//we send a message to the scoreGUI so it will add 1 to the multiplyer
		if(name.Contains("superStar")&& (Application.loadedLevelName == "game")){
			scoreGUI.SendMessage("addBigStar", SendMessageOptions.DontRequireReceiver);
			GetComponent<SpriteRenderer>().enabled = false;
		}
		else if(name.Contains("cherry") && (Application.loadedLevelName == "game")){
			scoreGUI.SendMessage("addMultiplyer", SendMessageOptions.DontRequireReceiver);
			GetComponent<SpriteRenderer>().enabled = false;
		}
		else if(Application.loadedLevelName == "game")
			scoreGUI.SendMessage("addScore", SendMessageOptions.DontRequireReceiver);
		else{
			TutrialComplete.SetActive(true);
		}
		//play the pickup sound
		
		if(InitScript.sound)
			GetComponent<AudioSource>().PlayOneShot(pickupSound);
		//turn off the renderer
		GetComponent<Renderer>().enabled = false;
		//wait for a bit so the sound can play
		yield return new WaitForSeconds(1.0f);
		//then destroy the object.
		Destroy(gameObject);
	}
}