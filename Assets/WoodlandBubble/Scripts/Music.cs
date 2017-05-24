using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {
	public Music Instance;
	// Use this for initialization
	void Start () {
		Instance = this;
		if(Application.loadedLevelName != "game") Destroy(gameObject);
	}

	
	// Update is called once per frame
	void Update () {
		if(Application.loadedLevelName != "game") Destroy(gameObject);
	}
}
