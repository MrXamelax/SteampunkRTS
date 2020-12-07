using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

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

    #endregion

    #region Unity Callbacks

    void Start()
    {
        roomNameLabel.text = "Connected to room: " + PhotonNetwork.CurrentRoom.Name;
        
        Instance = this;
        
        if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManager.GetActiveScene().name);
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate

            PhotonNetwork.Instantiate(this.coalMinePrefab.name, new Vector3(0f, 0f, 0f), Quaternion.Euler(0,0,0), 0);

            //TODO: Spawn for both players the main building on the spawn vectors from the level .. or set the membership to its owner

            // only for test
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(-5f, 1f, 0f), Quaternion.Euler(0, 0, 0), 0);
            }
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2) 
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(5f, 1f, 0f), Quaternion.Euler(0,180,0), 0);
        }
    }

    #endregion

    #region Photon Callbacks

    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    public override void OnLeftRoom()
    {
        //Scene 0 = the Launcher Scene
        SceneManager.LoadScene(0);
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

    #region Private Methods

    //Load the real game scene if we are the master 
    //(will mostly happen when we were the first one in the room)
    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
        Debug.LogFormat("PhotonNetwork : Loading Level RollAPhotonScene for {0} players.", PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.LoadLevel("RollAPhotonScene");
    }
    #endregion

    #region Public Methods

    //let us leave the room (public, cause we want to to this whenever we want)
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        //this gives us the "OnLeftRoom" callback, whose behaviour we specified above
    }
    #endregion
}