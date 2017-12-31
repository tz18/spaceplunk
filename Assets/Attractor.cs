using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{

    public static List<Attractor> attractors;
    const double G = 6.67408E-2; //gravitational constant
    public Rigidbody2D rb;
    public bool canBeAttracted = false;

    void FixedUpdate()
    {
        foreach (Attractor attractor in attractors)
        {
            if (attractor != this && attractor.canBeAttracted)
            {
                Attract(attractor);
            }
        }
    }

    void OnEnable()
    {
        if (attractors == null)
            attractors = new List<Attractor>();
        attractors.Add(this);
    }

    void OnDisable()
    {

        attractors.Remove(this);
    }

    void Attract(Attractor objToAttract)
    {
        Rigidbody2D rbToAttract = objToAttract.rb;
        Vector2 direction = rb.position - rbToAttract.position;
        float distance = direction.magnitude;
        if (distance == 0)
        {
            return;
        }
        float forceMagnitude = (float)((G * (rb.mass * rbToAttract.mass) / Mathf.Pow(distance, 2)));
        Vector2 force = direction.normalized * forceMagnitude;

        rbToAttract.AddForce(force);
    }
    //calculates gravitational force per unit mass at a point due to all attractors
    public static Vector2 FieldAtPoint(Vector2 point)
    {
        Vector2 field = new Vector2(0, 0);
        foreach (Attractor attractor in attractors)
        {
            field += attractor.PartialFieldAtPoint(point);
        }
        return field;
    }
    //calculates gravitational force per unit mass at a point, due to this attractor
    Vector2 PartialFieldAtPoint(Vector2 point)
    {
        Vector2 direction = rb.position - point; //line from point to obj
        float distance = direction.magnitude; 
        if (distance == 0 || (GetComponent<CircleCollider2D>() && GetComponent<CircleCollider2D>().bounds.Contains(point)))
        {
            return new Vector2(0, 0);
        }
        float forceMagnitude = (float)((G * (1 * rb.mass) / Mathf.Pow(distance, 2)));
        Vector2 PartialField = direction.normalized * forceMagnitude;
        return PartialField;
    }
}
