using UnityEngine;
using System.Collections;

public class CameraOffset : MonoBehaviour {

	public Transform offset;
	public Transform notOffset;
	public bool isOffset = false;

	// Use this for initialization
	void Start () {	

		offset.localPosition = new Vector3(-2.23f, transform.localPosition.y, transform.localPosition.z);
		notOffset.localPosition = new Vector3(2.23f, transform.localPosition.y, transform.localPosition.z);
		transform.localPosition = notOffset.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis("Horizontal") != 0.0f) {
			if (Input.GetAxis("Horizontal") < 0.0f) {
				isOffset = true;
			} else {
				isOffset = false;
			}
		}
		if(isOffset) {
			transform.localPosition = Vector3.Lerp(transform.localPosition, offset.localPosition, (float)(2.0f*Time.deltaTime));
		} else if(!isOffset) {
			transform.localPosition = Vector3.Lerp(transform.localPosition, notOffset.localPosition, (float)(2.0f*Time.deltaTime));
		}
	}
}
