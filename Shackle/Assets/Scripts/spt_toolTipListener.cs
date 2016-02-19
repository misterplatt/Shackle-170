using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
spt_toolTipListener

Author(s): Hayden Platt

Revision 1

Listens for event cues to show and hide tooltips.
*/

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

	// Use this for initialization
	void Start () {
        currentImage = GetComponentInChildren<Image>();
        currentText = GetComponentInChildren<Text>();
        StartCoroutine(setToolTip(RS, "Press to Toggle Flashlight", 3f));
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.H)) clearToolTip();

        if (Input.GetKeyDown(KeyCode.J)) StartCoroutine(setToolTip(LS_A, "To Move Some Objects", 1f));
    }

    //Function which sets the toolTip image and text after delayTime seconds, then clears after 3 seconds
    IEnumerator setToolTip(Sprite newSprite, string newText, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        // Now do your thing here
        currentImage.sprite = newSprite;
        currentText.text = newText;
        Invoke("clearToolTip", 3f);
    }

    //Empties the tool tip display
    public void clearToolTip()
    {
        currentImage.sprite = empty;
        currentText.text = "";
    }

}
