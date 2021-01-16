using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;
using System.Linq;

public class UIManager : MonoBehaviour
{
    [SerializeField] protected List<GameObject> menus;
    [SerializeField] protected Text coalAmount;
    private Text roomname;

    void Start()
    {
        menus.ForEach((m) => m.SetActive(false));
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient) coalAmount.text = ResourceManager.Instance.getCoal('m').ToString();
        else coalAmount.text = ResourceManager.Instance.getCoal('c').ToString();
    }

    public void openMenu(GameObject menu)
    {
        menus.ForEach((m) =>
        {
            if (m.name != menu.name || menu.activeSelf)
                m.SetActive(false);
            else
                m.SetActive(true);
        }
        );
    }
}
