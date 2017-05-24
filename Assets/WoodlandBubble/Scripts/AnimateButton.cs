using UnityEngine;
using System.Collections;

public class AnimateButton : MonoBehaviour {
	Vector3 scale;
	// Use this for initialization
	void Start () {
		scale = transform.localScale;
	}

//	void flyUp(){
//		iTween.ScaleTo(gameObject,iTween.Hash("scale", Vector3.one*2, "time",1 , "easetype",iTween.EaseType.linear,"onComplete","flyDown", "ignoretimescale", true));
//	}
//	
//	void flyDown(){
//		iTween.ScaleTo(gameObject,iTween.Hash("scale", Vector3.one, "time",1,  "easetype",iTween.EaseType.linear,"onComplete","flyUp", "ignoretimescale", true));
//	}

	public void Play(){
		StartCoroutine(PlayCoroutine());
	}

	IEnumerator PlayCoroutine(){
		iTween.ScaleTo(gameObject,iTween.Hash("x", scale.x * 1.1f, "y", scale.y *  1.1f,"time",2,  "easetype",iTween.EaseType.easeInOutSine));
		yield return new WaitForSeconds(1);
		iTween.ScaleTo(gameObject,iTween.Hash("x", scale.x, "y", scale.y,"time",2,  "easetype",iTween.EaseType.easeInOutSine));
	}

	// Update is called once per frame
	void Update () {
	
	}
}
