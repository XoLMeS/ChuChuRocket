using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tabButton : MonoBehaviour {


    private GameObject go;
    private bool moves = false;
    private float speed = 3f;
    // Use this for initialization
    void Start () {
        go = GameObject.Find("Panel");
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = go.transform.position;
    
        if (pos.x >= 50f)
        {
            moves = false;
            var l = transform.Find("Text").GetComponent<Text>();
            l.text = "←";
        }
        if (moves){
            pos.x += speed;
         
            go.transform.position = pos;
           
        }
       
    }

    public void animate()
    {
        moves = true;
    }
}
