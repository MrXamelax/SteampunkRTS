using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Photon.Pun;

public class LoggingManager : MonoBehaviour {

    public static LoggingManager Instance; // Singleton - To be accessed from everywhere

    private string docName;
    private string player;
    private int rnd = 0;
    char actor;

    void Start() {
        if (PhotonNetwork.IsMasterClient) {
            player = "Master";
            actor = 'm';
        } else {
            player = "Client";
            actor = 'c';
        }
        Directory.CreateDirectory(Application.streamingAssetsPath + "/Event_Log/");
        docName = Application.streamingAssetsPath + "/Event_Log/" + player + ".csv";

        print("Logger starts!");

        if (!File.Exists(docName)) File.WriteAllText(docName, "Timestamp,Event,Player,Coal,Count_coal_mines,Mine1,Spiders1,Mine2,Spiders2,Mine3,Spiders3," +
                                                              "Mine4,Spiders4,Mine5,Spiders5,Mine6,Spiders6,Mine7,Spiders7,Mine8,Spiders8,Mine9,Spiders9," +
                                                              "Count_Spiders,Count_Deers,Count_CoalThrowers,Count_Sheeps,Count_Elephants,Count_MilitaryUnits," +
                                                              "Forges,BreedForges,HP\n");
    }

    private void Update() {
        LogState("Pimmel");
    }

    public void LogState(string whatHappened) {
        File.AppendAllText(docName, Time.time + "," + whatHappened + "," + player + "," + ResourceManager.Instance.getCoal(actor) + "," +
                                                                                          ResourceManager.Instance.getMines(actor) + "\n");
    }

}
