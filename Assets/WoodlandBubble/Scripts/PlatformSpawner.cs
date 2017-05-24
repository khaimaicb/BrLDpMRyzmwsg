// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;

public class PlatformSpawner : MonoBehaviour {
	int lastElem;
	
	//these 9 prefab objects are the chunks that are spawned at random. This is much more efficient than spawning blocks 1 by 1
	//this also creates a much cleaner randomly generated environment
	public GameObject element1;
	public GameObject element2;
	public GameObject element3;
	public GameObject element4;
	public GameObject element5;
	public GameObject element6;
	public GameObject element7;
	public GameObject element8;
	public GameObject element9;
	public GameObject element10;
	public GameObject element11;
	public GameObject element12;
	public GameObject element13;
	public GameObject element14;
	public GameObject element15;
	public GameObject element16;

	GameObject bottomSpikes;
	//this is the starting position we use where we start spawning terrain right after the first platform that already exists in the scene.
	public float startSpawnPosition = 20;
	//this is the y position that all chunks will be spawned
	public float spawnYPos = -8;
	
	//this variable is just to keep track of the random number chosen that will determine what chunk will be spawned
	private int randomChoice;
	//this is the variable that keeps track of the last position we spawned an object
	private float lastPosition;
	//we find the camera to reference its position so this script knows when to spawn the next object
	private GameObject cam;
	//we check to see if we can spawn or not based on camera position and lastposition
	private bool canSpawn= true;
	
	void  Start (){
		//we make lastposition start at start spawn position
		lastPosition = startSpawnPosition;
		//we find the camera in the scene and pair it to the variable cam
		cam = GameObject.Find("Main Camera");
	}

	IEnumerator SetRandomElement(){
		randomChoice = Random.Range(0,21);
		yield return new WaitForSeconds(0.01f);
		if(randomChoice == lastElem) StartCoroutine(SetRandomElement());
	}
	
	void  Update (){
		
		//if the camera is farther than the number last position minus 16, we allow a chunk to spawn.
		if(cam.transform.position.x >= lastPosition - 16 && canSpawn == true){
			//now we turn off can spawn so that this doesn't happen more than once and we'll turn it back on when we're ready to do another spawn.
			canSpawn = false;
			//we choose the random number that will determine what chunk will be spawned.
//			StartCoroutine(SetRandomElement());
//			Debug.Log("last " + lastElem + " rand " + randomChoice);
			randomChoice = Random.Range(0,21);
			//now we call the spawnobject function and send the random choice variable with it
			spawnObject(randomChoice);
		}
		
		//end of function update
	}
	
	//now we spawn an object based on the number randomChoice thats received. rand is used in place of randomChoice
	void  spawnObject ( int rand  ){
		//	Instantiate(bottomSpikes, new Vector3(lastPosition+6,spawnYPos,1), Quaternion.Euler(0,0,0));
		//if we receive the number 1 or 2, we spawn normalEight
		if(rand == 1 && lastElem != rand && Stage.Instance.stage <= 2){
			//we add half of the width of the object we spawn to the position we spawn it, in this case 6.
			Instantiate(element1, new Vector3(lastPosition,spawnYPos,0), Quaternion.Euler(0,0,0));
			//then we add 12 to last position so it will be ready to spawn another object in the right place next time one needs to be spawned.
			lastPosition += 5.12f;
			//we do this for the remaining 8 objects, just adding different numbers to lastPosition based on the width of the object being spawned.
			lastElem = rand;
		}
		//spawn normalTwo
		if(rand == 2 && lastElem != rand  && Stage.Instance.stage <= 2){
			Instantiate(element2, new Vector3(lastPosition,spawnYPos,0), Quaternion.Euler(0,0,0));
			lastPosition += 6.4f;
			lastElem = rand;
		}
		//spawn fourpieceslide
		if(rand == 3 && lastElem != rand ){
			Instantiate(element8, new Vector3(lastPosition,spawnYPos,0), Quaternion.Euler(0,0,0));
			lastPosition += 5.76f;
			lastElem = rand;
		}
		//spawn specialjumpslide
		if(rand == 4 && lastElem != rand  && Stage.Instance.stage >= 1  && Stage.Instance.stage <= 5){
			Instantiate(element9, new Vector3(lastPosition,spawnYPos,0), Quaternion.Euler(0,0,0));
			lastPosition += 11.52f;
			lastElem = rand;
		}
		//spawn fourpieceslide
		if(rand == 5 && lastElem != rand  && Stage.Instance.stage >= 2  && Stage.Instance.stage <= 5){
			Instantiate(element10, new Vector3(lastPosition,spawnYPos,0), Quaternion.Euler(0,0,0));
			lastPosition += 7.04f;
			lastElem = rand;
		}
		//spawn specialjumpslide
		if(rand == 6 && lastElem != rand  && Stage.Instance.stage >= 2  && Stage.Instance.stage <= 5){
			Instantiate(element13, new Vector3(lastPosition,spawnYPos,0), Quaternion.Euler(0,0,0));
			lastPosition += 10.23997f;
			lastElem = rand;
		}
		//spawn specialjumpslide
		if(rand == 7 && lastElem != rand  && Stage.Instance.stage >= 3 ){
			Instantiate(element14, new Vector3(lastPosition,spawnYPos,0), Quaternion.Euler(0,0,0));
			lastPosition += 7.68f;
			lastElem = rand;
		}
		//spawn space
		if(rand == 8 && lastElem != rand  && Stage.Instance.stage >= 2  && Stage.Instance.stage <= 5){
			Instantiate(element3, new Vector3(lastPosition,spawnYPos,0), Quaternion.Euler(0,0,0));
			lastPosition += 6.4f;
			lastElem = rand;
		}
		//spawn raised
		if(rand == 9 && lastElem != rand  && Stage.Instance.stage >= 2  ){
			Instantiate(element4, new Vector3(lastPosition,spawnYPos,0), Quaternion.Euler(0,0,0));
			lastPosition += 5.76f;
			lastElem = rand;
		}
		if(rand == 10 && lastElem != rand  && Stage.Instance.stage >= 2 ){
			Instantiate(element6, new Vector3(lastPosition,spawnYPos,0), Quaternion.Euler(0,0,0));
			lastPosition += 5.76f;
			lastElem = rand;
		}
		if(rand == 11 && lastElem != rand  && Stage.Instance.stage >= 2  && Stage.Instance.stage <= 5){
			Instantiate(element11, new Vector3(lastPosition,spawnYPos,0), Quaternion.Euler(0,0,0));
			lastPosition += 11.52f;
			lastElem = rand;
		}
		if(rand == 12 && lastElem != rand  && Stage.Instance.stage >= 2 && Stage.Instance.stage <= 5){
			Instantiate(element16, new Vector3(lastPosition,spawnYPos,0), Quaternion.Euler(0,0,0));
			lastPosition += 9.6f;
			lastElem = rand;
		}
		//spawn ridgeleft
		if(rand == 17 && lastElem != rand  && Stage.Instance.stage >= 4 ){
			Instantiate(element5, new Vector3(lastPosition,spawnYPos,0), Quaternion.Euler(0,0,0));
			lastPosition += 5.76f;
			lastElem = rand;
		}
		//spawn ridgeright
		//spawn twopieceslide
		if(rand == 18 && lastElem != rand  && Stage.Instance.stage >= 4){
			Instantiate(element7, new Vector3(lastPosition,spawnYPos,0), Quaternion.Euler(0,0,0));
			lastPosition += 6.4f;
			lastElem = rand;
		}
		if(rand == 19 && lastElem != rand  && Stage.Instance.stage >= 2 && Stage.Instance.stage <= 5){
			Instantiate(element12, new Vector3(lastPosition,spawnYPos,0), Quaternion.Euler(0,0,0));
			lastPosition += 7.04f;
			lastElem = rand;
		}
		if(rand == 20 && lastElem != rand  && Stage.Instance.stage >= 3){
			Instantiate(element15, new Vector3(lastPosition,spawnYPos,0), Quaternion.Euler(0,0,0));
			lastPosition += 10.88f;
			lastElem = rand;
		}
		//now we allow the script to get ready to spawn another platform, right after we spawn one.
		canSpawn = true;
	}
}