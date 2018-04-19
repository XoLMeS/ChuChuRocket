using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderCollision : MonoBehaviour {

    public Movement move;

    private void Start()
    {
        move = (Movement)this.GetComponent(typeof(Movement));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("border"))
        {
            move.turn();
        }

        if (other.name.Contains("arrow"))
        {
            if (other.name.Contains("R"))
            {
                move.right();
            }

            if (other.name.Contains("L"))
            {
                move.left();
            }

            if (other.name.Contains("D"))
            {
                move.down();
            }

            if (other.name.Contains("U"))
            {
                move.up();
            }
        }

    }
}
