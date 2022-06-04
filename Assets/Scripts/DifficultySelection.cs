using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DifficultySelection : MonoBehaviour
{
    public static float difficultyMultiplier = 1;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public void setEasy()
    {
        difficultyMultiplier = 1;
    }

    public void setNormal()
    {
        difficultyMultiplier = 1.4f;
    }

    public void setHard()
    {
        difficultyMultiplier = 2f;
    }
}
