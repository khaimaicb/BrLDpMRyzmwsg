using UnityEngine;  
using UnityEngine.UI;  
using System.Collections;  

namespace GameTaco
{

	public class TacoCamera : MonoBehaviour   
	{  
	//The texture that holds the video captured by the webcam  
		private WebCamTexture webCamTexture;



	// Use this for initialization  
		void Start ()   
		{  
		//Initialize the webCamTexture  
			webCamTexture = new WebCamTexture();  

		//Assign the images captured by the first available webcam as the texture of the containing game object  
			Image image = this.GetComponent<Image>();

		//image.sprite = Sprite.Create(webCamTexture., new Rect(0, 0, webCamTexture.width, webCamTexture.height), new Vector2(0, 0));;  

		//Start streaming the images captured by the webcam into the texture  
			webCamTexture.Play();  
		}  
	}

}