using UnityEngine;
using System.Collections;

public class PosterScript : MonoBehaviour
{
	private Material mat;
	private GameObject fire;
	private bool isBurning = false;
	private float currentBurnPercent = 0.0f;

	public float burnSpeed = 0.5f;

	void Start ()
	{
		mat = this.gameObject.transform.GetChild(0).GetComponent<Renderer>().material;
		fire = this.gameObject.transform.GetChild(1).gameObject;
	}

	public void Burn ()
	{
		isBurning = true;
		fire.SetActive(true);
	}

	void Update()
	{
		if(isBurning)
		{
			currentBurnPercent += (burnSpeed/100);
			mat.SetFloat("_SliceAmount", currentBurnPercent);

            if (currentBurnPercent >= 1.0f) {
                isBurning = false;
                Destroy(gameObject);
            }
		}
	}
}
