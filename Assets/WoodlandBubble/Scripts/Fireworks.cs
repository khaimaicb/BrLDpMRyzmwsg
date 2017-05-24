using UnityEngine;
using System.Collections;

public class Fireworks : MonoBehaviour {
	
	public GameObject[] fireworksObjects;
	bool startedFireworks;
	// Use this for initialization
	void Start () {
		Invoke("OpenGameOver",3);
	}
	
	IEnumerator StartFireworks(){
		for (int i = 0; i < 10; i++) {
			foreach (var item in fireworksObjects) {
				GameObject fx = Instantiate(item) as GameObject;
		//		fx.transform.parent = transform.parent;
//				fx.layer = 8;
				fx.transform.position = Camera.main.transform.position + Vector3.forward*10 + Vector3.up*2;
//				fx.transform.localScale = Vector3.one;
				yield return new  WaitForSeconds(1.0f);
			}
		}
	}

	void OpenGameOver(){

        GamePlay.Instance.GameStatus = GameState.GameOver;
		mainscript.Instance.CheckBoosts();
		transform.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
		//	OpenGameOver();
		}
		if(!startedFireworks){
			startedFireworks = true;
			StartCoroutine(StartFireworks());
		}
	}
}
