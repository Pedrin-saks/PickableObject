using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoObj : MonoBehaviour
{
    [Header("CONFIGURAÇÕES OBJETO")]
    public GameObject objViewer;
    public int coin;
    public bool isView;
    public GameObject pn_View3d;
    public string tagObject;
    public float speedRotObj;

    [Tooltip("Posição que será posicionado o objeto na mesa")]
    public Transform PosObjTable;
    [Tooltip("distância mínima para o objeto encaixar na mesa")]
    public float distanceMinTable;

    [SerializeField]
    bool isPlaced;
    Camera mainCamera;
    float rotXTemp;
    float rotYTemp;
    Collider col;
    Rigidbody rb;

    private void Start()
    {
        mainCamera = Camera.main;
        objViewer.SetActive(false);
        col = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();

        if(tagObject == "")
        {
            tagObject = gameObject.tag;
        }
        objViewer.tag = "Finish";
        objViewer.layer = 5;
    }

    private void Update()
    {
        if (isView)
        {
            pn_View3d.SetActive(true);
            objViewer.gameObject.SetActive(true);


            if (Input.GetMouseButton(0))
            {
                rotXTemp = Input.GetAxis("Mouse X") * speedRotObj;
                rotYTemp = Input.GetAxis("Mouse Y") * speedRotObj;

                objViewer.gameObject.transform.Rotate(mainCamera.transform.up, -rotXTemp);
                objViewer.gameObject.transform.Rotate(mainCamera.transform.right, rotYTemp);
            }
            if (Input.GetKey(KeyCode.Escape))
            {
                pn_View3d.SetActive(false);
                objViewer.gameObject.SetActive(false);
                isView = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        if(Vector3.Distance(transform.position, PosObjTable.position) < distanceMinTable)
        {
            transform.position = Vector3.MoveTowards(transform.position, PosObjTable.position, 5 * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if(transform.position == PosObjTable.position && isPlaced == false)
        {
            gameObject.tag = "Finish";
            rb.isKinematic = true;

            StartCoroutine(delaysUpdate(2, coin, 1));
            isPlaced = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Finish" && isPlaced)
        {
            gameObject.tag = tagObject;
            rb.isKinematic = false;
            StartCoroutine(delaysUpdate(2, -coin, -1));
            isPlaced = false;
        }
    }

    IEnumerator delaysUpdate(float time, int c, int i)
    {
        yield return new WaitForSeconds(time);
        GameManager._instance.setCoin(c);
        GameManager._instance.SetPlacedItems(i);
        GameManager._instance.updateUI();
    }

}
