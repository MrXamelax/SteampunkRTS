using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, IPunInstantiateMagicCallback {

    private string unit;

    public void OnPhotonInstantiate(PhotonMessageInfo info) {
        GameManager.Instance.units.Add(this.gameObject);
    }

    private void OnDestroy() {
        if (gameObject.name.Contains("Cbyder")) unit = "Cbyder";
        if (gameObject.name.Contains("Sheep")) unit = "Sheep";
        if (gameObject.name.Contains("Miner")) unit = "Miner";
        if (gameObject.name.Contains("Elephant")) unit = "Elephant";
        if (gameObject.name.Contains("Deer")) unit = "Deer";

        LoggingManager.Instance.RemoveUnit(unit);
        GameManager.Instance.units.Remove(this.gameObject);
    }
}
