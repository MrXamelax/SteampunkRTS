using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<GameObject> controlledUnits;
    public static GameController Instance;

    [SerializeField] protected Camera playerCam;
    public RectTransform selectionBox;
    Vector2 boxStartPos;

    private Utils utils;

    void Start()
    {
        utils = GetComponent<Utils>();
        Instance = this;
        if (!Debug.isDebugBuild)
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
            if (utils.IspointerOverUiObject())
                return;
            boxStartPos = Input.mousePosition;

            // TODO add drag and drop rectangle mark option
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity);
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
                if (!Input.GetKey(KeyCode.LeftShift) && controlledUnits.Count != 0)
                {
                    controlledUnits.ForEach((unit) => unit.BroadcastMessage("DeselectMe"));
                    controlledUnits.Clear();
                }
                // Select new unit
                hit.transform.gameObject.BroadcastMessage("SelectMe");
                // Add new unit to the controlled units
                controlledUnits.Add(hit.transform.gameObject);
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (!utils.IspointerOverUiObject())
                UpdateSelection(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (Vector2.Distance(boxStartPos, Input.mousePosition) > 1.75f) 
                ReleaseSelectionBox();
        }
        if (Input.GetMouseButtonDown(1))
        {


            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, LayerMask.GetMask("Ground"));

            //TODO IF HIT IS ON MINIMAP -> Send units

            if (utils.IspointerOverUiObject())
                return;
            if (hit && controlledUnits.Count != 0)
            {
                controlledUnits.ForEach((unit) => unit.BroadcastMessage("receiveCommand", hit));
            }
        }
    }

    void ReleaseSelectionBox()
    {
        controlledUnits.ForEach((unit) => unit.BroadcastMessage("DeselectMe"));
        controlledUnits.Clear();
        selectionBox.gameObject.SetActive(false);

        Vector2 min = playerCam.ScreenToWorldPoint(selectionBox.anchoredPosition - selectionBox.sizeDelta / 2);

        Vector2 max = playerCam.ScreenToWorldPoint(selectionBox.anchoredPosition + selectionBox.sizeDelta / 2);

        List<GameObject> units = GameManager.Instance.units;
        foreach (GameObject unit in units)
        {
            if (unit.GetPhotonView().IsMine)
            {
                Vector3 screenPos = unit.transform.position;
                if (screenPos.x > min.x && screenPos.x < max.x
                   && screenPos.y > min.y && screenPos.y < max.y)
                {
                    controlledUnits.Add(unit);
                    unit.BroadcastMessage("SelectMe");
                }
            }
        }
    }

    void UpdateSelection(Vector2 pos)
    {
        if (!selectionBox.gameObject.activeInHierarchy)
            selectionBox.gameObject.SetActive(true);

        float width = pos.x - boxStartPos.x;
        float height = pos.y - boxStartPos.y;

        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBox.anchoredPosition = boxStartPos + new Vector2(width / 2, height / 2);
    }
}

