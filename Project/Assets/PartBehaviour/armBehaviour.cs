using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class armBehaviour : MonoBehaviour
{
    public int partHP;
    public bool effectDone;
    public int baseAcc;

    // Update is called once per frame
    void Start()
    {
        effectDone = false;
    }
    void Update()
    {

        partHP = Mathf.Clamp(partHP, 0, 1000);
        if (!effectDone)
        {
            if (partHP == 0)
            {
                int _ap = GetComponentInParent<stats>().maxap;
                _ap = _ap / 2;

                GetComponentInParent<stats>().maxap = _ap;

                effectDone = true;
            }
        }

    }
}
