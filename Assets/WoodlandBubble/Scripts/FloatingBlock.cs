using UnityEngine;
using System.Collections;

public class FloatingBlock : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(flyUp());

	}

	IEnumerator flyUp(){
		while(true){
			iTween.MoveAdd(gameObject,iTween.Hash("y", 1f, "time",3f,"easetype",iTween.EaseType.easeInOutSine));
			yield return new WaitForSeconds(1f);
			iTween.MoveAdd(gameObject,iTween.Hash("y", -1f, "time",3,"easetype",iTween.EaseType.easeInOutSine));
			yield return new WaitForSeconds(1);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
