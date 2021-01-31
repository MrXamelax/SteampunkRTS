using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
{
    [NonSerialized] public PhotonView Pview;

    NavMeshAgent agent;
    [SerializeField] protected SpriteRenderer spriterenderer;
    float stopCooldown = 0;
    private GameObject target;

    private void Awake()
    {
        // Init PhotonView
        Pview = GetComponent<PhotonView>();

        // Init Nav agent
        agent = GetComponent<NavMeshAgent>();

        // Fix parameters for 2d nav mesh 
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        //agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
    }

    private void Update()
    {
        if(stopCooldown > 0 )
            stopCooldown -= Time.deltaTime;
        if (stopCooldown <= 0 && agent.velocity.sqrMagnitude > 0 && agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.isStopped = true;
        } 

        if (agent.pathEndPosition.x > transform.position.x)
            spriterenderer.flipX = false;
        else
            spriterenderer.flipX = true;
    }

    public void receiveCommand(RaycastHit2D _hit)
    {
        //if we hit a controllable unit which does not belong to us, attack it
        if (_hit.collider.CompareTag("Controllable") &&
          !_hit.collider.GetComponent<PhotonView>().IsMine)
        {
            //attackTarget = _hit.collider.gameObject;
            //issueAttack();
            Pview.RPC("attack", RpcTarget.All, _hit.collider.gameObject);
        }
        else
            //Debug.Log(_hit);
            //attackTarget = null;
            moveTo(_hit.point);
        //Pview.RPC("moveTo", RpcTarget.All, _hit.point);
    }

    [PunRPC]
    public void moveTo(Vector2 _destination)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            agent.SetDestination(_destination);
            stopCooldown = 1f;
            agent.isStopped = false;
        }
    }

    public void attack(GameObject _target)
    {
        target = _target;
    }

    private void OnDestroy()
    {
        GameController.Instance.removeFromSelection(this.gameObject);
    }
}
