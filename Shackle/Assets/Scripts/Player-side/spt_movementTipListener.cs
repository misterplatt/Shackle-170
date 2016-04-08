/*
spt_movementTipListener

Author(s): Hayden Platt

Revision 1

Ranger only. Displays movement tooltip, and clears after successful movement
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class spt_movementTipListener : MonoBehaviour
{

    public Sprite empty;
    public Sprite bumpers;
    public Sprite LS_bumpers;

    private Image currentImage;
    private Text currentText;
    private spt_inventory inventorySpt;
    public GameObject endPoint;
    private bool inventoryTipsShown = false;
    private bool manipulationTipsShown = false;
    private bool movableTipsShown = false;

    // Use this for initialization
    void Start()
    {
        if (SceneManager.GetActiveScene().name != "net_SpookyGarage")
        {
            gameObject.SetActive(false);
            return;
        }
        currentImage = GetComponentInChildren<Image>();
        currentText = GetComponentInChildren<Text>();
        StartCoroutine(setToolTip(bumpers, "To Hold Chair", 3f, 6f));
        StartCoroutine(setToolTip(LS_bumpers, "In Unison to Move While Holding Chairs", 13f, 4f));
        inventorySpt = transform.parent.transform.GetComponentInParent<spt_inventory>();
        endPoint = transform.parent.transform.FindChild("InspectPoint").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //Testing key
        if (Input.GetKeyDown(KeyCode.H)) clearToolTip();

        //if (CORRECT INPUT FOR CURRENT TOOL TIP) clearToolTip();
    }

    //Function which sets the toolTip image and text after delayTime seconds, then clears after 3 seconds
    IEnumerator setToolTip(Sprite newSprite, string newText, float delayTime, float displayTime)
    {
        yield return new WaitForSeconds(delayTime);

        //Display desired controller image and text
        currentImage.sprite = newSprite;
        currentText.text = newText;
        Invoke("clearToolTip", displayTime);
    }

    /*Wrapper function used to start the setToolTip coroutine from other scripts
    public void remote_setToolTip(string newSpriteName, string newText, float delayTime, float displayTime) {
        StartCoroutine(setToolTip((Sprite)this.GetType().GetField(newSpriteName).GetValue(this), newText, delayTime, displayTime));
    }*/

    //Empties the tool tip display
    public void clearToolTip()
    {
        currentImage.sprite = empty;
        currentText.text = "";
    }

}
