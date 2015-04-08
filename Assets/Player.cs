using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public float speed = 10.0F;
	void Start () {
	}
	void Update() {
		float vert = Input.GetAxis("Vertical") * speed;
		float hor = Input.GetAxis("Horizontal") * speed;
		vert *= Time.deltaTime;
		hor *= Time.deltaTime;
		transform.Translate(hor, 0, vert);
	}
}
