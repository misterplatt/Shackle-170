/*
spt_LayeredAudioManager

Author(s): The Dara Diba

Revision 1

This is dynamic/layered audio manager that kicks booty
*/

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class spt_LayeredAudioManager : MonoBehaviour {


    private float getSchwifty;

    public AudioClip fifthLayerAOne;
    public AudioClip fifthLayerATwo;
    public AudioClip sixthLayerAOne;
    public AudioClip sixthLayerATwo;
    private float roll;
    private bool rolled;
    private bool fifthLayerOnce;
    private bool sixthLayerOnce;
    private spt_monsterMotivation monster;
    private int totalLayers;
    private float layerTrigger;

    // Use this for initialization
    void Start () {  
        Fabric.EventManager.Instance.PostEvent("BackgroundMusic");
        fifthLayerOnce = false;
        sixthLayerOnce = false;
        getSchwifty = 0;
        totalLayers = 6;
    }

    // Update is called once per frame
    void Update(){
        monster = GameObject.FindObjectOfType<spt_monsterMotivation>();
        Debug.Log("Shit on the floor:" + getSchwifty);
        if (Input.GetMouseButtonDown(0)) getSchwifty += 10;
        if (Input.GetMouseButtonDown(1)) getSchwifty -= 10;
        if (monster != null)
        {
            // if (getSchwifty >= 20)
            if (monster.angerLevel >= LayerTriggerValue(2)) Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "SecondLayer", 1.0f, null);
            else Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "SecondLayer", 0f, null);

            //if (getSchwifty >= 40)
            if (monster.angerLevel >= LayerTriggerValue(3)) Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "ThirdLayer", 1.0f, null);
            else Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "ThirdLayer", 0f, null);

            //if (getSchwifty >= 60)
            if (monster.angerLevel >= LayerTriggerValue(4)) Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "FourthLayer", 1.0f, null);
            else Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "FourthLayer", 0f, null);

            //if (getSchwifty >= 80)
            if (monster.angerLevel >= LayerTriggerValue(5))
            {
                if (fifthLayerOnce == false)
                {
                    RolltheDieBitch("FifthLayer1", "FifthLayer2");
                    fifthLayerOnce = true;
                }
            }
            else
            {
                fifthLayerOnce = false;
                Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "FifthLayer1", 0f, null);
                Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "FifthLayer2", 0f, null);
            }

            //if (getSchwifty >= 100)
            if (monster.angerLevel >= LayerTriggerValue(6))
            {
                if (sixthLayerOnce == false)
                {
                    RolltheDieBitch("SixthLayer1", "SixthLayer2");
                    sixthLayerOnce = true;
                }
                else
                {
                    sixthLayerOnce = false;
                    Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "SixthLayer1", 0f, null);
                    Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "SixthLayer2", 0f, null);
                }
            }
        }
    }
      
    float LayerTriggerValue(float layerNumber)
    {
        return layerTrigger = (((monster.lowerThreshold) / 2) + ((monster.lowerThreshold / 2) / (totalLayers - 2))*(layerNumber -1));
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
