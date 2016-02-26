using UnityEngine;
using System.Collections;

public class spt_debug_foggle : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        DontDestroyOnLoad(transform.gameObject);
	}

    void OnPreRender()
    {
        if(! RenderSettings.fog)
            RenderSettings.fog = true;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
