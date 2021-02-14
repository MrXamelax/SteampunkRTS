using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    Renderer myRenderer;
    [SerializeField] protected Color unitColor;
    [SerializeField] protected Color enemyColor;
    PhotonView Pview;

    // Start is called before the first frame update
    void Start()
    {
        myRenderer = GetComponent<Renderer>();

        Pview = PhotonView.Get(this);

        if (Pview.IsMine)
            myRenderer.material.SetColor("_Color", unitColor);
        else
            myRenderer.material.SetColor("_Color", enemyColor);
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
        myRenderer.material.SetColor("_Color", unitColor);
    }


}
