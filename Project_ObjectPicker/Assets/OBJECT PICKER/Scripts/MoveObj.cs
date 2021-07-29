using System.Collections;
using UnityEngine;
using Cinemachine;

public class MoveObj : MonoBehaviour
{
    

    public string _tagObjects = "Respawn";
    public float distanceRay;
    [Tooltip("Distância máxima que o objeto possa ficar da camera")]
    public float distanceMaxObjCam;
    [Tooltip("Distância mínima que o objeto possa ficar da camera")]
    public float distanceMinObjCam;
    [Space(10)]
    public Sprite texturaMaoFechada;
    public Sprite texturaMaoAberta;

    PlayerController playerController;
    bool canMove;
    bool isMoving;
    float distance;
    float rotXTemp;
    float rotYTemp;
    float tempDistance;
    RaycastHit tempHit;
    Rigidbody rbTemp;
    Vector3 rayEndPoint;
    Vector3 tempDirection;
    Vector3 tempSpeed;
    GameObject tempObject;

    Camera mainCamera;
    GameObject objClosedHand;
    GameObject objOpenHand;

    CinemachineVirtualCamera cineVC;

    void Awake()
    {
        distance = 4;
        mainCamera = Camera.main;

        //automatic set layer in player
        GameObject refTemp = transform.root.gameObject;
        refTemp.layer = 2;
        foreach (Transform trans in refTemp.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = 2;
        }
        //
        float tempDistance = 0.3f;
        float tempfloatNear = mainCamera.nearClipPlane;
        if (tempfloatNear >= tempDistance)
        {
            tempDistance = tempfloatNear + 0.05f;
        }
        if (texturaMaoFechada)
        {
            objClosedHand = new GameObject("objHandTextureClosed");
            objClosedHand.transform.parent = this.transform;
            objClosedHand.AddComponent<SpriteRenderer>().sprite = texturaMaoFechada;
            objClosedHand.transform.localPosition = new Vector3(0.0f, 0.0f, tempDistance);
            objClosedHand.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            objClosedHand.transform.localRotation = Quaternion.identity;
            objClosedHand.SetActive(false);
        }
        if (texturaMaoAberta)
        {
            objOpenHand = new GameObject("objHandTextureOpen");
            objOpenHand.transform.parent = this.transform;
            objOpenHand.AddComponent<SpriteRenderer>().sprite = texturaMaoAberta;
            objOpenHand.transform.localPosition = new Vector3(0.0f, 0.0f, tempDistance);
            objOpenHand.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            objOpenHand.transform.localRotation = Quaternion.identity;
            objOpenHand.SetActive(false);
        }

        cineVC = FindObjectOfType(typeof(CinemachineVirtualCamera)) as CinemachineVirtualCamera;
        playerController = FindObjectOfType(typeof(PlayerController)) as PlayerController;
    }

    void Update()
    {
        //raycast camera forward
        Debug.DrawRay(transform.position, transform.forward * distanceRay, Color.red, 0.2F);
        rayEndPoint = transform.position + transform.forward * distance;
        if (Physics.Raycast(transform.position, transform.forward, out tempHit, distanceRay))
        {
            if (Vector3.Distance(transform.position, tempHit.point) <= distanceMaxObjCam && tempHit.transform.CompareTag(_tagObjects))
            {
                canMove = true;
            }
            else
            {
                canMove = false;
            }
            //
            if (Input.GetKeyDown(KeyCode.Mouse0) && canMove)
            {
                if (tempHit.rigidbody)
                {
                    tempHit.rigidbody.useGravity = true;
                    distance = Vector3.Distance(transform.position, tempHit.point);
                    tempObject = tempHit.transform.gameObject;
                    isMoving = true;
                }
            }
        }
        else
        {
            canMove = false;
        }

        distance += Input.GetAxis("Mouse ScrollWheel") * 10.0f;
        distance = Mathf.Clamp(distance, distanceMinObjCam, distanceMaxObjCam);
        if (tempObject)
        {
            rbTemp = tempObject.GetComponent<Rigidbody>();
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && tempObject)
        {
            rbTemp.useGravity = true;
            tempObject = null;
            rbTemp = null;
            isMoving = false;
        }

        if (tempObject)
        {
            if (Vector3.Distance(transform.position, tempObject.transform.position) > distanceMaxObjCam)
            {
                rbTemp.useGravity = true;
                tempObject = null;
                rbTemp = null;
                isMoving = false;
            }
        }

        if (tempObject)
        {
            InfoObj info = tempObject.GetComponent<InfoObj>();
            if (Input.GetKey(KeyCode.R))
            {
                Time.timeScale = 0;
                info.isView = true;
                playerController.currentState = GameState.PAUSE;
                cineVC.enabled = false;
                /*
                rotXTemp = Input.GetAxis("Mouse X") * 5.0f;
                rotYTemp = Input.GetAxis("Mouse Y") * 5.0f;
                info.objViewer.gameObject.transform.Rotate(mainCamera.transform.up, -rotXTemp, Space.World);
                info.objViewer.gameObject.transform.Rotate(mainCamera.transform.right, rotYTemp, Space.World);*/
            }

        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Time.timeScale = 1;
            cineVC.enabled = true;
            playerController.currentState = GameState.PLAY;
            
        }

        //sprite elements
        if (canMove && !isMoving && texturaMaoAberta)
        {
            objClosedHand.SetActive(false);
            objOpenHand.SetActive(true);
        }
        else if (isMoving && texturaMaoFechada)
        {
            objClosedHand.SetActive(true);
            objOpenHand.SetActive(false);
        }
        else
        {
            objClosedHand.SetActive(false);
            objOpenHand.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        if (tempObject)
        {
            rbTemp = tempObject.GetComponent<Rigidbody>();
            rbTemp.angularVelocity = new Vector3(0, 0, 0);
            tempSpeed = (rayEndPoint - rbTemp.transform.position);
            tempSpeed.Normalize();
            tempDistance = Vector3.Distance(rayEndPoint, rbTemp.transform.position);
            tempDistance = Mathf.Clamp(tempDistance, 0, 1);
            rbTemp.velocity = Vector3.Lerp(rbTemp.velocity, tempSpeed * 7.5f * tempDistance, Time.deltaTime * 12);
        }
    }
}
