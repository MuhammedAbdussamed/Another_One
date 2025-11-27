using System.Collections;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Random_Tick : MonoBehaviour
{

    // Scripts
    [SerializeField] private InputManager inputManager;

    // Canvas
    [SerializeField] private Canvas tapCanvas;
    [SerializeField] private Canvas failedCanvas;
    [SerializeField] private Canvas completeCanvas;
    [SerializeField] private TextMeshProUGUI ScoreText;

    // Variables
    private float randomNumber;
    private float countDown;
    private float maxCountDown;
    private float minCountDown;
    private float score;

    // Bools
    private bool isSelected;
    private bool isOpened;
    private bool isCoroutineStarted;
    private bool isCompleted;
    private bool isFinish;


    void Start()
    {
        maxCountDown = 6f;
        minCountDown = 0f;
    }

    void Update()
    {
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

    #endregion
}
