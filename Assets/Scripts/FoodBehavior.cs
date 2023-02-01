using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FoodBehavior : MonoBehaviour
{
    [Header("Snake Manager")]

    SnakeMovement SM;

    public int foodAmount;

    void Start()
    {
        SM = GameObject.FindGameObjectWithTag("SnakeManager").GetComponent<SnakeMovement>();

        foodAmount = Random.Range (1,10);
        //transform.GetComponentInChildren<TextMesh>().text = "" + foodAmount; 
        transform.GetComponentInChildren<TMP_Text>().text = "" + foodAmount;
    }

    void Update ()
    {
        if (SM.transform.childCount > 0 && transform.position.y - SM.transform.GetChild(0).position.y < -10)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider collision) //2d x 2
    {
        Destroy(this.gameObject);
    }
}
