using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // ============================
    // Temporary placeholder for Character/GameObject
    // ============================
    public GameObject character;

    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Quiz")
        {
            QuizControll quizController = FindFirstObjectByType<QuizControll>();

            if (quizController != null)
            {
                quizController.StartQuiz();
                Debug.Log("Quiz started automatically after scene load.");
            }
            else
            {
                Debug.LogError("QuizController not found in the new scene!");
            }
        }
    }

    // ============================
    // Restart Scene ปัจจุบัน
    // ============================
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // ============================
    // ไป Wait Screen
    // ============================
    public void WaitScreen()
    {
        SceneManager.LoadScene("WaitScreen");
    }

    // ============================
    // Character ชั่วคราว
    // ============================
    public GameObject GetCharacter() => character;

    public void SetCharacter(GameObject obj) => character = obj;

    public void Quiz()
    {
        SceneManager.LoadScene("Quiz");
    }
}
