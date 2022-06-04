using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject minionPrefab;
    public GameObject pauseMenu;
    public bool isWaitingForMinion = false;
    public float minionWaitTime = 5f;
    public float arenaRadius = 120f;
    public static bool isPaused = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        if (isWaitingForMinion) return;
        StartCoroutine(DeployMinion());
    }

    IEnumerator DeployMinion()
    {
        // Put one minion on a random place
        float angle = Random.Range(0, 1000) * Mathf.PI * 2 / 1000f;
        float range = Random.Range(0, 1000) / 1000f;
        float x = Mathf.Cos(angle) * arenaRadius * range;
        float z = Mathf.Sin(angle) * arenaRadius * range;
        Vector3 deployPosition = new Vector3(x, -2, z);
        Instantiate(minionPrefab, deployPosition, Quaternion.identity);
        isWaitingForMinion = true;
        yield return new WaitForSeconds(minionWaitTime);
        isWaitingForMinion = false;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        isPaused = true;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        isPaused = false;
    }
}
