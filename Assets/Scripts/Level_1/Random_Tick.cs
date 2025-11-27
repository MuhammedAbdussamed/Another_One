using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Random_Tick : MonoBehaviour
{

    [Header("Scripts")]
    [SerializeField] private InputManager inputManager;

    [Header("Canvas")]
    [SerializeField] private Canvas tapCanvas;
    [SerializeField] private Canvas failedCanvas;
    [SerializeField] private Canvas completeCanvas;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private UnityEngine.UI.Image scoreBackGround;
    [SerializeField] private TextMeshProUGUI bestScoreText;

    // Variables
    private float randomNumber;
    private float countDown;
    private float maxCountDown;
    private float minCountDown;
    private float score;
    private BackGroundColor backGroundColor;
    [SerializeField] private float bestScore;

    // Bools
    private bool isSelected;
    private bool isOpened;
    private bool isCoroutineStarted;
    private bool isCompleted;
    private bool isFinish;


    void Start()
    {
        maxCountDown = 4.5f;
        minCountDown = 0f;
        bestScore = 4f;
    }

    void Update()
    {
        ChangeBackGroundColor();

        SetValue();

        if(isFinish){return;}

        countDown = Mathf.Clamp(countDown,minCountDown,maxCountDown);

        if (!isSelected)
        {
            SetRandomNumber();
        }
        else if (isSelected && !isCoroutineStarted)
        {
            StartCoroutine(WaitAndOpen());
        }
        
        if (isOpened)
        {
            WaitAndClose();
        }
    }

    #region Functions

    void SetRandomNumber()
    {
        randomNumber = Random.Range(2f,5f);
        isSelected = true;
    }

    /*-------------------------------------*/

    IEnumerator WaitAndOpen()
    {
        if (isSelected && !isOpened)
        {
            isCoroutineStarted = true;
            yield return new WaitForSeconds(randomNumber);
            isOpened = true;
            isCoroutineStarted = false;
        }
    }

    /*-------------------------------------*/

    void WaitAndClose()
    {
        countDown += Time.deltaTime;

        if(countDown >= maxCountDown - 0.5f)
        {
            failedCanvas.gameObject.SetActive(true);
            isOpened = false;
        }
    }

    /*---------------------------------------------*/

    public void Tap()
    {
        if (!isCompleted && isOpened)
        {
            ResetAll();  
            score = countDown;
            countDown = 0f;
            isCompleted = true;
            isFinish = true;
            SetBestScore();
        }
        else if (isCompleted)
        {
            isCompleted = false;
            isFinish = false;
        }
    }

    /*---------------------------------------------*/

    void SetValue()
    {
        tapCanvas.gameObject.SetActive(isOpened);
        completeCanvas.gameObject.SetActive(isCompleted);

        ScoreText.text = score.ToString("F2") + " saniye ";
        
    }

    /*-------------------------------------*/

    public void ResetAll()
    {
        isSelected = false;
        isOpened = false;
        isCoroutineStarted = false;
    }

    /*-------------------------------------*/

    enum BackGroundColor
    {
        Yellow,
        Green,
        Blue,
        Red
    }

    /*------------------------------------*/

    void ChangeBackGroundColor()        // 0-0.25 orange // 0.25-0.50 Green // 0.50-2 Blue // 2-4 Red
    {
        if (score <= 0.25f)
        {
            backGroundColor = BackGroundColor.Yellow;
        }
        else if (score <= 0.50f)
        {
            backGroundColor = BackGroundColor.Green;
        }
        else if (score <= 2f)
        {
            backGroundColor = BackGroundColor.Blue;
        }
        else if (score <= 4f)
        {
            backGroundColor = BackGroundColor.Red;
        }

        /*---*/

        switch (backGroundColor)
        {
            case BackGroundColor.Yellow:
                scoreBackGround.color = Color.yellow;
                break;

            case BackGroundColor.Green:
                scoreBackGround.color = Color.green;
                break;

            case BackGroundColor.Blue:
                scoreBackGround.color = Color.blue;
                break;

            case BackGroundColor.Red:
                scoreBackGround.color = Color.red;
                break;
        }
    }

    /*-------------------------------------------*/

    void SetBestScore()
    {
        if (score > bestScore){return;}

        else{bestScore = score;}

        bestScoreText.text = "Best Score: " + bestScore.ToString("F2");
    }

    #endregion
}
