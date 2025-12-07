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
    private bool waitingToHidePanel = false;


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
    

    private bool quizActive = false;
    private bool waitingForNext = false;
    private bool sensorLocked = false;
    private bool waitForRelease = false;   // ✅ เพิ่ม

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
     
        quizActive = true;
        waitingForNext = false;
        sensorLocked = false;
        waitForRelease = false;
        ShowQuestion();
    }

    // =========================
    // Show Question
    // =========================
    void ShowQuestion()
    {
        panelTrue.SetActive(false);
        panelFalse.SetActive(false);

        if (currentQuestionIndex >= questions.Length)
        {
            ReturnToFirstScene();
            return;
        }

        questionImage.sprite = questions[currentQuestionIndex].questionSprite;

        quizActive = true;
        waitingForNext = false;
        waitingToHidePanel = false;
        sensorLocked = true;    // ป้องกัน double input frame แรก
        waitForRelease = true; // ต้องปล่อยก่อนกด
    }

    // =========================
    // Update Loop
    // =========================
    void Update()
    {

        CheckRelease();   // ✅ ให้ทำตลอด

        if (quizActive && !sensorLocked)
            CheckAnswerInput();

        if ((waitingToHidePanel || waitingForNext) && !sensorLocked)
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
            waitForRelease = true;
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
            panelTrue.SetActive(true);
            panelFalse.SetActive(false);
        }
        else
        {
            panelTrue.SetActive(false);
            panelFalse.SetActive(true);
        }

        waitingToHidePanel = true;   
    waitingForNext = false;
        sensorLocked = true;
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

        if (!pressed)
        {
            sensorLocked = false;
            return;
        }

        if (sensorLocked) return;

        sensorLocked = true;

        // ✅ STEP 1: ยังไม่ปิด Panel → ปิดก่อน
        if (waitingToHidePanel)
        {
            HideResultPanel();
            return;
        }

        // ✅ STEP 2: ปิดแล้ว → ไปข้อถัดไป
        if (waitingForNext)
        {
            NextQuestion();
        }
    }
    void HideResultPanel()
    {
        panelTrue.SetActive(false);
        panelFalse.SetActive(false);

        waitingToHidePanel = false;
        waitingForNext = true;
    }

    void NextQuestion()
    {
        // ✅ ปิด Panel ทันที (ก่อนเปลี่ยนคำถาม)
        panelTrue.SetActive(false);
        panelFalse.SetActive(false);

        waitingForNext = false;
        waitingToHidePanel = false;
        quizActive = true;

        currentQuestionIndex++;
        ShowQuestion();  // ✅ แสดง Q ใหม่หลัง Panel หายชัวร์
    }

    // =========================
    // Wait until sensor released
    // =========================
    void CheckRelease()
    {
        if (!IsAnySensorPressed())
        {
            waitForRelease = false;
            sensorLocked = false;
        }
    }

    // =========================
    // Check Any Sensor
    // =========================
    bool IsAnySensorPressed()
    {
        return
            pad != null &&
            (pad.f1 > threshold ||
             pad.f2 > threshold ||
             pad.f3 > threshold ||
             pad.f4 > threshold ||
             pad.f5 > threshold);
    }

    // =========================
    // Next Question or End
    // =========================
 


    // =========================
    // Return Scene
    // =========================
    void ReturnToFirstScene()
    {
        SceneManager.LoadScene(0);
    }
}
