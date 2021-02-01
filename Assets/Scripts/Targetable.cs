using Photon.Pun;
using UnityEngine;

public class Targetable : MonoBehaviour
{
    [SerializeField] protected int maxHp = 20;
    [SerializeField] protected GameObject hpGreenBar;

    private int currentHP;
    private bool myBase = false;
    private string unit = "";

    private void Start()
    {
        currentHP = maxHp;
        if (GetComponent<Base>() && GetComponent<PhotonView>().IsMine) myBase = true;

        if (GetComponent<PhotonView>().IsMine)
        {
            if (unit == "")
                getUnitFromName();
            LoggingManager.Instance.AddUnit(unit);
        }
    }

    [PunRPC]
    public void takeDMG(int _dmg)
    {
        currentHP -= _dmg;

        if (myBase) LoggingManager.Instance.updBaseHp(currentHP);

        hpGreenBar.transform.localScale = new Vector3(1.0f / maxHp * currentHP, hpGreenBar.transform.localScale.y, hpGreenBar.transform.localScale.z);

        if (currentHP <= 0)
            killself();
    }

    void killself() {
        if(GetComponent<Base>())
            GameManager.Instance.GameOver(GetComponent<PhotonView>().IsMine);

        if (GetComponent<PhotonView>().IsMine)
        {
            if (unit == "")
                getUnitFromName();
            LoggingManager.Instance.RemoveUnit(unit);
        }
        Destroy(this.gameObject); 
    }
    
    private void getUnitFromName()
    {
        if (gameObject.name.Contains("Cbyder")) unit = "Cbyder";
        if (gameObject.name.Contains("Sheep")) unit = "Sheep";
        if (gameObject.name.Contains("Miner")) unit = "Miner";
        if (gameObject.name.Contains("Elephant")) unit = "Elephant";
        if (gameObject.name.Contains("Deer")) unit = "Deer";
    }
}
