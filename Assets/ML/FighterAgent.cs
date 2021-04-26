using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Enums;

public class FighterAgent : Agent
{

    Move move;
    Combat combat;
    Controls controls;

    GameObject otherPlayer;

    public int playerID;

    public bool lightAttackDown;
    public bool heavyAttackDown;
    public bool blockDown;
    public int moveValue;

    private void Start()
    {
        combat = FindObjectOfType<Combat>();
        controls = FindObjectOfType<Controls>();

        if (playerID == 0) otherPlayer = combat.p2;
        else otherPlayer = combat.p1;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position.x);
        sensor.AddObservation(otherPlayer.transform.position.x);
        if (combat)
        {
            sensor.AddObservation(combat.p1HP);
            sensor.AddObservation(combat.p2HP);
            sensor.AddObservation(combat.timeRemaining);
            sensor.AddObservation(combat.p1Anim.GetCurrentAnimatorStateInfo(0).IsName("Heavy Attack") ? 1 : 0);
            sensor.AddObservation(combat.p1Anim.GetCurrentAnimatorStateInfo(0).IsName("Light Attack") ? 1 : 0);
            sensor.AddObservation(combat.p1Anim.GetCurrentAnimatorStateInfo(0).IsName("Block Idle") ? 1 : 0);
            sensor.AddObservation(combat.p2Anim.GetCurrentAnimatorStateInfo(0).IsName("Heavy Attack") ? 1 : 0);
            sensor.AddObservation(combat.p2Anim.GetCurrentAnimatorStateInfo(0).IsName("Light Attack") ? 1 : 0);
            sensor.AddObservation(combat.p2Anim.GetCurrentAnimatorStateInfo(0).IsName("Block Idle") ? 1 : 0);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[3] = (Input.GetKey(controls.GetKey(playerID, ControlKeys.Block))) ? 1 : 0;
        discreteActions[2] = (Input.GetKeyDown(controls.GetKey(playerID, ControlKeys.HeavyAttack))) ? 1 : 0;
        discreteActions[1] = (Input.GetKeyDown(controls.GetKey(playerID, ControlKeys.LightAttack))) ? 1 : 0;
        if (Input.GetKeyDown(controls.GetKey(playerID, ControlKeys.LeftKey)))
        {
            discreteActions[0] = 1;
        }
        else if (Input.GetKeyDown(controls.GetKey(playerID, ControlKeys.RightKey)))
        {
            discreteActions[0] = 0;
        }
        else
        {
            discreteActions[0] = 2;
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        //Movement for AI
        moveValue = actions.DiscreteActions[0];

        lightAttackDown = (actions.DiscreteActions[1] == 1);
        heavyAttackDown = (actions.DiscreteActions[2] == 1);
        blockDown = (actions.DiscreteActions[3] == 1);

    }

}
