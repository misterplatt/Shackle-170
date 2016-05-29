/*
spt_loadScreenTips

Author(s): Hayden Platt

Revision 1

Randomly sets the text component to a tip about the game
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class spt_loadScreenTips : MonoBehaviour {

    private string[] tips = { "The monster gives warning as it nears, keep an ear out.",
        "Using your flashlights at the same time can be dangerous.",
        "Communication is key - be sure to tell each other what you see.",
        "Beware - loud sounds attract the monster as well as light."};

	// Use this for initialization
	void Start () {
        GetComponent<Text>().text = tips[Random.Range(0,tips.Length)];
	}
    
}
