using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputmanager : MonoBehaviour
{

    public float cameraMoveSpeed = 0.5f;

    public float mouseDetect = 10f;
    // Start is called before the first frame update

    public Transform target;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        // float moveX = Camera.main.transform.position.x;
        // float moveY = Camera.main.transform.position.z;

        float moveX = 0;
        float moveY = 0;

        float xPos = Input.mousePosition.x;
        float yPos = Input.mousePosition.y;

        if (Input.GetKey(KeyCode.A) || xPos >= 0 && xPos <= mouseDetect)
        {
            moveX = -1;
        }
        else if (Input.GetKey(KeyCode.D) || xPos <= Screen.width && xPos >= Screen.width - mouseDetect)
        {
            moveX = 1;
        }


        if (Camera.main.transform.position.y < 11 && (Input.GetKey(KeyCode.W) || yPos <= Screen.height && yPos >= Screen.height - mouseDetect))
        {
            moveY = 1;
        }
        else if (Camera.main.transform.position.y > -11 && (Input.GetKey(KeyCode.S) || yPos >= 0 && yPos <= mouseDetect))
        {
            moveY = -1;
        }

        Vector3 newPosition = new Vector3(moveX, moveY, 0).normalized;


        //transform.RotateAround(target.position, newPosition, cameraMoveSpeed * Time.deltaTime);//20 * Time.deltaTime * cameraMoveSpeed
        Camera.main.transform.LookAt(target);
        Camera.main.transform.Translate(newPosition * cameraMoveSpeed * Time.deltaTime);
    }
}
