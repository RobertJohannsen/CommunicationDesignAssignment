using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RerollText : MonoBehaviour
{
    public TextMeshPro rerollText;
    public GameObject Barrier;
    public stats PlyStats;
    int acc, crit, hp, ap, ar , att ,agi;
    public bool showText;
    // Start is called before the first frame update
    void Start()
    {

        showText = true;
        acc = (int)Random.Range(20, 50);
        ar = (int)Random.Range(1, 10);
        crit = (int)Random.Range(1, 25);
        ap = 15;
        att = (int)Random.Range(1, 20);
        hp = (int)Random.Range(20, 50);
        agi = (int)Random.Range(5, 20);

    }

    // Update is called once per frame
    void Update()
    {
        if(showText)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PlyStats.acc = acc;
                PlyStats.armor = ar;
                PlyStats.critmod = crit;
                PlyStats.maxap = ap;
                PlyStats.currentap = ap;
                PlyStats.att = att;
                PlyStats.maxhp = hp;
                PlyStats.hp = hp;
                PlyStats.agi = agi;

                showText = false;


            }
            if (Input.GetMouseButtonDown(1))
            {
                acc = (int)Random.Range(5, 15);
                ar = (int)Random.Range(1, 10);
                crit = (int)Random.Range(1, 25);
                ap = 15;
                att = (int)Random.Range(1, 20);
                hp = (int)Random.Range(20, 50);
                agi = (int)Random.Range(5, 20);

            }

            rerollText.text = "player stats \n"
                + "HP : " + hp + " (Increases HP by a flat amount)\n"
                + "Acc : " + acc + " (effects your hit rate)\n"
                + "Armor : " + ar + " (reduces the damage you take by a flat amount)\n"
                + "Crit : " + crit + " (effects your crit rate)\n"
                + "AP : " + ap + " (effects your AP pool)\n"
                + "Att : " + att + " (increases the damage you do)\n"
                + "AGI : " + agi + " (Increase your chance to get attack first in combat)\n"
                + "\n" 
                + "Left click to finalize stats and start test, right click to reroll"+ "\n"
                + "\n"
                + "Press E to start combat , mouse scroll wheel to switch attacks , space to start selecting attacks\n"
                + " - press space again to select body part - press space again to select attack and queue it \n "
                + "Press esc to cancel attack , press q to finalize attacks and start attacking"
                + "\n + Press Delete at anytime to quit"
                 ;
        }
        else
        {
            rerollText.text = "";
            Barrier.GetComponent<SpriteRenderer>().enabled = false;

        }
       

    }
}
