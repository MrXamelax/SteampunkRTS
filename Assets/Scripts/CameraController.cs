using Assets.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] protected Camera cam;
    [SerializeField] float movementSpeed = 2f;
    [SerializeField] float scrollingSpeed = 2.0f;
    [SerializeField] float zoomMin = 8.0f;
    [SerializeField] float zoomMax = 20.0f;
    public float mouseDetect = 10f;
    // Start is called before the first frame update

    World world;
    private float cameraMoveSpeed = 0;
    private Vector3 levelPositionBottomLeft;
    private Vector3 levelPositionUpperRight;

    Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);

    private void Awake()
    {
        world = GameManager.Instance.world;
        (levelPositionBottomLeft, levelPositionUpperRight) = world.getCorners();
        Debug.Log((levelPositionBottomLeft, levelPositionUpperRight));
    }
    // Update is called once per frame
    void Update()
    {
        // init new coords for moving the camera
        float moveX = cam.transform.position.x;
        float moveY = cam.transform.position.y;

        //Get mouse position
        float xPos = Input.mousePosition.x;
        float yPos = Input.mousePosition.y;

        // Do nothing when windows is not focused
        if (!Cursor.visible || !screenRect.Contains(Input.mousePosition))
            return;

        float mouseScroll = Input.GetAxis("Mouse ScrollWheel") * scrollingSpeed;

        if (cam.orthographicSize - mouseScroll > 20.0f  || cam.orthographicSize - mouseScroll < 8.0f)
        {
            mouseScroll = 0.0f;
        }

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

        float verticalInput = Input.GetAxisRaw("Vertical");
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float xSize = cam.orthographicSize * Screen.width / Screen.height;

        cameraMoveSpeed = cam.orthographicSize / (movementSpeed * 10);

        // Left
        if (horizontalInput < 0f || (xPos >= 0 && xPos <= mouseDetect))
        {
            moveX = getOversethendes(levelPositionBottomLeft.x, moveX, -xSize, -cameraMoveSpeed);
        }
        // Right
        else if (horizontalInput > 0f || (xPos <= Screen.width && xPos >= Screen.width - mouseDetect))
        {
            moveX = getOversethendes(levelPositionUpperRight.x, moveX, xSize, cameraMoveSpeed);
        }
        // Up
        if ((verticalInput > 0f || (yPos <= Screen.height && yPos >= Screen.height - mouseDetect)))
        {
            moveY = getOversethendes(levelPositionUpperRight.y, moveY, cam.orthographicSize, cameraMoveSpeed);
        }
        // Down
        else if ((verticalInput < 0f || ( yPos >= 0 && yPos <= mouseDetect)))
        {
            moveY = getOversethendes(levelPositionBottomLeft.y, moveY, -cam.orthographicSize, -cameraMoveSpeed);
        }
        //Save new Position
        Vector3 newPosition = new Vector3(moveX, moveY, cam.transform.position.z);
        cam.transform.position = newPosition;
    }
    private float getOversethendes( float max, float position, float size, float step)
    {
        float oversethendes = max - position - size;
        if (oversethendes == 0 || (step > 0 && oversethendes <= step) || (step < 0 && oversethendes >= step))
            return max - size;
        return position + step;
    }
    private bool IsNotOnBottom()
    {
        return cam.transform.position.y - cam.orthographicSize > levelPositionBottomLeft.y;
    }
    private bool IsNotOnTop()
    {
        return cam.transform.position.y + cam.orthographicSize < levelPositionUpperRight.y;
    }
}
