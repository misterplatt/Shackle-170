using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class WaterPlanes : MonoBehaviour
{
	private Vector3 direction = new Vector3(0.001f,0,0);
	private Vector3 reset = new Vector3(-4,0,0);
	private Transform planeA;
	private Transform planeB;

    private bool active = false;

	void Awake ()
	{
		planeA = transform.Find("PlaneA");
		planeB = transform.Find("PlaneB");
	}

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "net_FinalScene") active = true;
        if (active) {
            planeA.GetComponent<MeshRenderer>().enabled = true;
            planeB.GetComponent<MeshRenderer>().enabled = true;
            StartCoroutine("ScreenRain");
        }
    }

    //Coroutine to animate screen rain
    IEnumerator ScreenRain() {
        while (true) {
            planeA.Translate(direction);
            planeB.Translate(direction);

            if (planeA.localPosition.y <= -0.65f)
            {
                planeA.Translate(reset);
            }

            if (planeB.localPosition.y <= -0.65f)
            {
                planeB.Translate(reset);
            }
            yield return null;
        }
    }
}
