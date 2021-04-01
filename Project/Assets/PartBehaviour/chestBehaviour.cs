using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chestBehaviour : MonoBehaviour
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
        if(!effectDone)
        {
            if (partHP == 0)
            {
                int _armor = GetComponentInParent<stats>().armor;
                _armor = _armor / 2;

                GetComponentInParent<stats>().armor = _armor;

                effectDone = true;
            }
        }
      
    }
}
