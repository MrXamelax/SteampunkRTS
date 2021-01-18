using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreedForge : MonoBehaviour
{ 
    public void openMenu()
    {
        UIManager.Instance.openBreedForgeMenu(this.gameObject);
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

