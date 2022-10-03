using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public PlayerController playerController;
    public LevelScripts levelScripts;

    private int m_score = 0;
    [SerializeField] private TextMeshProUGUI m_scoreTMP;

    private bool m_gameActive;

    private void Start()
    {
        SetGameActive(false);
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void SetGameActive(bool active)
    {
        m_gameActive = active;
        if (active)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
        }
    }

    public void AddScore(int amount)
    {
        m_score += amount;
        UpdateScoreToUI();
    }

    public void UpdateScoreToUI()
    {
        m_scoreTMP.text = "Score: " + m_score;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GetVariables()
    {
        LevelScripts levelHelper = playerController.GetComponent<LevelScripts>();
        m_scoreTMP = playerController.scoreText.GetComponent<TextMeshProUGUI>();
    }
}
