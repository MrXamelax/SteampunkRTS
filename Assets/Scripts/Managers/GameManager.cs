
using Assets.Models;
using Photon.Pun;
using Photon.Realtime;
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

    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;

    [Tooltip("The prefab to use for representing a coal mine")]
    public GameObject coalMinePrefab;

    public World world;
    [SerializeField] protected GameObject palyercamera;
    [SerializeField] protected Tilemap walls;
    [SerializeField] protected Tilemap ground;
    [SerializeField] protected TileBase wall;
    [SerializeField] protected GameObject spawnPointsObj;
    [SerializeField] protected GameObject headquaterMaster;
    [SerializeField] protected GameObject headquaterClient;

    public List<GameObject> units = new List<GameObject>();
    private Transform[] spawnPoints;
    private string gameResult;

    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        spawnPoints = spawnPointsObj.GetComponentsInChildren<Transform>(false);

        spawnPointsObj.SetActive(false);
    }

    void Start()
    {

        Instance = this;
        world = new World(ground: ground, walls: wall, wall: walls);

        if (PhotonNetwork.IsConnected)
        {
            initOnConnection();
        }

        //TODO: set waiting for other player screen if first one
        palyercamera.SetActive(true);

    }

    public void initOnConnection()
    {
        roomNameLabel.text = PhotonNetwork.CurrentRoom.Name;
        if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                PhotonNetwork.Instantiate("Buildings/" + "HeadquaterMaster", headquaterMaster.transform.position, Quaternion.identity);
                headquaterMaster.SetActive(false);
                Debug.Log("Now spawning " + (spawnPoints.Length - 1) + " coal mines.");
                for (int i = 1; i < spawnPoints.Length; i++)
                {
                    PhotonNetwork.Instantiate("Buildings/" + this.coalMinePrefab.name, spawnPoints[i].position, Quaternion.identity);
                }
                for (int i = 0; i < 5; i++)
                    SpawnUnit(playerPrefab, new Vector3(-1.5f * i, 1f, 0f));
            }
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                PhotonNetwork.Instantiate("Buildings/" + "HeadquaterClient", headquaterClient.transform.position, Quaternion.identity);
                headquaterClient.SetActive(false);
                SpawnUnit(playerPrefab, new Vector3(5f, 1f, 0f));
            }
        }
        //PhotonNetwork.Instantiate("Buildings/" + this.coalMinePrefab.name, new Vector3(0f, 0f, 0f), Quaternion.Euler(0, 0, 0), 0);
    }
    #endregion

    void SpawnUnit(GameObject prefab, Vector3 spawnPosition)
    {
        PhotonNetwork.Instantiate("Units/" + prefab.name, spawnPosition, Quaternion.identity, 0);
    }

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

    public void GameOver(GameObject destroyed)
    {
        gameResult = destroyed.GetComponent<PhotonView>().IsMine ? "verloren" : "gewonnen";
        print(gameResult);
        UIManager.Instance.showResult(gameResult);
        Time.timeScale = 0;
        PhotonNetwork.LeaveRoom();
    }
    #endregion
}