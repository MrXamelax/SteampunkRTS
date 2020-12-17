using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;
using Assets.Models;
using UnityEngine.Tilemaps;

//this class manages the initial setup of the game scene
public class GameManager : MonoBehaviourPunCallbacks {
    #region Public Fields
    public static GameManager Instance; //Singleton. To be accessed from everywhere

    public Text roomNameLabel;
    public Text numPlayersLabel;
    public Text eventLabel;

    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;

    [Tooltip("The prefab to use for representing a coal mine")]
    public GameObject coalMinePrefab;

    public World world;
    [SerializeField] protected GameObject palyercamera;
    [SerializeField] protected Tilemap walls;
    [SerializeField] protected Tilemap ground;
    [SerializeField] protected TileBase wall;
    [SerializeField] GameObject spawnPointsObj;

    private Transform[] spawnPoints;

    #endregion

    #region Unity Callbacks

    private void Awake() {
        spawnPoints = spawnPointsObj.GetComponentsInChildren<Transform>(false);
        spawnPointsObj.SetActive(false);
    }

    void Start() {

        Instance = this;
        world = new World(ground: ground, walls: wall, wall: walls);

        if (PhotonNetwork.IsConnected) {
            initOnConnection();
        }

        //TODO: set waiting for other player screen if first one
        palyercamera.SetActive(true);

    }

    public void initOnConnection() {
        roomNameLabel.text = "Connected to room: " + PhotonNetwork.CurrentRoom.Name;
        if (playerPrefab == null) {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        } else {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1) {

                Debug.Log("Now spawning " + spawnPoints.Length + " coal mines.");
                foreach (Transform spawnPoint in spawnPoints) {
                    PhotonNetwork.Instantiate("Buildings/" + this.coalMinePrefab.name, spawnPoint.position, Quaternion.identity);
                }

                PhotonNetwork.Instantiate("Units/" + this.playerPrefab.name, new Vector3(-5f, 1f, 0f), Quaternion.Euler(0, 0, 0), 0);
            }
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
                PhotonNetwork.Instantiate("Units/" + this.playerPrefab.name, new Vector3(5f, 1f, 0f), Quaternion.Euler(0, 180, 0), 0);
        }
        //PhotonNetwork.Instantiate("Buildings/" + this.coalMinePrefab.name, new Vector3(0f, 0f, 0f), Quaternion.Euler(0, 0, 0), 0);
    }

    #endregion

    #region Photon Callbacks

    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    public override void OnLeftRoom() {
        //Scene 0 = the Launcher Scene
        SceneManager.LoadScene(0);
    }

    //update optical indicators when player enters the room
    public override void OnPlayerEnteredRoom(Player other) {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting

        eventLabel.text = "A Player named " + other.NickName + " joined the room.";
        numPlayersLabel.text = "# of players: " + PhotonNetwork.CurrentRoom.PlayerCount;
    }

    //update optical indicators when player leavs the room
    public override void OnPlayerLeftRoom(Player other) {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

        eventLabel.text = "A Player named " + other.NickName + " left the room.";
        numPlayersLabel.text = "# of players: " + PhotonNetwork.CurrentRoom.PlayerCount;
    }
    #endregion

    #region Public Methods

    // Leave the Room, called by Leave Button, or after match has ended maybe ?
    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
        //this gives us the "OnLeftRoom" callback, whose behaviour we specified above
    }
    #endregion
}