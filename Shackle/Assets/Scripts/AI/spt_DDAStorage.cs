using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;

public class spt_DDAStorage : NetworkBehaviour {

    [SyncVar] [SerializeField]
    private int difficultyValue;
    bool alteration;

    public void incrementDiffValue() {
        if (alteration) return;
        if (difficultyValue <= 7) difficultyValue += 2;
        alteration = true;
    }

    public void decrementDiffValue() {
        if (alteration) return;
        if (difficultyValue > 2) difficultyValue--;
        alteration = true;
    }

    void Start()
    {
        alteration = false;
        initDiff();
    }

    void OnDestroy()
    {
        diffToFile();
    }

    public void initDiff()
    {
        //check if file exists, if it does load it
        if ( File.Exists("DataDump/Difficulty") )
        {
            string line;
            StreamReader reader =  new StreamReader("DataDump/Difficulty");

            while( (line = reader.ReadLine()) != null )
            {
                int.TryParse(line, out difficultyValue);
            }

            return;
        }
        //otherwise init to 5.
        difficultyValue = 5;
        Debug.Log("Could Not Locate Difficulty file.");  
    }

    public void diffToFile() {
        //open file and write difficultyValue to it
        //step through synclist and grab name and timestamps, save to datadump so we can retrieve metrics later

        StreamWriter writer = new StreamWriter("DataDump/Difficulty", false);
        writer.WriteLine(difficultyValue);
        writer.Close();
}

    public int getDiff()
    {
        switch (difficultyValue)
        {
            case 9:
                return 5;
            case 8:
            case 7:
                return 4;
            case 6:
            case 5:
            case 4:
                return 3;            
            case 3:
            case 2:
                return 2;
            
            case 1:
                return 1;
            default:
                break;                                           
        }

        //if none of these apply it's an error.
        return 0;
    }

}
