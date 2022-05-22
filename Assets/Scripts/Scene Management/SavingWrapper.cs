
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";
        [SerializeField] float fadeInTime = 2f;

        IEnumerator Start()
        {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return fader.FadeIn(fadeInTime);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
            }
        }
        public void Save()
        {
            // TODO : UI Component with filename
        }

        public void Load()
        {
            // TODO : UI Component with filename
        }
        public void Delete()
        {
            // TODO : UI Component with filename
        }
    }
}
