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
using UnityEngine.SceneManagement;

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
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "net_SpookyGarage" || SceneManager.GetActiveScene().name != "net_RangerOutpost_crash" || SceneManager.GetActiveScene().name != "net_OpticsLab") Destroy(gameObject);
    }
}
