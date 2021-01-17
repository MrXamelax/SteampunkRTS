using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour {

    private void Awake() {
        if (PhotonNetwork.IsMasterClient) ResourceManager.Instance.addBuildingToMaster();
        if (!PhotonNetwork.IsMasterClient) ResourceManager.Instance.addBuildingToClient();
    }

    public void openMenu() {
        UIManager.Instance.openFactoryMenu(this.gameObject);
    }
}
