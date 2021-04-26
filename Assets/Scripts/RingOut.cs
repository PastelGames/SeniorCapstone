using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingOut : MonoBehaviour
{

    Combat combat;

    // Start is called before the first frame update
    void Start()
    {
        combat = GameObject.FindObjectOfType<Combat>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player 1")) combat.p1HP = 0;
        if (collision.CompareTag("Player 2")) combat.p2HP = 0;
    }
}
