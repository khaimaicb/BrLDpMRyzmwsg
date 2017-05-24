using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Highscore : MonoBehaviour {
	public static Highscore Instance;
	Text font;
	int typeAccount = 0;

	public float highscore = 0;
	// Use this for initialization
	void Start () {
        font = GetComponent<Text>();
		Instance = this;
		typeAccount = PlayerPrefs.GetInt("highscoretype");
		if(typeAccount == 1)
			highscore = PlayerPrefs.GetFloat("highscore");
		else
			highscore = PlayerPrefs.GetFloat("highscoreArcade");
		font.text = highscore.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		//font.text = highscore.ToString();
	}

	public void UpdateManually() {
		font = GameObject.Find("highscore").GetComponent<Text>();
		font.text = highscore.ToString();
	}
}
