using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketCollision : MonoBehaviour
{

    private RocketAnimation anim;

    private void Start()
    {
        anim = (RocketAnimation)this.GetComponent(typeof(RocketAnimation));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("mouse"))
        {
            Destroy(GameObject.Find(other.name));
            anim.grow();
        }
        
    }
}
