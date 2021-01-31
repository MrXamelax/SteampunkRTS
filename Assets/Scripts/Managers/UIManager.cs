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
    [SerializeField] protected List<Text> factoryShopCooldowns;
    [SerializeField] protected Text cbyderCooldown;
    [SerializeField] protected Text coalAmount;
    [SerializeField] protected Text coalPerMinute;
    [SerializeField] protected GameObject resultScreen;
    [SerializeField] protected Text resultText;



    public static UIManager Instance;
    private GameObject currentFactory;
    private GameObject currentBreedForge;
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

        if (menus.First((m) => m.name == "FactoryMenu").gameObject.activeSelf)
        {
            (string, string) cooldown = currentFactory.GetComponent<Factory>().getCooldown();
            Text getQueuingCooldown = factoryShopCooldowns.Find((t) => t.name == cooldown.Item1);
            if (getQueuingCooldown != null)
                getQueuingCooldown.text = cooldown.Item2;
        }

        if (menus.First((m) => m.name == "BreedFactoryMenu").gameObject.activeSelf)
        {
            (string, string) cooldown = currentBreedForge.GetComponent<Factory>().getCooldown();
            cbyderCooldown.text = cooldown.Item2;
        }
    }

    #region public methods

    public void openMenu(GameObject menu)
    {
        print("try to open menu: " + menu.name);
        menus.ForEach((m) =>
        {
            if (m.name != menu.name || menu.activeSelf)
                m.SetActive(false);
            else
                m.SetActive(true);
        }
        );
    }

    public void openFactoryMenu(GameObject factory)
    {
        currentFactory = factory;
        factoryShopCooldowns.ForEach((t) => t.text = "");
        openMenu(menus.First((m) => m.name == "FactoryMenu"));
    }

    public void BuyFactoryUnitButton(string unitName)
    {
        Debug.Log("Pressed UnitButton: " + unitName);
        if (currentFactory.GetComponent<Factory>().timer <= 0)
            currentFactory.GetComponent<Factory>().spawnUnit(unitName);
    }

    public void openBreedForgeMenu(GameObject breed)
    {
        currentBreedForge = breed;
        openMenu(menus.First((m) => m.name == "BreedFactoryMenu"));
    }
    public void BuyBreedForgeUnitButton(string unitName)
    {
       currentBreedForge.GetComponent<BreedForge>().spawnUnit(unitName);
    }


    public void showResult(string gameResult)
    {
        menus.ForEach((m) => m.SetActive(false));
        resultText.text = "Du hast " + gameResult;
        resultScreen.SetActive(true);
    }

    public void updMines(byte amount)
    {
        print("updating coal screen");
        coalPerMinute.text = (amount * 30).ToString() + "/ min.";
    }

    #endregion

}
