using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelController : MonoBehaviour
{
    [SerializeField] private float timeToComplete = 90;
    [SerializeField] private float timeRemaining = 0;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private AudioClip introMusic;
    [SerializeField] private AudioClip levelMusic;
    [SerializeField] private AudioClip winMusic;
    [SerializeField] private AudioClip loseMusic;
    [SerializeField] private GameObject introGameObject;
    [SerializeField] private GameObject levelUI;

    private AudioSource _audioSource;
    private TimeOfDayController _timeOfDayController;
    private bool isLastSeconds = false;

    [Header("EndScreen")]
    [SerializeField] private GameObject endScreen;
    [SerializeField] private TMP_Text endSheepCounter;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _timeOfDayController = GetComponent<TimeOfDayController>();

        _audioSource.clip = introMusic;
        _audioSource.Play();

        StartLevel();
    }

    public void StartLevel()
    {
        timeRemaining = timeToComplete;
        _audioSource.clip = levelMusic;
        _audioSource.Play();

        introGameObject.SetActive(false);
        levelUI.SetActive(true);

        _timeOfDayController.StartDay(timeToComplete);

        DOTween.To(() => timeRemaining, x => timeRemaining = x, 0, timeToComplete).SetEase(Ease.Linear).OnUpdate(() =>
        {
            // Debug.Log(timeRemaining);
            timeText.text = "TIME: " + timeRemaining.ToString("F0");

            if (timeRemaining <= 10 && isLastSeconds == false)
            {
                isLastSeconds = true;
                timeText.DOColor(Color.red, 1).SetLoops(10).SetEase(Ease.OutSine).From();
                timeText.transform.DOShakePosition(10, 10, 10, 90, false,false).SetEase(Ease.InCirc);
            }

            _timeOfDayController.UpdateTime(timeRemaining);
        }).OnComplete(() =>
        {
            isLastSeconds = false;
            timeText.DOKill();
            timeText.transform.DOKill();

            // TODO Check if Lose or Win

            ShowEndScreen(true, 100);
        });
    }

    private void ShowEndScreen(bool win, int nbSheep)
    {
        levelUI.SetActive(false);
        endScreen.SetActive(true);

        // Grab a free Sequence to use
        Sequence mySequence = DOTween.Sequence();
        // Add a movement tween at the beginning
        mySequence.Append(endSheepCounter.DOCounter(0, nbSheep, 5, false));
        // Add a rotation tween as soon as the previous one is finished
        // mySequence.Append(transform.DORotate(new Vector3(0,180,0), 1));
        // // Delay the whole Sequence by 1 second
        // mySequence.PrependInterval(1);
        // // Insert a scale tween for the whole duration of the Sequence
        // mySequence.Insert(0, transform.DOScale(new Vector3(3,3,3), mySequence.Duration()));

        if (win)
        {
            _audioSource.clip = winMusic;
            _audioSource.Play();
        }
        else
        {
            _audioSource.clip = loseMusic;
            _audioSource.Play();
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
