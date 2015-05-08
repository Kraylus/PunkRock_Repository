using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	public float speed = 10.0F;
	public bool grounded = false;
	public float jumpSpeed = 10.0F;
	public Transform attackZone;
	public float rayDistance;
	public GameObject playerSprite;

	public GameObject mainCameraOffset;
	public GameObject prefab;
	public Vector3 respawnLoc;

	public int health = 100;

	private Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite> ();
	public Sprite[] spriteArray;

	private Rigidbody rb;

	private SpriteRenderer spriteRend;

	private bool[] CR_RunningArray;
	private bool CR_anim = false;

	public bool isDead = false;
	public bool isWaiting = false;

	private int currentIndex = 0;

	public bool isLeft = false;

	private Vector3 temp;

	void Start () {
		foreach(Sprite _sprite in spriteArray) {
			string _spriteName = _sprite.name.Substring(8);
			spriteDict.Add(_spriteName, _sprite);
			Debug.Log (_spriteName+" : "+_sprite.name);
		}
		temp = new Vector3(attackZone.localPosition.x, attackZone.localPosition.y, attackZone.localPosition.z);
		CR_RunningArray = new bool[] {false, false, false, false, false};
		rb = GetComponent<Rigidbody>();
		spriteRend = GetComponentInChildren<SpriteRenderer>();
		//playerSprite.transform.localPosition = Vector3.zero;
		mainCameraOffset = GameObject.Find("CameraOffset");
	}
	void Update() {
		if (!isDead && !isWaiting) {
			float _speed = speed;
			if (Input.GetAxis ("Vertical") != 0.0f && Input.GetAxis ("Horizontal") != 0.0f) {
				_speed *= 0.7f;
			}
			if ((Input.GetAxis ("Vertical") != 0.0f || Input.GetAxis ("Horizontal") != 0.0f) && !CR_RunningArray[0]) {
				StopCoroutine("AnimateIdle");
				CR_anim = false;
				resetTransform ();
				spriteRend.sprite = spriteDict ["walk"];
			} else if (!CR_RunningArray[0]) {
				if (!(CR_anim)) {
					StartCoroutine("AnimateIdle");
				}
				spriteRend.sprite = spriteDict ["idle"];
			}

			if (!(CR_RunningArray[0] && CR_RunningArray[2] && currentIndex == 2)) {
				float vert = Input.GetAxis ("Vertical") * _speed;
				float hor = Input.GetAxis ("Horizontal") * _speed;
				vert *= Time.deltaTime;
				hor *= Time.deltaTime;
				transform.Translate (hor, 0, vert);
				if (Input.GetAxis("Horizontal") != 0.0f) {
					if (Input.GetAxis("Horizontal") < 0.0f) {
						isLeft = true;
					} else {
						isLeft = false;
					}
				}
			}
			rb.AddForce (Vector3.down * 12.0f);

			if (isLeft) {
				attackZone.localPosition = new Vector3 (-0.676f, 0.0f, 0.0f);
				playerSprite.transform.localScale = new Vector3 (-0.1081449f, playerSprite.transform.localScale.y, playerSprite.transform.localScale.z);
			} else {
				attackZone.localPosition = new Vector3 (0.676f, 0.0f, 0.0f);
				playerSprite.transform.localScale = new Vector3 (0.1081449f, playerSprite.transform.localScale.y, playerSprite.transform.localScale.z);
			}


			if (Input.GetButtonDown("Fire1") && !CR_RunningArray[1] && !CR_RunningArray[0]) {
				CR_anim = false;
				StopCoroutine("AnimateIdle");
				resetTransform ();
				spriteRend.sprite = spriteDict ["punch"];
				gameObject.BroadcastMessage ("Punch");
				StartCoroutine (ReturnToIdle(new float[]{0.4f, 0.1f, 1.0f}));
			} else if (Input.GetButtonDown("Fire2")&& !CR_RunningArray[2] && !CR_RunningArray[0] && grounded) {
				CR_anim = false;
				StopCoroutine("AnimateIdle");
				resetTransform ();
				spriteRend.sprite = spriteDict ["groundPound"];
				gameObject.BroadcastMessage ("MegaPunch");
				temp = new Vector3(attackZone.localPosition.x, attackZone.localPosition.y, attackZone.localPosition.z);
				attackZone.localScale = new Vector3(1.69f, 1f, 1f);
				StartCoroutine (ReturnToIdle(new float[]{1.2f, 5.0f, 2.0f}));
			}

			if (CR_RunningArray[0] && CR_RunningArray[2] && currentIndex == 2) {
				attackZone.localPosition = Vector3.zero;
			}

			if (Input.GetButtonDown("Jump") && grounded && !CR_RunningArray[3] && !CR_RunningArray[0]) {
				CR_anim = false;
				StopCoroutine("AnimateIdle");
				resetTransform();
				spriteRend.sprite = spriteDict ["jump"];
				StartCoroutine (ReturnToIdle(new float[]{0.5f, 0.1f, 3.0f}));
				rb.AddForce (Vector3.up * jumpSpeed);
			}
		} else if (!isWaiting) {
			StartCoroutine("WaitForRespawn");
			StopCoroutine("AnimateIdle");
			StopCoroutine("ReturnToIdle");
			StopCoroutine("ReturnToIdle_2");
			isWaiting = true;
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

	void OnCollisionStay(Collision other) {
		if (other.collider.tag == "Ground") {
			grounded = true;
		}
	}

	void OnHit() {
		health -= 15;
		CR_anim = false;
		StopCoroutine("AnimateIdle");
		resetTransform ();
		spriteRend.sprite = spriteDict["hurt"];
		StartCoroutine (ReturnToIdle(new float[]{0.3f, 0, 4.0f}));
		if (health <= 0) {
			isDead = true;
			//Destroy(gameObject);
		}
	}

	void resetTransform () {
		playerSprite.transform.localScale = new Vector3 (playerSprite.transform.localScale.x, 0.1081449f, playerSprite.transform.localScale.z);
		playerSprite.transform.localPosition = Vector3.zero;
		attackZone.localPosition = temp;
		attackZone.localScale = new Vector3 (1f, 1f, 1f);
	}

	Vector3 CastRayToWorld() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Vector3 point = ray.origin + (ray.direction * rayDistance);
		//Debug.Log(point);
		return point;
	}

	IEnumerator ReturnToIdle(float[] _seconds) {
		currentIndex = (int)_seconds [2];
		CR_RunningArray[currentIndex] = true;
		CR_RunningArray[0] = true;
		yield return new WaitForSeconds(_seconds[0]);
		spriteRend.sprite = spriteDict["idle"];
		attackZone.localScale = new Vector3(1f, 1f, 1f);
		CR_RunningArray[0] = false;
		StartCoroutine (ReturnToIdle_2 (new float[]{_seconds [1], _seconds [2]}));
	}

	IEnumerator ReturnToIdle_2(float[] _seconds) {
		yield return new WaitForSeconds(_seconds[0]);
		int _index = (int)_seconds [1];
		CR_RunningArray[_index] = false;
	}

	IEnumerator AnimateIdle() {
		CR_anim = true;
		while (CR_anim) {
			yield return new WaitForSeconds (0.5f);
			playerSprite.transform.localScale = new Vector3 (playerSprite.transform.localScale.x, 0.1081449f, playerSprite.transform.localScale.z);
			playerSprite.transform.localPosition = new Vector3 (0, 0, 0);
			yield return new WaitForSeconds (0.5f);
			playerSprite.transform.localScale = new Vector3 (playerSprite.transform.localScale.x, 0.1054026f, playerSprite.transform.localScale.z);
			playerSprite.transform.localPosition = new Vector3 (0, -0.02f, 0);
			yield return new WaitForSeconds (0.5f);
			playerSprite.transform.localScale = new Vector3 (playerSprite.transform.localScale.x, 0.1039446f, playerSprite.transform.localScale.z);
			playerSprite.transform.localPosition = new Vector3 (0, -0.032f, 0);
			yield return new WaitForSeconds (0.5f);
			playerSprite.transform.localScale = new Vector3 (playerSprite.transform.localScale.x, 0.1054026f, playerSprite.transform.localScale.z);
			playerSprite.transform.localPosition = new Vector3 (0, -0.02f, 0);
		}
	}

	IEnumerator WaitForRespawn() {
		spriteRend.sprite = spriteDict["dead"];
		yield return new WaitForSeconds(1f);
		GameObject test = Instantiate (prefab, respawnLoc, Quaternion.identity) as GameObject;
		mainCameraOffset.GetComponent<Follow>().target = test.transform;
		mainCameraOffset.transform.position = new Vector3 (-16.86f, 8.206226f, -17.93f);
		Destroy (gameObject);
		//dis
	}
}
