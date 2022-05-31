using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace UX.CharacterInfo
{

public class NamePicker : MonoBehaviour
{
    float DebugTimer0;
    float DebugTimer1;

    //Call ReadList("firstname"),ReadList("lastname"), or ReadList("crime") to get the respective entry.

     string pathToLists = "Assets+Path.DirectorySeparatorChar+Lists";
    string crimePath = "crimes";
    string lNamePath = "lastnames";
    string fNamePath = "firstnames";
    [SerializeField] string streetPath= "streets.txt";
    string[] crimes = new string[5];
    string[] firstnames= new string[5];
    string[] lastnames = new string[5];
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        //ReadList("firstname");ReadList("lastname");ReadList("crime");
    
            //Load a text file (Assets/Resources/Text/textFile01.txt)
        crimes = Resources.Load<TextAsset>(@"Lists"+Path.DirectorySeparatorChar+crimePath).ToString().Split('\n');
        Debug.Log(crimes.Length);
        Debug.Log(crimes[Random.Range(0,crimes.Length-1)]);
        lastnames = Resources.Load<TextAsset>(@"Lists"+Path.DirectorySeparatorChar+lNamePath).ToString().Split('\n');
        firstnames = Resources.Load<TextAsset>(@"Lists"+Path.DirectorySeparatorChar+fNamePath).ToString().Split('\n');

    }

    public string ReadList(string listName)
    {   string nextPath ="";
       // DebugTimer0 = Time.realtimeSinceStartup;
        string nextString;
        string[] lines = new string[0];
        TextAsset file;
        string content;
            switch (listName)
        {

            /*Depreciated from 5.01; this works but reads the entire list each run, and doesn't run on webgl.

            case "firstname":
                    //    nextPath =  System.IO.Path.Join(pathToLists,fNamePath);

                    //nextPath = pathToLists+Path.DirectorySeparatorChar +fNamePath;
                    //lines = File.ReadAllLines(@nextPath);
                lines = File.ReadAllLines(Path.Combine(Application.streamingAssetsPath, fNamePath));
                break;
            case "lastname":
                    //                nextPath =  Path.Join(pathToLists,lNamePath);
                    //nextPath = pathToLists+Path.DirectorySeparatorChar +lNamePath;
                    //lines = File.ReadAllLines(@nextPath);
                    lines = File.ReadAllLines(Path.Combine(Application.streamingAssetsPath, lNamePath));
                    break;
            case "crime":
                    //              nextPath =  Path.Join(pathToLists,crimePath);
                    //nextPath = pathToLists+Path.DirectorySeparatorChar +crimePath;
                    //lines = File.ReadAllLines(@nextPath);

                    lines = File.ReadAllLines(Path.Combine(Application.streamingAssetsPath, crimePath));
                    break;
            case "street":
                    lines = File.ReadAllLines(Path.Combine(Application.streamingAssetsPath, streetPath));
                    //nextPath = pathToLists+Path.DirectorySeparatorChar +streetPath;
                    //lines = File.ReadAllLines(@nextPath);
                    break;
            case null:
                nextString = "null";
            break;
             */

            case "firstname":
            return firstnames[Random.Range(0,firstnames.Length-1)];
            break;
            case "lastname":
            return lastnames[Random.Range(0,lastnames.Length-1)];
            break;
            case "crime":
            return crimes[Random.Range(0,crimes.Length-1)];
            break;

        }
        if(lines.Length > 0)
        {
            nextString = lines[Random.Range(0,lines.Length-1)];
        //Debug.Log(nextString);
            //int lineNumber = Random.Range(0,lines.Length-1);
            //nextString = File.ReadLines(@nextPath).Skip(lineNumber).Take(1).First();;
         //   Debug.Log(Time.realtimeSinceStartup - DebugTimer0 + " ms");
          return nextString;
        }
        else return null;
    }
    
    

    // Update is called once per frame
    void Update()
    {
        
    }
}

}