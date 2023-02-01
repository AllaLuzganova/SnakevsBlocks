using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AutoDestroy : MonoBehaviour
{
    SnakeMovement SM;

    public int life; 
    public float lifeForColor;
    //TextMesh thisTextMesh;
    TMP_Text thisTextMesh;

    GameObject[] ToDestroy;
    GameObject[] ToUnparent;

    int maxLifeForRed = 50;

    Vector3 initialPos;
    public bool dontMove;

    void SetBoxSize()
    {
        float x;
        float y;

        transform.localScale *= ((float)Screen.width / (float)Screen.height / (9f / 16f));
    }

    void Start()
    {
        SetBoxSize();

        SM = GameObject.FindGameObjectWithTag("SnakeManager").GetComponent<SnakeMovement>();

        life = Random.Range(1, GameController.SCORE / 2 + 5);

        if (transform.tag == "SimpleBox")
        {
            life = Random.Range(5, 50);
        }

        lifeForColor = life;

        /*thisTextMesh = GetComponentInChildren<TextMesh>();
        thisTextMesh.text = "" + life;*/
        thisTextMesh = GetComponentInChildren<TMP_Text>();
        thisTextMesh.text = "" + life;

        ToDestroy = new GameObject[transform.childCount];
        ToUnparent = new GameObject[transform.childCount];

        StartCoroutine("EnableSomeBars");

        SetBoxColor();

        initialPos = transform.position;
        dontMove = false;
    }

    void Update()
    {
        if (dontMove)
            transform.position = initialPos;

        if (SM.transform.childCount > 0 && transform.position.y - SM.transform.GetChild(0).position.y < -10)
            Destroy(this.gameObject);

        lifeForColor = life;

        if(life <= 0)
        {
            transform.GetComponent<SpriteRenderer>().enabled = false; /////////////

            transform.GetComponentInChildren<MeshRenderer>().enabled = false;
            transform.GetComponent<BoxCollider>().enabled = false;

            if (transform.GetComponentInChildren<ParticleSystem>().isStopped)
                transform.GetComponentInChildren<ParticleSystem>().Play();

            Destroy(this.gameObject, 0.7f);
        }
    }

    public void UpdateText()
    {
        thisTextMesh.text = "" + life;
    }

    IEnumerator EnableSomeBars()
    {
        int i = 0;

        while(i < transform.childCount)
        {
            if (transform.GetChild(i).tag == "Bar")
            {
                int r = Random.Range(0, 6);
                if (r == 1)
                {
                    ToUnparent[i] = transform.GetChild(i).gameObject;
                }
                else
                    ToDestroy[i] = transform.GetChild(i).gameObject;

                i++;
                yield return new WaitForSeconds(0.01f);
            }
            else
                i++;
        }

        for (int k = 0; k < ToUnparent.Length; k++)
        {
            if (ToUnparent[k] != null)
                ToUnparent[k].transform.parent = null;
            if (ToDestroy[k] != null)
                Destroy(ToDestroy[k]);
        }

        yield return null;
    }

    private void OnTriggerEnter(Collider collision) // 2 x 2d
    {
        if (collision.transform.tag == "SimpleBox" && transform.tag == "Box")
        {
            Destroy(collision.transform.gameObject);
        }
        else if (transform.tag == "SimpleBox" && collision.transform.tag == "SimpleBox")
        {
            Destroy(collision.transform.gameObject);
        }
    }

    private void OnTriggerStay(Collider collision) // 2 x 2d
    {
        if (collision.transform.tag == "SimpleBox" && transform.tag == "Box")
        {
            Destroy(collision.transform.gameObject);
        }
        else if (transform.tag == "SimpleBox" && collision.transform.tag == "SimpleBox")
        {
            Destroy(collision.transform.gameObject);
        }
    }

    private void OnCollisionStay (Collision collision) // 2 x 2d
    {
        if (collision.transform.tag == "SimpleBox" && transform.tag == "Box")
        {
            Destroy(collision.transform.gameObject);
        }
        else if (collision.transform.tag == "SimpleBox" && transform.tag == "SimpleBox")
        {
            Debug.Log("OverLapping");
        }
    }

    public void SetBoxColor()
    {
        //Color32 thisImageColor = GetComponent<SpriteRenderer>().color; /////////
        Color32 thisImageColor = new Color32(255, 255, 255, 255);

        if (lifeForColor > maxLifeForRed)
            thisImageColor = new Color32(255, 0, 0, 255);

        else if (lifeForColor >= maxLifeForRed / 2 && lifeForColor <= maxLifeForRed)
            thisImageColor = new Color32(255, (byte)(510 * (1 - (lifeForColor / maxLifeForRed))), 0, 255);

        else if (lifeForColor > 0 && lifeForColor < maxLifeForRed)
            thisImageColor = new Color32((byte)(510 * lifeForColor / maxLifeForRed), 255, 0, 255);

        //////////////////////////
        //GetComponent <SpriteRenderer> ().color = thisImageColor; //////////
    }
}
