using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_GameTimer : MonoBehaviour
{
    [Header("UI Assign")]
    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private float timerValueTest = 0.0f;

    private float timeLeft;
    private bool isRunning = false;

    private void Start() {
        // TEST
        if (timerValueTest > 0) {
            StartTimer(timerValueTest);
        }
        
    }

    private void Update() {
        // TEST
        //if (Input.GetKeyDown(KeyCode.A)) {
        //    StopTimer();
        //}
    }

    /// <summary>
    /// �ܺο��� Ÿ�̸� ȣ���Ͽ� ����
    /// </summary>
    /// <param name="startSeconds">�� ������ �Է�. ex) 5�� * 60 -> 300 </param>
    public void StartTimer(float startSeconds) {
        timeLeft = startSeconds;
        isRunning = true;
        UpdateTimerText(timeLeft);
        StartCoroutine(TimerCoroutine());
    }

    private IEnumerator TimerCoroutine() {
        while (isRunning && timeLeft > 0f) {
            yield return new WaitForSeconds(1.0f);
            timeLeft -= 1f;
            UpdateTimerText(timeLeft);
            if (timeLeft < 0f) timeLeft = 0f;
        }

        if (timeLeft <= 0) {
            TimeGameOver();
        }
    }

    private void UpdateTimerText(float time) {
        int minute = Mathf.FloorToInt(time / 60f);
        int second = Mathf.FloorToInt(time % 60f);
        timerText.text = $"{minute:00}:{second:00}";
    }

    private void TimeGameOver() {
        Debug.Log("UI_GameTimer | TimeGameOver �ð� ����");
        // TODO
        // �ܺη� �޽��� �߻�
    }

    /// <summary>
    /// �ܺο��� ȣ���Ͽ� Ÿ�̸� ����
    /// </summary>
    public void StopTimer() {
        Debug.Log("UI_GameTimer | StopTimer Ÿ�̸� ����");
        isRunning = false;
    }
}
