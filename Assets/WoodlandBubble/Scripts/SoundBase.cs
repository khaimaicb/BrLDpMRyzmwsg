using UnityEngine;
using System.Collections;

public class SoundBase : MonoBehaviour {
	public static SoundBase Instance;
	public AudioClip click;
	public AudioClip wind;
	public AudioClip gas;
	public AudioClip sword1;
	public AudioClip sword2;
	public AudioClip electric;
	public AudioClip SlowFall;
	public AudioClip FallUp;
	public AudioClip eyes;
	public AudioClip Explosion;
	public AudioClip ExplosionPoof;
	public AudioClip Applaud;
	public AudioClip Chpok;
	public AudioClip Cash;
	public AudioClip Bang;
	public AudioClip Pops;
	public AudioClip Swish;
	public AudioClip Pickup;
	// Use this for initialization
	void Awake () {
		//DontDestroyOnLoad(gameObject);
		Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
