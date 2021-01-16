using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHover : MonoBehaviour {
    private int buildingsBehind = 0;
    private bool buildable = false;

    [SerializeField] protected GameObject triggerMaster;
    [SerializeField] protected GameObject triggerClient;

    private void Start() {
        GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.red);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Building")) {
            GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.red);
            buildingsBehind += 1;
        }
        if (col.gameObject.name == "buildingAreaMaster" && PhotonNetwork.IsMasterClient) {
            buildable = true;
            GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.green);
        }
        if (col.gameObject.name == "buildingAreaClient" && !PhotonNetwork.IsMasterClient) {
            buildable = true;
            GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.green);
        }
    }

    private void OnTriggerStay2D(Collider2D col) {
        if (col.gameObject.name == "buildingAreaMaster") {
            float master_x_cen = triggerMaster.GetComponent<Collider2D>().bounds.center.x;
            float master_y_cen = triggerMaster.GetComponent<Collider2D>().bounds.center.y;
            float master_x_size = triggerMaster.GetComponent<Collider2D>().bounds.size.x;
            float master_y_size = triggerMaster.GetComponent<Collider2D>().bounds.size.y;
            float dist_master_right = Mathf.Abs((this.gameObject.transform.position.x + this.gameObject.GetComponent<Collider2D>().bounds.size.x / 2) - master_x_cen);
            float dist_master_top = Mathf.Abs((this.gameObject.transform.position.y + this.gameObject.GetComponent<Collider2D>().bounds.size.y / 2) - master_y_cen);
            float dist_master_left = Mathf.Abs((this.gameObject.transform.position.x - this.gameObject.GetComponent<Collider2D>().bounds.size.x / 2) - master_x_cen);
            float dist_master_bottom = Mathf.Abs((this.gameObject.transform.position.y - this.gameObject.GetComponent<Collider2D>().bounds.size.y / 2) - master_y_cen);
            if (dist_master_right >= master_x_size / 2 || dist_master_top >= master_y_size / 2 || dist_master_left >= master_x_size / 2 || dist_master_bottom >= master_y_size / 2) {
                buildable = false;
                GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.red);
            } else {
                buildable = true;
                GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.green);
            }
        }

        if (col.gameObject.name == "buildingAreaClient") {
            float client_x_cen = triggerClient.GetComponent<Collider2D>().bounds.center.x;
            float client_y_cen = triggerClient.GetComponent<Collider2D>().bounds.center.y;
            float client_x_size = triggerClient.GetComponent<Collider2D>().bounds.size.x;
            float client_y_size = triggerClient.GetComponent<Collider2D>().bounds.size.y;
            float dist_client_right = Mathf.Abs((this.gameObject.transform.position.x + this.gameObject.GetComponent<Collider2D>().bounds.size.x / 2) - client_x_cen);
            float dist_client_top = Mathf.Abs((this.gameObject.transform.position.y + this.gameObject.GetComponent<Collider2D>().bounds.size.y / 2) - client_y_cen);
            float dist_client_left = Mathf.Abs((this.gameObject.transform.position.x - this.gameObject.GetComponent<Collider2D>().bounds.size.x / 2) - client_x_cen);
            float dist_client_bottom = Mathf.Abs((this.gameObject.transform.position.y - this.gameObject.GetComponent<Collider2D>().bounds.size.y / 2) - client_y_cen);
            if (dist_client_right >= client_x_size / 2 || dist_client_top >= client_y_size / 2 || dist_client_left >= client_x_size / 2 || dist_client_bottom >= client_y_size / 2) {
                buildable = false;
                GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.red);
            } else {
                buildable = true;
                GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.green);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        Debug.Log("Tschüssi");
        if (col.gameObject.CompareTag("Building")) {
            Debug.Log("Nix!");
            buildingsBehind -= 1;
            if (buildingsBehind == 0 && buildable)
                GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.green);
            if (buildingsBehind < 0) {
                Debug.LogError("int buildingsbehind is below 0, What happened?\n set buildingsbehind 0");
                buildingsBehind = 0;
            }
        }
        if (col.gameObject.name == "buildingAreaMaster" || col.gameObject.name == "buildingAreaClient") {
            buildable = false;
            GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.red);
        }
    }

    public bool getPlaceble() {
        return buildingsBehind == 0 && buildable;
    }

}
