using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    public bool moveX = true;
    private Vector3 savedMousePos;
    private GameObject gameController;

    private bool checkCollision;
    public float checkCollisionRayLength = 3f;

    private bool leftMoveEnabled = true;
    private bool rightMoveEnabled = true;
    private bool forwardMoveEnabled = true;
    private bool backwardMoveEnabled = true;

    private Vector3 currentAnchorPos;
 
    public int anchorLayer;

    public float snapHitDistance = 1.9f;

    private Vector3 lastMousePos;
    // Start is called before the first frame update
    void Start()
    {
      currentAnchorPos = transform.position;
    }

    private void Awake()
    {
        //snapTo();
    }
    // Update is called once per frame
    void Update()
    {
        if (checkCollision)
        {
            if (moveX == true)
            {
                int layermask = 1 << 7;

                RaycastHit hitOne;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hitOne, checkCollisionRayLength, layermask))
                {
                    leftMoveEnabled = false;
                    lastMousePos = savedMousePos;
                    // Debug.Log("HIT SOMETHING STOPPP!!");
                }

                RaycastHit hitTwo;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hitTwo, checkCollisionRayLength, layermask))
                {
                    rightMoveEnabled = false;
                    lastMousePos = savedMousePos;
                    // Debug.Log("HIT SOMETHING STOPPP!!");
                }
            }

            if (moveX == false)
            {
                int layermask = 1 << 7;

                RaycastHit hitOne;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitOne, checkCollisionRayLength, layermask))
                {
                    forwardMoveEnabled = false;
                    lastMousePos = savedMousePos;
                    // Debug.Log("HIT SOMETHING STOPPP!!");
                }

                RaycastHit hitTwo;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out hitTwo, checkCollisionRayLength, layermask))
                {
                    backwardMoveEnabled = false;
                    lastMousePos = savedMousePos;
                    // Debug.Log("HIT SOMETHING STOPPP!!");
                }
            }
        }
    }

    public void moveTo(Vector3 mousePos, GameObject actualGameController)
    {
        checkCollision = true;
        if(gameController == null)
        {
            gameController = actualGameController;
        }

        savedMousePos = mousePos;
        
        
        Vector3 objectPos = this.gameObject.transform.position;

        if (moveX == true)
        {
            if (leftMoveEnabled == false)
            {
                if (savedMousePos.x > transform.position.x)
                {
                   
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3 (mousePos.x, objectPos.y, objectPos.z), 0.1f);

                    leftMoveEnabled = true;
                }
            }

            else if (rightMoveEnabled == false)
            {
                if(savedMousePos.x < transform.position.x)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(mousePos.x, objectPos.y, objectPos.z), 0.1f);

                    rightMoveEnabled = true;
                }
            }

            else if(leftMoveEnabled == true && rightMoveEnabled == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(mousePos.x, objectPos.y, objectPos.z), 0.1f);
            }
            
            
        }
        else if(moveX == false)
        {
            if (forwardMoveEnabled == false)
            {
                if (savedMousePos.z < transform.position.z)
                {

                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(objectPos.x, objectPos.y, mousePos.z), 0.1f);

                    forwardMoveEnabled = true;
                }
            }

            else if (backwardMoveEnabled == false)
            {
                if (savedMousePos.z > transform.position.z)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(objectPos.x, objectPos.y, mousePos.z), 0.1f);

                    backwardMoveEnabled = true;
                }
            }

            else if (leftMoveEnabled == true && rightMoveEnabled == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(objectPos.x, objectPos.y, mousePos.z), 0.1f);
            }

        }
       
    }

    public void snapTo()
    {
        
        if (moveX == false)
        {
            float halfObject = this.gameObject.transform.localScale.z / 2;

            bool didOneHit = false;
            bool didTwoHit = false;

            int layermask = 1 << anchorLayer;

            RaycastHit hitOne;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitOne, (halfObject + 0.2f), layermask))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitOne.distance, Color.green, 2f);
                //Debug.Log("Did Hit One");
                didOneHit = true;
            }

            RaycastHit hitTwo;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out hitTwo, (halfObject + 0.2f), layermask))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.back) * hitTwo.distance, Color.red, 2f);
                // Debug.Log("Did Hit Two");
                didTwoHit = true;
            }

            if (didOneHit == true && didTwoHit == true)
            {
                Debug.Log("Both Ray hit stuff");

                if (hitOne.distance < snapHitDistance && hitTwo.distance < snapHitDistance)
                {
                    if (hitOne.distance < hitTwo.distance)
                    {
                        this.gameObject.transform.position = hitOne.transform.position;

                    }
                    else if (hitOne.distance > hitTwo.distance)
                    {
                        this.gameObject.transform.position = hitTwo.transform.position;
                    }
                }
            }
            else if (didOneHit == true)
            {
                if (hitOne.distance < snapHitDistance)
                {
                    this.gameObject.transform.position = hitOne.transform.position;
                }
            }
            else if (didTwoHit == true)
            {
                if (hitTwo.distance < snapHitDistance)
                {
                    this.gameObject.transform.position = hitTwo.transform.position;
                }
            }
            else
            {
                this.gameObject.transform.position = currentAnchorPos;
            }

        }

        
       else if(moveX == true)
        {

            float halfObject = this.gameObject.transform.localScale.x / 2;

            bool didOneHit = false;
            bool didTwoHit = false;
            int layermask = 1 << anchorLayer;

            RaycastHit hitOne;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hitOne, (halfObject + 0.2f), layermask))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * hitOne.distance, Color.green, 2f);
               // Debug.Log("hitOne Distance: " + hitOne.distance);
                didOneHit = true;
            }

            RaycastHit hitTwo;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hitTwo,(halfObject + 0.2f), layermask))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * hitTwo.distance, Color.red, 2f);
                //Debug.Log("hitTwo Distance: " + hitTwo.distance);
                didTwoHit = true;
            }

            if (didOneHit == true && didTwoHit == true)
            {
                //Debug.Log("Both Ray hit stuff");

                if (hitOne.distance < snapHitDistance && hitTwo.distance < snapHitDistance)
                {
                    if (hitOne.distance < hitTwo.distance)
                    {
                        this.gameObject.transform.position = hitOne.transform.position;
                       // Debug.Log("If 1");

                    }
                    else if (hitOne.distance > hitTwo.distance)
                    {
                        this.gameObject.transform.position = hitTwo.transform.position;
                        //Debug.Log("If 2");
                    }
                }
            }
            else if(didOneHit == true)
            {
                if (hitOne.distance < snapHitDistance)
                {
                    this.gameObject.transform.position = hitOne.transform.position;
                    //Debug.Log("If 3");
                }
            }
            else if(didTwoHit == true)
            {
                if (hitTwo.distance < snapHitDistance)
                {
                    this.gameObject.transform.position = hitTwo.transform.position;
                    //Debug.Log("If 4");
                }
            }
            else
            {
                this.gameObject.transform.position = currentAnchorPos;
            }
            
        }

        currentAnchorPos = this.gameObject.transform.position;

        if (gameController)
        {
            gameController.GetComponent<GameController>().removeObject();
        }

        checkCollision = false;

        forwardMoveEnabled = true;
        backwardMoveEnabled = true;
        rightMoveEnabled = true;
        leftMoveEnabled = true;
        // send 2 rays casts out, either y+/y- or x+/x-
        // whichever ray is closest to a snapping sphere move gameobject to that.
    }

    void checkForOtherObjects()
    {
        //send a small raycast out from both ends that check to see if we shouldn't move any further, eg hit another object or reached border.
    }
}
