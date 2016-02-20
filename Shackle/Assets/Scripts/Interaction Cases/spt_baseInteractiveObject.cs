using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_baseInteractiveObject : MonoBehaviour
    {
        [SerializeField]
        private string gateItemName;
        [SerializeField]
        protected int holdTime = 0;
        [SerializeField]
        private VRInteractiveItem m_InteractiveItem;

        [SerializeField] protected Image selectionRadial; //Reference to the image who's fill amount is adjusted to display the bar.
        protected bool radialActive;

        protected spt_inventory inventorySpt;
        protected bool holding = false;
        protected float timer = 0;

        private void OnEnable()
        {
            m_InteractiveItem.OnOver += HandleOver;
            m_InteractiveItem.OnOut += HandleOut;
            m_InteractiveItem.OnClick += HandleClick;
            m_InteractiveItem.OnDoubleClick += HandleDoubleClick;
            m_InteractiveItem.OnUp += HandleUp;
            m_InteractiveItem.OnDown += HandleDown;
        }


        private void OnDisable()
        {
            m_InteractiveItem.OnOver -= HandleOver;
            m_InteractiveItem.OnOut -= HandleOut;
            m_InteractiveItem.OnClick -= HandleClick;
            m_InteractiveItem.OnDoubleClick -= HandleDoubleClick;
            m_InteractiveItem.OnUp -= HandleUp;
            m_InteractiveItem.OnDown -= HandleDown;
        }

        //Use this for initialization
        virtual protected void Start()
        {
            //inventorySpt = GetComponent<VRInteractiveItem>().inventoryScript;
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
            Debug.Log("Show out state");
            selectionRadial = GetComponent<VRInteractiveItem>().radial;
            holding = false;
            selectionRadial.fillAmount = 0;
            selectionRadial.enabled = false;
            timer = 0;
        }


        //Handle the Click event
        virtual protected void HandleClick()
        {
            inventorySpt = GetComponent<VRInteractiveItem>().inventoryScript;
            Debug.Log(inventorySpt.retrieveObjectFromInventory(inventorySpt.activeItem).name);
            if (Input.GetButtonDown("aButton") && inventorySpt.retrieveObjectFromInventory(inventorySpt.activeItem).name == gateItemName) {
                clickSuccess();
            }
        }

        //To be modified in child scripts when A button has pressed over an object with the correct item active
        virtual protected void clickSuccess()
        {
            Debug.Log("Show click state");
        }

        //Handle the DoubleClick event
        virtual protected void HandleDoubleClick() { }

        //Handle the Down event
        virtual protected void HandleDown()
        {
            inventorySpt = GetComponent<VRInteractiveItem>().inventoryScript;
            selectionRadial = GetComponent<VRInteractiveItem>().radial;
            // User must press A to interact with the object, negates the case of user holding A previous to interaction
            if (Input.GetButtonDown("aButton") && inventorySpt.retrieveObjectFromInventory(inventorySpt.activeItem).name == gateItemName)//activeItem.Value.name == gateItemName)
            {
                holding = true;
                selectionRadial.enabled = true;
                Debug.Log("Show down state");
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
            if (!holding) {
                selectionRadial.fillAmount = 0;
                selectionRadial.enabled = false;
                timer = 0;
            }
        }

        virtual protected void holdSuccess() {
            Debug.Log("HOLD SUCCESS");
        }

        //Handle the Up event
        virtual protected void HandleUp()
        {
            Debug.Log("Show up state");
            selectionRadial = GetComponent<VRInteractiveItem>().radial;
            holding = false;
            selectionRadial.fillAmount = 0;
            selectionRadial.enabled = false;
            timer = 0;
        }
    }
}
