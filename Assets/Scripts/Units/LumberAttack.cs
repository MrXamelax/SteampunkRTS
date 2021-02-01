using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class LumberAttack : MonoBehaviour
{
    public int damage = 5;

    PhotonView PV;
    public SpriteRenderer LumberRend;
    float attackTimer = 0;
    private List<GameObject> targets = new List<GameObject>();

    void Start()
    {
        PV = GetComponent<PhotonView>();

    }
    void Update()
    {
        if (!PV?.IsMine ?? false) return;
        PV.RPC("ShowLumber", RpcTarget.All, transform.position, transform.position);
        attackTimer = Mathf.Clamp(attackTimer - Time.deltaTime, 0, 10);

        if (attackTimer <= 0f)
        {
            targets.ForEach((t) => t?.GetComponent<PhotonView>()?.RPC("takeDMG", RpcTarget.All, this.damage));
            if (this)
                PhotonNetwork.Destroy(this.gameObject);
        }
    }

    //triggered it we collide with something
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision with lumber attack");
        if (!PV?.IsMine ?? false)
            return;
        if (other.GetComponent<Targetable>() && !other.GetComponent<PhotonView>().IsMine)
            targets.Add(other.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!PV?.IsMine ?? false)
            return;
        if (collision.GetComponent<Targetable>() && !collision.GetComponent<PhotonView>().IsMine)
            targets.Remove(collision.gameObject);
    }

    [PunRPC]
    private void ShowLumber(Vector3 targetPosition, Vector3 startPosition)
    {
        LumberRend.color = new Color(0, 0, 0, (1f - 1 * attackTimer) / 2);
        this.transform.localScale = new Vector3(3f - 3 * attackTimer, 3f - 3 * attackTimer, 0);
    }

    public void attack(GameObject _targetToAttack)
    {
        attackTimer = 1f;
    }
}
