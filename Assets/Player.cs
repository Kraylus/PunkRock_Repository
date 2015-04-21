using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public float speed = 10.0F;
	public bool grounded = false;
	public float jumpSpeed = 10.0F;
	public Transform attackZone;
	private Rigidbody rb;
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	void Update() {
		float _speed = speed;
		if (Input.GetAxis("Vertical")!=0.0f && Input.GetAxis("Horizontal")!=0.0f) {
			_speed *= 0.7f;
		}
		float vert = Input.GetAxis("Vertical") * _speed;
		float hor = Input.GetAxis("Horizontal") * _speed;
		vert *= Time.deltaTime;
		hor *= Time.deltaTime;
		transform.Translate(hor, 0, vert);
		rb.AddForce(Vector3.down * 12.0f);

		float testVal = 1.0f;
		if (Input.GetAxis("Horizontal") < 0.0f) {
			testVal *= -1;
		}

		attackZone.localPosition = new Vector3((0.676f * testVal), 0.0f, 0.0f);

		if(Input.GetKeyDown(KeyCode.Space) && grounded) {
			rb.AddForce(Vector3.up *jumpSpeed);
		}
	}
	void OnCollisionEnter(Collision other) {
		if (other.collider.tag == "Ground") {
			grounded = true;
		}
	}
	void OnCollisionExit(Collision other) {
		if (other.collider.tag == "Ground") {
			grounded = false;
		}
	}
}
