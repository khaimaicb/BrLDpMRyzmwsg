

using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

namespace GameTaco
{

	public class TacoMessagePanel : MonoBehaviour {

		public static TacoMessagePanel Instance;

		public Text TitleText = null;


    // Use this for initialization
		void Start () {
			Instance = this;

		}
		


		public void Open (String title )
		{
			TitleText.text = title;
			
		}


		public void Close () {
			
		}
	}

}