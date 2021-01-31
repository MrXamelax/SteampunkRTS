using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetable : MonoBehaviour
{
    [SerializeField] protected int maxHp = 20;
    [SerializeField] protected GameObject hpGreenBar;

    private int currentHP;
    private bool myBase;

    private void Awake()
    {
        currentHP = maxHp;
        if (GetComponent<Base>() && GetComponent<PhotonView>().IsMine) myBase = true;
    }

    [PunRPC]
    public void takeDMG(int _dmg)
    {
        currentHP -= _dmg;

        if (myBase) LoggingManager.Instance.updBaseHp(currentHP);

        hpGreenBar.transform.localScale = new Vector3(1.0f / maxHp * currentHP, hpGreenBar.transform.localScale.y, hpGreenBar.transform.localScale.z);

        if (currentHP <= 0)
            killself();
    }

    void killself() => Destroy(this.gameObject);
    

}
