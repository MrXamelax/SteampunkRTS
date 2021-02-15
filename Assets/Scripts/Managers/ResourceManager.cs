﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{

    public static ResourceManager Instance; //Singleton. To be accessed from everywhere

    [SerializeField] protected float incomeCooldown = 3f;
    [SerializeField] protected int income = 1;

    private int factoriesCurrMaster = 0;
    private int factoriesCurrClient = 0;
    private int breedForgesCurrMaster = 0;
    private int breedForgesCurrClient = 0;
    private int minesMaster = 0;
    private int minesClient = 0;
    [SerializeField] public Dictionary<string, int> pricelist = new Dictionary<string, int>();

    private int coalMaster = 125;
    private int coalClient = 125;

    private bool incomeOnCooldown = false;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        pricelist.Add("Deer", 25);
        pricelist.Add("Miner", 50);
        pricelist.Add("Elephant", 150);
        pricelist.Add("Sheep", 65);
        pricelist.Add("Cbyder", 12);
        pricelist.Add("Factory", 75);
        pricelist.Add("Breedforge", 50);
    }

    private void Update()
    {
        if (GameManager.Instance.lobbyReady && !incomeOnCooldown)
        {
            addCoal(income, PhotonNetwork.IsMasterClient ? 'm' : 'c');
            Debug.Log("Adding coal for your hard work");
            incomeOnCooldown = true;
            StartCoroutine("incomeCooldownReset");
        }
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

    public bool buyUnit(string unit, char actor)
    {
        int price = pricelist?[unit] ?? 0;
        Debug.Log("Try to buy " + unit + " for " + price + " coal");

        if (actor == 'm')
        {
            if (this.coalMaster - price >= 0)
            {
                this.coalMaster -= price;
                if (PhotonNetwork.IsMasterClient) LoggingManager.Instance.LogState("Purchased " + unit);
                return true;
            }
        }
        else if (actor == 'c')
        {
            if (this.coalClient - price >= 0)
            {
                this.coalClient -= price;
                if (!PhotonNetwork.IsMasterClient) LoggingManager.Instance.LogState("Purchased " + unit);
                return true;
            }
        }
        return false;
    }

    IEnumerator incomeCooldownReset()
    {
        yield return new WaitForSeconds(incomeCooldown);
        incomeOnCooldown = false;
    }

    #region Getter and Setter

    public void addFactoryToMaster() => this.factoriesCurrMaster++;

    public void addFactoryToClient() => this.factoriesCurrClient++;

    public int getFactories(char actor) {
        if (actor == 'm') return this.factoriesCurrMaster;
        else if (actor == 'c') return this.factoriesCurrClient;
        else { Debug.LogError("Passed invalid Argument for getting Factories. Returning impossible value"); return -1; }
    }

    public void addBreedForgeToMaster() => this.breedForgesCurrMaster++;

    public void addBreedForgeToClient() => this.breedForgesCurrClient++;

    public int getBreedForges(char actor) {
        if (actor == 'm') return this.breedForgesCurrMaster;
        else if (actor == 'c') return this.breedForgesCurrClient;
        else { Debug.LogError("Passed invalid Argument for getting Breed Forges. Returning impossible value"); return -1; }
    }

    public int getMines(char actor) {
        if (actor == 'm') return this.minesMaster;
        else if (actor == 'c') return this.minesClient;
        else { Debug.LogError("Passed wrong actor char! Returning impossible value -1"); return -1; }
    }

    public void updMines(char sign, char actor) {
        if (actor == 'm') {
            if (sign == '+') {
                minesMaster++;
            } else if (sign == '-') {
                minesMaster--;
            }
        }
        else if (actor == 'c')
        {
            if (sign == '+') minesClient++;
            else if (sign == '-') minesClient--;
        }
    }
    #endregion
}
