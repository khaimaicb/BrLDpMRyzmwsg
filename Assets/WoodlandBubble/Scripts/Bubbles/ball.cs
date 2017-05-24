using UnityEngine;
using System.Collections;
using System.Threading;

public class ball : MonoBehaviour {
	public Sprite[] sprites;

//	 public OTSprite sprite;                    // This star's sprite class
    Vector2 speed =                     // Star movement speed / second
        new Vector2(250, 250);          
	public Vector3 target;
	Vector2 worldPos;
	Vector3 forceVect;
	public bool setTarget;
	public float startTime;
	float duration = 1.0f;
	public GameObject mesh;
	Vector2[] meshArray;
	public bool findMesh;
	Vector3 dropTarget;
	float row;
	string str;
	public bool newBall;
	float mTouchOffsetX;
	float mTouchOffsetY;
	float xOffset;
	float yOffset;
	public Vector3 targetPosition;
	bool stopedBall;
	public bool destroyed;
	public ArrayList nearBalls = new ArrayList();
//	private OTSpriteBatch spriteBatch = null;  
	GameObject Meshes;
	public int countNEarBalls;
	float bottomBorder;
	float topBorder;
	float leftBorder;
	float rightBorder;
	float gameOverBorder;
	bool gameOver;
	bool isPaused;
	public AudioClip swish;
	public AudioClip pops;
	public AudioClip join;
	Vector3 meshPos;
	bool dropedDown;
	bool rayTarget;
	RaycastHit2D[] bugHits;
	RaycastHit2D[] bugHits2;
	RaycastHit2D[] bugHits3;
    public bool animStarted;

	// Use this for initialization
	void Start () {
			meshPos = new Vector3(-1000, - 1000, - 10);
      //  sprite = GetComponent<OTSprite>();
		//sprite.passive = true;
	//	sprite.onCollision = OnCollision;
		dropTarget = transform.position;
//		spriteBatch = GameObject.Find("SpriteBatch").GetComponent<OTSpriteBatch>();    
		Meshes = GameObject.Find("-Balls");
		// Add the custom tile action controller to this tile
  //      sprite.AddController(new MyActions(this));  
		
		bottomBorder = Camera.main.GetComponent<mainscript>().bottomBorder;
		topBorder = Camera.main.GetComponent<mainscript>().topBorder;
		leftBorder = Camera.main.GetComponent<mainscript>().leftBorder;
		rightBorder = Camera.main.GetComponent<mainscript>().rightBorder;
		gameOverBorder = Camera.main.GetComponent<mainscript>().gameOverBorder;
		gameOver = Camera.main.GetComponent<mainscript>().gameOver;
		isPaused = Camera.main.GetComponent<mainscript>().isPaused;
		dropedDown = Camera.main.GetComponent<mainscript>().dropingDown;
	}

	IEnumerator AllowLaunchBall(){
		yield return new WaitForSeconds(2);
		mainscript.StopControl = false;

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonUp(0) ){
			GameObject ball = gameObject;
			if(!mainscript.Instance.notAllowDropDown && !setTarget && newBall && Camera.main.GetComponent<mainscript>().dropDownTime < Time.time && GamePlay.Instance.GameStatus == GameState.Playing) {
				worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

				if(worldPos.y < 5.5 && worldPos.y > -3.5 && !mainscript.StopControl){

					SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.Swish);
				//	AudioSource.PlayClipAtPoint(join, new Vector3(5, 1, 2));
					mTouchOffsetX = (worldPos.x - ball.transform.position.x); //+ MathUtils.random(-10, 10);
		            mTouchOffsetY = (worldPos.y - ball.transform.position.y);
		            xOffset = (float) Mathf.Cos( Mathf.Atan2(mTouchOffsetY, mTouchOffsetX ));
		            yOffset = (float) Mathf.Sin( Mathf.Atan2(mTouchOffsetY, mTouchOffsetX ));
			        speed = new Vector2(xOffset *10  , yOffset *10 );
					
					target = speed;
					if(name.IndexOf("bug")<0){
						int layerMask = 1<< LayerMask.NameToLayer("Ball");
						RaycastHit2D hit = Physics2D.Raycast  (gameObject.transform.position, transform.position  + target * 100, 10, layerMask);
						if (hit.collider != null){
					//		Debug.DrawRay(gameObject.transform.position, transform.position  + target * 100, Color.red);
							target = hit.collider.gameObject.transform.position;
	         			 //   TacoConfig.print(hit.collider.gameObject+" "+target);
							rayTarget = true;
							//GetComponent<bouncer>().bounceTo(target);
						}
					}
					if(name.IndexOf("bug")==0){
						int layerMask = 1<< LayerMask.NameToLayer("Ball");
						Vector3 vector3 = new Vector3(transform.position.x-15f, transform.position.y, -10f);
						bugHits = Physics2D.RaycastAll(vector3, transform.position  + target * 100, 20f, layerMask);
					//	Debug.DrawRay(vector3, transform.position  + target * 100, Color.red);
						vector3 = new Vector3(transform.position.x+15f, transform.position.y, -10f);
						bugHits2 = Physics2D.RaycastAll  (vector3, transform.position  + target * 100, 20f, layerMask);
					//	Debug.DrawRay(vector3, transform.position  + target * 100, Color.red);
						vector3 = new Vector3(transform.position.x, transform.position.y, -10f);
						bugHits3 = Physics2D.RaycastAll  (vector3, transform.position  + target * 100, 20f, layerMask);
					//	Debug.DrawRay(vector3, transform.position  + target * 100, Color.red);
					}
					
					setTarget = true;
					startTime = Time.time;
                    //Vector3 relativePos = target - transform.position;
                    //float angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
                    //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward) * Quaternion.Euler(0, 0, -90);
                    //RaycastHit2D hitRay = Physics2D.Raycast(transform.position, transform.position + ((Vector3)worldPos - transform.position ).normalized * 100, 10, LayerMask.NameToLayer("Ball"));
                    //if (hitRay != null) TacoConfig.print(hitRay.collider.gameObject);
                }
			}
		}
		if(transform.position != target && setTarget && !stopedBall && !isPaused && Camera.main.GetComponent<mainscript>().dropDownTime <Time.time ){ 
			dropTarget = transform.position;
			if(name.IndexOf("bug")<0 && rayTarget ){

				transform.position = Vector3.Lerp(dropTarget, target,  (Time.time - startTime)*0.7f);
			}
			else{
                //Vector3 relativePos = target - transform.position;
                //float angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
                //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward) * Quaternion.Euler(0, 0, 90);

				transform.position  += target * Time.deltaTime*2f;
			/*	if(name.IndexOf("bug")<0 && !rayTarget ){
					rayCasting(transform.position);
				}*/
			}
		//	 rigidbody.AddForce(target * 100);
		}		
		if(setTarget)
			triggerEnter();

        /*for(int i=0; i<10000; i++){
			GameObject g = gm[i];
		}*/
}

	void FixedUpdate(){
		
		if(stopedBall && GamePlay.Instance.GameStatus == GameState.Playing){
			
			transform.position = meshPos;
			stopedBall = false;
			if(newBall){
				newBall = false;
				gameObject.layer = 9;
				Camera.main.GetComponent<mainscript>().checkBall = gameObject;
				this.enabled = false;
			}

		}

	}
	
	
	public bool findInArray(ArrayList b, GameObject destObj){
		foreach(GameObject obj in b) {
			
			if(obj == destObj) return true;
		}
		return false;
	}
	
	public ArrayList addFrom(ArrayList b, ArrayList b2){
		foreach(GameObject obj in b) {
			if(!findInArray(b2, obj)){
				b2.Add(obj);
			}
		}
		return b2;
	}
	
	public void changeNearestColor(){
		GameObject gm = GameObject.Find ("Creator");
		Collider2D[] fixedBalls = Physics2D.OverlapCircleAll(transform.position, 0.5f, 1<<9);
		foreach(Collider2D obj in fixedBalls) {
		    gm.GetComponent<creatorBall>().createBall(obj.transform.position);
			Destroy(obj.gameObject);
		}
		
	}

	
	public void checkNextNearestColor(ArrayList b, int counter){
//		Debug.Log(b.Count);
		Vector3 distEtalon = transform.localScale;
//		GameObject[] meshes = GameObject.FindGameObjectsWithTag(tag);
//		foreach(GameObject obj in meshes) {
		int layerMask = 1<< LayerMask.NameToLayer("Ball");
		Collider2D[] meshes = Physics2D.OverlapCircleAll(transform.position, 1.0f, layerMask); 
		foreach(Collider2D obj1 in meshes) {
			if(obj1.gameObject.tag == tag){
				GameObject obj = obj1.gameObject;
				float distTemp = Vector3.Distance(transform.position, obj.transform.position);
				if(distTemp <= 1.0f  ){
					if(!findInArray(b, obj)){
						counter ++;
						b.Add(obj); 
						obj.GetComponent<bouncer>().checkNextNearestColor( b, counter);
				//		destroy();
						//obj.GetComponent<mesh>().checkNextNearestColor();
				//		obj.GetComponent<mesh>().destroy();
					}
				}
			}
		}
	}

	public void checkNearestColor(){
		int counter = 0;
		GameObject[] fixedBalls = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];			// change color tag of the rainbow
		foreach(GameObject obj in fixedBalls) {
			if(obj.layer == 9 && (obj.name.IndexOf("Rainbow")>-1)){ 
				obj.tag = tag;
			}
		}
		
		ArrayList b = new ArrayList() ;
		b.Add(gameObject);
		Vector3 distEtalon = transform.localScale;
		GameObject[] meshes = GameObject.FindGameObjectsWithTag(tag);
		foreach(GameObject obj in meshes) {    													// detect the same color balls
			float distTemp = Vector3.Distance(transform.position, obj.transform.position);
			if(distTemp <= 0.9f && distTemp > 0 ){
				b.Add(obj); 
				obj.GetComponent<bouncer>().checkNextNearestColor( b, counter);
			}
		}
		if(b.Count >= 3 ){
			destroy (b);
		}
		if(b.Count < 3 ){
			Camera.main.GetComponent<mainscript>().bounceCounter ++;
			
		}
		
		b.Clear();
	//	destroyAloneBall();
	//	Camera.main.GetComponent<mainscript>().destroyAloneBall();
		Camera.main.GetComponent<mainscript>().dropingDown = false;
	//	StartCoroutine(Camera.main.GetComponent<mainscript>().destroyAloneBall());
	//	Debug.Log(Camera.main.GetComponent<mainscript>().arraycounter);
		//Camera.main.GetComponent<mainscript>().connectNearBallsGlobal();
	}
	
	void destroyAloneBall(){
		Camera.main.GetComponent<mainscript>().destroyAloneBallDelayed();
	}
		
	public bool checkNearestBall(ArrayList b){
		if(transform.position.y >= 5.5f) {
			Camera.main.GetComponent<mainscript>().controlArray = addFrom(b, Camera.main.GetComponent<mainscript>().controlArray);
			b.Clear();
			return true;    /// don't destroy
		}
		if(findInArray(Camera.main.GetComponent<mainscript>().controlArray, gameObject)){b.Clear(); return true;} /// don't destroy
		b.Add(gameObject); 
		foreach(GameObject obj in nearBalls) {
			if(obj != gameObject && obj!=null){
	 		if(obj.gameObject.layer == 9){
				//if(findInArray(Camera.main.GetComponent<mainscript>().controlArray, obj.gameObject)){b.Clear(); return true;} /// don't destroy
				//else{
					float distTemp = Vector3.Distance(transform.position, obj.transform.position);
					if(distTemp <= 1f && distTemp > 0 ){
					if(!findInArray(b, obj.gameObject)){
						Camera.main.GetComponent<mainscript>().arraycounter ++;
							if(obj.GetComponent<ball>().checkNearestBall( b))
								return true;
						}
					}
				//}
			}
			}
		}
		return false;
		
	}
	
	public void connectNearBalls(){
		int layerMask = 1<< LayerMask.NameToLayer("Ball");
		Collider2D[] fixedBalls = Physics2D.OverlapCircleAll(transform.position, 0.5f, layerMask);
		nearBalls.Clear();
		foreach(Collider2D obj in fixedBalls) {
            if(nearBalls.Count<=7){
//				LineRenderer line = obj.gameObject.GetComponent<LineRenderer>();
//				line.SetWidth(0.05f,0.05f);
//				line.SetPosition(0, transform.position + Vector3.back*5);
//				line.SetPosition(1, obj.transform.position + Vector3.back*5);

			    nearBalls.Add( obj.gameObject);
			}
		}
		countNEarBalls = nearBalls.Count;
	}
	
	IEnumerator pullToMesh(){
        //	AudioSource.PlayClipAtPoint(join, new Vector3(5, 1, 2));
        creatorBall.Instance.EnableGridColliders();

        dropTarget = transform.position;
        target = transform.position;
        mainscript.StopControl = true;
		GameObject busyMesh = null;
		float searchRadius = 0.2f;
		while(findMesh){				
//			if(other.gameObject.tag == "Mesh"){	
			Collider2D[] fixedBalls1 = Physics2D.OverlapCircleAll(transform.position, 0.1f, 1<<10);  //meshes

            foreach (Collider2D obj1 in fixedBalls1)
            {
                if (obj1.gameObject.GetComponent<Grid>().busy == null)
                {
                    findMesh = false;
                    stopedBall = true;
                    if (meshPos.y <= obj1.gameObject.transform.position.y)
                    {
//                        Debug.Log("meshes around " + obj1 + "!! " + obj1.gameObject.transform.position);
                        meshPos = obj1.gameObject.transform.position;
                   //     Debug.Log(meshPos);
                        busyMesh = obj1.gameObject;
                    }
                    //StopCoroutine("pullToMesh");
                    //break;
                    //yield return new WaitForSeconds(1f / 10f);
                }
                else if (obj1.gameObject.GetComponent<Grid>().busy.GetComponent<ball>().destroyed)
                {
                    findMesh = false;
                    stopedBall = true;
                    if (meshPos.y <= obj1.gameObject.transform.position.y)
                    {
                        //                        Debug.Log("meshes around " + obj1 + "!! " + obj1.gameObject.transform.position);
                        meshPos = obj1.gameObject.transform.position;
                        //     Debug.Log(meshPos);
                        busyMesh = obj1.gameObject;
                    }
                    //StopCoroutine("pullToMesh");
                    //break;
                    //yield return new WaitForSeconds(1f / 10f);

                }
            }
            if (findMesh)
            {
                Collider2D[] fixedBalls = Physics2D.OverlapCircleAll(transform.position, searchRadius, 1 << 10);  //meshes
                foreach (Collider2D obj in fixedBalls)
                {
                    if (obj.gameObject.GetComponent<Grid>().busy == null)
                    {
                        findMesh = false;
                        stopedBall = true;


                        if (meshPos.y <= obj.gameObject.transform.position.y)
                        {
                            meshPos = obj.gameObject.transform.position;
                            busyMesh = obj.gameObject;
                        }

                        //yield return new WaitForSeconds(1f/10f);
                    }
                    else if (obj.gameObject.GetComponent<Grid>().busy.GetComponent<ball>().destroyed)
                    {
                        findMesh = false;
                        stopedBall = true;


                        if (meshPos.y <= obj.gameObject.transform.position.y)
                        {
                            meshPos = obj.gameObject.transform.position;
                            busyMesh = obj.gameObject;
                        }

                        //yield return new WaitForSeconds(1f/10f);
                    }
                }
            }
			if(busyMesh!=null){
                gameObject.GetComponent<BoxCollider2D>().size = new Vector2(0.6f, 0.6f);
                gameObject.GetComponent<BoxCollider2D>().offset = Vector2.zero;
                gameObject.GetComponent<CircleCollider2D>().enabled = true;
                gameObject.GetComponent<CircleCollider2D>().radius = 0.34f;

                Destroy(gameObject.GetComponent<Rigidbody2D>());

				busyMesh.GetComponent<Grid>().busy = gameObject;
				gameObject.GetComponent<bouncer>().offset = busyMesh.GetComponent<Grid>().offset;
                //if (busyMesh != null)
                //{
                //    Hashtable animTable = mainscript.Instance.animTable;
                //    animTable.Clear();
                //    PlayHitAnim(transform.position, animTable);
                //}

			}
			StopCoroutine("pullToMesh");

			dropTarget = transform.position;
			if(findMesh) searchRadius += 0.2f;
            yield return new WaitForFixedUpdate();
		}
        transform.SetParent(creatorBall.Instance.Meshes.transform);
        creatorBall.Instance.OffGridColliders();


    }

    public void PlayHitAnim(Vector3 newBallPos, Hashtable animTable)
    {
        mainscript.Instance.notAllowDropDown = true;
        int layerMask = 1 << LayerMask.NameToLayer("Ball");
        Collider2D[] fixedBalls = Physics2D.OverlapCircleAll(transform.position, 0.5f, layerMask);
        float force = 0.05f;
        foreach (Collider2D obj in fixedBalls)
        {
            if (!animTable.ContainsKey(obj.gameObject) && obj.gameObject != gameObject && animTable.Count < 50)
                obj.GetComponent<ball>().PlayHitAnimCorStart(newBallPos, force, animTable);
        }
        if (fixedBalls.Length > 0 && !animTable.ContainsKey(gameObject))
            PlayHitAnimCorStart(fixedBalls[0].gameObject.transform.position, 0, animTable);
    }

    public void PlayHitAnimCorStart(Vector3 newBallPos, float force, Hashtable animTable)
    {
        if (!animStarted)
        {
            StartCoroutine(PlayHitAnimCor(newBallPos, force, animTable));
            PlayHitAnim(newBallPos, animTable);
        }
    }

    public IEnumerator PlayHitAnimCor(Vector3 newBallPos, float force, Hashtable animTable)
    {
        animStarted = true;
        animTable.Add(gameObject, gameObject);
        if (tag == "potion") yield break;
        yield return new WaitForFixedUpdate();
        float dist = Vector3.Distance(transform.position, newBallPos);
        force = 1 / dist + force;
        newBallPos = transform.position - newBallPos;
        if (transform.parent == null)
        {
            animStarted = false;
            yield break;
        }
        newBallPos = Quaternion.AngleAxis(transform.parent.rotation.eulerAngles.z, Vector3.back) * newBallPos;
        newBallPos = newBallPos.normalized;
        newBallPos = transform.localPosition + (newBallPos * force / 10);

        float startTime = Time.time;
        Vector3 startPos = transform.localPosition;
        float speed = force * 30;
        float distCovered = 0;
        while (distCovered < 1 && !float.IsNaN(newBallPos.x))
        {
            distCovered = (Time.time - startTime) * speed;
            if (this == null) yield break;
            //   if( destroyed ) yield break;
            transform.localPosition = Vector3.Lerp(startPos, newBallPos, distCovered);
            yield return new WaitForEndOfFrame();
        }
        Vector3 lastPos = transform.localPosition;
        startTime = Time.time;
        distCovered = 0;
        while (distCovered < 1 && !float.IsNaN(newBallPos.x))
        {
            distCovered = (Time.time - startTime) * speed;
            if (this == null) yield break;
            transform.localPosition = Vector3.Lerp(lastPos, startPos, distCovered);
            yield return new WaitForEndOfFrame();
        }
        transform.localPosition = startPos;
        animStarted = false;
        mainscript.Instance.startCheckForDropDown = true;

    }

	
	/*void OnCollisionEnter(Collision other){
        // stop
        if (other.gameObject.name.IndexOf("ball") == 0 && setTarget)
        {
			target = Vector2.zero;
			setTarget = false;
			findMesh = true;
		//	sprite._physics = OTObject.Physics.StaticBody;
        }
        if (other.gameObject.name == "top" && target.y > 0 && setTarget)
        {
			target = Vector2.zero;
			setTarget = false;
			findMesh = true;
          // target = new Vector2(target.x, target.y * -1);
        }
        else
            // check if we collided with a bottom block and adjust our speed and rotation accordingly
            if (other.gameObject.name == "bottom" && target.y < 0)
            {
                target = new Vector2(target.x, target.y * -1);
           }
            else
                // check if we collided with a left block and adjust our speed and rotation accordingly
                if (other.gameObject.name == "left" && target.x < 0)
                {
                    target = new Vector2(target.x * -1, target.y);
                 }
                else
                    // check if we collided with a right block and adjust our speed and rotation accordingly
                    if (other.gameObject.name == "right" && target.x > 0)
                    {
                        target = new Vector2(target.x * -1, target.y);
                     }
	}
	
/*	void OnCollisionStay(Collision other){
		if(findMesh)
			pullToMesh(other.gameObject);
	}*/

	
	void OnTriggerStay2D(Collider2D other){
		if(findMesh && other.gameObject.layer == 9){
		//	StartCoroutine(pullToMesh());
		}
	/*	if(other.gameObject.layer == 9 && name.IndexOf("bug")<0)
			findMesh = true;*/
	}
/*	public void OnCollision(OTObject owner){
        if (owner.collisionObject.name.IndexOf("ball") == 0 && setTarget  && name.IndexOf("bug")<0)
        {
			target = Vector2.zero;
			setTarget = false;
			findMesh = true;
       }
	}*/
	
	void OnTriggerEnter2D(Collider2D other)
	{
		// stop
        if (other.gameObject.name.Contains("ball") && setTarget && name.IndexOf("bug") < 0 )
        {
			if(gameObject.transform.position.y > GameObject.Find("GameOverBorder").transform.position.y){

				//transform.position = new Vector3(transform.position.x, transform.position.y - 25f/640f*Camera.main.orthographicSize, transform.position.z);
				target = Vector2.zero;
				setTarget = false;
				findMesh = true;
				StartCoroutine(pullToMesh());
			}
        }
        //else if (mainscript.Instance.targetSquare != null)
        //{
        //    if (Vector3.Distance(transform.position, mainscript.Instance.targetSquare.transform.position) <= 1.5f)
        //    {

        //        target = Vector2.zero;
        //        setTarget = false;
        //        findMesh = true;
        //        transform.position = mainscript.Instance.targetSquare.transform.position;
        //        mainscript.Instance.targetSquare = null;
        //        StartCoroutine(pullToMesh());

        //    }
        //}

        //else if (other.gameObject.name.IndexOf("ball") == 0 && setTarget && name.IndexOf("bug")==0)
        //{
        //    if(other.gameObject.tag == gameObject.tag){
        //        Destroy(other.gameObject);
        //        mainscript.Instance.PopupScore( 3, transform.position);
        //    }
        //}
	}

	void triggerEnter()
	{
        // stop
 /*       if (other.gameObject.name.IndexOf("ball") == 0 && setTarget && name.IndexOf("bug")<0)
        {
			target = Vector2.zero;
			setTarget = false;
			findMesh = true;
			sprite.collisionDepth = 1;
        }
        else if (other.gameObject.name.IndexOf("ball") == 0  && name.IndexOf("bug")==0)
        {
			if(other.gameObject.tag == gameObject.tag){
				Destroy(other.gameObject);
				Camera.main.GetComponent<mainscript>().score += 3;
			}
        }*/
		
 /*       if (transform.position.y <= gameOverBorder && gameObject.layer == 8 && name.IndexOf("bug")<0)
        {
			Camera.main.GetComponent<mainscript>().gameOver = true;
        }*/
		
        if (transform.position.y >= topBorder && name.IndexOf("bug")==0)
        {
		
			destroy(gameObject);
			Camera.main.GetComponent<mainscript>().dropingDown = false;
		}

        if (transform.position.y >= topBorder && target.y > 0 && name.IndexOf("bug") < 0)
        {
			if(!findMesh){
				target = Vector2.zero;
				transform.position = new Vector3(transform.position.x, transform.position.y - transform.localScale.y, transform.position.z);
				setTarget = false;
				findMesh = true;
				StartCoroutine(pullToMesh());
			}
          // target = new Vector2(target.x, target.y * -1);
        }
        else if (!rayTarget)
        {
            // check if we collided with a bottom block and adjust our speed and rotation accordingly
            if (transform.position.y <= bottomBorder && target.y < 0)
            {
                target = new Vector2(target.x, target.y * -1);
            }
            else
                // check if we collided with a left block and adjust our speed and rotation accordingly
                if (transform.position.x <= leftBorder && target.x < 0)
                {
                    if (name.IndexOf("bug") == 0)
                    {
                        destroy(gameObject);
                        destroyAloneBall();
                    }
                    else
                        target = new Vector2(target.x * -1, target.y);
                }
                else
                    // check if we collided with a right block and adjust our speed and rotation accordingly
                    if (transform.position.x >= rightBorder && target.x > 0)
                    {
                        if (name.IndexOf("bug") == 0)
                        {
                            destroy(gameObject);
                            destroyAloneBall();
                        }
                        else
                            target = new Vector2(target.x * -1, target.y);
                    }
        }
	}
    public void destroy( ArrayList b, float speed = 0.1f )
    {
        StartCoroutine( DestroyCor( b, speed ) );
    }

    IEnumerator DestroyCor( ArrayList b, float speed = 0.1f ){
        ArrayList l = new ArrayList();
        foreach (GameObject obj in b)
        {
            l.Add(obj);
        }

		Camera.main.GetComponent<mainscript>().bounceCounter = 0;
		int scoreCounter = 0;
		int rate = 0;
		int soundPool=0;
		foreach(GameObject obj in l) {
			if(obj.name.IndexOf("ball")==0) obj.layer = 0;
			if(soundPool<5)
				obj.GetComponent<ball>().growUpPlaySound();
			else
				obj.GetComponent<ball>().growUp();
			soundPool++;
 		   GetComponent<Collider2D>().enabled = false;
			if(scoreCounter > 3){
				rate +=3;
				scoreCounter += rate;
			}
			scoreCounter ++;
			obj.GetComponent<ball>().destroyed = true;
	//		Destroy(obj);

			Camera.main.GetComponent<mainscript>().explode(obj.gameObject);
            //if (b.Count < 10 || soundPool % 20 == 0)
            //    yield return new WaitForSeconds(speed);
        }
		if(name.IndexOf("bug")<0)
            mainscript.Instance.PopupScore(scoreCounter, transform.position);
                yield return new WaitForSeconds(speed);

	}

	public void destroy(){
		growUpPlaySound();
		destroy(gameObject);
	}
	
	public void destroy( GameObject obj){
		if(obj.name.IndexOf("bug")==0){
			foreach(RaycastHit2D hitObj in bugHits) {
				if(hitObj.collider != null){
					if(obj.tag == hitObj.collider.gameObject.tag)
						destroy(hitObj.collider.gameObject);
				}
			}
			foreach(RaycastHit2D hitObj in bugHits2) {
				if(hitObj.collider != null){
					if(obj.tag == hitObj.collider.gameObject.tag)
						destroy(hitObj.collider.gameObject);
				}
			}
			foreach(RaycastHit2D hitObj in bugHits3) {
				if(hitObj.collider != null){
					if(obj.tag == hitObj.collider.gameObject.tag)
						destroy(hitObj.collider.gameObject);
				}
			}
			destroyAloneBall();

		}
		if(obj.name.IndexOf("ball")==0) obj.layer = 0;

		Camera.main.GetComponent<mainscript>().bounceCounter = 0;
	//	collider.enabled = false;
		obj.GetComponent<ball>().destroyed = true;
	//	Destroy(obj);
		//obj.GetComponent<ball>().growUpPlaySound();
		obj.GetComponent<ball>().growUp();
	//	Invoke("playPop",1/(float)Random.Range(2,10));
		Camera.main.GetComponent<mainscript>().explode(obj.gameObject);
        //if(name.IndexOf("bug")<0)
        //    mainscript.Instance.PopupScore(3, transform.position);

	}
	
	public void growUp(){
		StartCoroutine(explode());
	}

	public void growUpPlaySound(){
		Invoke("growUpDelayed",0);
	}

	public void growUpDelayed(){
		SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot(SoundBase.Instance.Pops);
	//	AudioSource.PlayClipAtPoint(pops, new Vector3(5, 1, 2));
//		if(!Camera.main.GetComponent<mainscript>().noSound)
//			audio.Play();
		StartCoroutine(explode());
	}
	
	void playPop(){
	//	if(!Camera.main.GetComponent<mainscript>().noSound)
		//	audio.Play();
	}


	IEnumerator explode(){
		float startTime = Time.time;
		float endTime = Time.time+0.4f;
		Vector3 tempPosition = transform.localScale;
		Vector3 targetPrepare = transform.localScale*1.2f;
        
        if (GetComponent<CircleCollider2D>()!= null)
            GetComponent<CircleCollider2D>().enabled = false;
        if (GetComponent<BoxCollider2D>() != null)
            GetComponent<BoxCollider2D>().enabled = false;

		
		while (!isPaused && transform.localScale!= targetPrepare && endTime>Time.time){
			//transform.position  += targetPrepare * Time.deltaTime;
			transform.localScale = Vector3.Lerp(tempPosition, targetPrepare,  (Time.time - startTime)*50);
			//	transform.position  = targetPrepare ;
			yield return new WaitForEndOfFrame();
		}
        GameObject prefab = Resources.Load("Particles/BubbleExplosion") as GameObject;
        GameObject explosion = (GameObject)Instantiate(prefab, gameObject.transform.position + Vector3.back * 20f, Quaternion.identity);

		if(!isPaused)
			Destroy(gameObject);
	}
	
/*	 void OnDestroy() {
		if(gameObject.name.IndexOf("bug")<0)
        	AudioSource.PlayClipAtPoint(pops, transform.position);
    }*/
	
	/*public bool isDropDownRunning(){
		if(sprite!=null)
			return Actions(sprite.GetComponent<OTObject>()).IsRunning(str);
		else
			return false;
	}*/

}
