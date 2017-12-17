using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour {

    public static List<Attractor> attractors;
    const float G = 6.674f;
    const float distancescale = 1;
    const float massscale = 1;
    public Rigidbody2D rb;

    void FixedUpdate() {
        foreach (Attractor attractor in attractors) {
            if (attractor != this) {
                Attract(attractor);
            }
        }
    }

    void OnEnable() {
        if (attractors == null)
            attractors = new List<Attractor>();
        attractors.Add(this);
    }

    void OnDisable() {

        attractors.Remove(this);
    }

    void Attract(Attractor objToAttract) {
        Rigidbody2D rbToAttract = objToAttract.rb;
        Vector2 direction = rb.position - rbToAttract.position;
        float distance = direction.magnitude;
        if (distance == 0) {
            return;
        }
        float forceMagnitude = G * (rb.mass * rbToAttract.mass / Mathf.Pow(distance,2));
        Vector2 force = direction.normalized * forceMagnitude;

        rbToAttract.AddForce(force);
    }
}
