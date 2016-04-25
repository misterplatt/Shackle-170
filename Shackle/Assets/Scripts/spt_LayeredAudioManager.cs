using UnityEngine;
using System.Collections;

public class spt_LayeredAudioManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Fabric.EventManager.Instance.PostEvent("BackgroundMusic");
        //Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "SecondLayer", 0, null);
        //Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "ThirdLayer", 0, null);

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("z")) Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "SecondLayer", 1.0f, null);
        if (Input.GetKeyDown("x")) Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "ThirdLayer", 1.0f, null);
        if (Input.GetKeyDown("c")) Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "FourthLayer", 1.0f, null);
        if (Input.GetKeyDown("v")) Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "FifthLayer", 1.0f, null);



    }
}
