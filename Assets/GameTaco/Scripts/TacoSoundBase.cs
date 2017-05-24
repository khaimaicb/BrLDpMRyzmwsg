using UnityEngine;
using System.Collections;

namespace GameTaco
{


	public class TacoSoundBase : MonoBehaviour {
		public static TacoSoundBase Instance;
		public AudioClip click;

	// Use this for initialization
		void Awake () {
		//DontDestroyOnLoad(gameObject);
			Instance = this;
		}

	// Update is called once per frame
		void Update () {

		}
	}

}
