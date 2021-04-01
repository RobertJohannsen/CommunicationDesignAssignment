using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attacks : MonoBehaviour
{
    public Sprite[] state;
    public enum atts{ none };
    public atts[] attQueue;
    public int ammo, clip, apCost, baseDmg, index, basecrit, baseacc, attIndex, attQueueLength;
    public bool hasHat, hasWep;

    // Start is called before the first frame update
    void Start()
    {
        hasWep = true;
        hasHat = true;
        doAttack();
    }

    // Update is called once per frame
    void Update()
    {
        if (attIndex > 1)
        {
            attIndex = 1;
        }


        if(this.GetComponentInChildren<hatBehaviour>().partHP == 0)
        {
            hasHat = false;
            this.GetComponent<SpriteRenderer>().sprite = state[1];
        }
        if(this.GetComponentInChildren<weaponBehaviour>().partHP == 0)
        {
            hasWep = false;
            this.GetComponent<SpriteRenderer>().sprite = state[2];
        }

        if(!hasHat && !hasWep)
        {
            this.GetComponent<SpriteRenderer>().sprite = state[3];
        }


    }

    public void doAttack()
    {
        attIndex = 1;
        baseDmg = 7;
        apCost = 2;
        basecrit = 40;
        baseacc = 50;

           
    }
}
