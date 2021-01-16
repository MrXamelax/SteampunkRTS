using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHover: MonoBehaviour {
    private int buildingsBehind = 0;

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Building")) {
            GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.red);
            buildingsBehind += 1;
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        Debug.Log("Tschüssi");
        if (col.gameObject.CompareTag("Building")) {
            Debug.Log("Nix!");
            buildingsBehind -= 1;
            if(buildingsBehind == 0)
                GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.white);
            if (buildingsBehind < 0)
            {
                Debug.LogError("int buildingsbehind is below 0, What happened?\n set buildingsbehind 0");
                buildingsBehind = 0;
            }
        }
    }

    public bool getPlaceble() {
        return buildingsBehind == 0;
    }

}
