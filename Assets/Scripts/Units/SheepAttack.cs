using Photon.Pun;
using UnityEngine;

public class SheepAttack : MonoBehaviour
{
    GameObject targetGO = null;
    public int damage = 8;

    PhotonView PV;
    public LineRenderer LineRend;
    [SerializeField] float chargeAttackTime = 5f;
    [SerializeField] float attackRange = 11f;
    float attackTimer = 0;
    private GameObject sheep;
    
    void Start()
    {
        PV = GetComponent<PhotonView>();
        transform.position = sheep.transform.position;
    }
    void Update()
    {
        if (targetGO)
        {
            if (!PV?.IsMine ?? false) return;



            PV.RPC("ShowBeam", RpcTarget.All, targetGO.transform.position, transform.position);
            if (transform.position != sheep.transform.position || Vector3.Distance(targetGO.transform.position, transform.position) > attackRange)
            {
                sheep.GetComponent<UnitController>().attackOnCooldown = false;
                if (this)
                    PhotonNetwork.Destroy(this.gameObject);
                return;
            }

            attackTimer = Mathf.Clamp(attackTimer - Time.deltaTime, 0, 10);

            if (attackTimer <= 0f && targetGO.GetComponent<Targetable>())
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

    public void SetSheep(GameObject go)
    {
        sheep = go;
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
        attackTimer = chargeAttackTime;
    }
}