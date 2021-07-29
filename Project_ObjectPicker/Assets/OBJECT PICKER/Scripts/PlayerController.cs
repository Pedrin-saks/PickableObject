using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum GameState
{
    PLAY, PAUSE
}

public class PlayerController : MonoBehaviour
{

    public      GameState       currentState;
    public      GameObject      playerBody;
    public      Transform       cam;

    [SerializeField]
    private     CharacterController     controller;

    [SerializeField]    
    private     float           mouseSensivity;


    public float speed;
    //private Vector3 velocity;

    private void Awake()
    {
        currentState = GameState.PAUSE;
        StartCoroutine(delayInicio());
        //FadeInOut._instance.Fade();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;

        
    }

    // Update is called once per frame
    void Update()
    {

        switch (currentState)
        {
            case GameState.PLAY:
                float mouseX = Input.GetAxis("Mouse X") * mouseSensivity * Time.deltaTime;

                float x = Input.GetAxis("Horizontal");
                float z = Input.GetAxis("Vertical");

                Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                move = cam.forward * move.z + cam.right * move.x;
                move.y = 0f;
                controller.Move(move * speed * Time.deltaTime);
                break;

            case GameState.PAUSE:
                Cursor.lockState = CursorLockMode.None;
                break;
        }

    }

    IEnumerator delayInicio()
    {
        yield return new WaitForSeconds(3);
        yield return new WaitUntil(() => FadeInOut._instance.isFadeComplete);
        FadeInOut._instance.Fade();
        currentState = GameState.PLAY;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
