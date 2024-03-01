using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public AudioClip[] bgm;
    public AudioSource audioSource;

    bool onlyOnceInMenu = false, onlyOnceInGame = false;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            onlyOnceInGame = false;

            if (!onlyOnceInMenu)
            {
                onlyOnceInMenu = true;

                audioSource.clip = bgm[0];
                audioSource.Play();
            }
        }
        else if (SceneManager.GetActiveScene().name == "Level")
        {
            onlyOnceInMenu = false;

            if (!onlyOnceInGame)
            {
                onlyOnceInGame = true;

                audioSource.clip = bgm[1];
                audioSource.Play();
            }
        }
    }
}
