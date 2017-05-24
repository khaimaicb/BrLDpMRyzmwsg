using UnityEngine;
using System.Collections;

public class Finger : MonoBehaviour {

	// Use this for initialization
	void Start () {
//		StartCoroutine(flyUp());
		iTween.MoveAdd(gameObject,iTween.Hash("y", -0.5f, "time",0.5f,"easetype",iTween.EaseType.easeInOutCubic, "loopType", iTween.LoopType.pingPong));

	}

	IEnumerator flyUp(){
		while(true){
			iTween.MoveAdd(gameObject,iTween.Hash("y", -0.5f, "time",1f,"easetype",iTween.EaseType.easeInBack));
			yield return new WaitForSeconds(1f);
			iTween.MoveAdd(gameObject,iTween.Hash("y", 0.5f, "time",1,"easetype",iTween.EaseType.easeInBack));
			yield return new WaitForSeconds(1);
		}
	}
}
