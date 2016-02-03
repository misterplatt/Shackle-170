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
        [SerializeField]
        private Renderer m_Renderer;
        bool currentState = false;

        public spt_inventory inventory;
        public spt_inventoryUI inventUI;

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
            inventory.pickUp(gameObject);
            inventUI.inventorySpriteOn(gameObject.name);
            gameObject.SetActive(false);
        }
    }
}
