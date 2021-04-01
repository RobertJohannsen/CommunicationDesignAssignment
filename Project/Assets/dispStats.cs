using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class dispStats : MonoBehaviour
{
    public GameObject StatBar;
    public stats objStats;
    public string angerStrg;

    // Start is called before the first frame update
    void Start()
    {
        objStats = GetComponentInParent<stats>();
    }

    // Update is called once per frame
    void Update()
    {
        
        StatBar.GetComponent<TextMeshPro>().text = "HP : " + objStats.hp+"/"+ objStats.maxhp + "" +"\n"
        +"AP : " + objStats.currentap + "/" + objStats.maxap + angerStrg + "\n"
        +"AR : " + objStats.armor;

        if (GetComponentInParent<plyAttacks>())
        {
            if(GetComponentInParent<plyAttacks>().apCostTotal != 0)
            {
                angerStrg = " -(" + GetComponentInParent<plyAttacks>().apCostTotal + ")";
            }
            else
            {
                angerStrg = "";
            }
            
        }
    }
}
