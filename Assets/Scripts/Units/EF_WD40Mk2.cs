using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EF_WD40Mk2 : MonoBehaviour
{
    public PhotonView Pview;

    [SerializeField] Pathfinder pathfinder;
    [SerializeField] public float moveSpeed = 1f;
    //receive command from the GameController
    List<Vector3> path;

    Vector2 finalTarget;

    private void Awake()
    {
        path = new List<Vector3>();
        Pview = GetComponent<PhotonView>();
    }


    private void Update()
    {
        if(path != null && path.Count != 0)
        {
            
            float step = moveSpeed * Time.deltaTime; // calculate distance to move
            //transform.position = Vector3.MoveTowards(this.transform.position, path[0] + transform.up / 2, step);
            
            
            setPosition(Vector3.MoveTowards(this.transform.position, path[0], step));
            //Vector3 newTarget = Vector3.MoveTowards(this.transform.position, path[0] + transform.up / 2, step);
            //Pview.RPC("setPosition", RpcTarget.All, new Vector2(newTarget.x, newTarget.y));
        }
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

        //attackTarget = null;

        Pview.RPC("moveTo", RpcTarget.All,_hit.point);

        finalTarget = _hit.point;
    }

    //re-enable movement and set a destination
    [PunRPC]
    void moveTo(Vector2 _destination)
    {
        //Debug.Log("MK2 at " + this.transform.position.ToString() + " wants move to "  + _destination);

        Vector3 target = new Vector3(_destination.x, _destination.y, 0f);

        path = pathfinder.findPath(this.transform.position, target);
    }

    void setPosition(Vector2 _destination)
    {

        

        if (transform.position.x < _destination.x)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 180, 0);

        transform.position = _destination;

        if (_destination == finalTarget)
            path.Remove(_destination);
    }
}
