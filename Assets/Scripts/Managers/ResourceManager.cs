using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {

    public static ResourceManager Instance; //Singleton. To be accessed from everywhere

    private int coalMaster = 3000;
    private int coalClient = 1337;
    private int buildingsMax = 4; // TODO: later change to units per Player (upgrades)
    private int buildingsCurrMaster = 1;
    private int buildingsCurrClient = 1;
    private byte minesMaster;
    private byte minesClient;

    // Start is called before the first frame update
    void Start() {
        Instance = this;
    }

    // Update is called once per frame
    void Update() {
        
    }

    public int getCoal(char actor) {
        if (actor == 'm') return this.coalMaster;
        if (actor == 'c') return this.coalClient;
        return 0;
    }

    [PunRPC]
    public void addCoal(int coal, char actor) {
        if (actor == 'm') this.coalMaster += coal;
        if (actor == 'c') this.coalClient += coal;
    }

    [PunRPC]
    public void remCoal(int coal, char actor) {
        if (actor == 'm') this.coalMaster -= coal;
        if (actor == 'c') this.coalClient -= coal;
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
