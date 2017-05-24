using UnityEngine;
using System.Collections;

public class Tut1 : MonoBehaviour {

	// Use this for initialization
	void Start () {

		Invoke("ShowGratz", 1);
		Invoke("ChangeScene", 3);
	}

	void ShowGratz(){
		transform.FindChild("Label").gameObject.SetActive(true);
	}

	void ChangeScene(){
		if(Application.loadedLevelName == "tut1") Application.LoadLevel("tut2");
		else if(Application.loadedLevelName == "tut2") Application.LoadLevel("tut3");
		else if(Application.loadedLevelName == "tut3") Application.LoadLevel("game");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
