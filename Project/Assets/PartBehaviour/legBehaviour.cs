using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class legBehaviour : MonoBehaviour
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
                int _agi = GetComponentInParent<stats>().agi;
                _agi = _agi / 2;

                GetComponentInParent<stats>().agi = _agi;

                effectDone = true;
            }
        }

    }
}
