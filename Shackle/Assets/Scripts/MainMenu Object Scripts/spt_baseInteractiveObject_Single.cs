/*
spt_baseInteractiveObject_Single

Author(s): Hayden Platt

Revision 1

This is the base class used in the heirarchy of item cases in the menu.
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_baseInteractiveObject_Single : MonoBehaviour
    {
        [SerializeField]
        protected float holdTime = 0;
        [SerializeField]
        private VRInteractiveItem_Single m_InteractiveItemSingle;

        protected Image selectionRadial; //Reference to the image who's fill amount is adjusted to display the bar.
        protected bool radialActive;

        protected bool holding = false;
        protected float timer = 0;

        private void OnEnable()
        {
            m_InteractiveItemSingle.OnOver += HandleOver;
            m_InteractiveItemSingle.OnOut += HandleOut;
            m_InteractiveItemSingle.OnClick += HandleClick;
            m_InteractiveItemSingle.OnDoubleClick += HandleDoubleClick;
            m_InteractiveItemSingle.OnUp += HandleUp;
            m_InteractiveItemSingle.OnDown += HandleDown;
        }


        private void OnDisable()
        {
            m_InteractiveItemSingle.OnOver -= HandleOver;
            m_InteractiveItemSingle.OnOut -= HandleOut;
            m_InteractiveItemSingle.OnClick -= HandleClick;
            m_InteractiveItemSingle.OnDoubleClick -= HandleDoubleClick;
            m_InteractiveItemSingle.OnUp -= HandleUp;
            m_InteractiveItemSingle.OnDown -= HandleDown;
        }

        //Use this for initialization
        virtual protected void Start()
        {

        }

        // Update is called once per frame
        virtual protected void Update()
        {

        }

        //Handle the Over event
        virtual protected void HandleOver()
        {

        }


        //Handle the Out event
        virtual protected void HandleOut()
        {
            //Debug.Log("Show out state");
            selectionRadial = GetComponent<VRInteractiveItem>().radial;
            holding = false;
            selectionRadial.fillAmount = 0;
            selectionRadial.enabled = false;
            timer = 0;
        }


        //Handle the Click event
        virtual protected void HandleClick()
        {
            if (Input.GetButtonDown("aButton"))
            {
                clickSuccess();
            }
        }

        //To be modified in child scripts when A button has pressed over an object with the correct item active
        virtual protected void clickSuccess()
        {

        }

        //Handle the DoubleClick event
        virtual protected void HandleDoubleClick() { }

        //Handle the Down event
        virtual protected void HandleDown()
        {
            selectionRadial = GetComponent<VRInteractiveItem_Single>().radial;
            // User must press A to interact with the object, negates the case of user holding A previous to interaction
            if (Input.GetButtonDown("aButton"))
            {
                holding = true;
                selectionRadial.enabled = true;
                //m_Renderer.material = m_DownMaterial;
            }
            if (holding)
            {
                selectionRadial.fillAmount = timer / holdTime;
                timer += Time.deltaTime;
                if (timer >= holdTime || holdTime == 0)
                {
                    selectionRadial.enabled = false;
                    holdSuccess();
                }
            }

            //START HERE FOR RADIAL FADING
            if (!holding)
            {
                selectionRadial.fillAmount = 0;
                selectionRadial.enabled = false;
                timer = 0;
            }
        }

        virtual protected void holdSuccess()
        {
            //Debug.Log("HOLD SUCCESS");
        }

        //Handle the Up event
        virtual protected void HandleUp()
        {
            //Debug.Log("Show up state");
            selectionRadial = GetComponent<VRInteractiveItem_Single>().radial;
            holding = false;
            selectionRadial.fillAmount = 0;
            selectionRadial.enabled = false;
            timer = 0;
        }

        virtual public void resetItem()
        {

        }
    }
}