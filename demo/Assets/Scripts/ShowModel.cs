using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using QGMiniGame;
using UnityEngine;
using UnityEngine.UI;

public class ShowModel : MonoBehaviour
{
    void Start()
    {
        playQGShowModal();
    }

    private static bool hasBeenCalled = false;

    private static object lockObject = new object();

    public void playQGShowModal()
    {
        lock (lockObject)
        {
            if (!hasBeenCalled)
            {
                hasBeenCalled = true;
                QG.ShowModal();
            }
        }
    }
}
