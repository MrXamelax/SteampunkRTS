using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    Renderer myRenderer;
    [SerializeField] Color colorMaster;
    [SerializeField] Color colorClient;


    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent<Renderer>(out myRenderer);
    }

    //"interface", to be called from external function
    public void SelectMe()
    {
        //Debug.Log("Selected " + this.name);
        if (myRenderer)
        myRenderer.material.SetColor("_Color", Color.green);
    }
    public void DeselectMe()
    {
       //Debug.Log("Deselected " + this.name);
        if (myRenderer)
        myRenderer.material.SetColor("_Color", Color.white);
    }


}
