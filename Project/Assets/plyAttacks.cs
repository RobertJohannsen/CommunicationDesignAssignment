using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class plyAttacks : MonoBehaviour
{
    public GameObject target , pointerUI ,pointerUItext , attDisp , desDisp;
    public GameObject attQDisp, attQDispAd1, attQDispAd2;


    public enum attacks { none, attShoot, attMelee, attReload };
    public List<GameObject> where = new List<GameObject>();
    public List<attacks> attQ = new List<attacks>();

    public int ammo, clip, apCost, baseDmg, index, basecrit, baseacc, attIndex ,apCostTotal;
    public bool aimMode , selectAtt ,beginAtt;
    public int whatPart , _apCost;
    public string _attDispText, _attDesText;
    public int _dmg, _ap;
    public float _critChance;
    public GameObject partDisp; //the object with the part display
    public string basedString ,basedStringAd1,basedStringAd2;

    public combatLoop cLoop ;


    public int attQLast , attQMax; //attQlast is the last index where an attack was submitted , attQMax is the maximum length of attQ based on MAXAP
    // Start is called before the first frame update
    void Start()
    {

        beginAtt = false;
        selectAtt = false;
        aimMode = false;
        attQMax = (int)this.GetComponentInParent<stats>().maxap/4;
        
     

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            beginAtt = !beginAtt;
        }

        UpdateAttQDisp();
        if(beginAtt)
        {
            
            AssignAttacks();
            UpdateDisp();
        }
        else
        {
          //  attDisp.GetComponent<TextMeshPro>().text = _attDispText;
          //  desDisp.GetComponent<TextMeshPro>().text = _attDesText;

            _attDispText = "";
            _attDesText =  "";
        }

        if(cLoop.combatPhase > 1)
        {
            pointerUI.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            pointerUItext.GetComponent<MeshRenderer>().enabled = false;
            partDisp.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            pointerUI.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            pointerUItext.GetComponent<MeshRenderer>().enabled = true;
            partDisp.GetComponent<MeshRenderer>().enabled = true;
        }

        if(cLoop.attackObj2.GetComponent<stats>().hp <= 0)
        {
            pointerUI.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            pointerUItext.GetComponent<MeshRenderer>().enabled = false;
            partDisp.GetComponent<MeshRenderer>().enabled = false;
        }
     

    }

    void attShoot()
    {
            
         baseDmg = 5;
         apCost = 2;
        baseacc = 20;
        basecrit = 10;

    }
    
    void attMelee()
    {
        baseDmg = 2;
         apCost = 1;
        basecrit = 40;
    }


    void attReload()
    {
       // GetComponentInParent<stats>().currentap += 2;

    }

   public  void loadAtt(int i)
    {
        //this need fixing
        switch (attQ[i])
        {
            case attacks.attShoot:
                attShoot();
                break;
            case attacks.attMelee:
                attMelee();
                break;
            case attacks.attReload:
                attReload();
                break;

        }
    }

    void AssignAttacks()
    {

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            attIndex += 1;
            if(aimMode)
            {
                whatPart += 1;
            }
           
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            attIndex -= 1;
            if(aimMode)
            {
                whatPart -= 1;
            }
           
        }

        attIndex = Mathf.Clamp(attIndex, 1, 3);
        whatPart = Mathf.Clamp(whatPart, 0, 7);

        if (!selectAtt)
        {

            //select part here
            if (aimMode)
            {

                AimMode();

                //select part here ends

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    aimMode = false;
                    selectAtt = true;

                }

            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                aimMode = true;
            }


         
        }
        else
        {

            aimMode = false;

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                selectAtt = false;
                aimMode = true;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                

                //attacks are submitted here

                if (apCostTotal <= this.GetComponent<stats>().currentap)
                {

                    if(attQLast != attQMax)
                    {
                        switch (attIndex)
                        {

                            case 1:
                                attQ.Add(attacks.attShoot);
                                attShoot();
                                _apCost = apCost; //I promise this will make sense
                                apCostTotal += apCost;
                                //   attQDisp[attQueueLength].GetComponent<TextMeshPro>().text = ">shoot";
                                break;
                            case 2:
                                attQ.Add(attacks.attMelee);
                                attMelee();
                                _apCost = apCost; //I promise this will make sense
                                apCostTotal += apCost;
                                //   attQDisp[attQueueLength].GetComponent<TextMeshPro>().text = ">melee";
                                break;
                            case 3:
                                attQ.Add(attacks.attReload);
                                attReload();
                                _apCost = apCost; //I promise this will make sense
                                apCostTotal += apCost;
                                //   attQDisp[attQueueLength].GetComponent<TextMeshPro>().text = ">rest";
                                break;



                        }
                        where.Add(target.GetComponent<stats>().bodyPart[whatPart]);
                        attQLast++;
                    }
                 
             

                    if(apCostTotal >= this.GetComponent<stats>().currentap)
                    {
                        attQLast -= 1;
                        attQ[attQLast] = attacks.none;
                        apCostTotal -= _apCost;

                        //reduce apcost back to thing because attack cant register

                    }
                }
                else
                {
                    //cant submit attacks
                }
               

                
            }


            attQLast = Mathf.Clamp(attQLast, 0, attQMax); //remove this if there is a problem
        }



       

    }

    void UpdateDisp()
    {

        switch (attIndex)
        {
            case 1:
                attShoot();
                _dmg = baseDmg;
                _ap = apCost;
                _critChance = Mathf.Clamp(((basecrit / 100) + 1) * (GetComponentInParent<stats>().critmod + 25), 0, 100);


                _attDispText = ">Shoot<";
                _attDesText = 
                    "Shoot the target \n"
                    +"AP cost : " + _ap+ "\n"
                    +"Damage : "+_dmg+"\n"
                    +"Crit Chance : "+_critChance+"%" ;
                break;   
            case 2:

                attMelee();
                _dmg = baseDmg;
                _ap = apCost;
                _critChance = Mathf.Clamp(((basecrit / 100) + 1) * (GetComponentInParent<stats>().critmod + 25), 0, 100);

                _attDispText = ">Melee<";
                _attDesText =
                   "Stab the target \n"
                   + "AP cost : " + _ap + "\n"
                   + "Damage : " + _dmg + "\n"
                   + "Crit Chance : " + _critChance + "%";
                break;
            case 3:

                attReload();
                _ap = apCost;
                _attDispText = ">Rest<";
                _attDesText =
                   "Rest to gain 2 AP\n"
                   + "AP cost : " + _ap
                  ;
                break;




        }
        attDisp.GetComponent<TextMeshPro>().text = _attDispText;
        desDisp.GetComponent<TextMeshPro>().text = _attDesText;


      
    }

    void AimMode()
    {
        int _accMod = GetComponentInParent<stats>().acc;


        pointerUI.transform.position = new Vector3(target.GetComponent<stats>().bodyPart[whatPart].transform.position.x, target.GetComponent<stats>().bodyPart[whatPart].transform.position.y, target.GetComponent<stats>().bodyPart[whatPart].transform.position.z - 1);

        switch (target.GetComponent<stats>().bodyPart[whatPart].tag)
        {
            case "head":
                baseacc = target.GetComponent<stats>().bodyPart[whatPart].GetComponent<headBehaviour>().baseAcc;
                break;
            case "chest":
                baseacc = target.GetComponent<stats>().bodyPart[whatPart].GetComponent<chestBehaviour>().baseAcc;
                break;
            case "arm":
                baseacc = target.GetComponent<stats>().bodyPart[whatPart].GetComponent<armBehaviour>().baseAcc;
                break;
            case "leg":
                baseacc = target.GetComponent<stats>().bodyPart[whatPart].GetComponent<legBehaviour>().baseAcc;
                break;
            case "hat":
                baseacc = target.GetComponent<stats>().bodyPart[whatPart].GetComponent<hatBehaviour>().baseAcc;
                break;
            case "weapon":
                baseacc = target.GetComponent<stats>().bodyPart[whatPart].GetComponent<weaponBehaviour>().baseAcc;
                break;
        }



        int _finalAcc = Mathf.Clamp((baseacc + (_accMod * 2)), 0, 100);
        partDisp.GetComponent<TextMeshPro>().text = "(" + target.GetComponent<stats>().bodyPart[whatPart].name + ")";
        pointerUItext.GetComponent<TextMeshPro>().text = _finalAcc + "%";
    }


   void UpdateAttQDisp()
    {
 
        if(attQLast > 0)
        {
            if (attQLast < 4)
            {
                attQDisp.GetComponent<MeshRenderer>().enabled = true;
                attQDispAd1.GetComponent<MeshRenderer>().enabled = false;
                attQDispAd2.GetComponent<MeshRenderer>().enabled = false;

                for (int i = 0; i < attQLast; i++)
                {
                    switch (attQ[i])
                    {
                        case attacks.attShoot:
                            basedString = basedString + ">shoot (" + where[i].name + ")\n";
                            break;
                        case attacks.attMelee:
                            basedString = basedString + ">melee (" + where[i].name + ")\n";
                            break;
                        case attacks.attReload:
                            basedString = basedString + ">rest \n";
                            break;
                        case attacks.none:
                            break;
                    }
                }

                attQDisp.GetComponent<TextMeshPro>().text = basedString;
                basedString = "";

               
            }
            else
            {
                attQDispAd1.GetComponent<MeshRenderer>().enabled = true;
                attQDispAd2.GetComponent<MeshRenderer>().enabled = true;
                attQDisp.GetComponent<MeshRenderer>().enabled = false;

                for (int i = Mathf.Clamp(attQMax-5,0 ,100); i < attQLast; i++)
                    {
                    if(i <= 1)
                    {
                        switch (attQ[i])
                        {
                            case attacks.attShoot:
                                basedStringAd1 = basedStringAd1 + ">shoot (" + where[i].name + ")\n";
                                break;
                            case attacks.attMelee:
                                basedStringAd1 = basedStringAd1 + ">melee (" + where[i].name + ")\n";
                                break;
                            case attacks.attReload:
                                basedStringAd1 = basedStringAd1 + ">rest \n";
                                break;
                            case attacks.none:
                                break;
                        }
                    }
                    else
                    {
                        switch (attQ[i])
                        {
                            case attacks.attShoot:
                                basedStringAd2 = basedStringAd2 + ">shoot (" + where[i].name + ")\n";
                                break;
                            case attacks.attMelee:
                                basedStringAd2 = basedStringAd2 + ">melee (" + where[i].name + ")\n";
                                break;
                            case attacks.attReload:
                                basedStringAd2 = basedStringAd2 + ">rest \n";
                                break;
                            case attacks.none:
                                break;
                        }
                    }
                     
                    }

                    attQDispAd1.GetComponent<TextMeshPro>().text = basedStringAd1;
                    attQDispAd2.GetComponent<TextMeshPro>().text = basedStringAd2;

                    basedStringAd1 = "";
                    basedStringAd2 = "";

                  
                }
        }
        else
        {
            attQDispAd1.GetComponent<MeshRenderer>().enabled = false;
            attQDispAd2.GetComponent<MeshRenderer>().enabled = false;
            attQDisp.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
