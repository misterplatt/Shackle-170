using UnityEngine;
using System.Collections;

public class spt_LayeredAudioManager : MonoBehaviour {


    private float getSchwifty;
	// Use this for initialization
	void Start () {
        Fabric.EventManager.Instance.PostEvent("BackgroundMusic");
        getSchwifty = 0;
        //Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "SecondLayer", 0, null);
        //Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "ThirdLayer", 0, null);

    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log("Shit on the floor:" + getSchwifty);
        if (Input.GetMouseButtonDown(0)) getSchwifty += 10;
        if (Input.GetMouseButtonDown(1)) getSchwifty -= 10;


        if (getSchwifty >= 20) Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "SecondLayer", 1.0f, null);
        else Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "SecondLayer", 0f, null);

        if (getSchwifty >= 40) Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "ThirdLayer", 1.0f, null);
        else Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "ThirdLayer", 0f, null);

        if (getSchwifty >= 60) Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "FourthLayer", 1.0f, null);
        else Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "FourthLayer", 0f, null);

        if (getSchwifty >= 10) Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "FifthLayer", 1.0f, null);
        else Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "FifthLayer", 0f, null);
        // if (Input.GetKeyDown("z")) Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "SecondLayer", 1.0f, null);
        //if (Input.GetKeyDown("x")) Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "ThirdLayer", 1.0f, null);
        //if (Input.GetKeyDown("c")) Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "FourthLayer", 1.0f, null);
        //if (Input.GetKeyDown("v")) Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "FifthLayer", 1.0f, null);
    }
}
