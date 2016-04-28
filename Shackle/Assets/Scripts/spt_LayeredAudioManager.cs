/*
spt_LayeredAudioManager

Author(s): The Dara Diba

Revision 1

This is dynamic/layered audio manager that kicks booty
*/

using UnityEngine;
using System.Collections;

public class spt_LayeredAudioManager : MonoBehaviour {


    private float getSchwifty;

    public AudioClip fifthLayerAOne;
    public AudioClip fifthLayerATwo;
    private Fabric.Component fifthLayerComponents;
    private float roll;
    private bool rolled;
    private bool once;
    private GameObject fuck;

    // Use this for initialization
    void Start () {
        Fabric.EventManager.Instance.PostEvent("BackgroundMusic");
        once = false;
        getSchwifty = 0;

        fifthLayerComponents = Fabric.FabricManager.Instance.GetComponentByName("Audio Component");

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

        if (getSchwifty >= 80)
        {
            if (once == false)
            {
                RolltheDieBitch("FifthLayer1", "FifthLayer2");
                once = true;
            }
        }
        else
        {
            once = false;
            Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "FifthLayer1", 0f, null);
            Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "FifthLayer2", 0f, null);
        }
        if(getSchwifty >= 100)
        {

        }
        else
        {

        }
      
    }

    void RolltheDieBitch(string layerNameOne, string layerNameTwo)
    {
        roll = Random.Range(0f, 10f);
        rolled = false;

        if (roll > 5f && rolled == false)
        {
            //Debug.Log("SUCK A FUCK");
            Fabric.EventManager.Instance.SetParameter("BackgroundMusic", layerNameOne, 1.0f, null);
            rolled = true;
        }
        else
        {
            //Debug.Log("DON'T SUCK A FUCK");
            Fabric.EventManager.Instance.SetParameter("BackgroundMusic", layerNameTwo, 1.0f, null);
        }
    }
}
