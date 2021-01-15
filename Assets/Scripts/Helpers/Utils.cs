using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Utils : MonoBehaviour
{
    public bool IspointerOverUiObject()
    {
        PointerEventData EventDataCurrentPosition = new PointerEventData(EventSystem.current);
        EventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(EventDataCurrentPosition, result);
        result.RemoveAll((r) => r.gameObject.tag == "Ignored");
        return result.Count > 0;
    }
}
