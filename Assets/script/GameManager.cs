using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void WaitScreen()
    {
        SceneManager.LoadScene("WaitScreen");
    }

    // ✅ แก้ Character เป็น GameObject ชั่วคราว
    public void StartSceneTransition(string sceneName, GameObject characterToStore)
    {
        if (characterToStore != null)
        {
            StoreCharacterData(characterToStore);
        }
        else
        {
            Debug.LogWarning("Character is null during scene transition. Skipping data storage.");
        }

        // ✅ ใช้ SceneManager แทน SceneLoader
        SceneManager.LoadScene(sceneName);
    }

    // ✅ เพิ่มเมธอดนี้เพื่อไม่ให้ Error
    void StoreCharacterData(GameObject character)
    {
        Debug.Log("Storing character data: " + character.name);
        // คุณสามารถเพิ่มระบบ Save จริงตรงนี้ภายหลังได้
    }

    public void GoToQuizScene()
    {
        //Time.timeScale = 1f; // กรณีเกมถูกหยุดไว้
        SceneManager.LoadScene("Quiz");
    }
}