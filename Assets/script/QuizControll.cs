using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizControll : MonoBehaviour
{
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
    public ForcePadReader pad;          // ตัวอ่านเซ็นเซอร์
    public float threshold = 50f;       // ค่าที่ถือว่า "กดแล้ว"

    private int currentQuestionIndex = 0;
    private int score = 0;
    private bool quizActive = false;

    private List<char> playerAnswers = new List<char>();
    private bool sensorLocked = false; // กันเด้ง

    void Start()
    {
        if (quizPanel != null)
            quizPanel.SetActive(true);

        StartQuiz();
    }

    public void StartQuiz()
    {
        currentQuestionIndex = 0;
        score = 0;
        playerAnswers.Clear();
        quizActive = true;

        ShowQuestion(currentQuestionIndex);
    }

    void ShowQuestion(int index)
    {
        if (index >= questions.Length)
        {
            ShowResults();
            return;
        }

        Question q = questions[index];

        if (questionImage != null && q.questionSprite != null)
            questionImage.sprite = q.questionSprite;

        Debug.Log($"Showing question {index + 1}");
    }

    void Update()
    {
        if (!quizActive) return;

        CheckInput();
    }

    void CheckInput()
    {
        char selected = '\0';

        // -------------------------
        // 1. รับค่าจากคีย์บอร์ด
        // -------------------------
        if (Input.GetKeyDown(KeyCode.A)) selected = 'A';
        else if (Input.GetKeyDown(KeyCode.B)) selected = 'B';
        else if (Input.GetKeyDown(KeyCode.C)) selected = 'C';

        // -------------------------
        // 2. รับค่าจาก ForcePad
        // -------------------------
        if (pad != null)
        {
            if (pad.f3 > threshold) selected = 'A';
            else if (pad.f4 > threshold) selected = 'B';
            else if (pad.f5 > threshold) selected = 'C';

            // ปล่อยล็อกเมื่อไม่มีแรงกด
            if (pad.f3 <= threshold && pad.f4 <= threshold && pad.f5 <= threshold)
                sensorLocked = false;
        }

        // -------------------------
        // Process answer
        // -------------------------
        if (selected != '\0' && !sensorLocked)
        {
            sensorLocked = true;
            ProcessAnswer(selected);
        }
    }

    void ProcessAnswer(char selectedChar)
    {
        quizActive = false;
        playerAnswers.Add(selectedChar);

        if (selectedChar == questions[currentQuestionIndex].correctAnswer)
        {
            score++;
            Debug.Log($"Q{currentQuestionIndex + 1}: Correct!");
        }
        else
        {
            Debug.Log($"Q{currentQuestionIndex + 1}: Incorrect. Correct answer: {questions[currentQuestionIndex].correctAnswer}");
        }

        currentQuestionIndex++;
        Invoke(nameof(ContinueQuiz), 0.3f);
    }

    void ContinueQuiz()
    {
        if (currentQuestionIndex >= questions.Length)
        {
            ShowResults();
            return;
        }

        quizActive = true;
        ShowQuestion(currentQuestionIndex);
    }

    void ShowResults()
    {
        Debug.Log($"Final Score: {score}/{questions.Length}");

        for (int i = 0; i < playerAnswers.Count; i++)
        {
            char playerAnswer = playerAnswers[i];
            char correctAnswer = questions[i].correctAnswer;
            Debug.Log($"Q{i + 1}: Player chose {playerAnswer}, Correct answer {correctAnswer}");
        }

        quizActive = false;
    }
}
