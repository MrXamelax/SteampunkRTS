using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    private void Awake() {
        if (PhotonNetwork.IsMasterClient) ResourceManager.Instance.addBuildingToMaster();
        if (!PhotonNetwork.IsMasterClient) ResourceManager.Instance.addBuildingToClient();
    }

    public void spawnUnit(string name)
    {
        bool isMaster = GetComponent<PhotonView>().Owner.IsMasterClient;

        if (ResourceManager.Instance.buyUnit(name, isMaster ? 'm' : 'c'))
        {
            UnitManager.Instance.SpawnUnit(isMaster, name);
        }
    }
}

    public void openMenu() {
        UIManager.Instance.openBreedForgeMenu(this.gameObject);
    }
}
