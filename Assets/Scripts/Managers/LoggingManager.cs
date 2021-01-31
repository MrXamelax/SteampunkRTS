using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;
using System;

public class LoggingManager : MonoBehaviour {

    public static LoggingManager Instance; // Singleton - To be accessed from everywhere

    private string docName;
    private string player;
    private int countCbyders = 0;
    private int countDeers = 0;
    private int countSheeps = 0;
    private int countElephants = 0;
    private int countMiners = 0;
    private int countMilitaryUnits = 0;
    private int baseHP = 1000;

    char actor;

    void Start() {
        Instance = this;
        if (PhotonNetwork.IsMasterClient) {
            player = "Master";
            actor = 'm';
        } else {
            player = "Client";
            actor = 'c';
        }
        Directory.CreateDirectory(Application.streamingAssetsPath + "/Event_Log/");
        docName = Application.streamingAssetsPath + "/Event_Log/" + PhotonNetwork.LocalPlayer.NickName + "_" + player + "_" + DateTime.Now.ToString("dd.MM.yyyy_HH.mm.ss") + ".csv";

        print("Logger starts!");

        if (!File.Exists(docName)) File.WriteAllText(docName, "Timestamp,Event,Player,Coal,Count_coal_mines,Mine1,Cbyders1,Mine2,Cbyders2,Mine3,Cbyders3," +
                                                              "Mine4,Cbyders4,Mine5,Cbyders5,Mine6,Cbyders6,Mine7,Cbyders7,Mine8,Cbyders8,Mine9,Cbyders9," +
                                                              "Count_Cbyders,Count_Deers,Count_Miners,Count_Sheeps,Count_Elephants,Count_MilitaryUnits," +
                                                              "Factory,BreedForges,HP\n");
    }

    #region public methods

    public void LogState(string whatHappened) {

        File.AppendAllText(docName, Time.time + "," + whatHappened + "," + player + "," +
                                    ResourceManager.Instance.getCoal(actor) + "," + ResourceManager.Instance.getMines(actor) + "," +
                                    "0" + "," + "0" + "," + //Mine1 Cbyder 1
                                    "0" + "," + "0" + "," + //Mine1 Cbyder 2
                                    "0" + "," + "0" + "," + //Mine1 Cbyder 3
                                    "0" + "," + "0" + "," + //Mine1 Cbyder 4
                                    "0" + "," + "0" + "," + //Mine1 Cbyder 5
                                    "0" + "," + "0" + "," + //Mine1 Cbyder 6
                                    "0" + "," + "0" + "," + //Mine1 Cbyder 7
                                    "0" + "," + "0" + "," + //Mine1 Cbyder 8
                                    "0" + "," + "0" + "," + //Mine1 Cbyder 9
                                    countCbyders + "," + countDeers + "," + countMiners + "," + countSheeps + "," + countElephants + "," + countMilitaryUnits + "," +
                                    ResourceManager.Instance.getFactories(actor) + "," + ResourceManager.Instance.getBreedForges(actor) + "," +
                                    this.baseHP + "\n");
    }

    public void AddUnit(string u) { 
        switch (u) {
            case "Cbyder":
                countCbyders++;
                break;
            case "Deer":
                countDeers++;
                break;
            case "Sheep":
                countSheeps++;
                break;
            case "Elephant":
                countElephants++;
                break;
            case "Miner":
                countMiners++;
                break;
            default:
                Debug.LogError("Passed wrong unit type! Incrementing nothing");
                break;
        }

        if (!u.Equals("Cbyder")) countMilitaryUnits++;
        LogState("Unit spawned");
    }

    public void RemoveUnit(string u) {
        switch (u) {
            case "Cbyder":
                countCbyders--;
                break;
            case "Deer":
                countDeers--;
                break;
            case "Sheep":
                countSheeps--;
                break;
            case "Elephant":
                countElephants--;
                break;
            case "Miner":
                countMiners--;
                break;
            default:
                Debug.LogError("Passed wrong unit type! Decrementing nothing");
                break;
        }

        if (!u.Equals("Cbyder")) countMilitaryUnits--;
        LogState("Unit spawned");
    }

    public void updBaseHp(int currentHP) {
        baseHP = currentHP;
        LogState("Base damaged");
    }

    #endregion

}
