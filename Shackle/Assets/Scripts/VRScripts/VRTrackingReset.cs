using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.VR;

namespace VRStandardAssets.Utils
{
    // This class simply insures the head tracking behaves correctly when the application is paused.
    public class VRTrackingReset : NetworkBehaviour
    {
        private void OnApplicationPause(bool pauseStatus)
        {
            if (!isLocalPlayer) return;
            //InputTracking.Recenter();
        }
    }
}