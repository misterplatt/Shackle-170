using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Required in C#

public class spt_playerControls : MonoBehaviour
{
    private Rigidbody rb;
    public float speed;
    static PlayerIndex playerIndex;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // fixedUpdate is called before any physics calculations
    void FixedUpdate()
    {
        aButtonPressed();
        bButtonPressed();
        xButtonPressed();
        rightThumbstickButtonPressed();
        leftThumbstickMoved();
        triggers();
        startButtonPressed();
    }

    // Checks if A button is pressed, which is used for general interaction in the world
    public static bool aButtonPressed()
    {
        if (Input.GetButton("aButton"))
        {
            Debug.Log("SUCCESS for A button");
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
            Debug.Log("SUCCESS for B button");
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
            GamePad.SetVibration(playerIndex, 1.0f, 1.0f);

            Debug.Log("SUCCESS for x button");
            return true;
        }
        else return false;
    }

    // Checks if the right thumbstick is pressed, which is used for toggling the flashlight
    public static bool rightThumbstickButtonPressed()
    {
        if (Input.GetButton("rightThumbstickButton"))
        {
            Debug.Log("SUCCESS for RightThumbstick button");
            return true;
        }
        else return false;
    }

    // Checks if left thumbstick is moved, which is used to move player chairs
    void leftThumbstickMoved()
    {
        float moveHorizontal = Input.GetAxis("leftThumbstickHoriz");
        float moveVertical = Input.GetAxis("leftThumbstickVert");
        //Debug.Log("Left Thumbstick Hori:" + moveHorizontal);
        //Debug.Log("Left Thumbstick Vert:" + moveVertical);
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);
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
        if (Input.GetButton("startButton"))
        {
            GamePad.SetVibration(playerIndex, 0, 0);
            Debug.Log("SUCCESS for Start button");
            return true;
        }
        else return false;
    }

    // Checks if either of the triggers are pressed, which is used for cycling through the inventory
    public static void triggers()
    {
        if (Input.GetAxis("triggers") < 0)
        {
            Debug.Log("LEFT TRIGGER HIT");
            //code for cycling the inventory to the left
        }
        else if (Input.GetAxis("triggers") > 0)
        {
            Debug.Log("RIGHT TRIGGER HIT");
            //code for cycling the inventory to the right
        }
    }

    // Checks if the right thumbstick is moved , which is used for rotating and manipulating the field of view when interacting with an object
    void rightThumbstickMoved()
    {
        float moveHorizontal = Input.GetAxis("rightThumbstickHoriz");
        float moveVertical = Input.GetAxis("rightThumbstickVert");
        Debug.Log("Right Thumbstick Hori:" + moveHorizontal);
        Debug.Log("Right Thumbstick Vert:" + moveVertical);
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);

    }

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
        //GamePad.SetVibration(playerIndex, Input.GetAxis("leftThumbstickVert"), 0);
        GamePad.SetVibration(playerIndex, 1.0f, 0);
    }

    // Uses the right motor to create a smoother vibration pattern
    public static void smoothVibration()
    {
        // GamePad.SetVibration(playerIndex, 0, Input.GetAxis("leftThumbstickVert"));
        GamePad.SetVibration(playerIndex, 0, 1.0f);
    }

    // Uses both the left and right motors to create a greater vibration 
    public static void simulVibration()
    {
        // GamePad.SetVibration(playerIndex, Input.GetAxis("leftThumbstickVert"), Input.GetAxis("leftThumbstickVert"));
        GamePad.SetVibration(playerIndex, 1.0f, 1.0f);
    }
}