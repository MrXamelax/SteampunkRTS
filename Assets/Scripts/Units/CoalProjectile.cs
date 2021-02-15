using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoalProjectile : MonoBehaviour
{
    GameObject targetGO = null;
    public int moveSpeed = 5;
    public int damage = 1;

    PhotonView PV;

    void Start()
    {
        PV = GetComponent<PhotonView>();
    }
    void Update()
    {
        if (targetGO)
        {
            if (targetGO.transform.position == transform.position)
            {
                Debug.Log("gleiche position wie ziel");
                hitEvaluation(targetGO);
            }
            float step = moveSpeed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(this.transform.position, targetGO.transform.position, step);
        }
        else if (PV?.IsMine ?? false)
            PhotonNetwork.Destroy(this.gameObject);
    }

    public void attack(GameObject _targetToAttack)
    {
        Debug.Log(_targetToAttack);
        this.targetGO = _targetToAttack;
    }

    //triggered it we collide with something
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (PV?.IsMine ?? false)
            hitEvaluation(other.gameObject);
    }

    private void hitEvaluation(GameObject other)
    {
        var targetableScript = other.GetComponent<Targetable>();
        if (targetableScript)
        {
            PhotonView otherPV = PhotonView.Get(other);
            if (otherPV.IsMine)
                return;

            Debug.Log("Hit enemy? : " + other.name);

            otherPV.RPC("takeDMG", RpcTarget.All, this.damage);
        }
        if(this)
            PhotonNetwork.Destroy(this.gameObject);
    }
}