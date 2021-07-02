using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;
    public Button level4Button;
    public Button level5Button;
    public GameObject panel;

    private Button[] buttons;

    public Button howToPlayButton;

    private int lastLevel;
    private LevelDataManager dataManager;

    private void Start()
    {
        CloseHowToPlay();
        Cursor.lockState = CursorLockMode.None;

        buttons = new Button[] { level1Button, level2Button, level3Button, level4Button, level5Button };
        level1Button.onClick.AddListener(() => { PlayLevel(GetSceneName(1)); });
        level2Button.onClick.AddListener(() => { PlayLevel(GetSceneName(2)); });
        level3Button.onClick.AddListener(() => { PlayLevel(GetSceneName(3)); });
        level4Button.onClick.AddListener(() => { PlayLevel(GetSceneName(4)); });
        level5Button.onClick.AddListener(() => { PlayLevel(GetSceneName(5)); });

        howToPlayButton.onClick.AddListener(() => { panel.SetActive(true); });

        StartCoroutine(GetLevelData());

    }

    string GetSceneName(int level)
    {
        return "Level" + level + "Scene";
    }

    IEnumerator GetLevelData()
    {
        yield return new WaitUntil(() => dataManager != null);
        lastLevel = dataManager.GetLastLevel();
        EnumerateButtons();
    }

    void EnumerateButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            Button button = buttons[i];
            if (i > lastLevel)
            {
                DisableButton(button);
            }
        }
    }

    private void Update()
    {
        if (!dataManager)
        {
            dataManager = FindObjectOfType<LevelDataManager>();
        }
    }

    void DisableButton(Button button)
    {
        button.interactable = false;
        TextMeshProUGUI buttonText = button.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.color = Color.gray;
    }

    public void PlayLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void CloseHowToPlay()
    {
        panel.SetActive(false);
    }

    void Destroy()
    {
        foreach (Button button in buttons)
        {
            button.onClick.RemoveAllListeners();
        }
    }
}
