using UnityEngine;
using VRStandardAssets.Utils;
using UnityEngine.UI;

namespace VRStandardAssets.Examples
{
    public class spt_interactivePickUp : MonoBehaviour
    {
        //Access to InteractiveItem script
        [SerializeField]
        private VRInteractiveItem m_InteractiveItem;
        bool currentState = false;

        public spt_inventory inventoryScript;

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
            inventoryScript = GetComponent<VRInteractiveItem>().inventoryScript;
            inventoryScript.pickUp(gameObject);
            //gameObject.SetActive(false); PLACEHOLDER: Can no longer deactivate without breaking inventory
        }
    }
}