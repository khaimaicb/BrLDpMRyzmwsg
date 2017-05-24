using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour {
    public bool onlyHide;
	// Use this for initialization
	void OnEnable () {
        if (onlyHide)
            Invoke("Hide", 3);
        else
            Destroy(gameObject, 2);
	}

	void Hide(){
		gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
