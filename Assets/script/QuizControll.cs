using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizControll : MonoBehaviour
{
    [Header("UI References")]
    public GameObject quizPanel;
    public Image questionImage;             // Image แสดงรูปคำถาม

    [System.Serializable]
    public class Question
    {
        public Sprite questionSprite;
        public char correctAnswer;  // คำตอบที่ถูกเป็นตัวอักษร 'A','B','C'
    }

    [Header("Question Data")]
    public Question[] questions;

    private int currentQuestionIndex = 0;
    private int score = 0;
    private bool quizActive = false;

    // บันทึกคำตอบของผู้เล่น
    private List<char> playerAnswers = new List<char>();

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

    void CheckInput()
    {
        if (!quizActive) return;

        char selected = '\0';

        if (Input.GetKeyDown(KeyCode.A)) selected = 'A';
        else if (Input.GetKeyDown(KeyCode.B)) selected = 'B';
        else if (Input.GetKeyDown(KeyCode.C)) selected = 'C';

        if (selected != '\0')
            ProcessAnswer(selected);
    }

    void ProcessAnswer(char selectedChar)
    {
        quizActive = false;

        // บันทึกคำตอบผู้เล่น
        playerAnswers.Add(selectedChar);

        // ตรวจสอบคำตอบถูก
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

        // แสดงคำตอบผู้เล่นและคำตอบที่ถูกต้อง
        for (int i = 0; i < playerAnswers.Count; i++)
        {
            char playerAnswer = playerAnswers[i];
            char correctAnswer = questions[i].correctAnswer;
            Debug.Log($"Q{i + 1}: Player chose {playerAnswer}, Correct answer {correctAnswer}");
        }

        quizActive = false;
    }

    void Update()
    {
        CheckInput();
    }
}
