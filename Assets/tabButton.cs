using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tabButton : MonoBehaviour {


    private GameObject go;
    private bool moves = false;
    private float speed = 3f;

    private bool on_left = false;
    // Use this for initialization
    void Start () {
        go = GameObject.Find("Panel");
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = go.transform.position;

        if (on_left)
        {
            if (pos.x >= 50f)
            {
                moves = false;
                on_left = false;
                var l = transform.Find("Text").GetComponent<Text>();
                l.text = "←";
            }
        }
        else
        {
            if (pos.x <= -50f)
            {
                moves = false;
                on_left = true;
                var l = transform.Find("Text").GetComponent<Text>();
                l.text = "→";
            }
        }
       
        if (moves){
            if (!on_left)
            {
                pos.x -= speed;
            }
            else
            {
                pos.x += speed;
            }
         
            go.transform.position = pos;
           
        }
       
    }

    public void animate()
    {
        moves = true;
    }
}
