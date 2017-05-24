using UnityEngine;
using System.Collections;

public class NewHighscore : MonoBehaviour {

	// Use this for initialization
	void Start () {
		iTween.MoveTo(gameObject, iTween.Hash("y", 100 * 0.002777778f, "time",3));
		SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.Applaud);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
