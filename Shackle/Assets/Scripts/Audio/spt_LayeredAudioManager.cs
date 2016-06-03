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
    public static bool musicPlay;

    // Use this for initialization
    void Start () {  
        Fabric.EventManager.Instance.PostEvent("BackgroundMusic");
        fifthLayerOnce = false;
        sixthLayerOnce = false;
        totalLayers = 6;
        //layerTrigger = 0;
        musicPlay = false;
    }

    // Update is called once per frame
    void Update(){
        if (SceneManager.GetActiveScene().name == "net_SpookyGarage" || SceneManager.GetActiveScene().name == "net_RangerOutpost_crash" || SceneManager.GetActiveScene().name == "net_OpticsLab")
        {
            musicPlay = true;
            DontDestroyOnLoad(gameObject);
        }
        monster = GameObject.FindObjectOfType<spt_monsterMotivation>();
        if (!musicPlay) shutThisBitchDown();
        if (monster != null && musicPlay)
        {
            if (monster.angerLevel >= layerTriggerValue(2)) Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "SecondLayer", 1.0f, null);
            else Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "SecondLayer", 0f, null);

            if (monster.angerLevel >= layerTriggerValue(3)) Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "ThirdLayer", 1.0f, null);
            else Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "ThirdLayer", 0f, null);

            if (monster.angerLevel >= layerTriggerValue(4)) Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "FourthLayer", 1.0f, null);
            else Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "FourthLayer", 0f, null);

            if (monster.angerLevel >= layerTriggerValue(5))
            {
                if (fifthLayerOnce == false)
                {
                    rollTheDieBitch("FifthLayer1", "FifthLayer2");
                    fifthLayerOnce = true;
                }
            }
            else
            {
                fifthLayerOnce = false;
                Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "FifthLayer1", 0f, null);
                Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "FifthLayer2", 0f, null);
            }

            if (monster.angerLevel >= layerTriggerValue(6))
            {
                if (sixthLayerOnce == false)
                {
                    rollTheDieBitch("SixthLayer1", "SixthLayer2");
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
      
    float layerTriggerValue(float layerNumber)
    {
        return (((monster.lowerThreshold) / 2.5f) + ((monster.lowerThreshold / 2f) / (totalLayers - 2f))*(layerNumber -1f));
    }

    void rollTheDieBitch(string layerNameOne, string layerNameTwo)
    {
        roll = Random.Range(0f, 10f);
        rolled = false;

        if (roll > 5f && rolled == false)
        {
            Fabric.EventManager.Instance.SetParameter("BackgroundMusic", layerNameOne, 1.0f, null);
            rolled = true;
        }
        else
        {
            Fabric.EventManager.Instance.SetParameter("BackgroundMusic", layerNameTwo, 1.0f, null);
        }
    }

    public static IEnumerator shutThisBitchDown()
    {
        yield return new WaitForSeconds(5f);
        Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "SecondLayer", 0f, null);
        Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "ThirdLayer", 0f, null);
        Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "FourthLayer", 0f, null);
        Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "FifthLayer1", 0f, null);
        Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "FifthLyaer2", 0f, null);
        Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "SixthLayer1", 0f, null);
        Fabric.EventManager.Instance.SetParameter("BackgroundMusic", "SixthLayer2", 0f, null);
        
    }
}
