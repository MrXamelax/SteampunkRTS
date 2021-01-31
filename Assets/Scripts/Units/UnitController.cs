using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
{
    [NonSerialized] public PhotonView Pview;
    public int range = 1;
    public int speed = 1;
    bool attackOnCooldown = false;
    [SerializeField] protected float attackCooldown = 1f;

    public GameObject projectile;
    NavMeshAgent agent;
    [SerializeField] protected SpriteRenderer spriterenderer;
    float stopCooldown = 0;
    private GameObject target;
    bool inRange = false;

    private void Awake()
    {
        // Init PhotonView
        Pview = GetComponent<PhotonView>();

        // Init Nav agent
        agent = GetComponent<NavMeshAgent>();

        // Fix parameters for 2d nav mesh 
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        agent.speed = speed;

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

        if (target)
        {
            tryPerformAttackMove();
        }
    }

    public void receiveCommand(RaycastHit2D _hit)
    {
        if ((_hit.collider.CompareTag("Controllable") || _hit.collider.CompareTag("Building")) &&
          !_hit.collider.GetComponent<PhotonView>().IsMine)
        {
            target = _hit.collider.gameObject;           
        }
        else
            moveTo(_hit.point);
    }

    public void moveTo(Vector2 _destination)
    {
        agent.SetDestination(_destination);
        stopCooldown = 1f;
        agent.isStopped = false;
        target = null;
    }

    void tryPerformAttackMove()
    {
        //when in range...start attacking
        float distance = (target.transform.position - this.transform.position).magnitude;
        if (distance <= range)
        {
            agent.isStopped = true;
            inRange = true;
        }
        else //...move to the target
        {
            this.inRange = false;
            moveTo(target.transform.position);
        }

        //if everything is good...trigger the attack
        if (target && inRange && !attackOnCooldown)
            issueAttack();

    }
    void issueAttack()
    {
        attackOnCooldown = true;

        Vector3 projectileSpawnPos = this.transform.position +
            Vector3.up / 2 + (target.transform.position - this.transform.position).normalized;

      
        // Attack if have an attack move
        if (projectile)
        {
            GameObject projectileClone = PhotonNetwork.Instantiate("Projectiles/" + projectile.name, projectileSpawnPos, Quaternion.identity);
            projectileClone.BroadcastMessage("attack", target);
        }

        //start timer to reset cooldown
        StartCoroutine("attackCooldownReset");
    }

    private void OnDestroy()
    {
        GameController.Instance.removeFromSelection(this.gameObject);
    }

    //attack will only trigger when cooldown is "false"
    IEnumerator attackCooldownReset()
    {
        yield return new WaitForSeconds(attackCooldown);
        attackOnCooldown = false;
    }
}
