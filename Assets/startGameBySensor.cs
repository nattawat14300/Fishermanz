using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameByAnySensor : MonoBehaviour
{
    public ForcePadReader pad;
    public float threshold = 300f;
    public string gameplaySceneName = "GamePlay Orca"; // 👈 ใส่ชื่อ Scene ของคุณตรงนี้

    private bool started = false;

    void Update()
    {
        if (started || pad == null) return;

        // ✅ ถ้าแตะ sensor ตัวใดก็ตาม
        if ((pad.f1 > threshold || pad.f2 > threshold) ||(pad.f3 > threshold || pad.f4 > threshold))
        {
            StartGame();
        }
    }

    void StartGame()
    {
        started = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameplaySceneName);
    }
}
