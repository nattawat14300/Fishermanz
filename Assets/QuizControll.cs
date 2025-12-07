using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuizControll : MonoBehaviour
{
    [Header("Quiz UI")]
    public GameObject quizPanel;
    public Image questionImage;

    [Header("Result Panels")]
    public GameObject panelTrue;
    public GameObject panelFalse;

    [System.Serializable]
    public class Question
    {
        public Sprite questionSprite;
        public char correctAnswer; // A B C
    }

    public Question[] questions;

    [Header("Force Sensor")]
    public ForcePadReader pad;
    public float threshold = 50f;

    private int currentQuestionIndex = 0;
    private int score = 0;

    private bool quizActive = false;
    private bool waitingForNext = false;
    private bool sensorLocked = false;

    // =========================
    // Start
    // =========================
    void Start()
    {
        quizPanel.SetActive(true);
        panelTrue.SetActive(false);
        panelFalse.SetActive(false);

        StartQuiz();
    }

    // =========================
    // PUBLIC: Allow GameManager Call
    // =========================
    public void StartQuiz()
    {
        currentQuestionIndex = 0;
        score = 0;
        quizActive = true;
        waitingForNext = false;
        sensorLocked = false;
        ShowQuestion();
    }

    // =========================
    // Show Question
    // =========================
    void ShowQuestion()
    {
        if (currentQuestionIndex >= questions.Length)
        {
            ReturnToFirstScene();
            return;
        }

        questionImage.sprite = questions[currentQuestionIndex].questionSprite;
        quizActive = true;
        waitingForNext = false;
        sensorLocked = false;
    }

    // =========================
    // Update Loop
    // =========================
    void Update()
    {
        if (quizActive)
            CheckAnswerInput();

        if (waitingForNext)
            CheckNextInput();
    }

    // =========================
    // Answer Mapping
    // f4 = A
    // f3 = B
    // f5 = C
    // =========================
    void CheckAnswerInput()
    {
        char selected = '\0';

        if (pad != null)
        {
            if (pad.f4 > threshold) selected = 'A';
            else if (pad.f3 > threshold) selected = 'B';
            else if (pad.f5 > threshold) selected = 'C';

          
        }

        if (selected != '\0' && !sensorLocked)
        {
            sensorLocked = true;
            ProcessAnswer(selected);
        }
    }

    // =========================
    // Process Answer
    // =========================
    void ProcessAnswer(char selected)
    {
        quizActive = false;

        bool correct = selected == questions[currentQuestionIndex].correctAnswer;

        if (correct)
        {
            score++;
            panelTrue.SetActive(true);
            panelFalse.SetActive(false);
        }
        else
        {
            panelTrue.SetActive(false);
            panelFalse.SetActive(true);
        }

        sensorLocked = true;
        waitingForNext = true;
    }

    // =========================
    // Press Any Pad → Next
    // =========================
    void CheckNextInput()
    {
        if (pad == null) return;

        bool pressed =
            pad.f1 > threshold ||
            pad.f2 > threshold ||
            pad.f3 > threshold ||
            pad.f4 > threshold ||
            pad.f5 > threshold;

        if (pressed && !sensorLocked)
        {
            sensorLocked = true;
            NextStep();
        }

        if (!pressed)
            sensorLocked = false;
    }

    // =========================
    // Next Question or End
    // =========================
    void NextStep()
    {
        panelTrue.SetActive(false);
        panelFalse.SetActive(false);

        currentQuestionIndex++;
        ShowQuestion();
    }

    // =========================
    // Return Scene
    // =========================
    void ReturnToFirstScene()
    {
        SceneManager.LoadScene(0);
    }
}
