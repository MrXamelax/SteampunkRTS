﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour {
    // Start is called before the first frame update

    // Everything below 0 = own mine
    // Everything above 0 = enemey mine
    [Range(-500, 500)][SerializeField] int ownershipPoints;
    [SerializeField] int fogOfWar;
    private bool down = true;
    //private int unitsMaster = 0;
    //private int unitsClient = 0;

    void Start() {

    }

    // Update is called once per frame
    void Update() {

        if (ownershipPoints == -500) down = false;
        if (ownershipPoints == 500) down = true;

        if (down) ownershipPoints--;
        else ownershipPoints++;
    }

    //[PunRPC]
    //private void 
    
    private void OnTriggerEnter2D(Collider2D col) {
        Debug.Log("Kollision, mein Freund!");
        //if (col.GetComponent<PhotonView>().Owner) Debug.LogWarning("Meins!");
        //else Debug.LogWarning("Nicht meins!");
    }
    
    /*
    private void OnTriggerEnter(Collider col) {
        Debug.Log("Kollision, mein Freund!");
       // if (col.GetComponent<PhotonView>().Owner.IsMasterClient) Debug.LogWarning("Meins!");
       // else Debug.LogWarning("Nicht meins!");
    }
    */
}
