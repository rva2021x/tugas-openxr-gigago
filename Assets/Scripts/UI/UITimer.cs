using UnityEngine;
using UnityEngine.UI;

public class UITimer : MonoBehaviour
{
    [SerializeField] private Text TimerText;
    private bool playing;
    private float Timer;

    private void Start()
    {
        Timer = 0;
        playing = true;
    }

    void Update()
    {

        if (playing == true)
        {

            Timer += Time.deltaTime;
            int minutes = Mathf.FloorToInt(Timer / 60F);
            int seconds = Mathf.FloorToInt(Timer % 60F);
            int milliseconds = Mathf.FloorToInt((Timer * 100F) % 100F);
            TimerText.text = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + milliseconds.ToString("00");
        }

    }

    public void Pause()
    {
        playing = false;
    }

    public void ResetTimer()
    {
        Timer = 0;
    }

    public void Stop()
    {
        Pause();
        ResetTimer();
    }

    public void StartTimer()
    {
        playing = true;
    }

    public float GetTimer(){
        return Timer;
    }
}