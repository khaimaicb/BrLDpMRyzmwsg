using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Gems : MonoBehaviour {
	Text uiLabel;
	// Use this for initialization
	void Start () {
		uiLabel = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		uiLabel.text =""+ InitScript.Gems;
	}
}
