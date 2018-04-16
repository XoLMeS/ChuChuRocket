using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderCollision : MonoBehaviour {

    public Movement move;

	void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Triggered " + collider.name);
        move = (Movement) this.GetComponent(typeof(Movement));
        move.turn();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered " + other.name);
        move = (Movement)this.GetComponent(typeof(Movement));
        move.turn();

    }
}
