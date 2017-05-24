using UnityEngine;
using System.Collections;

public enum DirectionGas{
	UP,
	DOWN,
	LEFT,
	RIGHT
}

public class GasLeak : MonoBehaviour {
	public ParticleSystem ps;
	public float secs;
	public DirectionGas direction;
	[HideInInspector]
	public bool Blowing;

	// Use this for initialization
	void Start () {
		if(direction == DirectionGas.UP){
			BoxCollider coll = gameObject.GetComponent<BoxCollider>();
			coll.isTrigger = true;
			coll.size = new Vector3(0.5f, 5, 1);
		}
		if(direction == DirectionGas.DOWN){
			transform.rotation = Quaternion.Euler(Vector3.forward * 180);
			BoxCollider coll = gameObject.GetComponent<BoxCollider>();
			coll.isTrigger = true;
			coll.size = new Vector3(0.5f, 5, 1);
		}
		if(direction == DirectionGas.LEFT){
			transform.rotation = Quaternion.Euler(Vector3.forward * 90);
			BoxCollider coll = gameObject.GetComponent<BoxCollider>();
			coll.isTrigger = true;
			coll.size = new Vector3(0.5f, 5, 1);
		}
		if(direction == DirectionGas.RIGHT){
			transform.rotation = Quaternion.Euler(Vector3.forward * -90);
			BoxCollider coll = gameObject.GetComponent<BoxCollider>();
			coll.isTrigger = true;
			coll.size = new Vector3(0.5f, 5, 1);
		}
		StartCoroutine(StartLeak());
	}

	IEnumerator StartLeak(){
		while(true){
			yield return new WaitForSeconds(secs);
			SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.gas);
			//if(Random.Range(0,2)==1){
				ps.Play();
		//	}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(ps.isPlaying) Blowing = true;
		else Blowing = false;
	}
}
