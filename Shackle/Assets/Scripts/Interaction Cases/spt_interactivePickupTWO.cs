using UnityEngine;
using VRStandardAssets.Utils;
using System.Collections;

namespace VRStandardAssets.Examples
{
    public class spt_interactivePickupTWO : spt_baseInteractiveObject
    {

        //Handle the Click event
        override protected void clickSuccess()
        {
            Debug.Log("Show click state");
            inventorySpt = GetComponent<VRInteractiveItem>().inventoryScript;
            inventorySpt.pickUp(gameObject);
            //gameObject.SetActive(false); //Desired functionality
            transform.position = Vector3.down * 1000; //PLACEHOLDER: Sends objects to hell to prevent inventory breaking
        }

        //Ignore handle down
        override protected void HandleDown(){}
    }
}
