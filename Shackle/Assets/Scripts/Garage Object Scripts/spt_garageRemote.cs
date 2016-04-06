/*
spt_garageRemote

Author(s): Dara Diba

Revision 1


Checks if collision is between garage door remote and the floor and plays thump if so.
*/


using UnityEngine;
using System.Collections;

public class spt_garageRemote : MonoBehaviour {

    private AudioSource remoteThump;

    // Use this for initialization
    void Start () {
        remoteThump = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update() { }


    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Floor")
        {
            remoteThump.Play();
        }
    }
}
