using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButtonScript : MonoBehaviour {


    public Button myButton;

	// Use this for initialization
	void Start () {
        Button pause = myButton.GetComponent<Button>(); //The attached script object takes a UI component as an input. When using this script, drag the button onto the 'My Button' slot to get it.
        pause.onClick.AddListener(TaskOnClick); //Adds a function that triggers when you click it
	}

    void TaskOnClick()
    {
        if (Time.timeScale == 1.0F) //If we're at the default time scale
            Time.timeScale = 0.0F; //Freeze
        else
            Time.timeScale = 1.0F; //Otherwise, toggle back to full speed
    }

	// Update is called once per frame
	void Update () {
		
	}
}
