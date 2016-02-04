using UnityEngine;
using System.Collections;

public class spt_invDebug : MonoBehaviour {

    public GameObject invObj;

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.I)) invObj.SetActive(true);
	}
}
