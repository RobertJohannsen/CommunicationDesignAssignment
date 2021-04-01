using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endCritAnim : MonoBehaviour
{
    public combatLoop cLoop;
    // Start is called before the first frame update
    void Start()
    {
        cLoop = Camera.main.GetComponent<combatLoop>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndCrit()
    {
        cLoop.critAnim = false;
    }
}
