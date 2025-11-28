using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject gameOverUI;
    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 1. หาและซ่อน UI
        GameObject ui = GameObject.Find("GameOverUI");
        if (ui != null)
        {
            ui.SetActive(false);
            gameOverUI = ui;
        }

        // 2. รีเซ็ตสถานะเริ่มต้น
        isGameOver = false;
        Time.timeScale = 1f; // ให้เวลาเดินต่อ
    }

    private void Update()
    {
        // 🌟 เพิ่มการเช็ค null เพื่อป้องกันเกม Crash หากหา gameOverUI ไม่เจอ
        if (gameOverUI != null)
        {
            if (gameOverUI.activeInHierarchy)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    public void gameOver()
    {
        if (isGameOver) return;

        // ---------------------------------------------------------
        // 🔒 สูตรลับ: การหา GameObject ที่ถูกซ่อน (Inactive)
        // ---------------------------------------------------------
        if (gameOverUI == null)
        {
            GameObject canvas = GameObject.Find("Canvas");

            if (canvas != null)
            {
                Transform uiTransform = canvas.transform.Find("GameOverUI");

                if (uiTransform != null)
                {
                    gameOverUI = uiTransform.gameObject;
                }
            }
        }
        // ---------------------------------------------------------

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
            isGameOver = true;
            Time.timeScale = 0f; // 🛑 หยุดเวลา
        }
        else
        {
            Debug.LogError("ยังหาไม่เจอ! 1.เช็คว่ามี Canvas ไหม 2.เช็คว่า GameOverUI เป็นลูกของ Canvas โดยตรงหรือไม่");
        }
    }


    public void restart()
    {
        // ✅ [Fix] ต้องสั่งให้เวลาเดินต่อทันที
        Time.timeScale = 1f;
        isGameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToWaitToTouch()
    {
        // ✅ [Fix] ต้องสั่งให้เวลาเดินต่อทันที
        Time.timeScale = 1f;
        isGameOver = false;
        SceneManager.LoadScene("QuitToWaitToTouch");
    }
}