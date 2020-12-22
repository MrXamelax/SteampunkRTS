using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MiniMap : MonoBehaviour
{
	//attach this script to the camera

	[SerializeField] protected Camera cam; //for raycast instead of using Camera.main
	[SerializeField] protected GameObject camToMove; // the gameobject the camera is attached to
	[SerializeField] protected LayerMask mask; //set to your ground layer you want the raycast to hit
	[SerializeField] protected float lineWidth = 3f;
	RaycastHit hit;
	Ray ray;
	Vector3 movePoint;
	float YPos;
	float camWidth;
	float camHeight;
	Vector2 minMapPosBottomLeft;
	Vector2 minMapPosUpperRight;
	float miniMapCamRes;

	float camToMovRes;
	RectTransform thisRect;
    private void Start()
    {
		thisRect = GetComponent<RectTransform>();

		miniMapCamRes = 1.0f * cam.pixelWidth / cam.pixelHeight;
		camToMovRes = 1.0f * Screen.width / Screen.height;

		// position of MiniMap in UI
		minMapPosBottomLeft = thisRect.anchoredPosition - thisRect.sizeDelta / 2;
		minMapPosUpperRight = thisRect.anchoredPosition  + thisRect.sizeDelta / 2;


		// Camera Size of playercamera in minimapcam View

		Vector2 playerCamOnMiniCam = cam.WorldToScreenPoint(camToMove.transform.position);
		Debug.Log(playerCamOnMiniCam);
		camWidth = camToMove.GetComponent<Camera>().orthographicSize * Screen.width / Screen.height * cam.pixelWidth /camToMove.GetComponent<Camera>().pixelWidth;
		camHeight = camToMove.GetComponent<Camera>().orthographicSize * cam.pixelHeight / camToMove.GetComponent<Camera>().pixelHeight;

		Debug.Log("playercam width: " + camWidth + ", height: " + camHeight);
	}

    void Update()
	{
		if (IspointerOverUiObject())
		{
			if (Input.GetMouseButtonDown(0))
			{
				Vector2 input = Input.mousePosition / 2;
				// Clicked on MiniMap UI ?

				Debug.Log(input);
				Debug.Log(cam.pixelWidth + "+ " + cam.pixelHeight);

				if (input.x > minMapPosUpperRight.x || input.y > minMapPosUpperRight.y)
					return;

				input.x = input.x < camWidth ? camWidth
                    : input.x > cam.pixelWidth - camWidth ? cam.pixelWidth - camWidth
					: input.x;

                input.y = input.y < camHeight ? camHeight
					: input.y > cam.pixelHeight - camHeight ? cam.pixelHeight - camHeight
                    : input.y;


                Vector3 newCameraPosition = cam.ScreenToWorldPoint(new Vector3(input.x, input.y, cam.transform.position.z));

				camToMove.transform.position = newCameraPosition;
			}
			if (Input.GetMouseButtonDown(1))
			{
				// todo Commando to Units
			}
		}

	}

	//this function dectects clicks on ui objects
	private bool IspointerOverUiObject()
	{
		PointerEventData EventDataCurrentPosition = new PointerEventData(EventSystem.current);
		EventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		List<RaycastResult> result = new List<RaycastResult>();
		EventSystem.current.RaycastAll(EventDataCurrentPosition, result);
		return result.Count > 0;
	}

    private void OnPostRender()
    {
        Vector2 camPos = cam.transform.position;

        float minX = camPos.y - cam.orthographicSize * Screen.width / Screen.height;
        float minY = camPos.y - cam.orthographicSize;

        float maxX = camPos.y + cam.orthographicSize * Screen.width / Screen.height;
        float maxY = camPos.y + cam.orthographicSize;

        GL.PushMatrix();
        {
            GL.LoadOrtho();

            GL.Begin(GL.QUADS);
            GL.Color(Color.red);
            {

                GL.Vertex(new Vector3(minX, minY + lineWidth, 0));
                GL.Vertex(new Vector3(minX, minY - lineWidth, 0));
                GL.Vertex(new Vector3(maxX, minY - lineWidth, 0));
                GL.Vertex(new Vector3(maxX, minY + lineWidth, 0));


                GL.Vertex(new Vector3(minX + lineWidth, minY, 0));
                GL.Vertex(new Vector3(minX - lineWidth, minY, 0));
                GL.Vertex(new Vector3(minX - lineWidth, maxY, 0));
                GL.Vertex(new Vector3(minX + lineWidth, maxY, 0));



                GL.Vertex(new Vector3(minX, maxY + lineWidth, 0));
                GL.Vertex(new Vector3(minX, maxY - lineWidth, 0));
                GL.Vertex(new Vector3(maxX, maxY - lineWidth, 0));
                GL.Vertex(new Vector3(maxX, maxY + lineWidth, 0));

                GL.Vertex(new Vector3(maxX + lineWidth, minY, 0));
                GL.Vertex(new Vector3(maxX - lineWidth, minY, 0));
                GL.Vertex(new Vector3(maxX - lineWidth, maxY, 0));
                GL.Vertex(new Vector3(maxX + lineWidth, maxY, 0));

            }
            GL.End();
        }
        GL.PopMatrix();
    }
}
