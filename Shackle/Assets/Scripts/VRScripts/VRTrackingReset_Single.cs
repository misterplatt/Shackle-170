using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.VR;

namespace VRStandardAssets.Utils
{
    // This class simply insures the head tracking behaves correctly when the application is paused.
    public class VRTrackingReset_Single : MonoBehaviour
    {
        private void OnApplicationPause(bool pauseStatus) {
            InputTracking.Recenter();
        }
    }
}