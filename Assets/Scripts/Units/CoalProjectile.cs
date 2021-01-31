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

    //arrow that follows opponent once fired. 
    //(yep, its facing upwards. more like a moving stick)
    //known bug: when targetGO gets deleted (e.g. when dead) the arrow just stops in place

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
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
            transform.position = Vector3.MoveTowards(this.transform.position, targetGO.transform.position + transform.up / 2, step);
            Debug.Log(targetGO.transform.position + " :: " + transform.position);
        }
        else if(PV?.IsMine ?? false)
            PhotonNetwork.Destroy(this.gameObject);
    }

    public void attack(GameObject _targetToAttack)
    {
        //Debug.Log(_targetToAttack);
        this.targetGO = _targetToAttack;
    }

    //triggered it we collide with something
    private void OnTriggerEnter2D(Collider2D other)
    {
        hitEvaluation(other.gameObject);
    }

    private void hitEvaluation(GameObject other)
    {
        Debug.Log("We hit " + other);
        //only the owner should handle actions of its bullet
        //so we just return if its not ours
        if (!PV.IsMine) return;

        //Debug.Log("We hit " + other);
        //only trigger dmg on targetable objects
        var targetableScript = other.GetComponent<Targetable>();
        if (targetableScript)
        {
            PhotonView otherPV = PhotonView.Get(other);
            if (otherPV.IsMine)
                return;
            otherPV.RPC("takeDMG", RpcTarget.All, this.damage);
        }
        if (PV.IsMine)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}