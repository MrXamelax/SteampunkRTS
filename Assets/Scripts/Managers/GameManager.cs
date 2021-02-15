﻿
using Assets.Models;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

//this class manages the initial setup of the game scene
public class GameManager : MonoBehaviourPunCallbacks
{
    #region Public Fields
    public static GameManager Instance; //Singleton. To be accessed from everywhere

    public Text roomNameLabel;
    public Text numPlayersLabel;
    public Text eventLabel;
    public GameObject waitingForPlayer;

    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;

    [Tooltip("The prefab to use for representing a coal mine")]
    public GameObject coalMinePrefab;

    public bool lobbyReady = false;

    public World world;
    [SerializeField] protected GameObject cameraController;
    [SerializeField] protected Tilemap walls;
    [SerializeField] protected Tilemap ground;
    [SerializeField] protected TileBase wall;
    [SerializeField] protected GameObject spawnPointsObj;
    [SerializeField] protected GameObject headquaterMaster;
    [SerializeField] protected GameObject headquaterClient;
    [SerializeField] protected GameObject FogOfWarCircles;
    [SerializeField] protected GameObject FogOfWarBase;

    public List<GameObject> units = new List<GameObject>();
    private Transform[] spawnPoints;
    private string gameResult;
    private bool initialized;
    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        initialized = false;
        spawnPoints = spawnPointsObj.GetComponentsInChildren<Transform>(false);
        spawnPointsObj.SetActive(false);
    }

    void Start()
    {
        waitingForPlayer.SetActive(true);
        Instance = this;
        world = new World(ground: ground, walls: wall, wall: walls);
        if (roomNameLabel)
            roomNameLabel.text = PhotonNetwork.CurrentRoom.Name;
    }

    private void Update()
    {
        if (!initialized && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            initialized = true;
            SetUp();
        }
    }

    public void SetUp()
    {
        if (PhotonNetwork.IsMasterClient)
        {

            GameObject masterBase = PhotonNetwork.Instantiate("Buildings/" + "Headquatermaster", headquaterMaster.transform.position, Quaternion.identity);
            GameObject newCircles = Instantiate(FogOfWarBase, new Vector3(masterBase.transform.position.x -5 , masterBase.transform.position.y, masterBase.transform.position.z), Quaternion.identity);
            newCircles.transform.parent = masterBase.transform;
            headquaterMaster.SetActive(false);
            //Debug.Log("Now spawning " + (spawnPoints.Length - 1) + " coal mines.");
            for (int i = 1; i < spawnPoints.Length; i++)
            {
                GameObject mine = PhotonNetwork.Instantiate("Buildings/" + this.coalMinePrefab.name, spawnPoints[i].position, Quaternion.identity);
                Debug.Log("Spawn Mine: " + i);
                mine.GetComponent<PhotonView>().RPC("setMineID", RpcTarget.All, Int32.Parse(spawnPoints[i].name));
            }
            UnitManager.Instance.SpawnUnit(true, "Cbyder");
        }
        else
        {
            UnitManager.Instance.SpawnUnit(false, "Cbyder");
            GameObject clientBase = PhotonNetwork.Instantiate("Buildings/" + "HeadquaterClient", headquaterClient.transform.position, Quaternion.identity);
            GameObject newCircles = Instantiate(FogOfWarBase, new Vector3(clientBase.transform.position.x + 5, clientBase.transform.position.y, clientBase.transform.position.z), Quaternion.identity);
            newCircles.transform.parent = clientBase.transform;
            headquaterClient.SetActive(false);
        }
        cameraController.SetActive(true);
        SetReady();
    }
    #endregion

    #region Photon Callbacks

    public override void OnLeftRoom()
    {
        if (gameResult == null)
            Loadlauncher();
    }

    //update optical indicators when player enters the room
    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting

        eventLabel.text = "A Player named " + other.NickName + " joined the room.";
        numPlayersLabel.text = "# of players: " + PhotonNetwork.CurrentRoom.PlayerCount;
    }

    //update optical indicators when player leavs the room
    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

        eventLabel.text = "A Player named " + other.NickName + " left the room.";
        numPlayersLabel.text = "# of players: " + PhotonNetwork.CurrentRoom.PlayerCount;
    }
    #endregion

    #region Public Methods

    // Leave the Room, called by Leave Button, or after match has ended maybe ?
    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public void Loadlauncher() => SceneManager.LoadScene(0);

    public void GameOver(bool isMine)
    {
        gameResult = isMine ? "verloren" : "gewonnen";
        print(gameResult);
        UIManager.Instance.showResult(gameResult);
        Time.timeScale = 0;
        PhotonNetwork.LeaveRoom();
        lobbyReady = false;
    }
    #endregion

    public void SetReady()
    {
        lobbyReady = true;
        waitingForPlayer.SetActive(false);
    }
}