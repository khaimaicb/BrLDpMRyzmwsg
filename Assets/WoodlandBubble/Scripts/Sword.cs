using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour {

	// Use this for initialization
	void Start () {
//		if(Random.Range(0,2)==0)
//			flyDown();
//		else
		if(transform.rotation.eulerAngles.z == 180)
			StartCoroutine(flyDown());

		else
			StartCoroutine(flyUp());

	}

	IEnumerator flyUp(){
		while(true){
			iTween.MoveAdd(gameObject,iTween.Hash("y", 1f, "time",0.5f,"easetype",iTween.EaseType.easeInOutSine));
			SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.sword1);
			yield return new WaitForSeconds(1f);
			iTween.MoveAdd(gameObject,iTween.Hash("y", -1f, "time",1,"easetype",iTween.EaseType.easeInOutSine));
			SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.sword2);
			yield return new WaitForSeconds(5);
		}
	}

	IEnumerator flyDown(){
		while(true){
			iTween.MoveAdd(gameObject,iTween.Hash("y", -1f, "time",0.5f,"easetype",iTween.EaseType.easeInOutSine));
			SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.sword1);
			yield return new WaitForSeconds(1f);
			iTween.MoveAdd(gameObject,iTween.Hash("y", 1f, "time",1,"easetype",iTween.EaseType.easeInOutSine));
			SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.sword2);
			yield return new WaitForSeconds(5);
		}
	}


	// Update is called once per frame
	void Update () {
	
	}
}
