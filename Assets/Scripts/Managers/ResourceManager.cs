using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{

    public static ResourceManager Instance; //Singleton. To be accessed from everywhere

    private int buildingsMax = 4; // TODO: later change to units per Player (upgrades)
    private int buildingsCurrMaster = 1;
    private int buildingsCurrClient = 1;
    private byte minesMaster;
    private byte minesClient;
    [SerializeField] Dictionary<string, int> pricelist = new Dictionary<string, int>();

    private int coalMaster = 100;
    private int coalClient = 100;

    // Start is called before the first frame update
    void Start()
    {
        pricelist.Add("Deer", 40);
        pricelist.Add("Miner", 50);
        pricelist.Add("Elephant", 100); 
        pricelist.Add("Sheep", 20); 
        pricelist.Add("Cbyder", 100);
        Instance = this;
    }

    public int getCoal(char actor)
    {
        if (actor == 'm') return this.coalMaster;
        if (actor == 'c') return this.coalClient;
        return 0;
    }

    [PunRPC]
    public void addCoal(int coal, char actor)
    {
        if (actor == 'm') this.coalMaster += coal;
        if (actor == 'c') this.coalClient += coal;
    }

    [PunRPC]
    public bool buyUnit(string unit, char actor)
    {
        int price = pricelist?[unit] ?? 0;
        Debug.Log("Try to buy " + unit + " for " + price+ " coal");
        
        if (actor == 'm')
        {
            if (this.coalMaster - price >= 0)
            {
                this.coalMaster -= price;
                return true;
            }
        }
        else if (actor == 'c')
        {
            if (this.coalClient - price >= 0)
            {
                this.coalClient -= price;
                return true;
            }
        }
        return false;
    }

    #region Getter and Setter

    public void addBuildingToMaster() {
        this.buildingsCurrMaster++;
    }

    public void addBuildingToClient() {
        this.buildingsCurrClient++;
    }

    public int getBuildingsCurrMaster() {
        return this.buildingsCurrMaster;
    }

    public int getBuildingsCurrClient() {
        return this.buildingsCurrClient;
    }

    public int getBuildingsMax() {
        return this.buildingsMax;
    }

    public byte getMinesMaster() {
        return this.minesMaster;
    }

    public byte getMinesClient() {
        return this.minesClient;
    }

    public void updMines(char sign, char actor) {
        if (actor == 'm') {
            if (sign == '+') {
                minesMaster++;
            } else if (sign == '-') {
                minesMaster--;
            }
        } else if (actor == 'c') {
            if (sign == '+') {
                minesClient++;
            } else if (sign == '-') {
                minesClient--;
            }
        }
    }

    #endregion

}
