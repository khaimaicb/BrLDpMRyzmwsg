using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class creatorBall : MonoBehaviour
{
    public static creatorBall Instance;

    public GameObject ball_hd;
    public GameObject ball_ld;
    public GameObject bug_hd;
    public GameObject bug_ld;
    public GameObject thePrefab;
    GameObject ball;
    GameObject bug;
    string[] ballsForCatapult = new string[11];
    string[] ballsForMatrix = new string[11];
    string[] bugs = new string[11];
    int columns = 10;
    int rows = 14;
    float offsetStep = 0.30f;
    public List<GameObject> squares = new List<GameObject>();

    public GameObject Meshes;
    // Use this for initialization
    void Start()
    {
        Instance = this;

        ball = ball_hd;
        bug = bug_hd;
        thePrefab.transform.localScale = new Vector3(0.68f, 0.6f, 1);
    }

    public void InitGame()
    {
        mainscript.score = 0;
        mainscript.stage = 1;
        Meshes = GameObject.Find("-Ball");
        foreach (Transform obj1 in Meshes.transform)
        {
            Destroy(obj1.gameObject);
        }
        GamePlay.Instance.watchedVideo = false;
        GameObject[] meshes = GameObject.FindGameObjectsWithTag("Mesh");
        foreach (GameObject obj1 in meshes)
        {
            Destroy(obj1);
        }

        
        squares.Clear();

        ballsForCatapult[0] = "ball_Orange";
        ballsForCatapult[1] = "ball_Red";
        ballsForCatapult[2] = "ball_Yellow";
        ballsForCatapult[3] = "ball_Pearl";
        ballsForCatapult[4] = "ball_Blue";
        ballsForCatapult[5] = "ball_Violet";
        ballsForCatapult[6] = "ball_Brown";
        ballsForCatapult[7] = "ball_Pink";
        ballsForCatapult[8] = "ball_Gray";
        ballsForCatapult[9] = "ball_DarkBlue";
        ballsForCatapult[10] = "ball_Green";

        ballsForMatrix[0] = "ball_Orange";
        ballsForMatrix[1] = "ball_Red";
        ballsForMatrix[2] = "ball_Yellow";
        ballsForMatrix[3] = "ball_Rainbow";
        ballsForMatrix[4] = "ball_Blue";
        ballsForMatrix[5] = "ball_Violet";
        ballsForMatrix[6] = "ball_Brown";
        ballsForMatrix[7] = "ball_Pink";
        ballsForMatrix[8] = "ball_Gray";
        ballsForMatrix[9] = "ball_DarkBlue";
        ballsForMatrix[10] = "ball_Green";

        bugs[0] = "bug_Orange";
        bugs[1] = "bug_Red";
        bugs[2] = "bug_Yellow";
        bugs[3] = "bug_Red";
        bugs[4] = "bug_Blue";
        bugs[5] = "bug_Violet";
        bugs[6] = "bug_Brown";
        bugs[7] = "bug_Pink";
        bugs[8] = "bug_Gray";
        bugs[9] = "bug_DarkBlue";
        bugs[10] = "bug_Green";


        createMesh();
        createRow(0);
        createRow(1);
        createRow(2);
        createRow(3);
        createRow(4);
        Camera.main.GetComponent<mainscript>().connectNearBallsGlobal();
        StartCoroutine(getBallsForMesh());

    }

    // Update is called once per frame
    void Update()
    {
	
    }

    IEnumerator getBallsForMesh()
    {
        GameObject[] meshes = GameObject.FindGameObjectsWithTag("Mesh");
        foreach (GameObject obj1 in meshes)
        {    											
            Collider2D[] fixedBalls = Physics2D.OverlapCircleAll(obj1.transform.position, 0.2f, 1 << 9);  //balls
            foreach (Collider2D obj in fixedBalls)
            {
                obj1.GetComponent<Grid>().busy = obj.gameObject;
                //	obj.GetComponent<bouncer>().offset = obj1.GetComponent<Grid>().offset;
            }
        }
        yield return new WaitForSeconds(0.5f);
    }

	
    public void createRow(int j)
    {
        float offset = 0;
        GameObject gm = GameObject.Find("Creator");
        for (int i = 0; i < columns; i++)
        {
            if (j % 2 == 0)
                offset = 0;
            else
                offset = offsetStep;
            Vector3 v = new Vector3(transform.position.x + i * thePrefab.transform.localScale.x + offset, transform.position.y - j * thePrefab.transform.localScale.y, transform.position.z);
            createBall(v);
        }
    }

    public void createBall(Vector3 vec)
    {
        GameObject b = null;
        string sTag = "";
        int color = 0;
        if (Stage.Instance.stage < 6)
            color = Random.Range(0, 4 + Stage.Instance.stage - 1);
        else
            color = Random.Range(0, 4 + 6);

        /*		if(color>2){   //increase chance of 1-3   
			if(Camera.main.GetComponent<mainscript>().stage<6)
				color = Random.Range(0,4 + Camera.main.GetComponent<mainscript>().stage-1);
			else
				color = Random.Range(0,4 + 6);
		}*/
        if (color == 3)
        {   
            if (Random.Range(0, 100) == 1)
            {
                color = 3;  //raring for rainbow ball
            }
            else
            {
                color = Random.Range(0, 4 + Stage.Instance.stage - 1);
                if (color == 3)
                {
                    color = Random.Range(0, 4 + Stage.Instance.stage - 1);
                }
            }
        }
        b = Instantiate(ball, transform.position, transform.rotation) as GameObject;
        b.transform.position = new Vector3(vec.x, vec.y, ball.transform.position.z);
        b.transform.Rotate(new Vector3(0f, 180f, 0f));
        b.GetComponent<ColorBallScript>().SetColor(color);
        b.transform.parent = Meshes.transform;
        b.GetComponent<SpriteRenderer>().sortingOrder = -1;
        StartCoroutine(ChangeSorting(b));
        //b.GetComponent<ball>().pullToMesh();
        b.GetComponent<ColorBallScript>().SetColor(color);
        b.GetComponent<ball>().enabled = false;
        b.GetComponent<BoxCollider2D>().size = new Vector2(0.6f, 0.6f);
        b.GetComponent<BoxCollider2D>().offset = Vector2.zero;
        Destroy(b.GetComponent<Rigidbody2D>());
        b.GetComponent<CircleCollider2D>().enabled = true;
        b.GetComponent<CircleCollider2D>().radius = 0.34f;

        if (color == 0)
            sTag = "Orange";
        else if (color == 1)
            sTag = "Red";
        else if (color == 2)
            sTag = "Yellow";
        else if (color == 3)
            sTag = "Rainbow";
        else if (color == 4)
            sTag = "Blue";
        else if (color == 5)
            sTag = "Green";
        else if (color == 6)
            sTag = "Pink";
        else if (color == 7)
            sTag = "Violet";
        else if (color == 8)
            sTag = "Brown";
        else if (color == 9)
            sTag = "DarkBlue";
        else if (color == 10)
            sTag = "Gray";	
        b.tag = sTag;
        if (sTag == "Rainbow")
            b.name = b.name + "Rainbow";
        GameObject[] fixedBalls = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        b.name = b.name + fixedBalls.Length.ToString();
        //	b.GetComponent<SpriteSheetNG>().setIndex(setColorFrame(sTag));
		
        Destroy(b.GetComponent<Rigidbody>());
    }

    IEnumerator ChangeSorting(GameObject b)
    {
        yield return new WaitForSeconds(1);
        if (b != null)
            b.GetComponent<SpriteRenderer>().sortingOrder = 0;
    }

    public GameObject createFirstBall(Vector3 vec)
    {
        GameObject b = null;
        int color = 0;
        string sTag = "";
        if (Random.Range(0, 20) == 0)
        {
            return createBug(vec); 
        }
        else
        {
            if (mainscript.stage < 6)
                color = Random.Range(0, 4 + mainscript.stage - 1);
            else
                color = Random.Range(0, 4 + 6);
            if (color > 2)
            {     //increase chance of 1-3
                if (mainscript.stage < 6)
                    color = Random.Range(0, 4 + mainscript.stage - 1);
                else
                    color = Random.Range(0, 4 + 6);
            }
            if (Random.Range(0, 20) == 1)
            {
                color = 11;  //raring for perl ball
            }
            if (color == 3)
            {
                if (Random.Range(0, 100) == 1) //raring rainbow ball
					color = 3;
                else
                    color = Random.Range(0, 4 + mainscript.stage - 1);
            }
            b = Instantiate(ball, vec, transform.rotation) as GameObject;
            b.transform.position = new Vector3(vec.x, vec.y, ball.transform.position.z);
            b.transform.Rotate(new Vector3(0f, 180f, 0f));
            b.GetComponent<ColorBallScript>().SetColor(color);
            b.gameObject.layer = 0;
            if (color == 0)
                sTag = "Orange";
            else if (color == 1)
                sTag = "Red";
            else if (color == 2)
                sTag = "Yellow";
            else if (color == 3)
                sTag = "Rainbow";
            else if (color == 4)
                sTag = "Blue";
            else if (color == 5)
                sTag = "Green";
            else if (color == 6)
                sTag = "Pink";
            else if (color == 7)
                sTag = "Violet";
            else if (color == 8)
                sTag = "Brown";
            else if (color == 9)
                sTag = "DarkBlue";
            else if (color == 10)
                sTag = "Gray";
            else if (color == 11)
                sTag = "Pearl";	
            b.tag = sTag;
            if (sTag == "Rainbow")
                b.name = b.name + "Rainbow";
            GameObject[] fixedBalls = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
            b.name = b.name + fixedBalls.Length.ToString();
            //			b.GetComponent<SpriteSheetNG>().setIndex(setColorFrame(sTag));
            //b.transform.parent = Meshes.transform;

            return b.gameObject;
        }
        //	b.GetComponent<ball>().newBall = true;
        //	b.tag = "Ball";
    }

    public GameObject createBug(Vector3 vec)
    {
        GameObject b = null;
        int color = 0;
        string sTag = "";
        if (mainscript.stage < 6)
            color = Random.Range(0, 4 + mainscript.stage - 1);
        else
            color = Random.Range(0, 4 + 6);
        b = Instantiate(bug, vec, transform.rotation) as GameObject;
        b.transform.position = new Vector3(vec.x, vec.y, ball.transform.position.z - 1);
        b.transform.Rotate(new Vector3(0f, 180f, 0f));
        b.GetComponent<ColorBallScript>().SetColor(color);
        b.gameObject.layer = 0;
        if (color == 0)
            sTag = "Orange";
        else if (color == 1)
            sTag = "Red";
        else if (color == 2)
            sTag = "Yellow";
        else if (color == 3)
            sTag = "Yellow";
        else if (color == 4)
            sTag = "Blue";
        else if (color == 5)
            sTag = "Green";
        else if (color == 6)
            sTag = "Pink";
        else if (color == 7)
            sTag = "Violet";
        else if (color == 8)
            sTag = "Brown";
        else if (color == 9)
            sTag = "DarkBlue";
        else if (color == 10)
            sTag = "Gray";	
        b.tag = sTag;
        //	b.GetComponent<SpriteSheetNG>().setIndex(setColorFrameBug(sTag));
        return b.gameObject;
    }

    int setColorFrame(string sTag)
    {
        int frame = 0;
//		if(Camera.main.GetComponent<mainscript>().hd){
        if (sTag == "Orange")
            frame = 7;
        else if (sTag == "Red")
            frame = 3;
        else if (sTag == "Yellow")
            frame = 1;
        else if (sTag == "Rainbow")
            frame = 4;
        else if (sTag == "Pearl")
            frame = 6;
        else if (sTag == "Blue")
            frame = 11;
        else if (sTag == "DarkBlue")
            frame = 8;
        else if (sTag == "Green")
            frame = 10;
        else if (sTag == "Pink")
            frame = 5;
        else if (sTag == "Violet")
            frame = 2;
        else if (sTag == "Brown")
            frame = 9;
        else if (sTag == "Gray")
            frame = 12;	
/*		}
		else{
			if(sTag == "Orange") frame = 5;
			else if(sTag == "Red") frame = 3;	
			else if(sTag == "Yellow") frame = 1;	
			else if(sTag == "Rainbow") frame = 4;	
			else if(sTag == "Pearl") frame = 10;	
			else if(sTag == "Blue") frame = 12;	
			else if(sTag == "DarkBlue") frame = 11;	
			else if(sTag == "Green") frame = 8;	
			else if(sTag == "Pink") frame = 7;	
			else if(sTag == "Violet") frame = 2;	
			else if(sTag == "Brown") frame = 9;	
			else if(sTag == "Gray") frame = 6;	
		}*/
        return frame;
    }

    int setColorFrameBug(string sTag)
    {
        int frame = 0;
        if (sTag == "Orange")
            frame = 5;
        else if (sTag == "Red")
            frame = 3;
        else if (sTag == "Yellow")
            frame = 1;
        else if (sTag == "Rainbow")
            frame = 4;
        else if (sTag == "Pearl")
            frame = 10;
        else if (sTag == "Blue")
            frame = 10;
        else if (sTag == "DarkBlue")
            frame = 8;
        else if (sTag == "Green")
            frame = 7;
        else if (sTag == "Pink")
            frame = 4;
        else if (sTag == "Violet")
            frame = 2;
        else if (sTag == "Brown")
            frame = 9;
        else if (sTag == "Gray")
            frame = 6;	
        return frame;
    }

    public string getRandomColorTag()
    {
        int color = 0;
        string sTag = "";
        if (mainscript.stage < 6)
            color = Random.Range(0, 4 + mainscript.stage - 1);
        else
            color = Random.Range(0, 4 + 6);
		
        if (color == 0)
            sTag = "Orange";
        else if (color == 1)
            sTag = "Red";
        else if (color == 2)
            sTag = "Yellow";
        else if (color == 3)
            sTag = "Rainbow";
        else if (color == 4)
            sTag = "Blue";
        else if (color == 5)
            sTag = "Green";
        else if (color == 6)
            sTag = "Pink";
        else if (color == 7)
            sTag = "Violet";
        else if (color == 8)
            sTag = "Brown";
        else if (color == 9)
            sTag = "Gray";	
        return sTag;
    }

    public void createMesh()
    {
        GameObject Meshes = GameObject.Find("-Meshes");
        //int[][] meshMatrix = Camera.main.GetComponent<mainscript>().meshMatrix;
        float offset = 0;
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows + 1; j++)
            {
                if (j % 2 == 0)
                    offset = 0;
                else
                    offset = offsetStep;
//				Debug.Log(offset);
                GameObject b = Instantiate(thePrefab, transform.position, transform.rotation) as GameObject;
                Vector3 v = new Vector3(transform.position.x + i * b.transform.localScale.x + offset, transform.position.y - j * b.transform.localScale.y, transform.position.z);
                b.transform.position = v;
                //		b.transform.localScale = new Vector3(b.transform.localScale.x/640f*Camera.main.orthographicSize, b.transform.localScale.y/640f*Camera.main.orthographicSize, b.transform.localScale.z);
                GameObject[] fixedBalls = GameObject.FindGameObjectsWithTag("Mesh");
                b.name = b.name + fixedBalls.Length.ToString();
                b.GetComponent<Grid>().offset = offset;
                b.transform.parent = Meshes.transform;
                squares.Add(b);

            }
        }
        creatorBall.Instance.OffGridColliders();

    }

    public void EnableGridColliders()
    {
        foreach (GameObject item in squares)
        {
            item.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    public void OffGridColliders()
    {
        foreach (GameObject item in squares)
        {
            item.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

}
