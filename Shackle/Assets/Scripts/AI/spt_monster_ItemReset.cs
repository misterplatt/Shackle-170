using UnityEngine;
using System.Collections;

public class spt_monster_ItemReset : MonoBehaviour {

    public delegate void ResetFunction();
    public ResetFunction resetFunction;

    void Start() {
        VRStandardAssets.Examples.spt_baseInteractiveObject thisObjSpt = GetComponent<VRStandardAssets.Examples.spt_baseInteractiveObject>();
        if (thisObjSpt != null) {
            resetFunction = thisObjSpt.resetItem;
        }
        else {
            Debug.Log("Monster_ItemReset : Failure to acquire Delegate.");
        }
    }

	public void resetState() {
        resetFunction();
    }

}
