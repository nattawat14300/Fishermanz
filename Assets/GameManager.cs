using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private bool isLoadingScene = false;

    // ===============================
    //           AWAKE
    // ===============================
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ===============================
    //        SCENE EVENT (SAFE)
    // ===============================
    private void OnEnable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // ✅ กันซ้อน
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("✅ Scene Loaded: " + scene.name);

        Time.timeScale = 1f;
        isLoadingScene = false; // ✅ reset ทุกครั้งที่โหลด scene เสร็จ

        if (scene.name == "Quiz")
        {
            QuizControll quiz = FindFirstObjectByType<QuizControll>();
            if (quiz != null)
            {
                quiz.StartQuiz();
            }
            else
            {
                Debug.LogError("❌ QuizControll not found");
            }
        }
    }

    // ===============================
    //           RESTART
    // ===============================
    public void Restart()
    {
        if (isLoadingScene) return;

        Debug.Log("🔄 Restart Game");

        isLoadingScene = true;
        Time.timeScale = 1f;

        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);
    }

    // ===============================
    //           QUIZ
    // ===============================
    public void GoToQuizScene()
    {
        if (isLoadingScene) return; // ✅ กดได้ครั้งเดียว

        Debug.Log("➡️ Go To Quiz Scene");

        isLoadingScene = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene("Quiz");
    }

    // ===============================
    //         WAIT SCREEN
    // ===============================
    public void WaitScreen()
    {
        if (isLoadingScene) return;

        isLoadingScene = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene("WaitScreen");
    }

    // ===============================
    //      GENERIC TRANSITION
    // ===============================
    public void StartSceneTransition(string sceneName, GameObject characterToStore = null)
    {
        if (isLoadingScene) return;

        isLoadingScene = true;
        Time.timeScale = 1f;

        if (characterToStore != null)
        {
            Debug.Log("Storing character data: " + characterToStore.name);
        }

        SceneManager.LoadScene(sceneName);
    }
}
