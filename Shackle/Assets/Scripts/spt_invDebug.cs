using UnityEngine;
using System.Collections;

public class spt_invDebug : MonoBehaviour {

    public GameObject invObj;

    // Script for offline inventory testing. Press I to activate.
    void Update () {
        if (Input.GetKeyDown(KeyCode.I)) invObj.SetActive(true);
	}
}
