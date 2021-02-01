using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
	Utils utils;

	private void Start()
	{
		utils = GetComponent<Utils>();
		thisRect = GetComponent<RectTransform>();
		camRect = camToMove.GetComponent<Camera>().rect;

		// position of MiniMap in UI
		minMapPosBottomLeft = thisRect.anchoredPosition - thisRect.sizeDelta / 2;
		minMapPosUpperRight = thisRect.anchoredPosition + thisRect.sizeDelta / 2;

	}

	void Update()
	{
		//playerCamOnMiniCamPosition = cam.WorldToScreenPoint(camToMove.transform.position);
		if (GameManager.Instance.lobbyReady && utils.IspointerOverUiObject())
		{
			if (Input.GetMouseButtonDown(0))
			{
				Vector2 input = Input.mousePosition * cam.pixelWidth / (minMapPosUpperRight.x - minMapPosBottomLeft.x);
				// Clicked on MiniMap UI ?
				if (minMapPosUpperRight.x < Input.mousePosition.x || minMapPosUpperRight.y < Input.mousePosition.y)
					return;

				camWidth = camRect.width * camToMove.GetComponent<Camera>().orthographicSize * Screen.width / Screen.height;
				camHeight = camRect.height * camToMove.GetComponent<Camera>().orthographicSize;

				input.x = clampCoord(input.x, cam.pixelWidth, 0, camWidth * 2);
				input.y = clampCoord(input.y, cam.pixelHeight, 0, camHeight * 2);

				camToMove.transform.position = cam.ScreenToWorldPoint(new Vector3(input.x, input.y, cam.transform.position.z));
			}
			if (Input.GetMouseButtonDown(1))
			{
				Vector2 input = Input.mousePosition * cam.pixelWidth / (minMapPosUpperRight.x - minMapPosBottomLeft.x);
				List<RaycastHit2D> hits = Physics2D.RaycastAll(cam.ScreenToWorldPoint(new Vector3(input.x, input.y, cam.transform.position.z)), Vector2.zero, Mathf.Infinity).ToList();
				RaycastHit2D buildingHit = hits.FirstOrDefault((h) => h.collider.CompareTag("Building"));
				RaycastHit2D unitHit = hits.FirstOrDefault((h) => h.collider.CompareTag("Controllable"));
				RaycastHit2D groundHit = hits.First();

				RaycastHit2D hit = buildingHit ? buildingHit : unitHit ? unitHit : groundHit;

				if (hit && GameController.Instance.controlledUnits.Count != 0)
				{
					GameController.Instance.controlledUnits.ForEach((unit) => unit.BroadcastMessage("receiveCommand", hit));
				}
			}
		}

	}

	private float clampCoord(float initialValue, float maxValue, float minValue, float size)
	{
		return initialValue < minValue + size ? minValue + size
			: initialValue > maxValue - size ? maxValue - size
			: initialValue;
	}
}