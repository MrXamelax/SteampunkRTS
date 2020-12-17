using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour {
    // Start is called before the first frame update

    // Everything below 0 = own mine
    // Everything above 0 = enemey mine
    [Range(-500, 500)] [SerializeField] int ownershipPoints;
    [SerializeField] int fogOfWar;
    private bool down = true;
    [SerializeField] int unitsMaster = 0;
    [SerializeField] int unitsClient = 0;

    [SerializeField] Transform arrowTf;

    private PhotonView pv;

    private void Awake() {
        pv = this.gameObject.GetComponent<PhotonView>();
    }

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
        // Only cound units on Master Client
        if (!pv.Owner.IsMasterClient) return;
        //Debug.Log("Kollision, mein Freund!");
        if (col.gameObject.tag.Equals("Controllable")) {
            if (col.GetComponent<PhotonView>().Owner.IsMasterClient) {
                //Debug.LogWarning("Meins!");
                unitsMaster++;
            } else {
                //Debug.LogWarning("Nicht meins!");
                unitsClient++;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        // Only count units on Master Client
        if (!pv.Owner.IsMasterClient) return;

        // Only checks objects, that are controllable by players
        if (col.gameObject.tag.Equals("Controllable")) {

            // If it is your Unit, your count is now 1 less
            if (col.GetComponent<PhotonView>().Owner.IsMasterClient) {
                unitsMaster--;

            // If not, your opponent has now 1 less
            } else {
                unitsClient--;
            }
        }
    }
}
