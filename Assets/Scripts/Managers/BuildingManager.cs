using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour {

    private bool isFactory = false;
    private bool isForge = false;

    [SerializeField] GameObject factoryGo;
    [SerializeField] GameObject forgeGo;
    [SerializeField] Camera cam;

    private void Update() {

        if (isFactory) {
            factoryGo.transform.position = cam.ScreenToWorldPoint(Input.mousePosition);
            factoryGo.transform.position = new Vector3(factoryGo.transform.position.x, factoryGo.transform.position.y, 0);
            if (Input.GetMouseButtonDown(0)) {
                Debug.Log("Fabrik hinstellen!");
                factoryGo.SetActive(false);
                isFactory = false;
                PhotonNetwork.Instantiate("Buildings/Factory", factoryGo.transform.position, Quaternion.identity);
            }
        }

        if (isForge) {
            forgeGo.transform.position = cam.ScreenToWorldPoint(Input.mousePosition);
            forgeGo.transform.position = new Vector3(forgeGo.transform.position.x, forgeGo.transform.position.y, 0);
            if (Input.GetMouseButtonDown(0)) {
                Debug.Log("Brutschmiede hinstellen!");
                forgeGo.SetActive(false);
                isForge = false;
                //PhotonNetwork.Instantiate("Buildings/BreedForge", forgeGo.transform.position, Quaternion.identity);
            }
        }
    }

    public void buildFactory() {
        Debug.LogWarning("baue Fabrik bitte!");
        isFactory = true;
        factoryGo.SetActive(true);
    }

    public void buildForge() {
        Debug.LogWarning("baue Brutschmiede bitte!");
        isForge = true;
        forgeGo.SetActive(true);
    }
}
