/* spt_angerObject.cs
 * 
 * Created by: Lauren Cunningham
 * 
 * Last Revision Date: 4/3/2016
 *
 * This file is attached to all objects that are used as anger increasers for the monster.
 *  It instantiates a custom-made base anger class variable for each that stores data regarding how the monster is affected by that object.
 *  
 *  In the scope of the world, any game object with this script attatched is an empty game object used to track the actions of the players within the world.
 *  They are not visible in any way within the game (to the players), but they contain a variable that allows the monster to "see" or "not see" them.
 *  There will be one of these for every action that the players can do to anger the monster.
 *  When the players perform that specific action, the game object with this attatched will turn its visibility on and travel to the location
 *      that the action took place in.
 *  These allow for easy interractions between the players' actions and the monster. **/

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class spt_angerObject : MonoBehaviour {

    // Base anger class variable used to store the specifics of each action.
    //  This could be an actual base class object, or one of its children.
    private spt_baseAngerClass data;

    // For setting the initial amount that the object angers the monster
    public int angerNum;

    //Handle on the monster so its position can be tracked and anger can be updated.
    private spt_monsterMotivation monster;

    // Is this action a flashlight-tracker?
    public bool isFlashlight;

    // Does the object have a duration-orientated action associated with it?
    public bool isDurationObject = false;

    private bool checkingToggles = false;
    private int numToggles = 0;
    
    // Use this for initialization
	void Start () {

        if (isFlashlight)
            data = new spt_flashlightClass(angerNum);
        else if (isDurationObject)
            data = new spt_durationAngerClass(angerNum);
        else
            data = new spt_baseAngerClass(angerNum);

        monster = GameObject.FindObjectOfType<spt_monsterMotivation>();

        // Performs the "per-tick" calculations needed on actions with a duration (like the flashlight).
        InvokeRepeating("incrementTimer", data.getDuration(), data.getDuration());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // Returns the base anger class variable
    public spt_baseAngerClass getData(){
        return data;
    }

    public void toggleVisibility(){
        if (SceneManager.GetActiveScene().name == "net_playerlobby") return;
        getData().toggleVisibility();

        //if the object becomes visible...
        if (data.getSeen() == false){

            if (!checkingToggles)
            {
                checkingToggles = true;
                Invoke("depreciateToggles", 5);
            }
            numToggles = numToggles + 1;

            //if the monster can immediately see the object that just came into existence...
            if (monster.canSeeSomething(this.transform))
            {
                //Update the monster's anger with an increased field of view boost.
                monster.updateAnger(data.getAnger() + data.getAnger(), gameObject.transform);
            }
            else
            {
                monster.updateAnger(data.getAnger(), gameObject.transform);
            }
            //mark the object as "seen" i.e. the monster has acknowledged its initial presence.
            data.markAsSeen();
        }
    }

    
    // Function that is called "per-tick". Used for actions that have a duration.
    public void incrementTimer(){

        //if the action is visible to the monster and does in fact have a duration...
        if (data.getVisible() && data.getHasDuration()){
            
            // Find out if the monster can see that action
            if (monster.canSeeSomething(transform)) {
                
                // Add additional anger if the monster can observe the action taking place
                monster.updateAnger(data.getAngerIncrease(), gameObject.transform);
            }
            
            // Add base amount of anger for the action happening
            monster.updateAnger(data.getAngerIncrease(), gameObject.transform);
        }
    }

    // Allows you to easily move this object to another position in 3D space.
    public void moveObject(Transform pos){
        this.transform.position = pos.position;
    }

    // Runs off the function y = (1/2)sqrt(x), where x is the number of toggles.
    //  Will only take anger off the monster if a player has done less than 4 toggles.
    public void depreciateToggles()
    {
        checkingToggles = false;
        
        if (numToggles >= 4)
        {
            numToggles = 0;
            return;
        }

        double multiplier = 1.0 - (0.5 * Math.Sqrt(numToggles));
        int depreciation = (int)(-(numToggles * multiplier));
        if(!monster.angerUpdateDisabled) monster.updateAnger(depreciation, gameObject.transform);
        numToggles = 0;
        return;
    }
}
