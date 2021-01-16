using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    private PhotonView pview;
    private void Start()
    {
        pview = GetComponent<PhotonView>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && !pview.IsMine) GetComponent<PhotonView>().RPC("takeDMG", RpcTarget.All, 5);
    }

    private void OnDestroy() => GameManager.Instance.GameOver(this.gameObject);
}
