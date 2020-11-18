using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EF_WD40Mk2 : MonoBehaviour
{
    public PhotonView Pview;
    //receive command from the GameController

    private void Awake()
    {
        Pview = GetComponent<PhotonView>();
    }
    public void receiveCommand(RaycastHit2D _hit)
    {
        //if we hit a controllable unit which does not belong to us...attack it
        if (_hit.collider.CompareTag("Controllable") && //applys to all units on the field
          !_hit.collider.GetComponent<PhotonView>().IsMine) //applys to all enemy units
        {
            //attackTarget = _hit.collider.gameObject;
            //issueAttack();
        }
        //if we hit the floor...start moving to click location
        Debug.Log(_hit.collider.tag);
        if (_hit.collider.CompareTag("Walkable"))
        {

            //attackTarget = null;

            Pview.RPC("moveTo", RpcTarget.All,_hit.point);
            
        }
    }

    //re-enable movement and set a destination
    [PunRPC]
    void moveTo(Vector2 _destination)
    {
        Debug.Log(this.transform.position.ToString() + " "  + _destination);

        // TODO let pathfinding move the unit 
        // pathfinder.MoveTo(_destination);

        transform.position = _destination;
    }
}
