using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;
    [SerializeField] protected Transform spawnPositionMaster;
    [SerializeField] protected Transform spawnPositionClient;
    private void Start()
    {
        Instance = this;
    }
    public void SpawnUnit(bool isMaster, string unitName)
    {
        for(int i = 5; i > 0; i-- )
        PhotonNetwork.Instantiate("Units/" + unitName, isMaster ? spawnPositionMaster.position : spawnPositionClient.position, Quaternion.identity, 0);
    }
}