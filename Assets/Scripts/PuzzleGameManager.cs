using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGameManager: MonoBehaviour
{
    [SerializeField]
    private Transform[] transforms = new Transform[7];

    public GameObject winText;
    public static bool win;

    void Start()
    {
        win = false;
    }

    void Update()
    {
        if(!win)
        {
            if (transforms[0].rotation.z == 0 &&
            transforms[1].rotation.z == 0 &&
            transforms[2].rotation.z == 0 &&
            transforms[3].rotation.z == 0 &&
            transforms[4].rotation.z == 0 &&
            transforms[5].rotation.z == 0)
            {
                win = true;
                winText.SetActive(true);
            }
        }
    }
}
