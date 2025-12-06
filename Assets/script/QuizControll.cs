using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizControll : MonoBehaviour
{
    [Header("Result Panels")]
    public GameObject correctPanel;
    public GameObject wrongPanel;

    [Header("UI References")]
    public GameObject quizPanel;
    public Image questionImage;

    [System.Serializable]
    public class Question
    {
        public Sprite questionSprite;
        public char correctAnswer;  // 'A','B','C'
    }

    [Header("Question Data")]
    public Question[] questions;

    [Header("Force Pad")]
    public ForcePadReader pad;
    public float threshold = 50f;

    private int currentQuestionIndex = 0;
    private int score = 0;

    private bool sensorLocked = false;
    private bool waitForRelease = false;
    private bool waitingForShowResult = false;
    private bool waitingForNextQuestion = false;
    private char lastSelectedAnswer;

    private List<char> playerAnswers = new List<char>();

    void Start()
    {
        StartQuiz();
    }

    public void StartQuiz()
    {
        currentQuestionIndex = 0;
        score = 0;
        playerAnswers.Clear();
        sensorLocked = false;
        waitForRelease = false;
        waitingForShowResult = false;
        waitingForNextQuestion = false;

        if (quizPanel != null)
            quizPanel.SetActive(true);
        if (correctPanel != null) correctPanel.SetActive(false);
        if (wrongPanel != null) wrongPanel.SetActive(false);

        ShowQuestion(currentQuestionIndex);
    }

    void Update()
    {
        if (waitingForShowResult)
        {
            // รอให้ปล่อย Sensor / มือก่อน
            if (waitForRelease)
            {
                if (AllSensorsReleased())
                    waitForRelease = false;
                return;
            }

            // รอกดเพื่อแสดง panel ถูก/ผิด
            if (DetectAnyPress())
            {
                ShowAnswerResult();
            }
        }
        else if (waitingForNextQuestion)
        {
            // รอให้ปล่อย Sensor / มือก่อน
            if (waitForRelease)
            {
                if (AllSensorsReleased())
                    waitForRelease = false;
                return;
            }

            // รอกดเพื่อไปข้อถัดไป
            if (DetectAnyPress())
            {
                ContinueAfterResult();
            }
        }
        else
        {
            CheckInput(); // รับคำตอบใหม่
        }
    }

    void ShowQuestion(int index)
    {
        if (index >= questions.Length)
        {
            // ข้อสุดท้ายจบ Quiz
            GoToWaitScreen();
            return;
        }

        Question q = questions[index];
        if (questionImage != null && q.questionSprite != null)
            questionImage.sprite = q.questionSprite;

        Debug.Log($"Showing question {index + 1}");
        sensorLocked = false;
        waitForRelease = false;
    }

    void CheckInput()
    {
        if (sensorLocked) return;

        char selected = '\0';

        // Keyboard
        if (Input.GetKeyDown(KeyCode.A)) selected = 'A';
        else if (Input.GetKeyDown(KeyCode.B)) selected = 'B';
        else if (Input.GetKeyDown(KeyCode.C)) selected = 'C';

        // Sensor
        if (pad != null)
        {
            if (pad.f4 > threshold) selected = 'A';
            else if (pad.f3 > threshold) selected = 'B';
            else if (pad.f5 > threshold) selected = 'C';

            if (pad.f3 <= threshold && pad.f4 <= threshold && pad.f5 <= threshold)
                sensorLocked = false;
        }

        if (selected != '\0')
        {
            lastSelectedAnswer = selected;
            sensorLocked = true;
            waitForRelease = true;
            waitingForShowResult = true;
        }
    }

    void ShowAnswerResult()
    {
        waitingForShowResult = false;
        waitingForNextQuestion = true;
        sensorLocked = true;
        waitForRelease = true;

        playerAnswers.Add(lastSelectedAnswer);

        bool isCorrect = lastSelectedAnswer == questions[currentQuestionIndex].correctAnswer;
        if (isCorrect)
        {
            score++;
            if (correctPanel != null) correctPanel.SetActive(true);
        }
        else
        {
            if (wrongPanel != null) wrongPanel.SetActive(true);
        }

        Debug.Log($"Answer: {lastSelectedAnswer} | Correct: {questions[currentQuestionIndex].correctAnswer}");
    }

    void ContinueAfterResult()
    {
        waitingForNextQuestion = false;
        sensorLocked = false;
        waitForRelease = true;

        if (correctPanel != null) correctPanel.SetActive(false);
        if (wrongPanel != null) wrongPanel.SetActive(false);

        currentQuestionIndex++;

        if (currentQuestionIndex >= questions.Length)
        {
            GoToWaitScreen();
            return;
        }

        ShowQuestion(currentQuestionIndex);
    }

    bool DetectAnyPress()
    {
        if (Input.anyKeyDown) return true;

        if (pad != null)
        {
            if (pad.f3 > threshold || pad.f4 > threshold || pad.f5 > threshold) return true;
        }

        return false;
    }

    bool AllSensorsReleased()
    {
        if (pad == null) return true;
        return pad.f3 <= threshold && pad.f4 <= threshold && pad.f5 <= threshold;
    }

    void GoToWaitScreen()
    {
        Debug.Log($"Final Score: {score}/{questions.Length}");
        UnityEngine.SceneManagement.SceneManager.LoadScene("WaitScreen");
    }
}
