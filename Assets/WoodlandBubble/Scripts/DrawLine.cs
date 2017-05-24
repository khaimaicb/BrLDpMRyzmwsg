using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawLine : MonoBehaviour
{
    LineRenderer line;
    bool draw = false;
    Color col;

    public static Vector2[] waypoints = new Vector2[3];
    public float addAngle = 90;
    public GameObject pointer;
    GameObject[] pointers = new GameObject[15];
    GameObject[] pointers2 = new GameObject[3];
    Vector3 lastMousePos;
    private bool startAnim;
    private float newDist;

    // Use this for initialization
    void Start()
    {
        line = GetComponent<LineRenderer>();
        GeneratePoints();
        GeneratePositionsPoints();
        HidePoints();
        waypoints[0] = transform.position;
        waypoints[1] = transform.position + Vector3.up * 5;
    }

    void HidePoints()
    {
        foreach (GameObject item in pointers)
        {
            item.GetComponent<SpriteRenderer>().enabled = false;
        }

        foreach (GameObject item in pointers2)
        {
            item.GetComponent<SpriteRenderer>().enabled = false;
        }

    }

    private void GeneratePositionsPoints()
    {
        if (mainscript.Instance.boxCatapult.GetComponent<Grid>().busy != null)
        {
            col = mainscript.Instance.boxCatapult.GetComponent<Grid>().busy.GetComponent<SpriteRenderer>().sprite.texture.GetPixelBilinear(0.6f, 0.6f);
            col.a = 1;
        }

        HidePoints();

        for (int i = 0; i < pointers.Length; i++)
        {
            Vector2 AB = waypoints[1] - waypoints[0];
            AB = AB.normalized;
            float step = i / 1.5f;

            if (step < (waypoints[1] - waypoints[0]).magnitude)
            {
                pointers[i].GetComponent<SpriteRenderer>().enabled = true;
                pointers[i].transform.position = waypoints[0] + (step * AB);
                pointers[i].GetComponent<SpriteRenderer>().color = col;
                pointers[i].GetComponent<LinePoint>().startPoint = pointers[i].transform.position;
                pointers[i].GetComponent<LinePoint>().nextPoint = pointers[i].transform.position;
                if (i > 0)
                    pointers[i - 1].GetComponent<LinePoint>().nextPoint = pointers[i].transform.position;
            }
        }
        for (int i = 0; i < pointers2.Length; i++)
        {
            Vector2 AB = waypoints[2] - waypoints[1];
            AB = AB.normalized;
            float step = i / 2f;

            if (step < (waypoints[2] - waypoints[1]).magnitude)
            {
                pointers2[i].GetComponent<SpriteRenderer>().enabled = true;
                pointers2[i].transform.position = waypoints[1] + (step * AB);
                pointers2[i].GetComponent<SpriteRenderer>().color = col;
                pointers2[i].GetComponent<LinePoint>().startPoint = pointers2[i].transform.position;
                pointers2[i].GetComponent<LinePoint>().nextPoint = pointers2[i].transform.position;
                if (i > 0)
                    pointers2[i - 1].GetComponent<LinePoint>().nextPoint = pointers2[i].transform.position;
            }
        }
    }

    void GeneratePoints()
    {
        for (int i = 0; i < pointers.Length; i++)
        {
            pointers[i] = Instantiate(pointer, transform.position, transform.rotation) as GameObject;
            pointers[i].transform.parent = transform;
        }
        for (int i = 0; i < pointers2.Length; i++)
        {
            pointers2[i] = Instantiate(pointer, transform.position, transform.rotation) as GameObject;
            pointers2[i].transform.parent = transform;
        }
    }

    List<RaycastHit2D> GetWideRaycastLine(Vector2 start, Vector2 dir)
    {
        float width = 0.3f;
        Vector2 saveStart = start;
        Vector2 saveDir = dir;
        List<RaycastHit2D> hitList = new List<RaycastHit2D>();
        Vector2 colliderHit = dir;
        Vector2 colliderHit1 = dir;
        Vector2 colliderHit2 = dir;
        //RaycastHit2D hit = Physics2D.Linecast(start, start + (dir - start).normalized * 10, LayerMask.GetMask("Ball", "Border"));
        //Debug.DrawLine(start, start + (dir - start).normalized * 10, Color.blue);

        //if (hit != null)
        //{
        //    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ball"))
        //    {
        //        Vector2 ballPos = hit.transform.position;
        //        Vector2 checkSquarePos = ballPos + (dir - ballPos).normalized * 0.5f;
        //        RaycastHit2D[] hitGrid = Physics2D.CircleCastAll(checkSquarePos, 0.2f, Vector2.zero, 1, LayerMask.GetMask("Mesh"));
        //        Debug.DrawLine(ballPos, checkSquarePos, Color.green);
        //        float dist = 10;
        //        Transform square = null;
        //        foreach (RaycastHit2D item in hitGrid)
        //        {
        //            if (dist > Vector2.Distance(item.transform.position, checkSquarePos) && item.collider.gameObject.GetComponent<Grid>().busy == null)
        //            {
        //                dist = Vector2.Distance(item.transform.position, checkSquarePos);
        //                square = item.transform;
        //            }
        //        }
        //        mainscript.Instance.targetSquare = square;
        //    }

        //}

        RaycastHit2D hit = Physics2D.Linecast(start, start + (dir - start).normalized * 20, LayerMask.GetMask("Ball", "Border"));
        //if (hit.transform != null) colliderHit = hit.transform.position;
        //Debug.DrawLine(start, start + (dir - start).normalized * 20, Color.blue);
        //start += Vector2.right * width;
        //dir += Vector2.right * width;
        //RaycastHit2D hit1 = Physics2D.Linecast(start, start + (dir - start).normalized * 20, LayerMask.GetMask("Ball", "Border"));
        //if (hit1.transform != null) colliderHit1 = hit1.transform.position;
        //Debug.DrawLine(start, start + (dir - start).normalized * 20, Color.blue);
        //start -= Vector2.right * width * 2;
        //dir -= Vector2.right * width * 2;
        //RaycastHit2D hit2 = Physics2D.Linecast(start, start + (dir - start).normalized * 20, LayerMask.GetMask("Ball", "Border"));
        //if (hit2.transform != null) colliderHit2 = hit2.transform.position;
        //Debug.DrawLine(start, start + (dir - start).normalized * 20, Color.blue);

        //Vector2 newCollidedPos = colliderHit;
        //newDist = Vector2.Distance(saveStart, colliderHit);
        //if (Vector2.Distance(saveStart, colliderHit) > Vector2.Distance(saveStart, colliderHit1)) newDist = Vector2.Distance(saveStart, colliderHit1);
        //if (Vector2.Distance(saveStart, colliderHit1) > Vector2.Distance(saveStart, colliderHit2)) newDist = Vector2.Distance(saveStart, colliderHit2);
        ////Debug.DrawLine(saveStart, saveStart + (saveDir - saveStart).normalized * (newDist), Color.red);

        hitList.Add(hit);
        return hitList;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GamePlay.Instance.GameStatus == GameState.Playing)
        {
            draw = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            draw = false;
        }
        else if(GamePlay.Instance.GameStatus != GameState.Playing) {
            draw = false;
        }
        if (draw)
        {
            //  line.enabled = true;
            Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Vector3.back * 10;
            //     if( dir.y - 2 < transform.position.y ) { HidePoints(); return; }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!mainscript.StopControl)
            {//dir.y < 15.5 && dir.y > - 2 && 

                dir.z = 0;
                if (lastMousePos == dir)
                {
                    startAnim = true;
                }
                else
                    startAnim = false;
                lastMousePos = dir;
                line.SetPosition(0, transform.position);

                waypoints[0] = transform.position;
                //int layerMask = ~(1 << LayerMask.NameToLayer("Mesh"));

                List<RaycastHit2D> hitList = new List<RaycastHit2D>();
                hitList = GetWideRaycastLine(waypoints[0], (Vector2)dir);
                foreach (RaycastHit2D item in hitList)
                {
                    Vector2 point = item.point;
                    //    if (point.y - waypoints[0].y < 1.5f) point += Vector2.up * 5;
                    line.SetPosition(1, point);
                    addAngle = 180;

                    if (waypoints[1].x < 0)
                        addAngle = 0;
                    if (item.collider != null)
                    {
                        if (item.collider.gameObject.layer == LayerMask.NameToLayer("Border") && item.collider.gameObject.name != "GameOverBorder" && item.collider.gameObject.name != "borderForRoundedLevels")
                        {
                            Debug.DrawLine(waypoints[0], waypoints[1], Color.red);  //waypoints[0] + ( (Vector2)dir - waypoints[0] ).normalized * 10
                            Debug.DrawLine(waypoints[0], dir, Color.blue);
                            Debug.DrawRay(waypoints[0], waypoints[1] - waypoints[0], Color.green);
                            waypoints[1] = point;
                            waypoints[2] = point;
                            line.SetPosition(1, dir);
                            waypoints[1] = point;
                            float angle = 0;
                            angle = Vector2.Angle(waypoints[0] - waypoints[1], (point - Vector2.up * 100) - (Vector2)point);
                            if (waypoints[1].x > 0)
                                angle = Vector2.Angle(waypoints[0] - waypoints[1], (Vector2)point - (point - Vector2.up * 100));
                            waypoints[2] = Quaternion.AngleAxis(angle + addAngle, Vector3.back) * ((Vector2)point - (point - Vector2.up * 100));
                            Vector2 AB = waypoints[2] - waypoints[1];
                            AB = AB.normalized;
                            line.SetPosition(2, waypoints[2]);
                            break;
                        }
                        else if (item.collider.gameObject.layer == LayerMask.NameToLayer("Ball"))
                        {
                            //Debug.DrawLine( waypoints[0], waypoints[1], Color.red );  //waypoints[0] + ( (Vector2)dir - waypoints[0] ).normalized * 10
                            //Debug.DrawLine( waypoints[0], dir, Color.blue );
                            //Debug.DrawRay( waypoints[0], waypoints[1] - waypoints[0], Color.green );
                            line.SetPosition(1, waypoints[0] + ((Vector2)dir - waypoints[0]).normalized * 10);
                            waypoints[1] = waypoints[0] + ((Vector2)dir - waypoints[0]).normalized * 10;
                            waypoints[2] = waypoints[0] + ((Vector2)dir - waypoints[0]).normalized * 10;
                            Vector2 AB = waypoints[2] - waypoints[1];
                            AB = AB.normalized;
                            line.SetPosition(2, (0.1f * AB));
                            break;
                        }
                        else
                        {

                            waypoints[1] = waypoints[0] + ((Vector2)dir - waypoints[0]).normalized * 10;
                            waypoints[2] = waypoints[0] + ((Vector2)dir - waypoints[0]).normalized * 10;
                        }
                    }


                }
                if (!startAnim)
                    GeneratePositionsPoints();

            }

        }
        else if (!draw)
        {
            HidePoints();
        }

    }
}
