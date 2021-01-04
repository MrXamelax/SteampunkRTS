using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class MiniMap : MonoBehaviour
{
	[SerializeField] protected Camera cam; //for raycast instead of using Camera.main
	[SerializeField] protected GameObject camToMove; // the gameobject the camera is attached to
	[SerializeField] protected LayerMask mask; //set to your ground layer you want the raycast to hit
	[SerializeField] protected float lineWidth = 3f;

	float camWidth;
	float camHeight;
	Vector2 minMapPosBottomLeft;
	Vector2 minMapPosUpperRight;
	Rect camRect;
	RectTransform thisRect;

    private void Start()
    {
		thisRect = GetComponent<RectTransform>();
		camRect = camToMove.GetComponent<Camera>().rect;

		// position of MiniMap in UI
		minMapPosBottomLeft = thisRect.anchoredPosition - thisRect.sizeDelta / 2;
		minMapPosUpperRight = thisRect.anchoredPosition + thisRect.sizeDelta / 2;
	
	}

    void Update()
	{
		//playerCamOnMiniCamPosition = cam.WorldToScreenPoint(camToMove.transform.position);
		if (IspointerOverUiObject())
		{
			if (Input.GetMouseButtonDown(0))
			{
				Vector2 input = Input.mousePosition * cam.pixelWidth / (minMapPosUpperRight.x - minMapPosBottomLeft.x);
				// Clicked on MiniMap UI ?

				camWidth = camRect.width * camToMove.GetComponent<Camera>().orthographicSize * Screen.width / Screen.height;
				camHeight = camRect.height * camToMove.GetComponent<Camera>().orthographicSize;

				input.x = clampCoord(input.x, cam.pixelWidth, 0, camWidth*2);
				input.y = clampCoord(input.y, cam.pixelHeight, 0, camHeight*2);

				camToMove.transform.position = cam.ScreenToWorldPoint(new Vector3(input.x, input.y, cam.transform.position.z));
			}
			if (Input.GetMouseButtonDown(1))
			{
				// todo Commando to Units
			}
		}

	}

    private void OnEnable()
    {
		RenderPipelineManager.endCameraRendering += RenderPipelineManager_endCameraRendering;
	}

    private float clampCoord(float initialValue, float maxValue,float minValue, float size)
    {
		return initialValue < minValue + size ? minValue + size
			: initialValue > maxValue - size ? maxValue - size
			: initialValue;
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

	private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext context, Camera camera)
	{
		Debug.Log("rendern");
		OnPostRender();
	}

	private void OnPostRender()
    {
        Vector2 camPos = cam.transform.position;

        float minX = camPos.y - cam.orthographicSize * Screen.width / Screen.height;
        float minY = camPos.y - cam.orthographicSize;

        float maxX = camPos.y + cam.orthographicSize * Screen.width / Screen.height;
        float maxY = camPos.y + cam.orthographicSize;


		Debug.Log(minX + " | " + minY + " | " + maxX + " | " + maxY);
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
