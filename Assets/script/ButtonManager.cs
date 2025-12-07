using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public void OnRestartClick()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Restart();
        }
        else
        {
            Debug.LogError("❌ GameManager Instance not found!");
        }
    }

    // ✅ ปุ่มไปหน้า Quiz
    public void OnGoToQuizClick()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GoToQuizScene();
        }
        else
        {
            Debug.LogError("❌ GameManager Instance not found!");
        }
    }

    // ✅ ปุ่มกลับหน้า WaitScreen (ถ้ามี)
    public void OnGoToWaitScreen()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.WaitScreen();
        }
        else
        {
            Debug.LogError("❌ GameManager Instance not found!");
        }
    }

    public void QuizScreen()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GoToQuizScene();
        }
        else
        {
            Debug.LogError("❌ GameManager Instance not found!");
        }
    }
}
