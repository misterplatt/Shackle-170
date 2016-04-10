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
    private bool movementTipsShown = false;

    // Use this for initialization
    void Start()
    {
        if (SceneManager.GetActiveScene().name != "net_RangerOutpost")
        {
            gameObject.SetActive(false);
            return;
        }
        currentImage = GetComponentInChildren<Image>();
        currentText = GetComponentInChildren<Text>();
        StartCoroutine(setToolTip(bumpers, "To Hold Chair", 10f, spt_playerControls.bumpersPressed));
        inventorySpt = transform.parent.transform.GetComponentInParent<spt_inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        //Testing key
        if (Input.GetKeyDown(KeyCode.H)) clearToolTip();

        //Displays the movement tooltip after showing the grabbing tooltip
        if (spt_playerControls.bumpersPressed() && !movementTipsShown) {
            StartCoroutine(setToolTip(LS_bumpers, "In Unison to Move While Holding Chairs", 4f, spt_playerControls.playerMovementControls));
            movementTipsShown = true;
        }
    }

    //Catch-all "variable type" for input successes
    public delegate bool InputCompletion();

    //Coroutine started after a tooltip is displayed. Once the predicate is met, stops and clears tooltip after x seconds.
    IEnumerator inputListener(InputCompletion predicate)
    {
        while (true)
        {
            if (predicate())
            {
                yield return new WaitForSeconds(.5f);
                clearToolTip();
                yield break;
            }
            else yield return null;
        }
    }

    //Function which sets the toolTip image and text after delayTime seconds, then clears after 3 seconds
    IEnumerator setToolTip(Sprite newSprite, string newText, float delayTime, InputCompletion predicate)
    {
        yield return new WaitForSeconds(delayTime);

        //Display desired controller image and text
        currentImage.sprite = newSprite;
        currentText.text = newText;
        StartCoroutine(inputListener(predicate));
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
