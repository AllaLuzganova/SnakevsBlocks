using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxBehavior : MonoBehaviour
{
    SnakeMovement SM;

    void Start()
    {
        SM = transform.GetComponentInParent<SnakeMovement>();
    }

    private void OnCollisionEnter3D(Collision collision) ////////
    {
        if (collision.transform.tag == "Box" && transform == SM.BodyParts[0])
        {
            if (SM.BodyParts.Count > 1 && SM.BodyParts[1] != null)
            {
                SM.PartAmountTextMesh.transform.parent = SM.BodyParts[1]; //отсюда  и далее PartsAmountTextMesh
                SM.PartAmountTextMesh.transform.position = SM.BodyParts[1].position + new Vector3(0, 0.5f, 0);
            }
            else if (SM.BodyParts.Count == 1)
            {
                SM.PartAmountTextMesh.transform.parent = null;
            }

            SM.SnakeParticle.Stop();

            SM.SnakeParticle.transform.position = collision.contacts[0].point;

            SM.SnakeParticle.Play();

            Destroy(this.gameObject);

            GameController.SCORE++;

            collision.transform.GetComponent<AutoDestroy>().life -= 1;
            collision.transform.GetComponent<AutoDestroy>().UpdateText();

            collision.transform.GetComponent<AutoDestroy>().SetBoxColor();

            SM.BodyParts.Remove(SM.BodyParts[0]);
        }

        else if (collision.transform.tag == "SimpleBox" && transform == SM.BodyParts[0])
        {
            SM.SnakeParticle.Stop();

            SM.SnakeParticle.transform.position = collision.contacts [0].point;

            SM.SnakeParticle.Play();

            if (SM.BodyParts.Count > 1 && SM.BodyParts[1] != null) 
            {
                SM.PartAmountTextMesh.transform.parent = SM.BodyParts[1];
                SM.PartAmountTextMesh.transform.position = SM.BodyParts[1].position + new Vector3 (0, 0.5f, 0);
            }
            else if (SM.BodyParts.Count == 1)
            {
                SM.PartAmountTextMesh.transform.parent = null;
            }

            Destroy(this.gameObject);

            GameController.SCORE++;

            collision.transform.GetComponent<AutoDestroy>().life -= 1;
            collision.transform.GetComponent<AutoDestroy>().UpdateText();

            collision.transform.GetComponent<AutoDestroy>().SetBoxColor();
            SM.BodyParts.Remove(SM.BodyParts[0]);
        }
        else if (collision.transform.tag == "SimpleBox" && transform != SM.BodyParts[0])
        {
            Physics.IgnoreCollision(transform.GetComponent <Collider>(), collision.transform.GetComponent <Collider>()); ////////////////3
            //Physics2D.IgnoreCollision(transform.GetComponent<Collider2D>(), collision.transform.GetComponent<Collider2D>());
            collision.transform.GetComponent<AutoDestroy>().dontMove = true;
        }
    }


    ////////////////
    private void OnTriggerEnter(Collider collision) // 2 x 2d
    {
        if(SM.BodyParts.Count > 0)
        {
            if(collision.transform.tag == "Food" && transform == SM.BodyParts[0]) 
            {
                for (int i = 0; i < collision.transform.GetComponent <FoodBehavior>().foodAmount; i++)
                {
                    SM.AddBodyPart();
                }

                Destroy(collision.transform.gameObject);
            }
        }
    }
}
