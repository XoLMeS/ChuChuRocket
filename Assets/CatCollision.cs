using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatCollision : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("mouse"))
        {
            Destroy(GameObject.Find(other.name));
        }

    }
}
