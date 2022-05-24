using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace NameReader
{

public class NamePicker : MonoBehaviour
{

    //Call ReadList("firstname"),ReadList("lastname"), or ReadList("crime") to get the respective entry.

    [SerializeField] string pathToLists = "Assets"+Path.DirectorySeparatorChar+"Lists";
    [SerializeField] string crimePath = "crimes.txt";
    [SerializeField] string lNamePath = "lastnames.txt";
    [SerializeField] string fNamePath = "firstnames.txt";
    [SerializeField] string streetPath= "streets.txt";
    // Start is called before the first frame update
    void Start()
    {
        ReadList("firstname");ReadList("lastname");ReadList("crime");
    }

    public string ReadList(string listName)
    {   string nextPath;
        string nextString;
        string[] lines = new string[0];
        switch(listName)
        {

            case "firstname":
            //    nextPath =  System.IO.Path.Join(pathToLists,fNamePath);
                nextPath = pathToLists+Path.DirectorySeparatorChar +fNamePath;
                lines = File.ReadAllLines(@nextPath);
            break;
            case "lastname":
//                nextPath =  Path.Join(pathToLists,lNamePath);
                nextPath = pathToLists+Path.DirectorySeparatorChar +lNamePath;
                lines = File.ReadAllLines(@nextPath);
            break;
            case "crime":
  //              nextPath =  Path.Join(pathToLists,crimePath);
                nextPath = pathToLists+Path.DirectorySeparatorChar +crimePath;
                lines = File.ReadAllLines(@nextPath);
            break;
            case "street":
                nextPath = pathToLists+Path.DirectorySeparatorChar +streetPath;
                lines = File.ReadAllLines(@nextPath);
            break;
            case null:
                nextString = "null";
            break;

        }
        if(lines.Length > 0)
        {
            nextString = lines[Random.Range(0,lines.Length-1)];
           Debug.Log(nextString);

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