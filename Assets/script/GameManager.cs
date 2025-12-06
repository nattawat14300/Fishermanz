using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Temporary Character")]
    public GameObject character;

    private void Awake()
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

    // =========================
    // Restart Scene ปัจจุบัน
    // =========================
    public void Restart()
    {
        Time.timeScale = 1f; // รีเซ็ต TimeScale
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // =========================
    // ไป Wait Screen
    // =========================
    public void WaitScreen()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("WaitScreen");
    }

    // =========================
    // ไป Quiz Scene
    // =========================
    public void Quiz()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Quiz");
    }

    // =========================
    // Character ชั่วคราว
    // =========================
    public GameObject GetCharacter() => character;
    public void SetCharacter(GameObject obj) => character = obj;
}
