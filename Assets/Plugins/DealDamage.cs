using UnityEngine;
using System.Collections;


public class DealDamage : MonoBehaviour {

	public bool colliderInRange = false;
	public Collider other;

	public string selfTag = "";

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider _other){
		if (!(_other.gameObject.tag == selfTag || _other.gameObject.tag == "Ground")) {
			colliderInRange = true;
			other = _other;
		}
	}

	void OnTriggerExit() {
		colliderInRange = false;
		other = null;
	}

	void Punch() {
		if (colliderInRange) {
			other.gameObject.BroadcastMessage ("OnHit", SendMessageOptions.DontRequireReceiver);
			//Debug.Log("HIT");
		}
	}

	void MegaPunch() {
		if (colliderInRange) {
			other.gameObject.BroadcastMessage ("OnMegaHit", SendMessageOptions.DontRequireReceiver);
		}
	}
}

