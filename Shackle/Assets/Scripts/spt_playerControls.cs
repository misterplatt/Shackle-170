using UnityEngine;
using System.Collections;

public class spt_playerControls : MonoBehaviour
{
    private Rigidbody rb;
    public float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    // fixedUpdate is called before any physics calculations
    void FixedUpdate()
    {
        aButtonPressed();
        bButtonPressed();
        rightThumbstickButtonPressed();
        leftThumbstickMoved();
        triggers();
        startButtonPressed();
    }

    public static bool aButtonPressed()
    {
        if (Input.GetButton("aButton"))
        {
            Debug.Log("SUCCESS for A button");
            return true;
        }
        else return false;
    }

    public static bool bButtonPressed()
    {
        if (Input.GetButton("bButton"))
        {
            Debug.Log("SUCCESS for B button");
            return true;
        }
        else return false;
    }

    bool rightThumbstickButtonPressed()
    {
        if (Input.GetButton("rightThumbstickButton"))
        {
            Debug.Log("SUCCESS for RightThumbstick button");
            return true;
        }
        else return false;
    }

    void leftThumbstickMoved()
    {
        float moveHorizontal = Input.GetAxis("leftThumbstickHoriz");
        float moveVertical = Input.GetAxis("leftThumbstickVert");
        Debug.Log("Left Thumbstick Hori:" + moveHorizontal);
        Debug.Log("Left Thumbstick Vert:" + moveVertical);
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);
    }

    public static float leftThumb(string s) {
        if (s == "Horizontal") return Input.GetAxis("leftThumbstickHoriz");
        else if (s == "Vertical") return Input.GetAxis("leftThumbstickVert");
        return 0;
    }

    bool startButtonPressed()
    {
        if (Input.GetButton("startButton"))
        {
            Debug.Log("SUCCESS for Start button");
            return true;
        }
        else return false;
    }

    void triggers()
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

    void rightThumbstickMoved()
    {
        float moveHorizontal = Input.GetAxis("rightThumbstickHoriz");
        float moveVertical = Input.GetAxis("rightThumbstickVert");
        Debug.Log("Right Thumbstick Hori:" + moveHorizontal);
        Debug.Log("Right Thumbstick Vert:" + moveVertical);
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);
        //Add code to rotate the current item being inspected

    }

    public static float rightThumb(string s)
    {
        if (s == "Horizontal") return Input.GetAxis("rightThumbstickHoriz");
        else if (s == "Vertical") return Input.GetAxis("rightThumbstickVert");
        return 0;
    }
}