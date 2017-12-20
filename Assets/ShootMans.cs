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
    public GameObject ammunition;
    const float linez = 10;
    public float mansloaded; //how many mans are suited up to shoot

    // Use this for initialization
    void Start () {
        rb = gameObject.GetComponent<Rigidbody2D> ();
        lr = gameObject.GetComponent<LineRenderer>();
        cap = gameObject.GetComponent<Capturable>();

    }
    void OnMouseDown()
    {
        dragBeginning = Input.mousePosition;
        dragBeginning.z = linez;
        dragBeginning = Camera.main.ScreenToWorldPoint(dragBeginning);
        isDrag = false;
    }
    void OnMouseDrag()
    {
        isDrag = true;
        lr.SetPosition(0, dragBeginning);
        Vector3 dragCurrent = Input.mousePosition;
        dragCurrent.z = linez;
        dragCurrent = Camera.main.ScreenToWorldPoint(dragCurrent);
        lr.SetPosition(0, dragBeginning);
        lr.SetPosition(1, dragCurrent);
    }
    void OnMouseUp()
    {
        if (!isDrag)
            return;
        
        dragEnd = Input.mousePosition;
        dragEnd.z = linez;
        dragEnd = Camera.main.ScreenToWorldPoint(dragEnd);
        Vector2 direction = dragEnd - dragBeginning;
        if (mansloaded <= 0)
        {
            mansloaded = Mathf.Min(direction.magnitude, cap.res);
            cap.res = cap.res - mansloaded;
        }
        else {
            GameObject shot=Instantiate(ammunition,rb.position - direction.normalized, Quaternion.identity); //todo fix this so it never spawns stuff in the planet
            shot.GetComponent<Munition>().res = mansloaded;
            shot.GetComponent<Munition>().team = cap.team;
            Rigidbody2D shotrb = shot.GetComponent<Rigidbody2D>();
            shotrb.mass = mansloaded/100;
            float forcemagnitude = (direction.magnitude) * (direction.magnitude);            
            Vector2 force = -direction.normalized * forcemagnitude*shotrb.mass;
            shotrb.AddForce(force);
            mansloaded = 0;
        }
        lr.SetPosition(0, new Vector3(0, 0, 0));
        lr.SetPosition(1, new Vector3(0, 0, 0));
    }
}
