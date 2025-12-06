using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Temporary placeholder for Character/GameObject
    public GameObject character;

    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable()
    {
        // ป้องกัน subscribe ซ้ำ
        SceneManager.sceneLoaded -= OnSceneLoaded;
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
        StartCoroutine(LoadSceneAsync(SceneManager.GetActiveScene().buildIndex));
    }

    // ============================
    // ไป Wait Screen
    // ============================
    public void WaitScreen()
    {
        StartCoroutine(LoadSceneAsync("WaitScreen"));
    }

    // ============================
    // ไป Quiz Scene
    // ============================
    public void Quiz()
    {
        StartCoroutine(LoadSceneAsync("Quiz"));
    }

    // ============================
    // Coroutine โหลด Scene Async
    // ============================
    private IEnumerator LoadSceneAsync(object scene)
    {
        // scene อาจเป็น int หรือ string
        AsyncOperation op = null;
        if (scene is int)
            op = SceneManager.LoadSceneAsync((int)scene);
        else if (scene is string)
            op = SceneManager.LoadSceneAsync((string)scene);

        if (op == null)
            yield break;

        // รอโหลดจบ
        while (!op.isDone)
        {
            yield return null;
        }

        yield return null;
    }

    // ============================
    // Character ชั่วคราว
    // ============================
    
}
