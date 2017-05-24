using UnityEngine;
using System.Collections;

public class Electric : MonoBehaviour {
	GameObject balloon;
	// Use this for initialization
	void OnEnable () {
//		if(balloon == null)
//			balloon = GameObject.Find("Balloon").transform.Find("Sphere").gameObject;
		InvokeRepeating("Noise",0,10);
	}

	void OnDisable(){
		CancelInvoke("Noise");
	}

	void Noise(){
		SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.electric);
	}
	
	// Update is called once per frame
	void Update () {
//		if(transform.position.x < Camera.main.transform.position.x - 10) Destroy(gameObject);
//		if(balloon != null)
//			if(Vector3.Distance(transform.position, balloon.transform.position) < 2) 
//				transform.position = Vector3.Lerp(transform.position, balloon.transform.position, Time.deltaTime*0.3f);
	}
}
