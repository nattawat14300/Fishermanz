using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // Singleton ที่ปลอดภัยจริง
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name);

        // ✅ รีเซ็ตเวลาเสมอเมื่อเปลี่ยน Scene
        Time.timeScale = 1f;

        if (scene.name == "Quiz")
        {
            QuizControll quizController = FindFirstObjectByType<QuizControll>();

            if (quizController != null)
            {
                quizController.StartQuiz();
                Debug.Log("✅ Quiz started automatically after scene load.");
            }
            else
            {
                Debug.LogError("❌ QuizController not found in the new scene!");
            }
        }
    }

    // ===============================
    // ✅ RESTART (แก้จุดพังหลัก)
    // ===============================
    public void Restart()
    {
        Debug.Log("🔄 Restart Game");

        Time.timeScale = 1f;

        // ✅ รีโหลด Scene ปัจจุบันแบบชัวร์ ๆ
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void WaitScreen()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("WaitScreen");
    }

    // ===============================
    // ✅ SCENE TRANSITION
    // ===============================
    public void StartSceneTransition(string sceneName, GameObject characterToStore)
    {
        Time.timeScale = 1f;

        if (characterToStore != null)
        {
            StoreCharacterData(characterToStore);
        }
        else
        {
            Debug.LogWarning("Character is null during scene transition. Skipping data storage.");
        }

        SceneManager.LoadScene(sceneName);
    }

    void StoreCharacterData(GameObject character)
    {
        Debug.Log("Storing character data: " + character.name);
        // ขยายระบบ Save เพิ่มได้ภายหลัง
    }

    public void GoToQuizScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Quiz");
    }
}
