using UnityEngine;
using System.Collections;

public class ExampleScript : MonoBehaviour {

	public float moveSpeed;
	public float jumpHeight;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetKeyDown(KeyCode.R))
		{
			gameObject.GetComponent<Renderer>().material.color = Color.red;
		}
		if(Input.GetButtonDown("Forward")){
			transform.Translate(transform.forward * moveSpeed * Time.deltaTime);
		}
		if(Input.GetButtonDown("Back")){
			transform.Translate(-transform.forward * moveSpeed * Time.deltaTime);
		}
		if(Input.GetButtonDown("Right")){
			transform.Translate(transform.right * moveSpeed * Time.deltaTime);
		}
		if(Input.GetButtonDown("Left")){
			transform.Translate(-transform.right * moveSpeed * Time.deltaTime);
		}

		if (Input.GetKeyDown(KeyCode.Mouse0)){
			print("Hey F**KER!");
		}
		if (Input.GetKeyDown(KeyCode.Mouse1)){
			print("You're World Will Burn");
		}
	}
}
