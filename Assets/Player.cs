using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public float speed = 10.0F;
	public bool grounded = false;
	public float jumpSpeed = 10.0F;
	public Transform attackZone;
	public Transform weapon;
	public float rayDistance;
	private Rigidbody rb;
	private Animation anim;
	public bool isLeft = false;
	void Start () {
		rb = GetComponent<Rigidbody>();
		anim = GetComponentInChildren<Animation>();
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

		Vector3 mouseLoc = CastRayToWorld();
		if (mouseLoc.x < transform.position.x) {
			isLeft = true;
		} else {
			isLeft = false;
		}

		if (isLeft) {
			attackZone.localPosition = new Vector3(-0.676f, 0.0f, 0.0f);
			weapon.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
		} else {
			attackZone.localPosition = new Vector3(0.676f, 0.0f, 0.0f);
			weapon.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		}


		if (Input.GetMouseButtonDown(0)) {
			anim.Play();
		}


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

	Vector3 CastRayToWorld() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Vector3 point = ray.origin + (ray.direction * rayDistance);
		//Debug.Log(point);
		return point;
	}
}
