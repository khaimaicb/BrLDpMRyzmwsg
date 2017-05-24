using UnityEngine;
using System.Collections;

public class ColorBallScript : MonoBehaviour
{
    public Sprite[] sprites;

    // Use this for initialization
    void Start()
    {

    }

    public void SetColor(int color)
    {
        GetComponent<SpriteRenderer>().sprite = sprites[color];
    }

    // Update is called once per frame
    void Update()
    {

//        if ((GamePlay.Instance.GameStatus == GameState.GameOver || GamePlay.Instance.GameStatus == GameState.MainMenu) && !GamePlay.Instance.watchedVideo)
//            Destroy(gameObject);

    }
}
