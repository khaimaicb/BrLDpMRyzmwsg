using UnityEngine;
using System.Collections;


public class HideIf : MonoBehaviour {

	public GameState HideCondition;
	public GameState ShowCondition;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (GamePlay.Instance.GameStatus == HideCondition && gameObject.activeSelf)
			gameObject.SetActive(false);
//		if(GamePlay.gameStatus == ShowCondition && !gameObject.activeSelf)
//			gameObject.SetActive(true);
	}
}
