using UnityEngine;
using System.Collections;

public class spt_warningListener : MonoBehaviour {

    spt_monsterMotivation monster;
    Light flashlight;

    public float minFlickerSpeed = 0.2f;
    public float maxFlickerSpeed = 1f;

    private float minNormalTime = 6f;
    private float maxNormalTime = 20f;

    private float timer = 0;
    private bool flickering = false;

    private bool flickerTriggered = false;

    private int puzzleCompletionMonsterIndex = -1;

    // Use this for initialization
    void Start()
    {
        timer = Random.Range(minNormalTime, maxNormalTime);
    }

    // Update is called once per frame
    void Update()
    {
        flashlight = gameObject.transform.parent.GetChild(4).GetComponent<Light>();

        timer -= Time.deltaTime; //Decrement timer every frame

        //If the timer runs out and flashlight hasn't been flickering, start flickering
        if (timer < 0 && !flickering && flickerTriggered)
        {
            timer = Random.Range(.5f, 1.5f);
            flickering = true;
        }

        if (flickering) StartCoroutine("warningFlicker");

        if (timer < 0 && flickering)
        {
            StopCoroutine("warningFlicker");
            flashlight.enabled = true;
            timer = Random.Range(minNormalTime, maxNormalTime);
            flickering = false;
            flickerTriggered = false;
        }
    }

    //Coroutine which turns the flashlight red and flickers it
    IEnumerator warningFlicker()
    {
        flashlight.color = new Color32(215, 212, 104, 255);
        flashlight.enabled = false;
        yield return new WaitForSeconds(Random.Range(minFlickerSpeed, maxFlickerSpeed));
        flashlight.enabled = true;
        warningFlicker();
    }

    private void stopFlicker()
    {
        StopCoroutine("warningFlicker");
        flashlight.color = new Color32(215, 212, 104, 255);
        flashlight.enabled = true;
    }

    public void triggerFlicker()
    {
        flickerTriggered = true;
        Invoke("warningFlicker", 1);
    }
}
