using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{

    private bool isFactory = false;
    private bool isForge = false;
    public bool placeble = true;

    [SerializeField] protected GameObject factoryGo;
    [SerializeField] protected GameObject forgeGo;
    [SerializeField] protected GameObject buildMasterGo;
    [SerializeField] protected GameObject buildClientGo;
    [SerializeField] protected Camera cam;
    [SerializeField] protected int maxFactories = 7;
    [SerializeField] protected int maxForges = 5;

    private void Update()
    {

        if (isFactory)
        {
            factoryGo.transform.position = cam.ScreenToWorldPoint(Input.mousePosition);
            factoryGo.transform.position = new Vector3(factoryGo.transform.position.x, factoryGo.transform.position.y, 0);
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Fabrik hinstellen!");
                bool isPlaceable = factoryGo.GetComponent<BuildingHover>().getPlaceble();
                factoryGo.SetActive(false);
                isFactory = false;
                char actor = PhotonNetwork.IsMasterClient ? 'm' : 'c';
                bool enoughCoal = ResourceManager.Instance.getCoal(actor) >= ResourceManager.Instance.pricelist["Factory"];
                Debug.Log("Enough Coal ?: " + enoughCoal);
                if (isPlaceable && enoughCoal)
                {
                    Debug.Log("Try to build Factory");
                    if (PhotonNetwork.IsMasterClient && ResourceManager.Instance.getBuildingsCurrMaster() <= maxFactories)
                    {
                        ResourceManager.Instance.buyUnit("Factory", actor);
                        PhotonNetwork.Instantiate("Buildings/Factory", factoryGo.transform.position, Quaternion.identity);
                    }
                    if (!PhotonNetwork.IsMasterClient && ResourceManager.Instance.getBuildingsCurrClient() <= maxFactories)
                    {
                        ResourceManager.Instance.buyUnit("Factory", actor);
                        PhotonNetwork.Instantiate("Buildings/Factory", factoryGo.transform.position, Quaternion.identity);
                    }
                }
                if (PhotonNetwork.IsMasterClient) buildMasterGo.SetActive(false);
                if (!PhotonNetwork.IsMasterClient) buildClientGo.SetActive(false);
            }
        }

        if (isForge)
        {
            forgeGo.transform.position = cam.ScreenToWorldPoint(Input.mousePosition);
            forgeGo.transform.position = new Vector3(forgeGo.transform.position.x, forgeGo.transform.position.y, 0);
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Brutschmiede hinstellen!");
                bool isPlaceable = forgeGo.GetComponent<BuildingHover>().getPlaceble();
                char actor = PhotonNetwork.IsMasterClient ? 'm' : 'c';
                bool enoughCoal = ResourceManager.Instance.getCoal(actor) >= ResourceManager.Instance.pricelist["Breedforge"];
                Debug.Log("Enough Coal ?: " + enoughCoal);
                forgeGo.SetActive(false);
                isForge = false;
                if (isPlaceable && enoughCoal)
                {
                    if (PhotonNetwork.IsMasterClient && ResourceManager.Instance.getBuildingsCurrMaster() <= maxForges)
                    {
                        ResourceManager.Instance.buyUnit("Breedforge", actor);
                        PhotonNetwork.Instantiate("Buildings/BreedForge", forgeGo.transform.position, Quaternion.identity);
                    }
                    if (!PhotonNetwork.IsMasterClient && ResourceManager.Instance.getBuildingsCurrClient() <= maxForges)
                    {
                        ResourceManager.Instance.buyUnit("Breedforge", actor);
                        PhotonNetwork.Instantiate("Buildings/BreedForge", forgeGo.transform.position, Quaternion.identity);
                    }
                }
                if (PhotonNetwork.IsMasterClient) buildMasterGo.SetActive(false);
                if (!PhotonNetwork.IsMasterClient) buildClientGo.SetActive(false);
            }
        }
    }

    public void buildFactory()
    {
        Debug.LogWarning("baue Fabrik bitte!");
        if (PhotonNetwork.IsMasterClient) buildMasterGo.SetActive(true);
        if (!PhotonNetwork.IsMasterClient) buildClientGo.SetActive(true);
        isFactory = true;
        factoryGo.SetActive(true);
    }

    public void buildForge()
    {
        Debug.LogWarning("baue Brutschmiede bitte!");
        if (PhotonNetwork.IsMasterClient) buildMasterGo.SetActive(true);
        if (!PhotonNetwork.IsMasterClient) buildClientGo.SetActive(true);
        isForge = true;
        forgeGo.SetActive(true);
    }

}
