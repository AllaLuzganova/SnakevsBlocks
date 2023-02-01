using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using TMPro;

public class SnakeMovement : MonoBehaviour
{
    [Header("Manager")]
    public GameController GC;

    [Header("Some Snake Variable & Storing")]

    public List<Transform> BodyParts = new List<Transform>();
    public float minDistance = 0.25f;
    public int initialAmount;
    public float speed = 1;
    public float rotationSpeed = 50;
    public float LerpTimeX;
    public float LerpTimeY;
    public float LerpTimeZ; /////

    [Header("Snake Head Prefab")]
    public GameObject BodyPrefab;

    [Header("parts TextAmount Management")]

    //public TextMesh PartAmountTextMesh;
    public TMP_Text PartAmountTextMesh;

    [Header("Private Fields")]
    private float distance;
    private Vector3 refVelocity;

    private Transform curBodyPart;
    private Transform prevBodyPart;

    private bool firstPart;

    [Header("MouseControl Variable")]
    Vector2 mousePreviousPos; ///////
    Vector2 mouseCurrentPos; ///////

    [Header("Particle System Management")]

    public ParticleSystem SnakeParticle;

    void Start()
    {
        firstPart = true;

        for (int i = 0; i < initialAmount; i++) 
        {
            Invoke("AddBodyPart", 0.1f);
        }
    }

    public void SpawnBodyPart()
    {
        firstPart = true;

        for (int i = 0; i < initialAmount; i++)
        {
            Invoke("AddBodyPart", 0.1f);
        }
    }

    void Update()
    {
        if (GameController.gameState == GameController.GameState.GAME)
        {
            Move();

            if (BodyParts.Count == 0)
                GC.SetGameover();
        }

        if (PartAmountTextMesh != null)
            PartAmountTextMesh.text = transform.childCount + "";
    }

    public void Move()
    {
        float curSpeed = speed;
        /*if (BodyParts.Count > 0)
            BodyParts[0].Translate (Vector2.up * curSpeed * LerpTimeX.smoothDeltatime);*/
        /////////////////////////////

        float maxX = Camera.main.orthographicSize * Screen.width / Screen.height; ////////

        /*if (BodyParts.Count > 0)
        {
            if (BodyParts[0].position > maxX) 
            {
                BodyParts[0].position = new Vector3(maxX - 0.01f, BodyParts[0].position.y, BodyParts[0].position.z);
            }
            else if (BodyParts[0].position.x < maxX)
            {
                BodyParts[0].position = new Vector3(-maxX + 0.01f, BodyParts[0].position.y, BodyParts[0].position.z);
            }
        }*/

        if (Input.GetMouseButtonDown (0))
        {
            mousePreviousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButtonDown (0))
        {
            if (BodyParts.Count > 0 && Mathf.Abs(BodyParts[0].position.x) < maxX)
            {
                mouseCurrentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                float deltaMousePos = Mathf.Abs(mousePreviousPos.x - mouseCurrentPos.x);
                float sign = Mathf.Sign(mousePreviousPos.x - mouseCurrentPos.x);

                BodyParts[0].GetComponent<Rigidbody>().AddForce(Vector3.right * rotationSpeed * deltaMousePos * -sign); ; //twice was 2d and 2
                mousePreviousPos = mouseCurrentPos;

            }
            else if (BodyParts.Count > 0 && BodyParts[0].position.x > maxX)
            {
                BodyParts[0].position = new Vector3(maxX - 0.01f, BodyParts[0].position.y, BodyParts[0].position.z);
            }
            else if (BodyParts.Count > 0 && BodyParts[0].position.x < maxX)
            {
                BodyParts[0].position = new Vector3(-maxX + 0.01f, BodyParts[0].position.y, BodyParts[0].position.z);
            }
        }

        for (int i = 0; i < BodyParts.Count; i++)
        {
            curBodyPart = BodyParts[i];
            prevBodyPart = BodyParts[i - 1];

            distance = Vector3.Distance(prevBodyPart.position, curBodyPart.position);

            Vector3 newPos = prevBodyPart.position;

            newPos.z = BodyParts[0].position.z; ///////

            Vector3 pos = curBodyPart.position;

            pos.x = Mathf.Lerp(pos.x, newPos.x, LerpTimeX);
            pos.y = Mathf.Lerp(pos.y, newPos.y, LerpTimeY);
            //pos.z = Mathf.Lerp(pos.z, newPos.z, LerpTimeZ); /////////////

            curBodyPart.position = pos;
        }
    }

    public void AddBodyPart()
    {
        Transform newPart;

        if (firstPart)
        {
            //Vector3 newPos = prevBodyPart.position; ////////// этого не было     ???????

            newPart = (Instantiate(BodyPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject).transform;

            //added
            //Quaternion rot = new Quaternion(0, 0, 0, 0);
            //Transform a = (Instantiate(BodyPrefab, new Vector3(0, 0.5f, 0), Quaternion.identity) as GameObject).transform;

            //

            //PartAmountTextMesh.transform.parent = newPart.position + a.position;
            ////////////(needed)//PartAmountTextMesh.transform.parent = newPart.position + new Vector3(0, 0.5f, 0);
            //PartAmountTextMesh.transform.parent = newPart.position + new Vector3(0, 0.5f, 0);


            firstPart = false;
        }
        else
            newPart = (Instantiate(BodyPrefab, BodyParts[BodyParts.Count - 1].position, BodyParts[BodyParts.Count - 1].rotation) as GameObject).transform;

        newPart.SetParent(transform);
        BodyParts.Add(newPart);
    }
}
