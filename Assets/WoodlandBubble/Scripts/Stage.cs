using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Stage : MonoBehaviour {
	public static Stage Instance;
	public int stage;
	public GameObject stagePopup;
	Text font;
	// Use this for initialization
	void Awake () {
		Instance = this;
		stage = 1;
        font = GetComponent<Text>();
	}

	public void NextStage(){
		stage ++;
	//	GamePlay.Instance.NextStageDistance += 100;
		font.text = "STAGE " + stage;
	//	GameObject stagepopup = Instantiate(stagePopup) as GameObject;
	//	bc.NextStage();
//		stagepopup.transform.parent = transform.parent;
//		stagepopup.transform.localScale = Vector3.one*80;


	}
	
	// Update is called once per frame
	void Update () {
		stage = mainscript.stage;
		font.text = "STAGE " + stage;
	}
}
