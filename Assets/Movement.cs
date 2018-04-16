using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    private float _x = 1;
    private float _y = 0;

    private bool _active = true;

    private bool _do_turn = false;

    private float _speed = 10;

	// Use this for initialization
	void Start () {
		
	}
	
	void FixedUpdate () {

        if (_do_turn)
        {
            do_turn();
            _do_turn = false;
        }
        if (_active)
        {
            Vector3 pos = transform.position;
            pos.x += _x / _speed;
            pos.y += _y / _speed;
            transform.position = pos;
        }

    }

    public void left()
    {
        _x = -1;
        _y = 0;
    }

    public void right()
    {
        _x = 1;
        _y = 0;
    }

    public void down()
    {
        _x = 0;
        _y = -1;
    }

    public void up()
    {
        _x = 0;
        _y = 1;
    }

    private void do_turn()
    {
        Vector3 pos = transform.position;
        pos.x -= _x / _speed;
        pos.y -= _y / _speed;
        transform.position = pos;

        if (_y != 0)
        {
            _x = _y;
            _y = 0;
        }
        else if (_x != 0)
        {
            _y = -_x;
            _x = 0;
        }
    }

    public void turn()
    {
        _do_turn = true;
    }

    public void isActive(bool active)
    {
        _active = active;
    }

    public bool isActive()
    {
        return _active;
    }
}
