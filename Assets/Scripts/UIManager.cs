using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    [SerializeField] GameObject buildingMenu;
    [SerializeField] GameObject unitMenu;
    [SerializeField] GameObject breedMenu;
    [SerializeField] GameObject factoryMenu;
    [SerializeField] GameObject mineMenu;

    [SerializeField] Text coalAmount;

    // Start is called before the first frame update
    void Start() {
        buildingMenu.SetActive(false);
        unitMenu.SetActive(false);
        breedMenu.SetActive(false);
        factoryMenu.SetActive(false);
        mineMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        coalAmount.text = "C: " + ResourceManager.Instance.getCoal('m');
    }

    public void openBuildingMenu() {
        if (!buildingMenu.activeSelf) {
            buildingMenu.SetActive(true);
            mineMenu.SetActive(false);
            breedMenu.SetActive(false);
            factoryMenu.SetActive(false);
            unitMenu.SetActive(false);
        } else {
            buildingMenu.SetActive(false);
        }
    }

    public void openUnitMenu() {
        if (!unitMenu.activeSelf) {
            unitMenu.SetActive(true);
            buildingMenu.SetActive(false);
            breedMenu.SetActive(false);
            factoryMenu.SetActive(false);
            mineMenu.SetActive(false);
        } else {
            unitMenu.SetActive(false);
        }
    }

    public void openMineMenu() {
        if (!mineMenu.activeSelf) {
            mineMenu.SetActive(true);
            buildingMenu.SetActive(false);
            breedMenu.SetActive(false);
            factoryMenu.SetActive(false);
            unitMenu.SetActive(false);
        } else {
            mineMenu.SetActive(false);
        }
    }

    public void openBreedMenu() {
        if (!breedMenu.activeSelf) {
            mineMenu.SetActive(false);
            buildingMenu.SetActive(false);
            breedMenu.SetActive(true);
            factoryMenu.SetActive(false);
            unitMenu.SetActive(false);
        } else {
            breedMenu.SetActive(false);
        }
    }

    public void openFactoryMenu() {
        if (!factoryMenu.activeSelf) {
            mineMenu.SetActive(false);
            buildingMenu.SetActive(false);
            breedMenu.SetActive(false);
            factoryMenu.SetActive(true);
            unitMenu.SetActive(false);
        } else {
            factoryMenu.SetActive(false);
        }
    }
}
