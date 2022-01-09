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

    private bool isMoving = false;

    [SerializeField] private LayerMask _anchorLayermask;
    [SerializeField] private LayerMask _collisionLayermask;

    private Vector3 currentAnchorPos;
 
    public int anchorLayer;

    //private float snapHitDistance = 0.9f;
    private float stepSpeed = 20;

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

                RaycastHit hitOne;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hitOne, checkCollisionRayLength, _collisionLayermask))
                {
                    leftMoveEnabled = false;
                    //lastMousePos = savedMousePos;
                    Debug.Log("2"+ hitOne.collider.gameObject.name);
                    // Debug.Log("HIT SOMETHING STOPPP!!");
                }

                RaycastHit hitTwo;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hitTwo, checkCollisionRayLength, _collisionLayermask))
                {
                    rightMoveEnabled = false;
                    //lastMousePos = savedMousePos;
                    Debug.Log("1 " + hitTwo.collider.gameObject.name);
                    // Debug.Log("HIT SOMETHING STOPPP!!");
                }
            }

            if (moveX == false)
            {

                RaycastHit hitOne;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitOne, checkCollisionRayLength, _collisionLayermask))
                {
                    Debug.Log("3 "+ hitOne.collider.gameObject.name);
                    forwardMoveEnabled = false;
                    lastMousePos = savedMousePos;
                    // Debug.Log("HIT SOMETHING STOPPP!!");
                }

                RaycastHit hitTwo;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out hitTwo, checkCollisionRayLength, _collisionLayermask))
                {
                    backwardMoveEnabled = false;
                    lastMousePos = savedMousePos;
                    Debug.Log("4" + hitTwo.collider.gameObject.name);
                    // Debug.Log("HIT SOMETHING STOPPP!!");
                }
            }
        }
    }

   
    // moveTo Function
    public void moveTo(Vector3 mousePos, GameObject actualGameController)
    {
        isMoving = true;
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

                    //transform.position = new Vector3(mousePos.x, objectPos.y, objectPos.z);
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3 (mousePos.x, objectPos.y, objectPos.z), stepSpeed * Time.deltaTime);

                    leftMoveEnabled = true;
                }
            }

            else if (rightMoveEnabled == false)
            {
                if(savedMousePos.x < transform.position.x)
                {
                    //transform.position = new Vector3(mousePos.x, objectPos.y, objectPos.z);
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(mousePos.x, objectPos.y, objectPos.z), stepSpeed * Time.deltaTime);

                    rightMoveEnabled = true;
                }
            }

            else
            {
                //transform.position = new Vector3(mousePos.x, objectPos.y, objectPos.z);
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(mousePos.x, objectPos.y, objectPos.z), stepSpeed * Time.deltaTime);
            }
            
            
        }
        else if(moveX == false)
        {
            if (forwardMoveEnabled == false)
            {
                if (savedMousePos.z < transform.position.z)
                {
                    //transform.position = new Vector3(objectPos.x, objectPos.y, mousePos.z);
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(objectPos.x, objectPos.y, mousePos.z), stepSpeed * Time.deltaTime);

                    forwardMoveEnabled = true;
                }
            }

            else if (backwardMoveEnabled == false)
            {
                if (savedMousePos.z > transform.position.z)
                {
                    
                    //transform.position = new Vector3(objectPos.x, objectPos.y, mousePos.z);
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(objectPos.x, objectPos.y, mousePos.z), stepSpeed * Time.deltaTime);

                    backwardMoveEnabled = true;
                }
            }

            else
            {
                //transform.position = new Vector3(objectPos.x, objectPos.y, mousePos.z);
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(objectPos.x, objectPos.y, mousePos.z), stepSpeed * Time.deltaTime);
            }

        }
        isMoving = false;
    }


    // LATEST snapTo Function
    public void snapTo()
    {
        if (isMoving == false)
        {
            if (moveX == true)
            {
                float halfObject = this.gameObject.transform.localScale.x / 2;

                bool didOneHit = false;
                bool didTwoHit = false;

                RaycastHit hitOne;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hitOne, halfObject, _anchorLayermask))
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * hitOne.distance, Color.green, 2f);
                    // Debug.Log("hitOne Distance: " + hitOne.distance);
                    didOneHit = true;
                }

                RaycastHit hitTwo;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hitTwo, halfObject, _anchorLayermask))
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * hitTwo.distance, Color.red, 2f);
                    //Debug.Log("hitTwo Distance: " + hitTwo.distance);
                    didTwoHit = true;
                }

                if (didOneHit == true && didTwoHit == true)
                {
                    Debug.Log("Hit Right and Left");
                    if (hitOne.distance < hitTwo.distance && rightMoveEnabled == true)
                    {
                        transform.position = hitOne.transform.position;
                    }
                    else if (hitTwo.distance < hitOne.distance && leftMoveEnabled == true)
                    {
                        transform.position = hitTwo.transform.position;
                    }
                    else
                    {
                        this.gameObject.transform.position = currentAnchorPos;
                    }
                }
                else if (didOneHit == true && didTwoHit == false && rightMoveEnabled == true)
                {
                    transform.position = hitOne.transform.position;
                }
                else if (didTwoHit == true && didOneHit == false && leftMoveEnabled == true)
                {
                    transform.position = hitTwo.transform.position;
                }
                else
                {
                    this.gameObject.transform.position = currentAnchorPos;
                }
                currentAnchorPos = this.gameObject.transform.position;

            }

            if (moveX == false)
            {
                float halfObject = this.gameObject.transform.localScale.z / 2;

                bool didOneHit = false;
                bool didTwoHit = false;

                RaycastHit hitOne;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitOne, halfObject, _anchorLayermask))
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitOne.distance, Color.green, 2f);
                    // Debug.Log("hitOne Distance: " + hitOne.distance);
                    didOneHit = true;
                }

                RaycastHit hitTwo;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out hitTwo, halfObject, _anchorLayermask))
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.back) * hitTwo.distance, Color.red, 2f);
                    //Debug.Log("hitTwo Distance: " + hitTwo.distance);
                    didTwoHit = true;
                }

                if (didOneHit == true && didTwoHit == true)
                {
                    Debug.Log("Hit Right and Left");
                    if (hitOne.distance < hitTwo.distance && forwardMoveEnabled == true)
                    {
                        transform.position = hitOne.transform.position;
                    }
                    else if (hitTwo.distance < hitOne.distance && backwardMoveEnabled == true)
                    {
                        Debug.Log("WHOOPSIE");
                        transform.position = hitTwo.transform.position;
                    }
                    else
                    {
                        this.gameObject.transform.position = currentAnchorPos;
                    }
                }
                else if (didOneHit == true && didTwoHit == false && forwardMoveEnabled == true)
                {
                    transform.position = hitOne.transform.position;
                }
                else if (didTwoHit == true && didOneHit == false && backwardMoveEnabled == true)
                {
                    transform.position = hitTwo.transform.position;
                }
                else
                {
                    this.gameObject.transform.position = currentAnchorPos;
                }
                currentAnchorPos = this.gameObject.transform.position;
            }



            // this.gameObject.transform.position = currentAnchorPos;

           // currentAnchorPos = this.gameObject.transform.position;

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
    }



    //------------------- OLD SNAP FUNCTION BELOW----------------------
    /*
    public void snapTo()
    {
        
        if (moveX == false)
        {
            float halfObject = this.gameObject.transform.localScale.z / 2;

            bool didOneHit = false;
            bool didTwoHit = false;


            RaycastHit hitOne;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitOne, Mathf.Infinity, _anchorLayermask))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitOne.distance, Color.green, 2f);
                
                didOneHit = true;
            }

            RaycastHit hitTwo;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out hitTwo, Mathf.Infinity, _anchorLayermask))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.back) * hitTwo.distance, Color.red, 2f);
                Debug.Log(hitTwo.collider.gameObject.layer);
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
                    else
                    {
                        this.gameObject.transform.position = currentAnchorPos;
                    }

                }

            }
            else if (didOneHit == true && didTwoHit == false)
            {
                if (hitOne.distance < snapHitDistance)
                {
                    this.gameObject.transform.position = hitOne.transform.position;
                }
                else
                {
                    this.gameObject.transform.position = currentAnchorPos;
                }
            }
            else if (didTwoHit == true && didOneHit == false)
            {
                if (hitTwo.distance < snapHitDistance)
                {
                    this.gameObject.transform.position = hitTwo.transform.position;
                }
                else
                {
                    this.gameObject.transform.position = currentAnchorPos;
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

            RaycastHit hitOne;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hitOne, Mathf.Infinity, _anchorLayermask))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * hitOne.distance, Color.green, 2f);
               // Debug.Log("hitOne Distance: " + hitOne.distance);
                didOneHit = true;
            }

            RaycastHit hitTwo;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hitTwo, Mathf.Infinity, _anchorLayermask))
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
                   

                    }
                    else if (hitOne.distance > hitTwo.distance)
                    {
                        this.gameObject.transform.position = hitTwo.transform.position;
                     
                    }
                }
                else
                {
                    this.gameObject.transform.position = currentAnchorPos;
                }
            }
            else if(didOneHit == true)
            {
                if (hitOne.distance < snapHitDistance)
                {
                    this.gameObject.transform.position = hitOne.transform.position;
                    
                }
                else
                {
                    this.gameObject.transform.position = currentAnchorPos;
                }
            }
            else if(didTwoHit == true)
            {
                if (hitTwo.distance < snapHitDistance)
                {
                    this.gameObject.transform.position = hitTwo.transform.position;

                }
                else
                {
                    this.gameObject.transform.position = currentAnchorPos;
                }
            }
            else
            {
                this.gameObject.transform.position = currentAnchorPos;
            }
            
        }
        
        this.gameObject.transform.position = currentAnchorPos;

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
    */ 
   //---------------------------------------------------------------------

}
