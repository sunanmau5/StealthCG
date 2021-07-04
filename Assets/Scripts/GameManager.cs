using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//  Author: Sunan Regi Maunakea & Louis Sutopo
//  Video tutorial: https://www.youtube.com/watch?v=jUdx_Nj4Xk0
//  Changes made on saving the level progress and when player falls from the plattform.
public class GameManager : MonoBehaviour
{
    public GameObject gameLoseUI;
    public GameObject gameWinUI;

    [SerializeField]
    private int level;

    bool gameIsOver;

    // Start is called before the first frame update
    void Start()
    {
        GuardController.OnGuardHasSpottedPlayer += ShowGameLoseUI;
        FindObjectOfType<PlayerMovement>().OnPlayerFall += ShowGameLoseUI;
        FindObjectOfType<PlayerMovement>().OnReachedEndOfLevel += ShowGameWinUI;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    // Show Game Win UI when the game is won
    void ShowGameWinUI()
    {
        FindObjectOfType<LevelDataManager>().SetLastLevel(level);
        OnGameOver(gameWinUI);
    }

    // Show Game Lose UI when the game is lost
    void ShowGameLoseUI()
    {
        OnGameOver(gameLoseUI);
    }

    // Set the UI object to active depending whether the player win or lose the game
    void OnGameOver(GameObject gameOverUI)
    {
        gameOverUI.SetActive(true);

        gameIsOver = true;
        GuardController.OnGuardHasSpottedPlayer -= ShowGameLoseUI;
        FindObjectOfType<PlayerMovement>().OnPlayerFall -= ShowGameLoseUI;
        FindObjectOfType<PlayerMovement>().OnReachedEndOfLevel -= ShowGameWinUI;
    }
}
