using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class BreedForge : MonoBehaviour{

    string currentunit = "";
    public float timer = 0;
    PhotonView pview;

    private void Awake() {
        if (PhotonNetwork.IsMasterClient) { ResourceManager.Instance.addBuildingToMaster(); ResourceManager.Instance.addBreedForgeToMaster(); }
        if (!PhotonNetwork.IsMasterClient) { ResourceManager.Instance.addBuildingToClient(); ResourceManager.Instance.addBreedForgeToClient(); }
        pview = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (pview.IsMine && timer - Time.deltaTime <= 0 && currentunit != "")
        {
            UnitManager.Instance.SpawnUnit(pview.Owner.IsMasterClient, currentunit);
            currentunit = "";
            timer = 0;
        }
        else if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }
    public void spawnUnit(string name)
    {
        bool isMaster = GetComponent<PhotonView>().Owner.IsMasterClient;

        if (ResourceManager.Instance.buyUnit(name, isMaster ? 'm' : 'c'))
        {
            Debug.Log("Unit " + name + " buyed");
            currentunit = name;
            timer = 7;
        }
    }

    public void openMenu() {
        UIManager.Instance.openBreedForgeMenu(this.gameObject);
    }

    public (string, string) getCooldown()
    {
        string result = (int)timer != 0 ? ((int)timer).ToString() : "";
        return (currentunit, result);
    }
}
