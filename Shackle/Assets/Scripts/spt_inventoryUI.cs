using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using VRStandardAssets.Utils;

public class spt_inventoryUI : MonoBehaviour {

    private GameObject itemSprite;

    //Takes in the gameObject's name and searches for the sprite that corresponds to it and enables the rawImage so the inventory sprite will show
    public void inventorySpriteOn(string itemName)
    {
        itemSprite = GameObject.Find(itemName +"Spr");
        itemSprite.GetComponent<RawImage>().enabled = true;
    }

    //Takes in the gameObject's name and searches for the sprite that corresponds to it and disables the rawImage so the inventory sprite will dissappear
    public void inventorySpriteOff(string itemName)
    {
        itemSprite = GameObject.Find(itemName + "Spr");
        itemSprite.GetComponent<RawImage>().enabled = false;
    }

}

