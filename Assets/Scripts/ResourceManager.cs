﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {

    public static ResourceManager Instance; //Singleton. To be accessed from everywhere

    private int coalMaster = 3000;
    private int coalClient = 1337;

    // Start is called before the first frame update
    void Start() {
        Instance = this;
    }

    // Update is called once per frame
    void Update() {
        
    }

    public int getCoal(char actor) {
        if (actor == 'm') return this.coalMaster;
        if (actor == 'c') return this.coalClient;
        return 0;
    }

    [PunRPC]
    public void addCoal(int coal, char actor) {
        if (actor == 'm') this.coalMaster += coal;
        if (actor == 'c') this.coalClient += coal;
    }

    [PunRPC]
    public void remCoal(int coal, char actor) {
        if (actor == 'm') this.coalMaster -= coal;
        if (actor == 'c') this.coalClient -= coal;
    }
}
