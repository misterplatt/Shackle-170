using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class spt_settingControls : MonoBehaviour {

    //public string textToModify;
    private Text textComp;

	// Use this for initialization
	void Start () {
        textComp = GetComponent<Text>();
        float newVal = int.Parse(textComp.text);
        if (gameObject.name == "txt_volume") AudioListener.volume = newVal / 10;
        if (gameObject.name == "txt_brightness") RenderSettings.ambientLight = new Color(newVal / 10, newVal / 10, newVal / 10, 1);
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void decrement() {
        if(int.Parse(textComp.text) > 0){
            float newVal = int.Parse(textComp.text) - 1;
            textComp.text = newVal.ToString();
            if(gameObject.name == "txt_volume") AudioListener.volume = newVal / 10;
            if (gameObject.name == "txt_brightness") RenderSettings.ambientLight = new Color(newVal / 10, newVal / 10, newVal / 10, 1);
        }
    }

    public void increment(){
        if (int.Parse(textComp.text) < 10){
            float newVal = int.Parse(textComp.text) + 1;
            textComp.text = newVal.ToString();
            if(gameObject.name == "txt_volume") AudioListener.volume = newVal / 10;
            if(gameObject.name == "txt_brightness") RenderSettings.ambientLight = new Color(newVal / 10, newVal / 10, newVal / 10, 1);
        }
    }
}
