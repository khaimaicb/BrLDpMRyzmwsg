using UnityEngine;

public class MouseSensitivity : MonoBehaviour {
    
    public GameObject cursor;
    public Texture2D cursorImage;
    private int cursorSizeX = 30;
    private int cursorSizeY = 30;
	// Use this for initialization
	public void Start () {
        //originalRotation = transform.localRotation;
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        //Cursor.SetCursor(cursorImage,new Vector2(0f,0f),CursorMode.ForceSoftware) ;
        //cursor = new Vector2(0f,0f);
        //cursor.transform.localPosition =  new Vector2(Input.mousePosition.x, Input.mousePosition.y);
	}
	
	// Update is called once per frame
	void Update () {
        //_cursor.transform.position = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        //cursor = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        //Cursor.visible = false;
        //cursor.transform.localPosition =  new Vector2(Input.mousePosition.x, Input.mousePosition.y);
	}
    public void OnGUI()
     {
         float positionX = Event.current.mousePosition.x;
         float positionY = Event.current.mousePosition.y;
         //cursor.transform.localPosition =  new Vector2(Input.mousePosition.x, Input.mousePosition.y);
         //GUI.DrawTexture (new Rect(positionX-cursorSizeX/2, positionY-cursorSizeY/2, cursorSizeX, cursorSizeY), cursorImage);
     }
}
