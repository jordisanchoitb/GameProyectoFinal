using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitFPS : MonoBehaviour
{
    private int Limit = 60;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = Limit;
    }
}
