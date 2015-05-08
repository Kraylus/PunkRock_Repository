using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class Enemy : MonoBehaviour {

	public float speed = 10.0F;
	public float jumpSpeed = 10.0F;

	public float explosionForce = 1f;
	public float explosionRadius = 1.5f;

	public Transform target; //the enemy's target
	public float moveSpeed = 3f; //move speed
	public float rotationSpeed = 3f; //speed of turning
	public float range = 10f;
	public float range2 = 10f;
	public float stop= 0;
	
	public int health = 100;
	
	public Sprite[] spriteArray;
	
	//private Vector3 lastPos;
	
	public GameObject spriteObj;
	private SpriteRenderer spriteRend;
	
	private Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite> ();
	private bool[] CR_RunningArray;
	private bool CR_anim = false;
	private int currentIndex = 0;
	
	public bool isLeft = true;

	public bool grounded = false;
	
	public Rigidbody rb;
	
	public Transform myTransform; //current transform data of this enemy
	
	public GameObject attackZone;
	
	public bool isDead = false;
	
	public DealDamage dealDamage;

	void Awake () {
		myTransform = transform;  //cache transform data for easy access/preformance
	}

	// Use this for initialization
	void Start () {
		foreach(Sprite _sprite in spriteArray) {
			string _spriteName = _sprite.name.Substring(6);
			spriteDict.Add(_spriteName, _sprite);
			Debug.Log (_spriteName+" : "+_sprite.name);
		}
		CR_RunningArray = new bool[] {false, false, false, false, false};
		target = GameObject.FindWithTag("Player").transform; //target the player
		
		//lastPos = transform.position;


		//rb = gameObject.GetComponent<Rigidbody>();

		spriteRend = spriteObj.GetComponent<SpriteRenderer>();
		spriteRend.sprite = spriteArray[0];
		spriteObj.transform.localScale = new Vector3(-0.1081449f, spriteObj.transform.localScale.y, spriteObj.transform.localScale.z);
		//dealDamage = gameObject.GetComponentsInChildren.<DealDamage>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!isDead || (currentIndex == 2 && CR_RunningArray [0])) {
			float _speed = speed;
			if (Input.GetAxis ("Vertical") != 0.0f && Input.GetAxis ("Horizontal") != 0.0f) {
				_speed *= 0.7f;
			}
			if ((Input.GetAxis ("Vertical") != 0.0f || Input.GetAxis ("Horizontal") != 0.0f) && !CR_RunningArray [0]) {
				StopCoroutine ("AnimateIdle");
				CR_anim = false;
				resetTransform ();
				spriteRend.sprite = spriteDict ["walk"];
			} else if (!CR_RunningArray [0]) {
				if (!(CR_anim)) {
					StartCoroutine ("AnimateIdle");
				}
				spriteRend.sprite = spriteDict ["idle"];
			}
			
			if (!(CR_RunningArray [0] && CR_RunningArray [2] && currentIndex == 2)) {
				float vert = Input.GetAxis ("Vertical") * _speed;
				float hor = Input.GetAxis ("Horizontal") * _speed;
				vert *= Time.deltaTime;
				hor *= Time.deltaTime;
				transform.Translate (hor, 0, vert);
				if (Input.GetAxis ("Horizontal") != 0.0f) {
					if (Input.GetAxis ("Horizontal") < 0.0f) {
						isLeft = true;
					} else {
						isLeft = false;
					}
				}
			}
			//rb.AddForce (Vector3.down * 12.0f);
			
			if (isLeft) {
				attackZone.transform.localPosition = new Vector3 (-0.676f, 0.0f, 0.0f);
				spriteObj.transform.localScale = new Vector3 (-0.1081449f, spriteObj.transform.localScale.y, spriteObj.transform.localScale.z);
			} else {
				attackZone.transform.localPosition = new Vector3 (0.676f, 0.0f, 0.0f);
				spriteObj.transform.localScale = new Vector3 (0.1081449f, spriteObj.transform.localScale.y, spriteObj.transform.localScale.z);
			}
			
			
			if (Input.GetButtonDown ("Fire1") && !CR_RunningArray [1] && !CR_RunningArray [0]) {
				CR_anim = false;
				StopCoroutine ("AnimateIdle");
				resetTransform ();
				spriteRend.sprite = spriteDict ["attack"];
				gameObject.BroadcastMessage ("Punch");
				StartCoroutine (ReturnToIdle (new float[]{0.4f, 0.1f, 1.0f}));
			} 

			if (Input.GetButtonDown ("Jump") && grounded && !CR_RunningArray [3] && !CR_RunningArray [0]) {
				CR_anim = false;
				StopCoroutine ("AnimateIdle");
				resetTransform ();
				spriteRend.sprite = spriteDict ["jump"];
				StartCoroutine (ReturnToIdle (new float[]{0.5f, 0.1f, 3.0f}));
				rb.AddForce (Vector3.up * jumpSpeed);
			}
		} else {
			spriteRend.sprite = spriteDict["dead"];
		}
	}

	void resetTransform () {
		spriteObj.transform.localScale = new Vector3 (spriteObj.transform.localScale.x, 0.1081449f, spriteObj.transform.localScale.z);
		spriteObj.transform.localPosition = Vector3.zero;
	}

	void FixedUpdate () {
		//lastPos = transform.position;
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
		if (!isDead) {
			health -= 34;
			CR_anim = false;
			StopCoroutine ("AnimateIdle");
			resetTransform ();
			spriteRend.sprite = spriteDict ["hurt"];
			StartCoroutine (ReturnToIdle (new float[]{0.3f, 0, 4.0f}));
			if (health <= 0) {
				isDead = true;
				//Destroy(gameObject);
			}
		}
	}

	void OnMegaHit () {
		if (!isDead) {
			health -= 34;
			CR_anim = false;
			StopCoroutine ("AnimateIdle");
			resetTransform ();
			spriteRend.sprite = spriteDict ["dead"];
			StartCoroutine (ReturnToIdle (new float[]{1.5f, 0, 4.0f}));
			rb.AddExplosionForce (explosionForce, target.transform.position, explosionRadius);
			if (health <= 0) {
				isDead = true;
				//Destroy(gameObject);
			}
		}
	}

	IEnumerator ReturnToIdle(float[] _seconds) {
		currentIndex = (int)_seconds [2];
		CR_RunningArray[currentIndex] = true;
		CR_RunningArray[0] = true;
		yield return new WaitForSeconds(_seconds[0]);
		spriteRend.sprite = spriteDict["idle"];
		CR_RunningArray[0] = false;
		StartCoroutine (ReturnToIdle_2 (new float[]{_seconds [1], _seconds [2]}));
	}
	
	IEnumerator ReturnToIdle_2(float[] _seconds) {
		yield return new WaitForSeconds(_seconds[0]);
		int _index = (int)_seconds [1];
		CR_RunningArray[_index] = false;
	}

	IEnumerator KnockBack() {
		CR_RunningArray[0] = true;
		currentIndex = 2;
		spriteRend.sprite = spriteDict["hurt"];
		yield return new WaitForSeconds(0.5f);
		spriteRend.sprite = spriteDict["hurt"];
		yield return new WaitForSeconds(0.7f);
		spriteRend.sprite = spriteDict["idle"];
		CR_RunningArray[0] = false;
		currentIndex = 0;
	}

	IEnumerator AnimateIdle() {
		CR_anim = true;
		while (CR_anim) {
			yield return new WaitForSeconds (0.5f);
			spriteObj.transform.localScale = new Vector3 (spriteObj.transform.localScale.x, 0.1081449f, spriteObj.transform.localScale.z);
			spriteObj.transform.localPosition = new Vector3 (0, 0, 0);
			yield return new WaitForSeconds (0.5f);
			spriteObj.transform.localScale = new Vector3 (spriteObj.transform.localScale.x, 0.1054026f, spriteObj.transform.localScale.z);
			spriteObj.transform.localPosition = new Vector3 (0, -0.02f, 0);
			yield return new WaitForSeconds (0.5f);
			spriteObj.transform.localScale = new Vector3 (spriteObj.transform.localScale.x, 0.1039446f, spriteObj.transform.localScale.z);
			spriteObj.transform.localPosition = new Vector3 (0, -0.032f, 0);
			yield return new WaitForSeconds (0.5f);
			spriteObj.transform.localScale = new Vector3 (spriteObj.transform.localScale.x, 0.1054026f, spriteObj.transform.localScale.z);
			spriteObj.transform.localPosition = new Vector3 (0, -0.02f, 0);
		}
	}
}
