using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public void ChangeScene(int sceneIndex)
    {
      
      SceneManager.LoadScene(sceneIndex);
    }

    public void ExitGame()
    {
      if(!Application.isEditor) Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
