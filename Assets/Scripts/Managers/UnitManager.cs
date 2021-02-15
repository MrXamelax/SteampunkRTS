using Photon.Pun;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;
    [SerializeField] protected Transform spawnPositionMaster;
    [SerializeField] protected Transform spawnPositionClient;
    [SerializeField] protected GameObject FogOfWarCircles;
    private void Start()
    {
        Instance = this;
    }
    public GameObject SpawnUnit(bool isMaster, string unitName)
    { 
        GameObject unit = PhotonNetwork.Instantiate("Units/" + unitName, isMaster ? spawnPositionMaster.position : spawnPositionClient.position, Quaternion.identity, 0);
        GameObject newCircles = Instantiate(FogOfWarCircles, unit.transform.position, Quaternion.identity);
        newCircles.transform.parent = unit.transform;
        float range = unit.GetComponent<UnitController>().range;
        newCircles.transform.localScale *= Mathf.Clamp(range * 1.3f, 7f, 13f);
        return unit;
    }
}