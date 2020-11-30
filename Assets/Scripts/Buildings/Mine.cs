using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour {
    // Start is called before the first frame update

    [Range(-500, 500)][SerializeField] int hp;
    [SerializeField] int fogOfWar;
    private bool down = true;

    void Start() {

    }

    // Update is called once per frame
    void Update() {

        if (hp == -500) down = false;
        if (hp == 500) down = true;

        if (down) hp--;
        else hp++;
    }

    //private void 

}
