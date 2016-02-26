using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class spt_debug_overrideLoad : MonoBehaviour {

    public static bool wasLoaded = false;

    void Awake() {
        DontDestroyOnLoad(transform.gameObject);

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }
	// Use this for initialization
	void Start () {

        if (GameObject.Find("NetworkManager") != null) return;

        if (SceneManager.GetActiveScene().name != "MainMenu" && !(spt_debug_overrideLoad.wasLoaded))
            Debug.Log(SceneManager.GetActiveScene().name);     
            SceneManager.LoadScene("MainMenu");
	}
	
}
