using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	float fallLimit = -10;
	private GameObject cam;
	
	public GameObject gameOverMenu;
	
	// Use this for initialization
	void Start () {
		cam = GameObject.Find("Main Camera");
	}
	
	// Update is called once per frame
	void Update () {
		//here we check to see if the player fell or when too far to the left
		if(transform.position.y < fallLimit || transform.position.x < cam.transform.position.x - 13){
            GamePlay.Instance.GameStatus = GameState.GameOver;
			//and destroy the player
			Destroy(gameObject);
		}
	}
}


//using UnityEngine;
//using System.Collections;
//
//public class PlayerControl : MonoBehaviour {
//	//how fast the player walks
//	public float speed = 14.0f;
//	//how high the player can jump
//	public float jumpHeight = 8.0f;
//	//at what point the level resets if the player falls in a hole
//	public float fallLimit = -10;
//	//how fast the player recovers from being pushed back
//	public float recoverySpeed = 0.5f;
//	//jump sound
//	public AudioClip jumpSound;
//	
//	public GameObject standing;
//	public GameObject sliding;
//	
//	//we use a raycast hit to check how far the player is away from the ground
//	private RaycastHit hit;
//	private RaycastHit hitUp;
//	//using this to ensure the jump sound doesn't play more than once at a time.
//	private float jumpCounter = 0.0f;
//	//we use this statement to allow jumping to happen or not.
//	private bool canJump= false;
//	//here we get the camera to reference its position.
//	private GameObject cam;
//	//here we get the flash so we can tell it to do something if we die
//	private GameObject flash;
//	//we use this to save the original speed when starting the game.
//	private float origSpeed = 0.0f;
//	//we use this to check to see if we're standing or not
//	private bool isStanding= true;
//	private float speedup = 1.3f;
//	void  Start (){
//		
//		//we make sure the sliding animation for the character is deactivated
//		sliding.SetActive(false);
//		//we find the flash object
//		flash = GameObject.Find("flash");
//		//we find the main camera and pair it to cam
//		cam = GameObject.Find("Main Camera");
//		//we send a message to the camera with our speed.
//		cam.SendMessage("receiveSpeed", speed);
//		//we set the origspeed to speed so we can keep track of it while playing
//		origSpeed = speed;
//		
//	}
//	
//	void  Update (){
//		//here we check to see if the player fell or when too far to the left
//		if(transform.position.y < fallLimit || transform.position.x < cam.transform.position.x - 13){
//			GamePlay.gameStatus = GameState.GameOver;
//			//and destroy the player
//			Destroy(gameObject);
//		}
//		
//		//if the player gets behind the camera too much, we add a bit of speed so he recovers.
//		if(cam.transform.position.x > transform.position.x + 1 && speed == origSpeed){
//			//we add recovery speed to origspeed and make that the new speed
//			speed = origSpeed + recoverySpeed;
//			
//			speedup = 1.3f;
//		}
//		//if the player gets too far ahead he'll slow down instead of speed up, this would be rare or impossible, but here just to make sure.
//		if(cam.transform.position.x < transform.position.x - 1 && speed == origSpeed){
//			speed = origSpeed - recoverySpeed;
//			speedup = 0.7f;
//		}
//		
//		//if the player is back to the middle, we make the speed normal again.
//		if(cam.transform.position.x > transform.position.x - 1 && cam.transform.position.x < transform.position.x + 1 && speed != origSpeed){
//			speed = origSpeed;
//			speedup = 1;
//		}
//		
//		if (Physics.Raycast (transform.position - new Vector3(0,0.25f,0), new Vector3(1,0,0), out  hit)) {
//			if(hit.distance < 0.75f && hit.collider.isTrigger == false)
//				transform.position = transform.position - new Vector3(0.25f,0,0);
//		}
//		
//		
//		//jumpCounter becomes a timer.
//		jumpCounter += Time.deltaTime;
//		
//		//here we apply the speed to the character
//		
//		//rigidbody.velocity.x = speed;
//		//transform.position = new Vector3.Lerp(transform.position, transform.position + new Vector3.right*speed, Time.deltaTime);
//		transform.Translate(speed*Time.deltaTime,0,0);
//		
//		#if UNITY_WEBPLAYER
//		//Keyboard Controls for web versions (Same as Standalone because they both deal with keyboard)
//		//This checks to see if the player is pressing W or Space to jump
//		if (Physics.Raycast (transform.position - new Vector3(0,0.25f,0), new Vector3(0,-1,0), hit)) {
//			//if the hit distance of the raycast going down is less that 0.75f, we make it possible to jump
//			if(hit.distance < 0.75f){
//				canJump = true;
//				//if its not less, then we make it not possible.
//			}else{
//				canJump = false;
//			}
//		}
//		
//		//if the player presses w or space, and the player is standing and can jump, we allow him to jump.
//		if(Input.GetKey("w") || Input.GetKey("space")){
//			if(canJump == true && isStanding == true && hit.collider.isTrigger == false){
//				//we add speed to the jump
//				rigidbody.velocity.y = jumpHeight;
//				//if the jumpcounter > 0.25f we allow the sound to play.
//				if(jumpCounter > 0.25f){
//					//we play the sound
//					audio.PlayOneShot(jumpSound);
//					//and reset the counter
//					jumpCounter = 0.0f;	
//				}
//			}
//			//if the player is not pressing jump with w or space, we add velocity down. this helps make variable jump heights depending on how long the player holds the button.
//		}else{
//			rigidbody.velocity.y -= 32*Time.deltaTime;
//		}
//		
//		//here we use a raycast to check to see if the player's hit distance above him is small. this is for the sliding feature
//		if(Physics.Raycast (transform.position - new Vector3(0,0.25f,0), new Vector3(0,1,0), hitUp)){
//			//if the hit distance is greater than 0.4f and the player presses s, then we allow the player to slide
//			if(hitUp.distance > 0.4f){
//				if(Input.GetKeyDown("s")){
//					//this turns of isstanding so we can check it while the player is sliding
//					isStanding = false;
//					//we turn the sliding object on 
//					sliding.SetActive(true);
//					//and turn the standing object off
//					standing.SetActive(false);
//				}
//				//if the player lets go of s, we get him to stand back up with the same functions as above
//				if(Input.GetKeyUp("s")){
//					isStanding = true;
//					standing.SetActive(true);
//					sliding.SetActive(false);
//				}
//			}else{
//				//if the distance is less than 0.4f, then we make sure the player continues to slide. this forces the player to not be able to stand up while sliding. very polished!
//				if(isStanding == true && hitUp.collider.isTrigger == false){
//					isStanding = false;
//					sliding.SetActive(true);
//					standing.SetActive(false);	
//				}
//			}
//			//if the player is not pressing s but the hit distance is much higher than what would require to stay sliding, we force him to stand back up. again, very polished!
//			if(Input.GetKey("s") == false && hitUp.distance > 0.4f && isStanding == false){
//				isStanding = true;
//				standing.SetActive(true);
//				sliding.SetActive(false);	
//			}
//		}
//		#endif
//		
//		
//		
//		#if UNITY_STANDALONE
//		//Keyboard Controls for desktop versions (Same as web because they both deal with keyboard)
//		//This checks to see if the player is pressing W or Space to jump
//		if (Physics.Raycast (transform.position - new Vector3(0,0.25f,0), new Vector3(0,-1,0), hit)) {
//			if(hit.distance < 0.75f){
//				canJump = true;
//			}else{
//				canJump = false;
//			}
//		}
//		
//		if(Input.GetKey("w") || Input.GetKey("space")){
//			if(canJump == true && isStanding == true && hit.collider.isTrigger == false){
//				rigidbody.velocity.y = jumpHeight;
//				if(jumpCounter > 0.25f){
//					audio.PlayOneShot(jumpSound);
//					jumpCounter = 0.0f;	
//				}
//			}
//		}else{
//			rigidbody.velocity.y -= 32*Time.deltaTime;
//		}
//		
//		if(Physics.Raycast (transform.position - new Vector3(0,0.25f,0), new Vector3(0,1,0), hitUp)){
//			if(hitUp.distance > 0.4f){
//				if(Input.GetKeyDown("s")){
//					isStanding = false;
//					sliding.SetActive(true);
//					standing.SetActive(false);
//				}
//				if(Input.GetKeyUp("s")){
//					isStanding = true;
//					standing.SetActive(true);
//					sliding.SetActive(false);
//				}
//			}else{
//				if(isStanding == true && hitUp.collider.isTrigger == false){
//					isStanding = false;
//					sliding.SetActive(true);
//					standing.SetActive(false);	
//				}
//			}
//			if(Input.GetKey("s") == false && hitUp.distance > 0.4f && isStanding == false){
//				isStanding = true;
//				standing.SetActive(true);
//				sliding.SetActive(false);	
//			}
//		}
//		#endif
//		
//		#if UNITY_IOS
//		//iOS Controls (same as Android because they both deal with screen touches)
//		//we check to see if the player can jump with a raycast down, just like in the web and standalone controls
//		if (Physics.Raycast (transform.position - new Vector3(0,0.25f,0), new Vector3(0,-1,0), hit)) {
//			if(hit.distance < 0.75f){
//				canJump = true;
//			}else{
//				canJump = false;
//			}
//		}
//		
//		//if the touch count on the screen is higher than 0, then we allow stuff to happen to control the player.
//		if(Input.touchCount > 0){
//			foreach(Touch touch1 in Input.touches) {
//				//if the touch is on the top half of the screen, we do jump stuff
//				if(touch1.position.y > Screen.height/2){
//					//if the player can jump and is standing, then we do the jump
//					if(canJump == true && isStanding == true && hit.collider.isTrigger == false){
//						//here we apply the jump velocity
//						rigidbody.velocity.y = jumpHeight;
//						//if the jump counter is greater than 0.25f, we allow the sound to play
//						if(jumpCounter > 0.25f){
//							//we play the sound
//							audio.PlayOneShot(jumpSound);
//							//and reset the counter
//							jumpCounter = 0.0f;	
//						}
//					}
//				}
//				//if the touch position is on the bottom half of the screen, we do slide stuff
//				if(touch1.position.y < Screen.height/2){
//					rigidbody.velocity.y -= 32*Time.deltaTime;
//					isStanding = false;
//					sliding.SetActive(true);
//					standing.SetActive(false);
//				}else{
//					//if its not on the bottom half, we make him stand.
//					isStanding = true;
//					standing.SetActive(true);
//					sliding.SetActive(false);
//				}
//			}
//			
//		}else{
//			//if the touch count is 0, we add velocity down for the variable jump height, depending on how long the player holds jump
//			rigidbody.velocity.y -= 32*Time.deltaTime;
//			//now we check the raycast up to see if the player is in a sliding area or not
//			if(Physics.Raycast (transform.position - new Vector3(0,0.25f,0), new Vector3(0,1,0), hitUp)){
//				//if the player is in a sliding area, we force him to stay down
//				if(hitUp.distance < 0.4f && isStanding == true && hitUp.collider.isTrigger == false){
//					isStanding = false;
//					sliding.SetActive(true);
//					standing.SetActive(false);
//				}
//				//if the player is not in a sliding area but is still down and not touching the screen, we force him back up.
//				if(hitUp.distance > 0.4f && isStanding == false){
//					isStanding = true;
//					standing.SetActive(true);
//					sliding.SetActive(false);
//				}
//			}
//		}
//		#endif
//		
//		#if UNITY_ANDROID
//		//Android Controls (same as iOS because they both deal with screen touches)
//		if (Physics.Raycast (transform.position - new Vector3(1,0.25f,0), new Vector3(0,-1,0),out  hit)) {
//			if(hit.distance < 0.75f && hit.collider.name != "standing" && hit.collider.name != "sliding" ){
//				canJump = true;
//			}else{
//				canJump = false;
//			}
//		}
//		if (Physics.Raycast (transform.position - new Vector3(2,0.25f,0), new Vector3(2,-1,0),out  hit)) {
//			if(hit.distance < 0.75f && hit.collider.name != "standing" && hit.collider.name != "sliding"){
//				canJump = true;
//				
//			}
//		}
//		
//		if (Application.platform == RuntimePlatform.WindowsEditor){
//			if(Input.GetKey("w") || Input.GetKey("space")){
//				if(canJump == true && isStanding == true ){
//					if( hit.collider.isTrigger == false){
//						canJump = false;
//						rigidbody.velocity =  new Vector3(0, jumpHeight, 0);
//						if(jumpCounter > 0.25f){
//							audio.PlayOneShot(jumpSound);
//							jumpCounter = 0.0f;	
//						}
//					}
//				}
//			}else{
//				rigidbody.velocity -= new Vector3(0, 32*Time.deltaTime, 0); 
//			}
//			if(Physics.Raycast (transform.position - new Vector3(0,0.25f,0), new Vector3(0,1,0),out  hitUp)){
//				if(hitUp.distance > 0.4f){
//					if(Input.GetKeyDown("s")){
//						isStanding = false;
//						sliding.SetActive(true);
//						standing.SetActive(false);
//					}
//					if(Input.GetKeyUp("s")){
//						isStanding = true;
//						standing.SetActive(true);
//						sliding.SetActive(false);
//					}
//					//		}else{
//					//			if(isStanding == true && hitUp.collider.isTrigger == false){
//					//				isStanding = false;
//					//				sliding.SetActive(true);
//					//				standing.SetActive(false);	
//					//			}
//				}
//				if(Input.GetKey("s") == false && hitUp.distance > 0.4f && isStanding == false){
//					isStanding = true;
//					standing.SetActive(true);
//					sliding.SetActive(false);	
//				}
//			}
//		}
//		else{
//			if(Input.touchCount > 0){
//				foreach(Touch touch1 in Input.touches) {
//					if(touch1.position.x > Screen.width/2){
//						if(canJump == true ){
//							if( hit.collider.isTrigger == false){
//								canJump = false;
//								rigidbody.velocity =  new Vector3(0, 100*Time.deltaTime, 0); 
//								if(jumpCounter > 0.25f){
//									audio.PlayOneShot(jumpSound);
//									jumpCounter = 0.0f;	
//								}
//							}
//						}
//					}
//					if(touch1.position.x < Screen.width/2){
//						rigidbody.velocity -=  new Vector3(0, 100*Time.deltaTime, 0); 
//						isStanding = false;
//						sliding.SetActive(true);
//						standing.SetActive(false);
//						//	}else{
//						//		isStanding = true;
//						//		standing.SetActive(true);
//						//		sliding.SetActive(false);
//					}
//				}
//				
//			}else{
//				rigidbody.velocity -= new Vector3(0, 100*Time.deltaTime, 0); 
//				if(Physics.Raycast (transform.position - new Vector3(0,0.25f,0), new Vector3(0,1,0), out  hitUp)){
//					if(hitUp.distance < 0.2f && isStanding == true && hitUp.collider.isTrigger == false){
//						isStanding = false;
//						sliding.SetActive(true);
//						standing.SetActive(false);
//					}
//					if(hitUp.distance > 0.4f && isStanding == false){
//						isStanding = true;
//						standing.SetActive(true);
//						sliding.SetActive(false);
//					}
//				}
//			}
//		}
//		#endif
//		
//		////here we check to see if the player fell or when too far to the left
//		//if(transform.position.y < fallLimit || transform.position.x < cam.transform.position.x - 13){
//		//	//we check to see if flash is there, then we send him a message that its a game over.
//		//	if(flash != null){
//		//		//here we send the message
//		//		flash.SendMessage("gameOverFlash", SendMessageOptions.DontRequireReceiver);
//		//		
//		//	}
//		//	//and destroy the player
//		//	Destroy(gameObject);
//		//}
//		
//		
//		//end of function update
//	}
//}