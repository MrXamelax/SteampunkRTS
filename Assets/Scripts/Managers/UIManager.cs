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
    [SerializeField] protected GameObject resultScreen;
    private Text roomname;
    public static UIManager Instance;

    void Start()
    {
        Instance = this;
        menus.ForEach((m) => m.SetActive(false));
        resultScreen.SetActive(false);
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

    public void showResult(String gameResult)
    {
        print("HAllo ich bin eins UI Managram worken");
        menus.ForEach((m) => m.SetActive(false));
        resultScreen.SetActive(true);
    }
}
