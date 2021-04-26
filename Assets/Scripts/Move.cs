using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

public class Move : MonoBehaviour
{
    public int playerID;

    Controls controls;
    Rigidbody2D rb;
    Animator anim;
    FighterAgent fa;

    int moveMultiplier;
    public float moveSpeed = 5;
    bool moving;
    public bool locked;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = FindObjectOfType<Controls>();
        anim = GetComponent<Animator>();
        fa = GetComponent<FighterAgent>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DigestInput();

        if (locked)
        {
            moving = false;
            moveMultiplier = 0;
        }

        anim.SetBool("Moving", moving);
        anim.SetFloat("Move Multiplier", moveMultiplier * gameObject.transform.localScale.x);
        rb.velocity = new Vector2(moveSpeed * moveMultiplier, rb.velocity.y);
    }

    public void DigestInput()
    {
        if (Input.GetKey(controls.GetKey(playerID, ControlKeys.RightKey)) || (fa.playerID == playerID && fa.moveValue == 0 && fa.isActiveAndEnabled))
        {
            moveMultiplier = 1;
            moving = true;
        }
        else if (Input.GetKey(controls.GetKey(playerID, ControlKeys.LeftKey)) || (fa.playerID == playerID && fa.moveValue == 1 && fa.isActiveAndEnabled))
        {
            moveMultiplier = -1;
            moving = true;
        }
        else
        {
            moveMultiplier = 0;
            moving = false;
        }
    }
}
