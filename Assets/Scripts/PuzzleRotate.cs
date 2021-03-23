using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleRotate : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (!PuzzleGameManager.win)
        {
            transform.Rotate(0.0f, 0.0f, 90.0f);
        }
    }
}
