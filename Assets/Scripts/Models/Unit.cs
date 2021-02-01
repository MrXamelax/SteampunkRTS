using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, IPunInstantiateMagicCallback {

    private string unit;

    public void OnPhotonInstantiate(PhotonMessageInfo info) {
        GameManager.Instance.units.Add(this.gameObject);
    }

    private void OnDestroy() {
        GameManager.Instance.units.Remove(this.gameObject);
    }
}
