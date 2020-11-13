using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputmanager : MonoBehaviour
{
    public float cameraMoveSpeed = 0.5f;

    public float mouseDetect = 10f;
    // Start is called before the first frame update

    //TODO get this from a level class
    public Vector2 levelPositionUL = new Vector2(); // Level position upper left
    public Vector2 levelPositionBR = new Vector2(); // Level position bottom right



    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Debug.unityLogger.Log("Camera Distance: " + -Camera.main.transform.position.z);
    }

    void Update()
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        // init new coords for moving the camera
        float moveX = Camera.main.transform.position.x;
        float moveY = Camera.main.transform.position.y;

        //Get mouse position
        float xPos = Input.mousePosition.x;
        float yPos = Input.mousePosition.y;

        // Input.mousePresent

        //Mousewheel input

        float mouseScroll = Input.GetAxis("Mouse ScrollWheel");

        Camera.main.orthographicSize -= mouseScroll;

        if (!IsNotOnBottom())
        {
            if (!IsNotOnTop())
                Camera.main.orthographicSize += mouseScroll;
            moveY -= mouseScroll < 0 ? mouseScroll : 0;
        }

        if (!IsNotOnTop())
        {
            if (!IsNotOnBottom())
                Camera.main.orthographicSize += mouseScroll;
            moveY += mouseScroll < 0 ? mouseScroll : 0;
        }

        // move Input, mouse and keyboard

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
        //W
        if (IsNotOnTop() && ((Input.GetAxisRaw("Vertical") > 0f || yPos <= Screen.height && yPos >= Screen.height - mouseDetect)))
        {
            Debug.Log((levelPositionUL.y - Camera.main.transform.position.y + Camera.main.orthographicSize).ToString());
            if ((levelPositionUL.y - Camera.main.transform.position.y + Camera.main.orthographicSize) <= cameraMoveSpeed)
            {
                moveY += levelPositionUL.y - Camera.main.transform.position.y + Camera.main.orthographicSize;
                Debug.Log(levelPositionUL.y - Camera.main.transform.position.y + Camera.main.orthographicSize);
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
        Vector3 newPosition = new Vector3(moveX, moveY, Camera.main.transform.position.z);

        Camera.main.transform.position = newPosition;
    }

    private bool IsNotOnBottom()
    {
        return Camera.main.transform.position.y - Camera.main.orthographicSize > levelPositionBR.y;
    }
    private bool IsNotOnTop()
    {
        return Camera.main.transform.position.y + Camera.main.orthographicSize < levelPositionUL.y;
    }
}
