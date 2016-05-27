using UnityEngine;
using System.Collections;

public class WaterPlanes : MonoBehaviour
{
	private Vector3 direction = new Vector3(0.001f,0,0);
	private Vector3 reset = new Vector3(-4,0,0);
	private Transform planeA;
	private Transform planeB;

	void Awake ()
	{
		planeA = transform.Find("PlaneA");
		planeB = transform.Find("PlaneB");
	}

	void Update ()
	{
		planeA.Translate(direction);
		planeB.Translate(direction);

		if(planeA.localPosition.y <= -0.65f)
		{
			planeA.Translate(reset);
		}

		if(planeB.localPosition.y <= -0.65f)
		{
			planeB.Translate(reset);
		}
	}
}
