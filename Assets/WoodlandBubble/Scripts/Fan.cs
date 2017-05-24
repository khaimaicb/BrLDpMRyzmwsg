using UnityEngine;
using System.Collections;

public class Fan : MonoBehaviour {
	float speedUp;
	// Use this for initialization
	void OnEnable () {
		StartCoroutine(OffFan());
		speedUp = 10f;

	}

	IEnumerator OffFan(){
		yield return new WaitForSeconds(0.1f);
	//	gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		speedUp +=0.1f;
		transform.Rotate(new Vector3(0,0, -10 * Time.deltaTime + speedUp));
	}
}
