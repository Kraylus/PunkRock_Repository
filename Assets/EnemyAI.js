 var target : Transform; //the enemy's target
 var moveSpeed = 3; //move speed
 var rotationSpeed = 3; //speed of turning
 var range : float=10f;
 var range2 : float=10f;
 var stop : float=0;
 ;
 var health : int=100;
 
 var spriteArray : Sprite[];
 
 private var lastPos : Vector3;
 
 var spriteObj : GameObject;
 private var spriteRend : SpriteRenderer;
 
 var CR_Running : boolean = false;
 
 var isLeft : boolean = true;
 
 var myTransform : Transform; //current transform data of this enemy
 
 var attackZone : GameObject;
 
 var isDead : boolean = false;
 
 var dealDamage : DealDamage;
 
 function Awake()
 {
     myTransform = transform; //cache transform data for easy access/preformance
 }
  
 function Start()
 {
      	target = GameObject.FindWithTag("Player").transform; //target the player
      	
  		lastPos = transform.position;
  		
  		spriteRend = spriteObj.GetComponent.<SpriteRenderer>();
  		spriteRend.sprite = spriteArray[0];
  		spriteObj.transform.localScale = new Vector3(-0.1081449f, spriteObj.transform.localScale.y, spriteObj.transform.localScale.z);
  		//dealDamage = gameObject.GetComponentsInChildren.<DealDamage>();
 }
  
 function OnHit() {
 	health -= 34;
 	if (health <= 0) {
 		isDead = true;
 		//Destroy(gameObject);
 	}
 }
 
 function OnMegaHit() {
 	health -= 66;
 	if (health <= 0) {
 		isDead = true;
 		//Destroy(gameObject);
 	}
 }
  
 function Update () {
     //rotate to look at the player
     target = GameObject.FindWithTag("Player").transform;
     if (!isDead) {
	     var distance = Vector3.Distance(myTransform.position, target.position);
	     if (transform.position != lastPos && !CR_Running) {
	     	spriteRend.sprite = spriteArray[2];
	     	if (transform.position.x < lastPos.x) {
	     		isLeft = true;
	     		spriteObj.transform.localScale = new Vector3(-0.1081449f, spriteObj.transform.localScale.y, spriteObj.transform.localScale.z);
	     		attackZone.transform.localPosition = new Vector3(-0.676f, 0f, 0f);
	     	} else if (transform.position.x > lastPos.x) {
	     		isLeft = false;
	     		spriteObj.transform.localScale = new Vector3(0.1081449f, spriteObj.transform.localScale.y, spriteObj.transform.localScale.z);
				attackZone.transform.localPosition = new Vector3(0.676f, 0f, 0f);
	     	}
	      } 
	     if (distance<=range2 &&  distance>=range){
	//     	myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed*Time.deltaTime);
	     } else if(distance<=range && distance>stop){
	 
	     //move towards the player
	//     myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
	//     Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed*Time.deltaTime);
		 var testVect : Vector3 = (target.position - myTransform.position);
	     myTransform.position += testVect * (moveSpeed *Time.deltaTime);
	     }
	     else if (distance<=stop) {
	     	if (!CR_Running) {
	     		spriteRend.sprite = spriteArray[1];
	     		gameObject.BroadcastMessage("Punch");
	     		StartCoroutine(ReturnToIdle(0.5f, Random.value));
	     	}
	//     myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
	//     Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed*Time.deltaTime);
	     }
     } else {
     	spriteRend.sprite = spriteArray[4];
     }
 	
 }
 
 function ReturnToIdle (spriteReset : float, waitTime : float) {
		// suspend execution for waitTime seconds
		CR_Running = true;

		yield WaitForSeconds (spriteReset);
		spriteRend.sprite = spriteArray[0];
		yield WaitForSeconds (waitTime);
		CR_Running = false;
	}
 
 function FixedUpdate () {
 	lastPos = transform.position;
 }
 