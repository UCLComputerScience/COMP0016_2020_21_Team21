using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TableMove : MonoBehaviour
{
    private bool move = false;
    private Vector3 newPosition;
    public void OnTableMove()
    {
        newPosition = new Vector3(transform.localPosition.x, transform.localPosition.y,
            transform.localPosition.z - 0.32f);
        move = true;
    }

    void Update()
    {
        if(move)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, newPosition, 0.03f);
        }
    }
}
