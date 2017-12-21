using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capturable : MonoBehaviour
{
    public static Color[] teamcolors = new Color[10] {Color.grey,Color.red,Color.blue,Color.green,Color.yellow, Color.magenta, Color.magenta,Color.cyan, Color.black, Color.black};
    public float rescap = 1000;
    public float resgrowrate = 0.01f;
    public float res = 100; //how much health/resources does it have
    public int team = 0; //neutral is 0, player 1 is 1, player 2 is 2 etc.

    // Update is called once per frame
    void Update()
    {
        if (team > 0 && res < rescap)
        {
            res += resgrowrate;
        }
    }

    public void landMans(float mans, int otherteam)
    {
        if (team == otherteam)
        {
            res += mans;
        }
        else {
            res -= mans;
            if (res < 0) {
                team = otherteam;
                Renderer rend = GetComponent<Renderer>();
                rend.material.SetColor("_Color", teamcolors[team]);
                ShootMans shooter = GetComponent<ShootMans>();
                shooter.canShoot = true;
                res = -res;
            }
        }
    }
}
