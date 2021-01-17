using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    public void openMenu()
    {
        UIManager.Instance.openFactoryMenu(this.gameObject);
    }
}
