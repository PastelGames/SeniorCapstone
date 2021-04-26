using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Block : MonoBehaviour
{

    Animator anim;
    Controls controls;
    FighterAgent fa;

    bool blockingAllowed = true;

    public int playerID;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        controls = GameObject.FindObjectOfType<Controls>();
        fa = GetComponent<FighterAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (blockingAllowed)
        {
            if (Input.GetKey(controls.GetKey(playerID, ControlKeys.Block)) || (fa.playerID == playerID && fa.blockDown && fa.isActiveAndEnabled))
            {
                anim.SetBool("Blocking", true);
            }
            if (Input.GetKeyUp(controls.GetKey(playerID, ControlKeys.Block)) || (fa.playerID == playerID && !fa.blockDown && fa.isActiveAndEnabled))
            {
                anim.SetBool("Blocking", false);
            }
        }
    }

    public void NoMoreBlocking()
    {
        blockingAllowed = false;
        anim.SetBool("Blocking", false);
    }

    public void AllowBlocking()
    {
        blockingAllowed = true;
    }

}
