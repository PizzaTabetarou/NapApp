using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerUIManager : MonoBehaviour
{
    public TMP_InputField hoursInput;   //時間入力フィールド
    public TMP_InputField minutesInput; //分入力フィールド
    public TMP_Text countdownText;      //カウントダウン用テキスト
    public Button setButton;            //タイマーセット用ボタン
    public Button startButton;          //タイマー開始ボタン
    public Button stopButton;           //タイマー停止ボタン
    public GameObject rawImage;         //タイマー開始後暗くするよう
    public TimerCTRL timerCTRL;         //TimerCTRLスクリプト
    public MusicPlayer musicPlayer;     //MusicPlayerスクリプト
    public GameObject inputGroup;       //タイマー設定用のパネル
    public GameObject countdownGroup;   //カウントダウン用のパネル
    
    private int totalMinutes = 0;       //時間(分に直す)
    private bool isRunning = false;     //カウントしているかどうかの真偽値

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;  //android端末が、スリープモードに入らないようにするためのものらしい
        countdownGroup.SetActive(false);
        rawImage.SetActive(false);

        //インプットフィールドに初期値『0』を入れておく
        hoursInput.text = "0";
        minutesInput.text = "0";

        //AddListenerはButtonコンポーネントのメソッド
        //引数として、クリックされたときに実行したいメソッドを指定する
        setButton.onClick.AddListener(SetTimer);
        startButton.onClick.AddListener(OnStartButtonPressed);
        stopButton.onClick.AddListener(OnStopButtonPressed);

        //PlayAlarmをイベントハンドラーとして登録
        timerCTRL.OnTimerFinished += PlayAlarm;
    }

    void Update()
    {
        //残り時間を分と秒で表示
        float time = timerCTRL.countdownTime;
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        //文字列補間により、可読性アップ
        countdownText.text = $"{minutes:D2}分{seconds:D2}秒";
        
        if(time is 0)
        {
            isRunning = false;
        }
    }

    void SetTimer()
    {
        //入力された文字列を整数に変換し、int ○○に代入
        if(int.TryParse(hoursInput.text, out int hours) && int.TryParse(minutesInput.text, out int minutes))
        {
            //入力された時間を全て分に変換する
            totalMinutes = hours * 60 + minutes;

            //timerCTRL.ResetTimerに変換された時間を渡す
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
