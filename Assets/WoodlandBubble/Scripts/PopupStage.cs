using UnityEngine;
using System.Collections;

public class PopupStage : MonoBehaviour {

	// Use this for initialization
	void Start () {
	//	iTween.MoveTo(gameObject,iTween.Hash("y", -1f, "time",2f,"easetype",iTween.EaseType.easeInOutSine));
		Destroy(gameObject, 1);
		TextMesh text = GetComponent<TextMesh>();
		text.text = "stage " + Stage.Instance.stage.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(Camera.main.transform.position.x-2, transform.position.y, transform.position.z);
//		transform.Translate(Vector3.down*Time.deltaTime*2);
	}
}
