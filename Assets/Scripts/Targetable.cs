using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetable : MonoBehaviour
{
    [SerializeField] protected int maxHp = 20;
    [SerializeField] protected GameObject hpGreenBar;

    private int currentHP;

    private void Awake()
    {
        currentHP = maxHp;
    }

    [PunRPC]
    public void takeDMG(int _dmg)
    {
        currentHP -= _dmg;

        hpGreenBar.transform.localScale = new Vector3(1.0f / maxHp * currentHP, hpGreenBar.transform.localScale.y, hpGreenBar.transform.localScale.z);

        if (currentHP <= 0)
            killself();
    }

    void killself() => Destroy(this.gameObject);
    

}
