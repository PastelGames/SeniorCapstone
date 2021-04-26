using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//This class will manage the hitbox triggers.
public class Combat : MonoBehaviour
{
    public float lightHurtDuration;
    public float heavyHurtDuration;

    public GameObject p1;
    public GameObject p2;
    public BoxCollider2D p1Hurtbox;
    public BoxCollider2D p1Hitbox;
    public BoxCollider2D p2Hurtbox;
    public BoxCollider2D p2Hitbox;
    public AnimationClip p2HurtClip;
    public Recorder recorder;

    public int p1HP;
    public int p2HP;

    public TMP_Text p1HealthText;
    public TMP_Text p2Healthtext;

    public int startingHP;

    bool p1Hit;
    bool p2Hit;

    bool p1Heavy;
    bool p2Heavy;

    float p1HurtDuration;
    float p2HurtDuration;

    public Animator p1Anim;
    public Animator p2Anim;

    Vector3 p1StartingPosition;
    Vector3 p2StartingPosition;

    public float timeLimit;
    public float timeRemaining;
    public TMP_Text timerText;

    // Start is called before the first frame update
    void Start()
    {
        p1HP = startingHP;
        p2HP = startingHP;

        p1Anim = p1.GetComponent<Animator>();
        p2Anim = p2.GetComponent<Animator>();

        p1StartingPosition = p1.transform.position;
        p2StartingPosition = p2.transform.position;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeRemaining -= Time.deltaTime;

        p1Heavy = p1Anim.GetCurrentAnimatorStateInfo(0).IsName("Heavy Attack");
        p2Heavy = p2Anim.GetCurrentAnimatorStateInfo(0).IsName("Heavy Attack");

        //In the case that they both hit each other.
        if (p1Hitbox.IsTouching(p2Hurtbox) && p2Hitbox.IsTouching(p1Hurtbox))
        {
            p1Hit = true;
            p2Hit = true;
        }
        else if (p1Hitbox.IsTouching(p2Hurtbox))
        {
            p2Hit = true;
        }
        else if (p2Hitbox.IsTouching(p1Hurtbox))
        {
            p1Hit = true;
        }

        if (p1Hit)
        {
            if (p1Anim.GetCurrentAnimatorStateInfo(0).IsName("Block Idle") && !p2Heavy)
            {
                p1Anim.SetTrigger("Hit While Blocking");
            }
            else if (p2Heavy)
            {
                p1Anim.SetFloat("Hurt Speed", p2HurtClip.length / heavyHurtDuration);
                p1Anim.SetBool("Hurt", true);
                p1HP -= 2;
                p2.GetComponent<FighterAgent>().AddReward(2f);
                p1.GetComponent<FighterAgent>().AddReward(-2f);
            }
            else if (!p1Anim.GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
            {
                p1Anim.SetFloat("Hurt Speed", p2HurtClip.length / lightHurtDuration);
                p1Anim.SetBool("Hurt", true);
                p1Hurtbox.gameObject.SetActive(false);
                p1HP -= 1;
                p2.GetComponent<FighterAgent>().AddReward(1f);
                p1.GetComponent<FighterAgent>().AddReward(-1f);
            }

            p1Hit = false;
        }

        if (p2Hit)
        {
            if (p2Anim.GetCurrentAnimatorStateInfo(0).IsName("Block Idle") && !p1Heavy)
            {
                p2Anim.SetTrigger("Hit While Blocking");
            }
            else if (p1Heavy)
            {
                p2Anim.SetFloat("Hurt Speed", p2HurtClip.length / heavyHurtDuration);
                p2Anim.SetBool("Hurt", true);
                p2HP -= 2;
                p2.GetComponent<FighterAgent>().AddReward(-2f);
                p1.GetComponent<FighterAgent>().AddReward(2f);
            }
            else if (!p2Anim.GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
            {
                p2Anim.SetFloat("Hurt Speed", p2HurtClip.length / lightHurtDuration);
                p2Anim.SetBool("Hurt", true);
                p2Hurtbox.gameObject.SetActive(false);
                p2HP -= 1;
                p2.GetComponent<FighterAgent>().AddReward(-1f);
                p1.GetComponent<FighterAgent>().AddReward(1f);
            }
            p2Hit = false;
        }

        UserFeedback();

        if (IsFightOver())
        {
            ResetFight();
            p1.GetComponent<FighterAgent>().EndEpisode();
            p2.GetComponent<FighterAgent>().EndEpisode();
        }
        
    }

    public void ResetFight()
    {
        p1HP = startingHP;
        p2HP = startingHP;

        p1.transform.position = p1StartingPosition;
        p2.transform.position = p2StartingPosition;

        timeRemaining = timeLimit;
    }

    public bool IsFightOver()
    {

        if (timeRemaining < 0)
        {
            if (p1HP == 15 && p2HP == 15)
            {
                p2.GetComponent<FighterAgent>().AddReward(-5f);
                p1.GetComponent<FighterAgent>().AddReward(-5f);
            }

            if (p1HP < p2HP)
            {
                p2.GetComponent<FighterAgent>().AddReward(3.5f);
                p1.GetComponent<FighterAgent>().AddReward(-3.5f);
                recorder.data.player2Wins++;
            }
            else if (p1HP > p2HP)
            {
                p2.GetComponent<FighterAgent>().AddReward(-3.5f);
                p1.GetComponent<FighterAgent>().AddReward(3.5f);
                recorder.data.player1Wins++;
            }

            return true;
        }

        else if (p2HP <= 0 && p1HP <= 0)
        {
            return true;
        }

        else if (p2HP <= 0)
        {
            p2.GetComponent<FighterAgent>().AddReward(-5f);
            p1.GetComponent<FighterAgent>().AddReward(5f);
            recorder.data.player1Wins++;
            return true;
        }

        else if (p1HP <= 0)
        {
            p2.GetComponent<FighterAgent>().AddReward(5f);
            p1.GetComponent<FighterAgent>().AddReward(-5f);
            recorder.data.player2Wins++;
            return true;
        }

        return false;
    }

    void UserFeedback()
    {
        p1HealthText.text = p1HP + "/" + startingHP;
        p2Healthtext.text = p2HP + "/" + startingHP;
        timerText.text = Mathf.Floor(timeRemaining).ToString();
    }
}
