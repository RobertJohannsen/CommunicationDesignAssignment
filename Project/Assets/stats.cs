using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stats : MonoBehaviour
{

    public int maxhp ,hp, maxap, currentap, agi , att, armor,acc , critmod;
    public bool hasInitiative;
    public GameObject[] bodyPart;

    void Start()
    {
        hp = maxhp;
        currentap = maxap;
    }


}
