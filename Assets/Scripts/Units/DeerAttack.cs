using Photon.Pun;
using UnityEngine;

public class DeerAttack : MonoBehaviour
{
    GameObject targetGO = null;
    public int damage = 5;

    PhotonView PV;

    void Start()
    {
        PV = GetComponent<PhotonView>();
    }
    void Update()
    {
        if (targetGO)
        {

            if (!PV?.IsMine ?? false) return;

            var targetableScript = targetGO.GetComponent<Targetable>();
            if (targetableScript)
            {
                PhotonView otherPV = PhotonView.Get(targetGO);
                if (otherPV.IsMine)
                    return;
                Debug.Log("Deer kick: " + targetGO);
                otherPV.RPC("takeDMG", RpcTarget.All, this.damage);
            }
            if (this)
                PhotonNetwork.Destroy(this.gameObject);
        }
        else if (PV?.IsMine ?? false)
            PhotonNetwork.Destroy(this.gameObject);
    }

    public void attack(GameObject _targetToAttack)
    {
        Debug.Log(_targetToAttack);
        this.targetGO = _targetToAttack;
    }
}