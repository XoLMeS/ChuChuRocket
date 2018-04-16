using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketAnimation : MonoBehaviour {

    private float min_scale = 1f;
    private float curr_scale = 1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var scale = new Vector3(curr_scale,curr_scale,curr_scale);
        transform.localScale = scale;
        if (curr_scale > min_scale)
        {
            curr_scale -= 0.05f;
        }
	}


    public void grow()
    {
        curr_scale = 2.0f;
    }
}
