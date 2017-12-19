using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfShoot : MonoBehaviour {

    private Vector3 dragBeginning;
    private Vector3 dragEnd;
    private bool isDrag;
    public Rigidbody2D rb;
    public LineRenderer lr;
    const float linez = 10;

    void OnMouseDown() {
        dragBeginning = Input.mousePosition;
        dragBeginning.z = linez;
        dragBeginning= Camera.main.ScreenToWorldPoint(dragBeginning);
        isDrag = false;
    }
    void OnMouseDrag() {
        isDrag = true;
        lr.SetPosition(0, dragBeginning);
        Vector3 dragCurrent = Input.mousePosition;
        dragCurrent.z = linez;
        dragCurrent = Camera.main.ScreenToWorldPoint(dragCurrent);
        lr.SetPosition(0, rb.position);
        lr.SetPosition(1, dragCurrent);
    }
    void OnMouseUp() {
        if (!isDrag)
            return;
        dragEnd = Input.mousePosition;
        dragEnd.z = linez;
        dragEnd = Camera.main.ScreenToWorldPoint(dragEnd);
        Vector2 direction = dragEnd - dragBeginning;
        float forcemagnitude = (direction.magnitude)* (direction.magnitude) * rb.mass;
        Vector2 force = -direction.normalized * forcemagnitude;
        lr.SetPosition(0, new Vector3(0,0,0));
        lr.SetPosition(1, new Vector3(0, 0, 0));
        rb.AddForce(force);
    }
}
