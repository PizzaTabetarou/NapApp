using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerUIManager : MonoBehaviour
{
    public TMP_InputField hoursInput;
    public TMP_InputField minutesInput;
    public TMP_Text countdownText;
    public Button setButton;
    public Button startButton;
    public Button stopButton;
    public GameObject rawImage;
    public TimerCTRL timerCTRL;
    public MusicPlayer musicPlayer;
    public GameObject inputGroup;
    public GameObject countdownGroup;
    
    private int totalMinutes = 0;
    private bool isRunning = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        countdownGroup.SetActive(false);
        rawImage.SetActive(false);

        hoursInput.text = "0";
        minutesInput.text = "0";

        setButton.onClick.AddListener(SetTimer);
        startButton.onClick.AddListener(OnStartButtonPressed);
        stopButton.onClick.AddListener(OnStopButtonPressed);

        timerCTRL.OnTimerFinished += PlayAlarm;
    }

    void Update()
    {
        float time = timerCTRL.countdownTime;
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        countdownText.text = $"{minutes:D2}分{seconds:D2}秒";
        
        if(time is 0)
        {
            isRunning = false;
        }
    }

    void SetTimer()
    {
        if(int.TryParse(hoursInput.text, out int hours) && int.TryParse(minutesInput.text, out int minutes))
        {
            totalMinutes = hours * 60 + minutes;

            if(totalMinutes > 0)
            {
                timerCTRL.ResetTimer(totalMinutes * 60);
            }

            inputGroup.SetActive(false);
            countdownGroup.SetActive(true);

            countdownText.text = $"{totalMinutes:D2}分";
        }
    }

    private void OnStartButtonPressed()
    {
        timerCTRL.StartTimer();
        rawImage.SetActive(true);
        isRunning = true;
    }

    private void OnStopButtonPressed()
    {
        if(isRunning)
        {
            rawImage.SetActive(false);
            timerCTRL.StopTimer();
            isRunning = false;
        }
        else
        {
            musicPlayer.StopAlarm();
            timerCTRL.ResetTimer(0);
            inputGroup.SetActive(true);
            countdownGroup.SetActive(false);
            rawImage.SetActive(false);
        }
    }

    private void PlayAlarm()
    {
        musicPlayer.PlayAlarm();
    }
}
