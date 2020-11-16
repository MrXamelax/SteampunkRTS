using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

//Photon Launcher, mainly copy pasted from the official tutorial.
//Note on the logics: We attempt to join a random room before creating our own with a name. 
//The random join 'never' works and can be seen as a connection check. 

public class Launcher : MonoBehaviourPunCallbacks
{
    #region Public Fields

    public string roomName = "Totally Balanced Room";
    public string sceneToLoadAfterConnect = "Playground";

    #endregion

    #region Private Fields

    // This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
    string gameVersion = "0.11";

    // The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    [SerializeField]
    private byte maxPlayersPerRoom = 2;

    [Tooltip("The Ui Panel to let the user enter name, connect and play")]
    [SerializeField]
    private GameObject controlPanel;
    [Tooltip("The UI Label to inform the user that the connection is in progress")]
    [SerializeField]
    private GameObject progressLabel;


    // Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon,
    // we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
    // Typically this is used for the OnConnectedToMaster() callback.
    bool isConnecting;

    #endregion

    #region MonoBehaviour CallBacks
        
    // MonoBehaviour method called on GameObject by Unity during early initialization phase.
    void Awake()
    {
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // MonoBehaviour method called on GameObject by Unity during initialization phase.
    void Start()
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    #endregion

    #region MonoBehaviourPunCallbacks Callbacks

    //called when connection to master server is established
    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            Debug.Log("OnConnectedToMaster() was called by PUN");
            // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
            PhotonNetwork.JoinRandomRoom();
            isConnecting = false;
        }
        else Debug.Log("OnConnectedToMaster fired without users intend.");
    }

    //called when we disconnect for any reason
    public override void OnDisconnected(DisconnectCause cause)
    {
        isConnecting = false;
        Debug.LogWarningFormat("OnDisconnected() was called by PUN with reason {0}", cause);
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    //called when we cant join a random room
    //if this fails, we create a room with <roomName>
    //(this will nearly always fail and basically works as a connection test)
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = maxPlayersPerRoom});
    }

    //when we finally manage to join a room...
    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room.");
        Debug.Log("Joined room  "+ PhotonNetwork.CurrentRoom.Name + ". Currently, there are " + PhotonNetwork.CurrentRoom.PlayerCount + " people here.");

        // #Critical: We only load if we are the first player, else we rely on `PhotonNetwork.AutomaticallySyncScene` to sync our instance scene.
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("We are the first one here. Lets load the game.");

            // #Critical
            // Load the Room Level.
            PhotonNetwork.LoadLevel(sceneToLoadAfterConnect);
            Debug.Log("Loaded the Level "+ sceneToLoadAfterConnect);
        }
    }

    #endregion

    #region Public Methods

    // Start the connection process.
    // - If already connected, we attempt joining a random room
    // - if not yet connected, Connect this application instance to Photon Cloud Network
    public void Connect()
    {
        //update visuals
        progressLabel.SetActive(true);
        controlPanel.SetActive(false);

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            //keep track of the players will to really join a room, because when we come back from the game we will get a callback that we are connected, 
            //so we need to know what to do then
            isConnecting = PhotonNetwork.ConnectUsingSettings();

            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    #endregion
}
