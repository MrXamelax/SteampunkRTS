﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] float cameraMoveSpeed = 0.5f;
    [SerializeField] float scrollingSpeed = 2f;
    public float mouseDetect = 10f;
    // Start is called before the first frame update

    //TODO get this from a level class
    public Vector2 levelPositionUL = new Vector2(); // Level position upper left
    public Vector2 levelPositionBR = new Vector2(); // Level position bottom right

    public List<GameObject> controlledUnits;


    void Start()
    {
        if(!Debug.isDebugBuild)
            Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        MoveCamera();
        ControlUnits();
    }

    #region UnitControl

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

    #endregion

    #region CameraMovement

    void MoveCamera()
    {
        // init new coords for moving the camera
        float moveX = cam.transform.position.x;
        float moveY = cam.transform.position.y;

        //Get mouse position
        float xPos = Input.mousePosition.x;
        float yPos = Input.mousePosition.y;

        // Input.mousePresent

        //Mousewheel input

        float mouseScroll = Input.GetAxis("Mouse ScrollWheel") * scrollingSpeed;

        cam.orthographicSize -= mouseScroll;

        if (!IsNotOnBottom())
        {
            if (!IsNotOnTop())
                cam.orthographicSize += mouseScroll;
            moveY -= mouseScroll < 0 ? mouseScroll : 0;
        }

        if (!IsNotOnTop())
        {
            if (!IsNotOnBottom())
                cam.orthographicSize += mouseScroll;
            moveY += mouseScroll < 0 ? mouseScroll : 0;
        }

        // move Input, mouse and keyboard

        if (Input.GetAxisRaw("Horizontal") < 0f || xPos >= 0 && xPos <= mouseDetect) //A
        {
            moveX -= cameraMoveSpeed;

            if ((levelPositionUL.x > cam.transform.position.x - cam.orthographicSize))
            {
                //TODO Load level from other side
            }
        }
        else if (Input.GetAxisRaw("Horizontal") > 0f || xPos <= Screen.width && xPos >= Screen.width - mouseDetect) //D
        {
            moveX += cameraMoveSpeed;

            if ((levelPositionBR.x < cam.transform.position.x + cam.orthographicSize))
            {
                //TODO Load level from other side
            }
        }
        //W
        if (IsNotOnTop() && ((Input.GetAxisRaw("Vertical") > 0f || yPos <= Screen.height && yPos >= Screen.height - mouseDetect)))
        {
            //Debug.Log((levelPositionUL.y - cam.transform.position.y + cam.orthographicSize).ToString());
            if ((levelPositionUL.y - cam.transform.position.y + cam.orthographicSize) <= cameraMoveSpeed)
            {
                moveY += levelPositionUL.y - cam.transform.position.y + cam.orthographicSize;
                //Debug.Log(levelPositionUL.y - cam.transform.position.y + cam.orthographicSize);
            }
            else
                moveY += cameraMoveSpeed;

        }
        //S 
        else if (IsNotOnBottom() && (Input.GetAxisRaw("Vertical") < 0f || yPos >= 0 && yPos <= mouseDetect))
        {
            moveY -= cameraMoveSpeed;
        }

        //Save new Position in var
        Vector3 newPosition = new Vector3(moveX, moveY, cam.transform.position.z);

        cam.transform.position = newPosition;
    }

    private bool IsNotOnBottom()
    {
        return cam.transform.position.y - cam.orthographicSize > levelPositionBR.y;
    }
    private bool IsNotOnTop()
    {
        return cam.transform.position.y + cam.orthographicSize < levelPositionUL.y;
    }

    # endregion
}
