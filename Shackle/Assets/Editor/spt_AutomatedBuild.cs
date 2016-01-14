using UnityEditor;
using UnityEngine;
using System.Collections;
using System;
//Automated Build Script designed to be called via command line build procedure
//Ryan Connors - Shackle - Winter 2016

public class spt_AutomatedBuild : MonoBehaviour {

	// Use this for initialization
	static void Start () {
		
		string buildNum_str = System.IO.File.ReadAllText("../Support/buildNum");
		int buildNum_int = Int32.Parse(buildNum_str);
		
        string[] scenes = { "Assets/Scenes/MainMenu.unity",
                            "Assets/Scenes/LevelTemplate.unity"};

        BuildPipeline.BuildPlayer(scenes, "C:/Builds/ShackleBuild_" + (++buildNum_int).ToString() +   ".exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
		System.IO.File.WriteAllText( "../Support/buildNum" , (buildNum_int).ToString() );
	}
}
