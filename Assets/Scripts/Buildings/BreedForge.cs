using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreedForge : MonoBehaviour
{ 
    public void openMenu()
    {
        UIManager.Instance.openBreedForgeMenu(this.gameObject);
    }
}
