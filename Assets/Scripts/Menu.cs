using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    GameManager gameManager;
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Close menu
            if (GetComponent<Canvas>().enabled == true)
            {
                gameManager.freeze = false;
                GetComponent<Canvas>().enabled = false;
            }
            else // Open menu
            {
                gameManager.freeze = true;
                GetComponent<Canvas>().enabled = true;
            }
                
        }
    }
    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    public void Restart()
    {
        FindObjectOfType<GameManager>().Restart();

        // Just incase
        Continue();
    }
    public void Continue()
    {
        gameManager.freeze = false;
        GetComponent<Canvas>().enabled = false;
    }
}
