using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class combatLoop : MonoBehaviour
{
    public GameObject attackObj1, attackObj2;
    public Animator critTextAnim , critBarAnim;
    public GameObject critText, critBar1,critBar2;
    public bool hasInitiative;
    public GameObject attacker, target;
    public int combatPhase, attIndex, _baseAtt, _baseAcc, _baseCrit;
    public GameObject[] combatLog;
    public int combatLogIndex, _attQueueLength;
    public string[] combatMsg;
    public int waitTimer, waitThreshold, timerCount;
    public bool waitTimerActive;
    public bool attDebounce , combatStepDebounce ,overDebounce;
    public bool testNewCombat , critAnim;
    public GameObject textLog , critLog;
    public int hitStep , hitMax;


    // Start is called before the first frame update
    void Start()
    {
        combatLogIndex = 0;
        combatPhase = 1;
        EvaluateTurn(attackObj1, attackObj2);
    }

    // Update is called once per frame
    void Update()
    {
        EvaluateTurn(attackObj1, attackObj2);
      
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Application.Quit();
            }
       
        if(Input.GetKeyDown(KeyCode.Q))
        {
            combatPhase = 2;
            testNewCombat = !testNewCombat;
        }
        if(testNewCombat)
        {
            doTurn();
        }
        critText.GetComponent<MeshRenderer>().enabled = critAnim;

        critBar1.GetComponent<SpriteRenderer>().enabled = critAnim;
        critBar2.GetComponent<SpriteRenderer>().enabled = critAnim;

    }


    void doTurn()
    {
        switch (combatPhase)
        {
            case 1:
                overDebounce = false;

                break;
            case 2:
                if(!combatStepDebounce)
                {
                  

                    // setup the stuff you only need once here oki uwu
                    EvaluateTurn(attackObj1, attackObj2);
                    if(attacker.GetComponent<plyAttacks>())
                    {
                        hitMax = attacker.GetComponent<plyAttacks>().attQLast;
                        hitStep = 0;
                    }
                    else
                    {
                        //change this if you give the enemy some amount of AI
                        hitMax = 1;
                        hitStep = 0;

                    }


                    combatStepDebounce = true;
                }

                combatStep();
                break;
            case 3:
                if (!combatStepDebounce)
                {

                    // setup the stuff you only need once here oki uwu
                    EvaluateTurn(attackObj1, attackObj2);
                    if (attacker.GetComponent<plyAttacks>())
                    {
                        hitMax = attacker.GetComponent<plyAttacks>().attQLast;
                        hitStep = 0;
                    }
                    else
                    {
                        //change this if you give the enemy some amount of AI
                        hitMax = 1;
                        hitStep = 0;

                    }


                    combatStepDebounce = true;
                }

                combatStep();
                break;
            case 4:
                critAnim = false;
                if (!overDebounce)
                {
                    critAnim = false;
                    overDebounce = true;
                    textLog.GetComponent<TextMeshPro>().text = "Combat Over";
                    combatPhase = 1;
                    attackObj1.GetComponent<stats>().currentap = Mathf.Clamp(attackObj1.GetComponent<stats>().currentap - attackObj1.GetComponent<plyAttacks>().apCostTotal, 0, attackObj1.GetComponent<stats>().maxap);
                    attackObj1.GetComponent<plyAttacks>().apCostTotal = 0;
                    attackObj1.GetComponent<stats>().currentap += 2;
                    attackObj1.GetComponent<plyAttacks>().attQ.Clear();
                    attackObj1.GetComponent<plyAttacks>().where.Clear();
                    attackObj1.GetComponent<plyAttacks>().attQLast = 0;
                    testNewCombat = false;

                    if (attackObj1.GetComponent<stats>().hp <= 0)
                    {
                        attackObj1.transform.position = new Vector3(9999, 999, 999);
                    }
                    if(attackObj2.GetComponent<stats>().hp <= 0)
                    {
                        Animator deathAnim = attackObj2.GetComponent<Animator>();
                        Debug.Log("died");
                        deathAnim.SetTrigger("died");
                        attackObj2.transform.position = new Vector3(9999, 999, 999);
                    }
                    if (attackObj1.GetComponent<stats>().hp <= 0)
                    {
                        Animator deathAnim = attackObj1.GetComponent<Animator>();
                        Debug.Log("died");
                        deathAnim.SetTrigger("died");
                        attackObj2.transform.position = new Vector3(9999, 999, 999);
                    }
                }
                //End of combat
                //check if dead
               
                break;
        }
    }


    void combatStep()
    {
      
        if(hitStep != hitMax)
        {
            if (waitTimer < waitThreshold)
            {
                waitTimer++;
                if (!attDebounce)
                {
                    critAnim = false;
                    if (attacker.GetComponent<plyAttacks>())
                    {
                        attacker.GetComponent<plyAttacks>().loadAtt(hitStep); //in theory loads att in attackers at the attack step needed
                        HandleDirectAttacks();
                    }
                    else
                    {
                        //add stuff here for ai retaliation
                        HandleDirectAttacks();
                    }
                    // do attack once
                    attDebounce = true;
                }
            }
            else
            {
                hitStep++;
                waitTimer = 0;
                attDebounce = false;

            }
        }
        else
        {

            attDebounce = false;
            combatStepDebounce = false;
            combatPhase++;
        }
    
    }
 
    void EvaluateTurn(GameObject obj1, GameObject obj2)
    {
        stats _objstats1 = obj1.GetComponent<stats>();
        stats _objstats2 = obj2.GetComponent<stats>();

        int _agi1 = _objstats1.agi;
        int _agi2 = _objstats2.agi;

        if(combatPhase != 3)
        {
            if (_agi1 > _agi2)
            {
                attacker = obj1;
                target = obj2;
            }
            else
            {
                attacker = obj2;
                target = obj1;
            }
        }
        else
        {
            if (_agi1 < _agi2)
            {
                attacker = obj1;
                target = obj2;
            }
            else
            {
                attacker = obj2;
                target = obj1;
            }
        }
       








    }

    void HandleDirectAttacks()
    {


        //get attacker stats
        float _crit;
        int _HP = attacker.GetComponent<stats>().hp;
        int _attMod = attacker.GetComponent<stats>().att;
        int _critmod = attacker.GetComponent<stats>().critmod;
        int _accMod = attacker.GetComponent<stats>().acc;

        if (attacker.tag == "ply")
        {
            plyAttacks _attacks = attacker.GetComponent<plyAttacks>();

            _baseAtt = _attacks.baseDmg;
            _baseCrit = _attacks.basecrit;

        }
        else
        {
            attacks _attacks = attacker.GetComponent<attacks>();

            _baseAtt = _attacks.baseDmg;
            _baseCrit = _attacks.basecrit;
            _baseAcc = _attacks.baseacc;
        }


        if (target.GetComponent<attacks>())
        {
            // sets the accuracy to be based on what part is being attacked
            switch (target.GetComponent<stats>().bodyPart[attIndex].tag)
            {
                case "head":
                    _baseAcc = target.GetComponent<stats>().bodyPart[attIndex].GetComponent<headBehaviour>().baseAcc;
                    break;
                case "chest":
                    _baseAcc = target.GetComponent<stats>().bodyPart[attIndex].GetComponent<chestBehaviour>().baseAcc;
                    break;
                case "arm":
                    _baseAcc = target.GetComponent<stats>().bodyPart[attIndex].GetComponent<armBehaviour>().baseAcc;
                    break;
                case "leg":
                    _baseAcc = target.GetComponent<stats>().bodyPart[attIndex].GetComponent<legBehaviour>().baseAcc;
                    break;
                case "hat":
                    _baseAcc = target.GetComponent<stats>().bodyPart[attIndex].GetComponent<hatBehaviour>().baseAcc;
                    break;
                case "weapon":
                    _baseAcc = target.GetComponent<stats>().bodyPart[attIndex].GetComponent<weaponBehaviour>().baseAcc;
                    break;
            }

        }

        int _targetArm = target.GetComponent<stats>().armor;


        int _finalAcc = Mathf.Clamp((_baseAcc + (_accMod * 2)), 0, 100);

        int accChance = Random.Range(1, 100);


        if (accChance < _finalAcc)
        {

            if (_baseCrit != 0)
            {
                float _critChance = Mathf.Clamp(((_baseCrit / 100) + 1) * (_critmod + 25), 0, 100);

                int ccRoll = Random.Range(1, 100);
                if (Random.Range(1, 100) <= _critChance)
                {
                    Debug.Log("crit");
                    _crit = 1.5f;
                    critAnim = true;
                }
                else
                {
                    _crit = 1;
                    critAnim = false;
                }
            }
            else
            {
                _crit = 1;
                critAnim = false;
            }

            int _bonusdmg = (_attMod / 10) * (_baseAtt / 2);
            int _dmg = (int)((_baseAtt + _bonusdmg) * (_crit));

            _dmg = Mathf.Clamp((_dmg - _targetArm), 0, 200);

            // damage section

            // damages the part you selected
            if (target.GetComponent<attacks>())
            {
                switch (target.GetComponent<stats>().bodyPart[attIndex].tag)
                {
                    case "head":
                        Debug.Log("head");
                        target.GetComponent<stats>().bodyPart[attIndex].GetComponent<headBehaviour>().partHP -= _dmg;
                        break;
                    case "chest":
                        Debug.Log("chest");
                        target.GetComponent<stats>().bodyPart[attIndex].GetComponent<chestBehaviour>().partHP -= _dmg;

                        break;
                    case "arm":
                        Debug.Log("arm");
                        target.GetComponent<stats>().bodyPart[attIndex].GetComponent<armBehaviour>().partHP -= _dmg;

                        break;
                    case "leg":
                        Debug.Log("leg");
                        target.GetComponent<stats>().bodyPart[attIndex].GetComponent<legBehaviour>().partHP -= _dmg;

                        break;
                    case "hat":
                        Debug.Log("hat");
                        target.GetComponent<stats>().bodyPart[attIndex].GetComponent<hatBehaviour>().partHP -= _dmg;

                        break;
                    case "weapon":
                        Debug.Log("weapon");
                        target.GetComponent<stats>().bodyPart[attIndex].GetComponent<weaponBehaviour>().partHP -= _dmg;

                        break;
                }

                target.GetComponent<stats>().hp -= _dmg;
            }

            if (target.tag == "ply")
            {
                target.GetComponent<stats>().hp -= _dmg;
            }

            textLog.GetComponent<TextMeshPro>().text = attacker.name + " hit " + target.name + " for " + _dmg;

            //end of damage section


        







        }
        else
        {
            textLog.GetComponent<TextMeshPro>().text = attacker.name + " missed ";
        }





    }
}
