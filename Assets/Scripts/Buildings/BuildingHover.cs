using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHover: MonoBehaviour {

    private bool placeble = true;

    private void OnTriggerEnter2D(Collider2D col) {
        Debug.Log("Hallo");
        if (col.gameObject.CompareTag("Building")) {
            Debug.Log("Gebäude!");
            GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.red);
            placeble = false;
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        Debug.Log("Tschüssi");
        if (col.gameObject.CompareTag("Building")) {
            Debug.Log("Nix!");
            GetComponent<SpriteRenderer>().material.SetColor("_Color", Color.white);
            placeble = true;
        }
    }

    public bool getPlaceble() {
        return placeble;
    }

}
