/*
spt_toolTipListener

Author(s): Hayden Platt

Revision 1

Listens for event cues to show and hide tooltips.
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class spt_toolTipListener : MonoBehaviour {

    public Sprite empty;
    public Sprite aButton;
    public Sprite LS_A;
    public Sprite triggers;
    public Sprite RS_B;
    public Sprite xButton;
    public Sprite RS;

    private Image currentImage;
    private Text currentText;
    private spt_inventory inventorySpt;
    private bool once = false;

	// Use this for initialization
	void Start () {
        currentImage = GetComponentInChildren<Image>();
        currentText = GetComponentInChildren<Text>();
        StartCoroutine(setToolTip(RS, "Press to Toggle Flashlight", 3f, 3f));
        StartCoroutine(setToolTip(aButton, "To Interact", 7f, 4f));
        inventorySpt = transform.parent.transform.GetComponentInParent<spt_inventory>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.H)) clearToolTip();
        if (Input.GetKeyDown(KeyCode.J)) StartCoroutine(setToolTip(LS_A, "To Move Some Objects", 1f, 2f));
        if (inventorySpt.inventorySize() > 1 && !once) {
            StartCoroutine(setToolTip(triggers, "To Cycle Inventory", 1f, 2f));
            StartCoroutine(setToolTip(aButton, "Hold to Use Items", 4f, 2f));
            StartCoroutine(setToolTip(xButton, "To Pass Held Item", 7f, 2f));
            once = true;
        }
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

    //Wrapper function used to start the setToolTip coroutine from other scripts
    public void remote_setToolTip(string newSpriteName, string newText, float delayTime, float displayTime) {
        StartCoroutine(setToolTip((Sprite)this.GetType().GetField(newSpriteName).GetValue(this), newText, delayTime, displayTime));
    }

    //Empties the tool tip display
    public void clearToolTip()
    {
        currentImage.sprite = empty;
        currentText.text = "";
    }

}
