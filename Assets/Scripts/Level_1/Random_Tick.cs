using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Random_Tick : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Sounds soundScript;

    [Header("Canvas")]
    [SerializeField] private Canvas tapCanvas;
    [SerializeField] private Canvas failedCanvas;
    [SerializeField] private Canvas completeCanvas;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private UnityEngine.UI.Image scoreBackGround;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private Button touchableDisplay;

    // Variables
    private float randomNumber;
    private float countDown;
    private float maxCountDown;
    private float minCountDown;
    private float score;
    private BackGroundColor backGroundColor;
    private float bestScore;

    // Bools
    private bool isSelected;
    private bool isOpened;
    private bool isCoroutineStarted;
    private bool isCompleted;
    private bool isFinish;
    private bool isFailed;
    private bool tapTooEarly;

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

        tapCanvas.gameObject.SetActive(isOpened);

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
            soundScript.isTapOpen = true;
            isCoroutineStarted = false;
        }
    }

    /*-------------------------------------*/

    void WaitAndClose()
    {
        countDown += Time.deltaTime;

        if(countDown >= maxCountDown - 0.5f)
        {
            countDown = 0f;
            isOpened = false;
            isFailed = true;
            isFinish = true;
        }
    }

    /*---------------------------------------------*/

    public IEnumerator Tap()
    {
        if (!isOpened && !isFinish)             // Ekran bomboş ise
        {
            touchableDisplay.interactable = false;
            yield return new WaitForSeconds(3f);
            touchableDisplay.interactable = true;
        }

        else if (!isCompleted && isOpened)      // Tap ekrani açik ise
        {
            score = countDown;
            countDown = 0f;
            isCompleted = true;
            isFinish = true;
            SetBestScore();
        }

        else if (isCompleted || isFinish)       // Skor ekrani açik ise
        {
            ResetAll();
        }

        else if (isFailed && isFinish)
        {
            ResetAll();
        }
        
    }

    /*---------------------------------------------*/

    void SetValue()
    {
        completeCanvas.gameObject.SetActive(isCompleted);
        failedCanvas.gameObject.SetActive(isFailed);
        tapCanvas.gameObject.SetActive(isOpened);

        ScoreText.text = score.ToString("F2") + " saniye ";
    }

    /*-------------------------------------*/

    public void ResetAll()
    {
        isCompleted = false;
        isSelected = false;
        isOpened = false;
        isFinish = false;
        isFailed = false;
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

    /*-------------------------------------------*/

    public void OnClickFunction()
    {
        StartCoroutine(Tap());
    }

    #endregion
}
