using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputmanager : MonoBehaviour
{
    public float cameraMoveSpeed = 0.5f;

    public float mouseDetect = 10f;
    // Start is called before the first frame update
    private float cameraDistance;



    //TODO get this from a level class
    public Vector2 levelPositionUL = new Vector2(); // Level position upper left
    public Vector2 levelPositionBR = new Vector2(); // Level position bottom right



    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        cameraDistance = -Camera.main.transform.position.z;
        Debug.unityLogger.Log("Camera Distance: " + -Camera.main.transform.position.z);

    }

    void Update()
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        float moveX = Camera.main.transform.position.x;
        float moveY = Camera.main.transform.position.y;

        float xPos = Input.mousePosition.x;
        float yPos = Input.mousePosition.y;
        // Input.mousePresent
        if (Input.GetAxisRaw("Horizontal") < 0f || xPos >= 0 && xPos <= mouseDetect) //A
        {
            moveX -= cameraMoveSpeed;

            if ((levelPositionUL.x > Camera.main.transform.position.x - Camera.main.orthographicSize))
            {
                //TODO Load level from other side
            }
        }
        else if (Input.GetAxisRaw("Horizontal") > 0f || xPos <= Screen.width && xPos >= Screen.width - mouseDetect) //D
        {
            moveX += cameraMoveSpeed;

            if ((levelPositionBR.x < Camera.main.transform.position.x + Camera.main.orthographicSize))
            {
                //TODO Load level from other side
            }
        }

        if (IsNotOnTop() && ((Input.GetAxisRaw("Vertical") > 0f || yPos <= Screen.height && yPos >= Screen.height - mouseDetect)))
        {
            if ((levelPositionUL.y - Camera.main.transform.position.y + Camera.main.orthographicSize) < cameraMoveSpeed)
            {
                moveY += levelPositionUL.y - Camera.main.transform.position.y + Camera.main.orthographicSize;
            }
            else
                moveY += cameraMoveSpeed;
        }
        else if (IsNotOnBottom() && (Input.GetAxisRaw("Vertical") < 0f || yPos >= 0 && yPos <= mouseDetect))
        {
            moveY -= cameraMoveSpeed;
        }

        float mouseScroll = Input.GetAxis("Mouse ScrollWheel");

        if (Camera.main.transform.position.y - Camera.main.orthographicSize <
         levelPositionBR.y)
            Camera.main.orthographicSize -= mouseScroll > 0 ? mouseScroll : 0;
        else if (Camera.main.transform.position.y + Camera.main.orthographicSize < levelPositionUL.y)
            Camera.main.orthographicSize -= mouseScroll < 0 ? mouseScroll : 0;

        Vector3 newPosition = new Vector3(moveX, moveY, -cameraDistance);

        Camera.main.transform.position = newPosition;
    }

    private bool IsNotOnBottom()
    {
        if (Camera.main.transform.position.y - Camera.main.orthographicSize > levelPositionBR.y)
            return true;
        return false;
    }
    private bool IsNotOnTop()
    {
        if (Camera.main.transform.position.y + Camera.main.orthographicSize <= levelPositionUL.y)
            return true;
        return false;
    }
}
