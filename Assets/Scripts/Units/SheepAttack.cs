using Photon.Pun;
using UnityEngine;

public class SheepAttack : MonoBehaviour
{
    GameObject targetGO = null;
    public int damage = 8;

    PhotonView PV;
    public LineRenderer LineRend;
    float attackTimer = 0;

    void Start()
    {
        PV = GetComponent<PhotonView>();
    }
    void Update()
    {
        if (targetGO)
        {
            if (!PV?.IsMine ?? false) return;
            PV.RPC("ShowBeam", RpcTarget.All, targetGO.transform.position, transform.position);

            attackTimer = Mathf.Clamp(attackTimer - Time.deltaTime, 0, 10);
            var targetableScript = targetGO.GetComponent<Targetable>();
            if (targetableScript && attackTimer <= 0f)
            {
                PhotonView otherPV = PhotonView.Get(targetGO);
                if (otherPV.IsMine)
                    return;
                otherPV.RPC("takeDMG", RpcTarget.All, this.damage);
                if (this)
                    PhotonNetwork.Destroy(this.gameObject);
            }
        }
        else if (PV?.IsMine ?? false)
            PhotonNetwork.Destroy(this.gameObject);
    }


    [PunRPC]
    private void ShowBeam(Vector3 targetPosition, Vector3 startPosition)
    {
        LineRend.positionCount = 2;
        LineRend.SetPosition(0, startPosition);
        LineRend.SetPosition(1, targetPosition);
    }
    
    public void attack(GameObject _targetToAttack)
    {
        Debug.Log(_targetToAttack);
        this.targetGO = _targetToAttack;
        attackTimer = 1f;
    }
}