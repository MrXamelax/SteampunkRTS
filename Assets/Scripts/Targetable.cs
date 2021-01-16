using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetable : MonoBehaviour
{
    public int hp = 1;

    //calling this via its own PhotonView.RPC() leads to the funktion getting called 
    //synchronously by all clients in the network 
    [PunRPC]
    public void takeDMG(int _dmg)
    {
        Debug.Log(this.gameObject.name + ": Ouch, i took " + _dmg + " dmg.");
        //take dmg
        this.hp -= _dmg;

        //when dead...die
        if (this.hp <= 0)
            killself();
    }

    //the takeDMG is called by everyone in the network, so wie only need to Destroy locally
    void killself()
    {
        print("killing myself - " + this.gameObject.name + " of player " + this.gameObject.GetComponent<PhotonView>().Owner.NickName);

        Destroy(this.gameObject);
    }

}
