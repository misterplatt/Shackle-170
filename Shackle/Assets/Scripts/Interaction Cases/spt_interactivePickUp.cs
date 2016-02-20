using UnityEngine;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Examples
{
    public class spt_interactivePickUp : MonoBehaviour
    {
        //Access to InteractiveItem script
        [SerializeField]
        private VRInteractiveItem m_InteractiveItem;
        bool currentState = false;

        public spt_inventory inventorySpt;

        void Start() {
            //inventory = GameObject.Find("Camera Player").GetComponent<spt_inventory>();
        }

        private void OnEnable()
        {
            m_InteractiveItem.OnClick += HandleClick;
        }


        private void OnDisable()
        {
            m_InteractiveItem.OnClick -= HandleClick;
        }

        //Handle the Click event
        private void HandleClick()
        {
            Debug.Log("Show click state");
            inventorySpt = GetComponent<VRInteractiveItem>().inventoryScript;
            inventorySpt.pickUp(gameObject);
            //gameObject.SetActive(false); //Desired functionality
            transform.position = Vector3.down * 1000; //PLACEHOLDER: Sends objects to hell to prevent inventory breaking
        }
    }
}