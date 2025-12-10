using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class QuizControll : MonoBehaviour
{
    [Header("Quiz UI")]
    public GameObject quizPanel;
    public Image questionImage;

    [Header("Result Panels")]
    public GameObject panelTrue;
    public GameObject panelFalse;

    [Header("End Settings")]
    public string nextSceneName = "WaitScreen";   // ซีนถัดไปเมื่อจบ Quiz
    public float answerShowTime = 2f;              // เวลาค้าง panel ถูก/ผิด
    public float endShowTime = 5f;                 // เวลาที่ค้าง panel สุดท้าย

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
    private bool sensorLocked = false;


    void Start()
    {
        quizPanel.SetActive(true);
        panelTrue.SetActive(false);
        panelFalse.SetActive(false);

        StartQuiz();
    }

    public void StartQuiz()
    {
        currentQuestionIndex = 0;
        ShowQuestion();
    }

    void ShowQuestion()
    {
        if (currentQuestionIndex >= questions.Length)
        {
            sensorLocked = true;      // ล็อก input ทันที
            Debug.Log("Quiz Finished → Going to next scene");
            StartCoroutine(EndAndLoadScene());
            return;
        }

        questionImage.sprite = questions[currentQuestionIndex].questionSprite;
        sensorLocked = true; // กันปุ่มเด้ง
    }


    void Update()
    {
        // ถ้าเกินจำนวนข้อแล้ว ไม่ต้องรับ input อีก
        if (currentQuestionIndex >= questions.Length)
            return;

        CheckSensorRelease();

        if (!sensorLocked)
            CheckAnswerInput();
    }


    void CheckAnswerInput()
    {
        char selected = '\0';

        if (pad != null)
        {
            if (pad.f4 > threshold) selected = 'A';
            else if (pad.f3 > threshold) selected = 'B';
            else if (pad.f5 > threshold) selected = 'C';
        }

        if (selected != '\0')
        {
            sensorLocked = true;
            ProcessAnswer(selected);
        }
    }

    void ProcessAnswer(char selected)
    {
        if (currentQuestionIndex < 0 || currentQuestionIndex >= questions.Length)
            return; // กัน index หลุด

        bool correct = selected == questions[currentQuestionIndex].correctAnswer;

        panelTrue.SetActive(correct);
        panelFalse.SetActive(!correct);

        StartCoroutine(DelayNextQuestion());
    }


    IEnumerator DelayNextQuestion()
    {
        // ค้าง panel 2 วิ
        yield return new WaitForSeconds(answerShowTime);

        // ปิด panel
        panelTrue.SetActive(false);
        panelFalse.SetActive(false);

        // ข้อต่อไป
        currentQuestionIndex++;

        // แสดงคำถามใหม่
        ShowQuestion();
    }

    IEnumerator EndAndLoadScene()
    {
        // ค้าง panel 5 วิ (แสดง panel True/False ล่าสุด)
        yield return new WaitForSeconds(endShowTime);

        // โหลดซีนใหม่
        SceneManager.LoadScene(nextSceneName);
    }

    void CheckSensorRelease()
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
        }
    }
}
