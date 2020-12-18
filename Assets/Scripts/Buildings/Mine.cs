using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour {
    // Start is called before the first frame update

    // Everything below 0 = master mine
    // Everything above 0 = client mine
    [SerializeField] float ownershipPoints;
    [SerializeField] int fogOfWar;
    //private bool down = true;
    [SerializeField] int unitsMaster = 0;
    [SerializeField] int unitsClient = 0;

    // balancing stuff for later (maybe)
    //[SerializeField] int maxUnits = 5;

    [SerializeField] float captureDirection = 0;
    [SerializeField] float captureRate = 0.2f;

    [SerializeField] Transform arrowTf;
    [SerializeField] GameObject captureBar;
    private float captureBarSize = 0;

    private Transform[] barParts;

    private PhotonView pv;

    private void Awake() {
        pv = this.gameObject.GetComponent<PhotonView>();
        barParts = captureBar.GetComponentsInChildren<Transform>();

        for (int i = 1; i < barParts.Length; i++) {
            captureBarSize += barParts[i].transform.localScale.x;
        }
    }

    void Start() {

    }

    //&& ownershipPoints <= captureBarSize/2) {

    // Update is called once per frame
    void Update() {

        if (captureDirection != 0) {
            if (ownershipPoints >= -captureBarSize / 2) {
                if (ownershipPoints <= captureBarSize / 2) {
                    arrowTf.position += new Vector3(captureRate * captureDirection * Time.deltaTime, 0, 0);
                    ownershipPoints += captureRate * captureDirection * Time.deltaTime;
                } else {
                    if (captureDirection < 0) {
                        arrowTf.position += new Vector3(captureRate * captureDirection * Time.deltaTime, 0, 0);
                        ownershipPoints += captureRate * captureDirection * Time.deltaTime;
                    }
                }
            } else {
                if (captureDirection > 0) {
                    arrowTf.position += new Vector3(captureRate * captureDirection * Time.deltaTime, 0, 0);
                    ownershipPoints += captureRate * captureDirection * Time.deltaTime;
                }
            }
        }
        //if (ownershipPoints == -10) down = false;
        //if (ownershipPoints == 10) down = true;

        //if (down) ownershipPoints--;
        //else ownershipPoints++;
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
                updateUnits(1, 'm');
            } else {
                //Debug.LogWarning("Nicht meins!");
                updateUnits(1, 'c');
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
                updateUnits(-1, 'm');

                // If not, your opponent has now 1 less
            } else {
                updateUnits(-1, 'c');
            }
        }
    }

    //[PunRPC]
    private void updateUnits(int i, char who) {
        if (who == 'm') unitsMaster += i;
        if (who == 'c') unitsClient += i;

        if (unitsMaster == unitsClient) {
            captureDirection = 0;
            return;
        }
        if (unitsMaster > unitsClient) {
            captureDirection = -1;
            return;
        }
        if (unitsMaster < unitsClient) {
            captureDirection = 1;
            return;
        }
    }

    //[PunRPC]
    private void captureCheck() {
        if (unitsMaster == unitsClient) return;

    }

}
