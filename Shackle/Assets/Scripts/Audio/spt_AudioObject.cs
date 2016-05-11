/* spt_AudioObject.cs
 * 
 * Created by: Dara Diba
 * 
 * Last Revision Date: 5/11/2016
 * 
 * This file destroys more than one instance of the Audio object in a room when loaded from Lobby.
 */


using UnityEngine;
using System.Collections;

public class spt_AudioObject : MonoBehaviour {

    public static spt_AudioObject instance;

    // Use this for initialization
    void Awake()
    {
        if (instance)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            DontDestroyOnLoad(transform.gameObject);
            instance = this;
        }
    }
}
