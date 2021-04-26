using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Attack : MonoBehaviour
{
    Animator anim;
    Controls controls;
    Move move;
    FighterAgent fa;

    public int playerID;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        controls = GameObject.FindObjectOfType<Controls>();
        move = GetComponent<Move>();
        fa = GetComponent<FighterAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //Light Attack
        if (Input.GetKeyDown(controls.GetKey(playerID, ControlKeys.LightAttack)) || (fa.playerID == playerID && fa.lightAttackDown && fa.isActiveAndEnabled)) 
        {
            anim.SetBool("Light Attack", true);
        }
        //Heavy Attack
        else if (Input.GetKeyDown(controls.GetKey(playerID, ControlKeys.HeavyAttack)) || (fa.playerID == playerID && fa.heavyAttackDown && fa.isActiveAndEnabled))
        {
            anim.SetBool("Heavy Attack", true);
        }
    }
}
