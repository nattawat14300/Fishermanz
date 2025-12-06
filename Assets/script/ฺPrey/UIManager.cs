using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    private static HashSet<string> displayedFishNames = new HashSet<string>();

    [Header("Sensor For Next")]
    public ForcePadReader pad;
    public float threshold = 300f;

    [Header("Delay Before Input")]
    public float inputDelay = 3f;   // ✅ เวลาหน่วงก่อนกดได้ (วินาที)

    private bool allowInput = false;
    private bool sensorConsumed = false;

    [System.Serializable]
    public struct FishUIPanel
    {
        public string fishName;
        public GameObject infoPanelObject;
    }

    [Header("Fish UI Database")]
    public FishUIPanel[] fishPanels;

    private Coroutine hideInfoCoroutine;
    private GameObject currentActivePanel = null;

    void Start()
    {
        foreach (FishUIPanel panelProfile in fishPanels)
        {
            if (panelProfile.infoPanelObject != null)
                panelProfile.infoPanelObject.SetActive(false);
        }

        Time.timeScale = 1f;
    }

    public void DisplayFishInfoByName(string targetFishName)
    {
        sensorConsumed = false;
        allowInput = false;

        if (displayedFishNames.Contains(targetFishName))
            return;

        if (currentActivePanel != null)
        {
            currentActivePanel.SetActive(false);
            if (hideInfoCoroutine != null)
                StopCoroutine(hideInfoCoroutine);
        }

        foreach (FishUIPanel panelProfile in fishPanels)
        {
            if (panelProfile.fishName.Equals(targetFishName, System.StringComparison.OrdinalIgnoreCase))
            {
                if (panelProfile.infoPanelObject != null)
                {
                    panelProfile.infoPanelObject.SetActive(true);
                    currentActivePanel = panelProfile.infoPanelObject;

                    Time.timeScale = 0f;

                    StartCoroutine(EnableInputAfterDelay());

                    hideInfoCoroutine = StartCoroutine(WaitForKeyPressToHide());

                    displayedFishNames.Add(targetFishName);
                    return;
                }
            }
        }

        Debug.LogWarning($"UI Panel for '{targetFishName}' not found in UIManager database.");
    }

    // ✅ หน่วงก่อนให้กดได้
    IEnumerator EnableInputAfterDelay()
    {
        yield return new WaitForSecondsRealtime(inputDelay);
        allowInput = true;
        Debug.Log("Info Panel: Input Enabled");
    }

    IEnumerator WaitForKeyPressToHide()
    {
        while (true)
        {
            yield return null;

            if (!allowInput) continue;

            bool anyKey = Input.anyKeyDown;
            bool anySensor = IsAnySensorPressed();

            if (anySensor && sensorConsumed)
                continue;

            if (anyKey || anySensor)
            {
                HideUIAndResumeGame();
                yield break;
            }

            if (!anySensor)
                sensorConsumed = false;
        }
    }

    public void HideUIAndResumeGame()
    {
        if (currentActivePanel != null)
        {
            currentActivePanel.SetActive(false);
            currentActivePanel = null;
        }

        Time.timeScale = 1f;

        if (hideInfoCoroutine != null)
        {
            StopCoroutine(hideInfoCoroutine);
            hideInfoCoroutine = null;
        }

        allowInput = false;
        sensorConsumed = false;
    }

    bool IsAnySensorPressed()
    {
        if (pad == null) return false;

        return pad.f1 > threshold ||
               pad.f2 > threshold ||
               pad.f3 > threshold ||
               pad.f4 > threshold ||
               pad.f5 > threshold;
    }
}
