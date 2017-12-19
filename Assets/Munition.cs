using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Munition : MonoBehaviour {

    public float res = 1;

    // Use this for initialization
    void Start () {
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D> ();
    }
	
}
