using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Elefant : MonoBehaviour
{
    [NonSerialized] public PhotonView Pview;

    NavMeshAgent agent;


    private void Awake()
    {
        // Init PhotonView
        Pview = GetComponent<PhotonView>();

        // Init Nav agent
        agent = GetComponent<NavMeshAgent>();

        // Fix parameters for 2d nav mesh 
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
    }

    private void Update()
    {
        if (agent.velocity.sqrMagnitude > 0 && agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.isStopped = true;
            agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        }
    }

    public void receiveCommand(RaycastHit2D _hit)
    {
        //if we hit a controllable unit which does not belong to us, attack it
        if (_hit.collider.CompareTag("Controllable") && 
          !_hit.collider.GetComponent<PhotonView>().IsMine) 
        {
            //attackTarget = _hit.collider.gameObject;
            //issueAttack();
        }
        //Debug.Log(_hit);
        //attackTarget = null;
        Pview.RPC("moveTo", RpcTarget.All, _hit.point);
    }

    [PunRPC]
    public void moveTo(Vector2 _destination)
    {
        agent.SetDestination(_destination);
        agent.isStopped = false;
    }
}
