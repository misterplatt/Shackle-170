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
    public Sprite interaction;
    public Sprite movable;

    private Image currentImage;
    private Text currentText;

	// Use this for initialization
	void Start () {
        currentImage = GetComponentInChildren<Image>();
        currentText = GetComponentInChildren<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.H)) {
            clearToolTip();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            setToolTip(movable, "To Move Some Objects");
        }
    }

    //Empties the tool tip display
    public void clearToolTip() {
        currentImage.sprite = empty;
        currentText.text = "";
    }

    public void setToolTip(Sprite newSprite, string newText) {
        currentImage.sprite = newSprite;
        currentText.text = newText;
    }
}
