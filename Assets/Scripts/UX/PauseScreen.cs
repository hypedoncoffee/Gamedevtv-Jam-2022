using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
  bool paused;
  [SerializeField] UIPanelMask panelView;
  AudioSource uiAudio;
  [SerializeField] int menuScene = 1;
    // Start is called before the first frame update
    void Start()
    {
     // panelView = GetComponent<UIPanelMask>();
      uiAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
          PauseGame(!paused);
        }
    }

    public void PauseGame(bool pauseNow)
    {
      if(pauseNow)
      {
        Debug.Log("Pausing");
        
        paused=true;
        panelView.Reveal();
        Time.timeScale = 0;

      }
      else if(!pauseNow)
      {
        Debug.Log("Resuming");
        paused=false;
        
        panelView.Hide();
        Time.timeScale=1;
      }
    }

    public void RestartLevel()
    {
      Time.timeScale=1;
      StartCoroutine(RestartStage());
    }

    IEnumerator RestartStage()
    {
      yield return new WaitForSecondsRealtime(1f);
      int currentscene = SceneManager.GetActiveScene().buildIndex;
      SceneManager.LoadScene(currentscene);
    }

    public void ReturnToMenu()
    {
      Time.timeScale= 1;
      StartCoroutine(MainMenu());
    }

    IEnumerator MainMenu()
    {
      Time.timeScale=1;
      yield return new WaitForSeconds(1f);
      SceneManager.LoadScene(menuScene);
    }
}
