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
    //calling this via its own PhotonView.RPC() leads to the funktion getting called 
    //synchronously by all clients in the network 
    [PunRPC]
    public void takeDMG(int _dmg)
    {
        Debug.Log(this.gameObject.name + ": Ouch, i took " + _dmg + " dmg.");
        //take dmg
        currentHP -= _dmg;

        hpGreenBar.transform.localScale = new Vector3(1.0f / maxHp * currentHP, hpGreenBar.transform.localScale.y, hpGreenBar.transform.localScale.z);

        //when dead...die
        if (currentHP <= 0)
            killself();
    }

    //the takeDMG is called by everyone in the network, so we only need to Destroy locally
    void killself()
    {
        //print("killing myself - " + this.gameObject.name + " of player " + this.gameObject.GetComponent<PhotonView>().Owner.NickName);
        Destroy(this.gameObject);
    }

}
