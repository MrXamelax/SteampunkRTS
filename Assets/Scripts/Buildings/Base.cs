using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    private PhotonView pview;

    bool isMine;
    private void Start()
    {
        pview = GetComponent<PhotonView>();
        isMine = pview.IsMine;

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && !pview.IsMine) GetComponent<PhotonView>().RPC("takeDMG", RpcTarget.All, 5);
    }

    private void OnDestroy() => GameManager.Instance.GameOver(isMine);
}
