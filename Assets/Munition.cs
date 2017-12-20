using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Munition : MonoBehaviour {

    public float res = 1;
    //Rigidbody2D rb;
    public int team = 0;
    // Use this for initialization
    void Start () {
        //rb = gameObject.GetComponent<Rigidbody2D> ();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Capturable cap = collision.gameObject.GetComponent<Capturable>();
        if (cap != null)
        {
            Debug.Log("hi");
            cap.landMans(res, team);
            Destroy(gameObject);
        }
    }
}
