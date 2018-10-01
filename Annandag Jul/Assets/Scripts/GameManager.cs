using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{

    private static GameManager instance = null;

    public int iCameFromThisLevel;
    public int difficulty;
    private int currentScene;

    public static GameManager Instance
    {

        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("Game Manager");
                go.AddComponent<GameManager>();
            }
            return instance;
        }

    }

    public bool IsDead { get; set; }


    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

    }

    void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        IsDead = false;
    }

    public void ChangeLevelFromMenu()
    {
        iCameFromThisLevel = 0;
    }

    public void changeScene(int sceneIndex)
    {
        currentScene = sceneIndex;
        SceneManager.LoadScene(sceneIndex);
    }


    public void SetDifficulty(int diff)
    {
        difficulty = diff;
    }
}
