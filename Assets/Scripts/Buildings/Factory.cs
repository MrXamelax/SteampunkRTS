using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    string currentunit = "";
    public float timer = 0;
    PhotonView pview;
    private void Awake()
    {        
        if (PhotonNetwork.IsMasterClient) ResourceManager.Instance.addBuildingToMaster();
        if (!PhotonNetwork.IsMasterClient) ResourceManager.Instance.addBuildingToClient();
        pview = GetComponent<PhotonView>();
    }

    private void Update()
    {  
       if(pview.IsMine && timer - Time.deltaTime <= 0 && currentunit != "")
        {
            UnitManager.Instance.SpawnUnit(pview.Owner.IsMasterClient, currentunit);
            currentunit = "";
            timer = 0;
        }
        else if ( timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }
    public void openMenu()
    {
        UIManager.Instance.openFactoryMenu(this.gameObject);
    }

    public void spawnUnit(string name)
    {
        if (ResourceManager.Instance.buyUnit(name , pview.Owner.IsMasterClient ? 'm' : 'c'))
        {
            Debug.Log("Unit " + name + " buyed");
            currentunit = name;
            timer = 7;
        }
    }

    public (string, string) getCooldown()
    {
        string result = (int) timer != 0 ? ((int)timer).ToString() : "";
        return (currentunit, result);
    }
}
