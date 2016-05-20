/*
spt_playerControls

Author(s): Dara Diba

Revision 1

Provides the logic and functions for player input
*/

using UnityEngine;
using System.Collections;
using System;
using XInputDotNetPure; // Required in C#

public class spt_playerControls : MonoBehaviour
{

    static PlayerIndex playerIndex = 0;
    private static DateTime timer;
    private static DateTime timert;
    private static DateTime timerend;

    // Checks if A button is pressed, which is used for general interaction in the world
    public static bool aButtonPressed()
    {
        if (Input.GetButton("aButton"))
        {
           // Debug.Log("SUCCESS for A button");
            return true;
        }
        else
        {
            return false;
        }
    }

    // Checks if B button is pressed, which is used as a return
    public static bool bButtonPressed()
    {
        if (Input.GetButton("bButton"))
        {
            return true;
        }
        else return false;
    }

    // Checks if X button is pressed, which is used for passing items between players
    // For now X button starts the controller vibration
    public static bool xButtonPressed()
    {
        if (Input.GetButton("xButton"))
        {
            return true;
        }
        else return false;
    }

    // Checks if Select button is pressed
    public static bool selectButtonPressed()
    {
        if (Input.GetButton("Select"))
        {
            return true;
        }
        else return false;
    }

    // Checks if the right thumbstick is pressed, which is used for toggling the flashlight
    public static bool rightThumbstickButtonPressed()
    {
        if (Input.GetButtonDown("rightThumbstickButton"))
        {
            return true;
        }
        else return false;
    }

    // Checks if the string being passed is either Horizontal or Vertical which comes from the game object's properties
    // This is used to keep objects manipulated/moved in the desired axis
    public static float leftThumb(string s) {
        if (s == "Horizontal") return Input.GetAxis("leftThumbstickHoriz");
        else if (s == "Vertical") return Input.GetAxis("leftThumbstickVert");
        return 0;
    }

    // Checks if start button is pressed, which is used for opening the menu
    // For now it is used to stop the controller vibrations
    public static bool startButtonPressed()
    {
        if (Input.GetButtonDown("startButton"))
        {
            return true;
        }
        else return false;
    }

    // Checks if either of the triggers are pressed, which is used for cycling through the inventory
    public static int triggers()
    {
        if (Input.GetAxis("triggers") < 0)
        {
            return -1;
        }
        else if (Input.GetAxis("triggers") > 0)
        {
            return 1;
        }
        else return 0;
    }

    //Function which returns true if either trigger is pressed
    public static bool triggerPressed()
    {
        return (Mathf.Abs(Input.GetAxis("triggers")) > 0);
    }

    // Checks if rightBumper button is pressed, which is used as a return
    public static bool rightBumperPressed()
    {
        if (Input.GetButton("rightBumper"))
        {
            return true;
        }
        else return false;
    }

    // Checks if leftBumper button is pressed, which is used as a return
    public static bool leftBumperPressed()
    {
        if (Input.GetButton("leftBumper"))
        {
            return true;
        }
        else return false;
    }

    public static bool bumpersPressed(){
        return (Input.GetButton("leftBumper") && Input.GetButton("rightBumper"));
    }

    //Function which returns true if the right stick is moved in any direction
    public static bool rightThumbstickMoved()
    {
        return (Mathf.Abs(Input.GetAxis("rightThumbstickHoriz")) > 0 || Mathf.Abs(Input.GetAxis("rightThumbstickVert")) > 0);
    }

    //Function which returns true if the left stick is moved in any direction
    public static bool leftThumbstickMoved()
    {
        return (Mathf.Abs(Input.GetAxis("leftThumbstickHoriz")) > 0 || Mathf.Abs(Input.GetAxis("leftThumbstickVert")) > 0);
    }

    //Function which returns true if the left stick is moved in any direction while the a button is pressed
    public static bool objectMovementControls()
    {
        return (aButtonPressed() && leftThumbstickMoved());
    }

    //Function which returns true if the left stick is moved vertically direction while the bumpers are held
    public static bool playerMovementControls()
    {
        return (bumpersPressed() && (Mathf.Abs(Input.GetAxis("leftThumbstickVert")) > 0));
    }

    //Function which returns true if the left stick is moved in any direction while the a button is pressed


    // Checks if the string being passed is either Horizontal or Vertical which comes from the game object's properties
    // This is used to keep objects manipulated/moved in the desired axis
    public static float rightThumb(string s)
    {
        if (s == "Horizontal") return Input.GetAxis("rightThumbstickHoriz");
        else if (s == "Vertical") return Input.GetAxis("rightThumbstickVert");
        return 0;
    }

    // Uses the left motor to create a rougher vibration pattern
    public static void roughVibration()
    {
        GamePad.SetVibration(playerIndex, 1.0f, 0);
    }

    // Uses the right motor to create a smoother vibration pattern
    public static void smoothVibration()
    {
        GamePad.SetVibration(playerIndex, 0, 1.0f);
    }

    // Uses both the left and right motors to create a greater vibration 
    public static void simulVibration()
    {
        GamePad.SetVibration(playerIndex, 1.0f, 1.0f);
    }

    // Stops the motors
    public static void stopVibration()
    {
        GamePad.SetVibration(playerIndex, 0, 0);
    }

    // Vibrates the controller with a given force, specific vibration motor and for a selected amount of time
    public static void controllerVibration(string motor, float force, double vibrateTime)
    {
        /*
        while (timer <= timerend)
        {
            if (motor == "Rough") GamePad.SetVibration(playerIndex, force, 0);
            if (motor == "Smooth") GamePad.SetVibration(playerIndex, 0, force);
            if (motor == "Both") GamePad.SetVibration(playerIndex, force, force);
            timert += Time.deltaTime;
        }
        if (timer > timerend)
        {
            GamePad.SetVibration(playerIndex, 0, 0);
            timer = 0;
        }*/
        
        timer = DateTime.Now;
        timerend = timer.AddSeconds(vibrateTime);

         while (timer.Second < timerend.Second)
        {

            if (motor == "Rough") GamePad.SetVibration(playerIndex, force, 0);
            if (motor == "Smooth") GamePad.SetVibration(playerIndex, 0, force);
            if (motor == "Both") GamePad.SetVibration(playerIndex, force, force);
            timer = DateTime.Now;
        }
            GamePad.SetVibration(playerIndex, 0, 0);
            //timer = timerend = 0;
    }
 
    public static IEnumerator NewVibrator(string motor, float force, double vibrateTime)
    {
        timer = DateTime.Now;
        timerend = timer.AddSeconds(vibrateTime);
        while (timer.Second < timerend.Second)
        {
            if (motor == "Rough") GamePad.SetVibration(playerIndex, force, 0);
            if (motor == "Smooth") GamePad.SetVibration(playerIndex, 0, force);
            if (motor == "Both") GamePad.SetVibration(playerIndex, force, force);
            timer = DateTime.Now;
        }
        if (timer.Second > timerend.Second)
        {
            GamePad.SetVibration(playerIndex, 0, 0);
            yield break;
        }
        yield return null;

    }
}