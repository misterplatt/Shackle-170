/*
spt_toolTipListener

Author(s): Hayden Platt

Revision 1

Listens for event cues to show and hide tooltips.
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class spt_toolTipListener : MonoBehaviour {

    private Animator animator;
    private Text currentText;

    private spt_inventory inventorySpt;
    public GameObject endPoint;
    private bool interactionTipsShown = false;
    private bool inventoryTipsShown = false;
    private bool itemTipsShown = false;
    private bool manipulationTipsShown = false;
    private bool movableTipsShown = false;
    private bool controlsTipsShown = false;

    private int toolTipsDisplayed = 0;

    // Use this for initialization
    void Start () {
		if (SceneManager.GetActiveScene().name != "net_SpookyGarage") {
            this.enabled = false;
			return;
		}
        animator = transform.FindChild("TooltipImage").GetComponent<Animator>();
        currentText = GetComponentInChildren<Text>();
        StartCoroutine(setToolTip("controls_flashlight", "Press to Toggle Flashlight", 3f, spt_playerControls.rightThumbstickButtonPressed));
        //StartCoroutine(setToolTip(aButton, "To Interact", 13f, spt_playerControls.aButtonPressed));
        inventorySpt = transform.parent.transform.GetComponentInParent<spt_inventory>();
        endPoint = transform.parent.transform.FindChild("InspectPoint").gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        //Testing key
        if (Input.GetKeyDown(KeyCode.H)) clearToolTip();

        if (spt_playerControls.rightThumbstickButtonPressed() && !interactionTipsShown) {
            StartCoroutine(setToolTip("controls_A", "To Interact", 5f, spt_playerControls.aButtonPressed));
            interactionTipsShown = true;
        } 

        //Show inventory cycling tooltip once an item is obtained
        if (inventorySpt.inventorySize() > 1 && !inventoryTipsShown) {
            StartCoroutine(setToolTip("controls_cycle", "To Cycle Inventory", 1f, spt_playerControls.triggerPressed));
            inventoryTipsShown = true;
        }

        //Show passing and using tips once cycling has been done
        if (inventoryTipsShown && spt_playerControls.triggerPressed() && !itemTipsShown) {
            StartCoroutine(setToolTip("controls_A", "Hold to Use Items", 4f, spt_playerControls.aButtonPressed));
            StartCoroutine(setToolTip("controls_X", "To Pass Held Item", 8f, spt_playerControls.xButtonPressed));
            itemTipsShown = true;
        }

        //Show manipulation tooltip once a manipulation item is inspected
        if (endPoint.tag == "manipulation" && !manipulationTipsShown)
        {
            StartCoroutine(setToolTip("controls_manipulate", "To Rotate, B to Return", 1f, spt_playerControls.bButtonPressed));
            manipulationTipsShown = true;
        }

        //Show movement tooltip on bucket interact
        if (GameObject.Find("spr_bucketMovePath").GetComponent<SpriteRenderer>().enabled == true && !movableTipsShown) {
            StartCoroutine(setToolTip("controls_movable", "To Move Some Objects", 0f, spt_playerControls.objectMovementControls));
            movableTipsShown = true;
        }

        //Show movement tooltip on bucket interact
        if (GameObject.Find("spr_boxMovePath").GetComponent<SpriteRenderer>().enabled == true && !movableTipsShown){
            StartCoroutine(setToolTip("controls_movable", "To Move Some Objects", 0f, spt_playerControls.objectMovementControls));
            movableTipsShown = true;
        }

        //CURRENTLY NOT FUNCTIONAL: Displays all controls tooltip after all other tooltips have been shown
        if (toolTipsDisplayed >= 7 && !controlsTipsShown) {
            StartCoroutine(setToolTip("controls_display", "To View All Controls", 10f, spt_playerControls.selectButtonPressed));
            controlsTipsShown = true;
        }

    }

    //Catch-all "variable type" for input successes
    public delegate bool InputCompletion();

    //Coroutine started after a tooltip is displayed. Once the predicate is met, stops and clears tooltip after x seconds.
    IEnumerator inputListener(InputCompletion predicate) {
        while (true) {
            if (predicate())
            {
                yield return new WaitForSeconds(.5f);
                clearToolTip();
                yield break;
            }
            else yield return null;
        } 
    }

    //Function which sets the toolTip image and text after delayTime seconds, then clears after input predicate is met
    IEnumerator setToolTip(string animation, string newText, float delayTime, InputCompletion predicate)
    {
        yield return new WaitForSeconds(delayTime);

        //Display desired controller image and text
        animator.Play(animation);
        currentText.text = newText;
        toolTipsDisplayed++;
        StartCoroutine(inputListener(predicate));
    }

    //Empties the tool tip display
    public void clearToolTip()
    {
        animator.Play("none");
        currentText.text = "";
    }
}
