using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class spt_attackListener : MonoBehaviour {

    spt_angerObject angerFlashlight;
    spt_monsterMotivation monster;
    Light flashlight;

    public float minFlickerSpeed = 0.2f;
    public float maxFlickerSpeed = 1f;

    private float minNormalTime = 6f;
    private float maxNormalTime = 20f;

    private float timer = 0;
    private bool flickering = false;
    
    private bool flickerTriggered = false;
    private bool isPosessed = false;
    
    // Use this for initialization
	void Start () {
        timer = Random.Range(minNormalTime, maxNormalTime);
	}
	
	// Update is called once per frame
	void Update () {

        angerFlashlight = gameObject.GetComponent<spt_angerObject>();
        monster = GameObject.Find("MonsterStandin").GetComponent<spt_monsterMotivation>();
        flashlight = gameObject.transform.parent.GetChild(4).GetComponent<Light>();

        timer -= Time.deltaTime; //Decrement timer every frame

        //If the timer runs out and flashlight hasn't been flickering, start flickering
        if (timer < 0 && !flickering && flickerTriggered && !isPosessed)
        {
            timer = Random.Range(.5f, 1.5f);
            flickering = true;
        }

        if (flickering) StartCoroutine("spookyFlicker");

        if (timer < 0 && flickering)
        {
            StopCoroutine("spookyFlicker");
            flashlight.enabled = true;
            timer = Random.Range(minNormalTime, maxNormalTime);
            flickering = false;
        }
        
        if (monster.isAttacking && angerFlashlight.getData().getVisible() && !flickerTriggered)
        {
            if (gameObject.transform.root.GetComponent<NetworkIdentity>().isServer && (monster.whichPlayer == 0))
            {
                monster.attackAfterFlashlightToggle(gameObject.transform);
                flickerTriggered = true;
                Invoke("stopFlicker", 1);
            }
            else if (!gameObject.transform.root.parent.GetComponent<NetworkIdentity>().isServer && (monster.whichPlayer == 1))
            {
                monster.attackAfterFlashlightToggle(gameObject.transform);
                flickerTriggered = true;
                Invoke("stopFlicker", 1);
            }
        }

        if (isPosessed)
        {
            flashlight.enabled = true;
            flashlight.color = Color.red;
        }
	}

    //Coroutine which turns the flashlight red and flickers it
    IEnumerator spookyFlicker()
    {
        flashlight.color = Color.red;
        flashlight.enabled = false;
        yield return new WaitForSeconds(Random.Range(minFlickerSpeed, maxFlickerSpeed));
        flashlight.enabled = true;
        spookyFlicker();
    }

    private void stopFlicker()
    {
        StopCoroutine("spookyFlicker");
        isPosessed = true;
    }
}
