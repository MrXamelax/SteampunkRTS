using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour {

    // Everything below 0 = master mine
    // Everything above 0 = client mine
    [SerializeField] float ownershipPoints;
    [SerializeField] int fogOfWar;
    [SerializeField] int unitsMaster = 0;
    [SerializeField] int unitsClient = 0;
    [SerializeField] float captureDirection = 0;
    [SerializeField] float captureRate = 0.2f;

    private bool isCaptured = false;
    private bool sendingCoal = false;

    // balancing stuff for later (maybe)
    //[SerializeField] int maxUnits = 5;

    [SerializeField] protected Transform arrowTf;
    [SerializeField] protected GameObject captureBar;
    private float captureBarSize = 0;
    private float captureValue;

    private Transform[] barParts;

    private PhotonView pv;

    private void Awake() {
        pv = this.gameObject.GetComponent<PhotonView>();
        barParts = captureBar.GetComponentsInChildren<Transform>();

        for (int i = 1; i < barParts.Length; i++) {
            captureBarSize += barParts[i].localScale.x;
        }
        captureValue = barParts[2].localScale.x/2;
    }

    void Start() {
        
    }

    void Update() {

        // Coal mine is now captured by a side
        if (ownershipPoints <= -captureValue || ownershipPoints >= captureValue) {
            isCaptured = true;
            if (!sendingCoal) {
                if (ownershipPoints <= -captureValue) {
                    StartCoroutine(coalCycle('m'));
                    Debug.Log("Sending coal to master now.");
                    ResourceManager.Instance.updMines('+', 'm');
                    if (PhotonNetwork.IsMasterClient) UIManager.Instance.updMines(ResourceManager.Instance.getMinesMaster());
                }
                if (ownershipPoints >= captureValue) {
                    StartCoroutine(coalCycle('c'));
                    Debug.Log("Sending coal to client now.");
                    ResourceManager.Instance.updMines('+', 'c');
                    if (!PhotonNetwork.IsMasterClient) UIManager.Instance.updMines(ResourceManager.Instance.getMinesClient());
                }
                sendingCoal = true;
            }
        }

        // Coal mine is now neutral
        if (ownershipPoints < captureValue && ownershipPoints > -captureValue) {
            isCaptured = false;
            if (sendingCoal) {
                Debug.Log("Coal delivery stopped.");
                sendingCoal = false;
            }
            
        }

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
    }

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

    IEnumerator coalCycle(char actor) {
            ResourceManager.Instance.addCoal(100, actor);
            yield return new WaitForSeconds(2f);
        if (isCaptured) StartCoroutine(coalCycle(actor));
        else {
            ResourceManager.Instance.updMines('-', actor);
            if (PhotonNetwork.IsMasterClient) UIManager.Instance.updMines(ResourceManager.Instance.getMinesMaster());
            if (!PhotonNetwork.IsMasterClient) UIManager.Instance.updMines(ResourceManager.Instance.getMinesClient());
        }
    }

}
