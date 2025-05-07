using KTA.Test;
using TMPro;
using UnityEngine;

public class TESTTimerUI : MonoBehaviour
{
    [Header("UI Assign")]
    [SerializeField] private TextMeshProUGUI timerText;
    
    private void Update()
    {
        if (TESTGamePlayManager.Instance)
        {
            float time = TESTGamePlayManager.Instance.remainingTime;
        
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);

            string timeText = string.Format("{0:00}:{1:00}", minutes, seconds);
        
            timerText.text = timeText;

        }
    }
}
