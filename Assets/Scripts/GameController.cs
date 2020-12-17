using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<GameObject> controlledUnits;


    void Start()
    {
        if(!Debug.isDebugBuild)
            Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        ControlUnits();
    }

    void ControlUnits()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // TODO add drag and drop rectangle mark option
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (controlledUnits.Count != 0 && hit.collider?.tag != "Controllable")
            {
                controlledUnits.ForEach((unit) => unit.BroadcastMessage("DeselectMe"));
                controlledUnits.Clear();
                return;
            }
            if (hit.collider?.tag == "Controllable" && //it only makes sense to select controllable objects
                    !controlledUnits.Contains(hit.transform.gameObject) && //we dont want to select the same thing to prevent side effects
                    hit.transform.gameObject.GetComponent<PhotonView>().IsMine) //only select if its OUR unit
            {
                // Select deselect all units und remove them from list, when clicking on one unit with no shift
                if (!Input.GetKey(KeyCode.LeftShift) && controlledUnits.Count != 0) { 
                    controlledUnits.ForEach((unit) => unit.BroadcastMessage("DeselectMe")); 
                    controlledUnits.Clear();
                }
                // Select new unit
                hit.transform.gameObject.BroadcastMessage("SelectMe");
                // Add new unit to the controlled units
                controlledUnits.Add(hit.transform.gameObject);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit && controlledUnits.Count != 0) 
            {
                controlledUnits.ForEach((unit) => unit.BroadcastMessage("receiveCommand", hit));
            }
        }
    }
}
