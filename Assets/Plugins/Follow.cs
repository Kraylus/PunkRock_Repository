using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {

	public Transform target;
	public float heightAboveCharacter;
	public float distanceFromCharacter;
	public GameObject mainCam;
	public Transform camTransform;
	public float cameraRotation;

	// Use this for initialization
	void Start () {
		camTransform = mainCam.GetComponent<Transform> ();
		Quaternion rotation = Quaternion.identity;
		rotation.eulerAngles = new Vector3 (cameraRotation, 0, 0);
		camTransform.rotation = rotation;
	}
	
	// Update is called once per frame
	void Update () {
		if (target.position.z > 5) {
			transform.position = new Vector3 (target.position.x, target.position.y + heightAboveCharacter, (-16.0f + (target.position.z - 5.0f)));
		} else if (target.position.z < -5) {
			transform.position = new Vector3 (target.position.x, target.position.y + heightAboveCharacter, (-16.0f + (target.position.z + 5.0f)));
		} else {
			transform.position = new Vector3 (target.position.x, target.position.y + heightAboveCharacter, -16.0f);
		}
	}
}
