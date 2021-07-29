using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float maxTime;
    public int qtdItemScene;
    public GameObject objFinishCam;
    public GameObject virtualCam;
    public GameObject POV;
    private int qtdCoin;
    private float timeLeft;
    private int placedItems;
    private PlayerController playerController;
    private bool isFinish;

    [Header("HUD")]
    public Image imgBar;
    public TMP_Text txtCoin;
    public GameObject pnWin;
    public GameObject pnLose;


    public static GameManager _instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }

        updateUI();
        timeLeft = maxTime;
    }

    private void Start()
    {
        playerController = FindObjectOfType(typeof(PlayerController)) as PlayerController;
        isFinish = false;
    }

    // Update is called once per frame
    void Update()
    {

        if(timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            imgBar.fillAmount = timeLeft / maxTime;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            playerController.enabled = false;
            virtualCam.SetActive(false);
            pnLose.SetActive(true);
        }

        if(placedItems == qtdItemScene && isFinish == false)
        {
            StartCoroutine(posCam());
        }
        
    }


    public void setCoin(int c)
    {
        qtdCoin += c;
    }

    public void SetPlacedItems(int i)
    {
        placedItems += i;
    }

    public void updateUI()
    {
        txtCoin.text = qtdCoin.ToString();
    }

    IEnumerator posCam()
    {
            FadeInOut._instance.Fade();
            isFinish = true;
            Cursor.lockState = CursorLockMode.None;
            playerController.enabled = false;
            virtualCam.SetActive(false);

            objFinishCam.SetActive(true);
            yield return new WaitUntil(() => FadeInOut._instance.isFadeComplete);
            //virtualCam.SetActive(true);
            //FadeInOut._instance.Fade();
            print("passou");
            FadeInOut._instance.Fade();



            yield return new WaitForEndOfFrame();

            pnWin.SetActive(true);

        
    }
}
