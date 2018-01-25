using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootMans : MonoBehaviour {

    private Vector3 dragBeginning;
    private Vector3 dragEnd;
    private bool isDrag;
    public Rigidbody2D rb;
    public LineRenderer lr;
    public Capturable cap;
    public float shooterradius;
    public GameObject ammunition;
    const float linez = 10;
    public float mansloaded; //how many mans are suited up to shoot
    public float manstoload; //how many mans are you about to load
    public Vector2 velocitytoshoot; //how fast mans will be shot
    public bool canShoot = false;
    public int lengthofprediction = 100; //BAD: constant should be moved to top

    // Use this for initialization
    void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        lr = gameObject.GetComponent<LineRenderer>();
        cap = gameObject.GetComponent<Capturable>();
        shooterradius = gameObject.GetComponent<CircleCollider2D>().radius;
        if (cap.team > 0) {
            canShoot = true;
        }
        mansloaded = 0;
        manstoload = 0;
        velocitytoshoot = new Vector2 (0,0);
    }
    void OnMouseDown()
    {
        if (!canShoot) return;
        dragBeginning = rb.position;
        dragBeginning.z = linez;
        //dragBeginning = Camera.main.ScreenToWorldPoint(dragBeginning);
        isDrag = false;
    }
    void OnMouseDrag()
    {
        if (!canShoot) return;
        isDrag = true;
        lr.SetPosition(0, dragBeginning);
        Vector3 dragCurrent = Input.mousePosition;
        dragCurrent.z = linez;
        dragCurrent = Camera.main.ScreenToWorldPoint(dragCurrent);
        Vector2 direction = dragCurrent - dragBeginning;
        if (mansloaded <= 0)
        {
            lr.positionCount = 2;
            lr.SetPosition(0, dragBeginning);
            lr.SetPosition(1, dragCurrent);
            manstoload = Mathf.Min(direction.magnitude * direction.magnitude, cap.res);
        }
        if (mansloaded > 0)
        {
            velocitytoshoot = -direction;
        }
    }

    void OnGUI() {
        GUI.Label(new Rect(20, 10, 100, 20), "Original Game Do Not Steal!");
        if (manstoload > 0)
        {
            Rect labelbox = new Rect(Camera.main.WorldToScreenPoint(rb.position), new Vector2(100, 20));
            GUI.Label(labelbox, manstoload.ToString());
        }
        if (mansloaded > 0 && isDrag) {
            Rect labelbox = new Rect(Camera.main.WorldToScreenPoint(rb.position), new Vector2(100, 20));
            GUI.Label(labelbox, velocitytoshoot.magnitude.ToString());
            lr.positionCount=lengthofprediction;
            int i = 0;
            foreach (Vector2 point in PredictPath(lengthofprediction)){
                lr.SetPosition(i, point);
                i++;
            }
        }

    }
    List<Vector2> PredictPath(int numberofpoints) {
        List<Vector2> points = new List<Vector2>();
        Vector2 lastposition = rb.position + (velocitytoshoot.normalized * shooterradius * 2);
        float dt = 0.03f;
        points.Add(lastposition);
        
        Vector2 acceleration = Attractor.FieldAtPoint(lastposition);
        Vector2 position = lastposition + velocitytoshoot * dt + (0.5f * acceleration * Mathf.Pow(dt,2));
        points.Add(position);

        Vector2 nextposition = new Vector2(0, 0);
        for (int i = 2; i < numberofpoints; i++)
        {
            acceleration = Attractor.FieldAtPoint(position);
            nextposition = 2 * position - lastposition + acceleration * Mathf.Pow(dt, 2);
            points.Add(nextposition);
            lastposition = position;
            position = nextposition;
        }
        return points;
    }
    void OnMouseUp()
    {
        if (!canShoot) return;
        if (!isDrag)
            return;
        
        dragEnd = Input.mousePosition;
        dragEnd.z = linez;
        dragEnd = Camera.main.ScreenToWorldPoint(dragEnd);
        Vector2 direction = dragEnd - dragBeginning;
        if (mansloaded <= 0)
        {
            mansloaded = Mathf.Min(direction.magnitude * direction.magnitude, cap.res);
            cap.res = cap.res - mansloaded;
            manstoload = 0;
        }
        else {
            GameObject shot=Instantiate(ammunition,rb.position - (direction.normalized * shooterradius*2), Quaternion.identity); //todo fix this so it never spawns stuff in the planet
            shot.GetComponent<Munition>().res = mansloaded;
            shot.GetComponent<Munition>().team = cap.team;
            Rigidbody2D shotrb = shot.GetComponent<Rigidbody2D>();
            shotrb.mass = mansloaded/100;
            shotrb.velocity= -direction;
            mansloaded = 0;
            velocitytoshoot = new Vector2(0,0);
        }
        lr.SetPosition(0, new Vector3(0, 0, 0));
        lr.SetPosition(1, new Vector3(0, 0, 0));
        isDrag = false;
    }
}
