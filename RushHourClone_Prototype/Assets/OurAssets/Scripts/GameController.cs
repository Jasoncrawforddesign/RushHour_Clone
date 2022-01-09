using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Camera cam;
    public GameObject objectToMove;
    private bool haveSelected = false;
    public Vector3 mouseWorldPos;
    private Vector3 savedMousePos;

    public GameObject levelOnePuzzlePrefab;
    private GameObject currentPuzzle;

    public static GameObject[] myPuzzles;
    public int levelcounter = 0;
    private GameObject nextPuzzle;
    private int numberOfEasyPuzzles;

    /* Ideas
     * Puzzle Creator Mode, allow users to create and save own puzzles.
     * Level Select, either per level or just difficulty
     * Level transition, eg. Congratulations screen, and loading next game, simple boat moves from left into screen position
     * boat moves from right off screen for win transition.
     */

    // Start is called before the first frame update
    void Start()
    {
        //Important note: place your prefabs folder(or levels or whatever) 
        //in a folder called "Resources" like this "Assets/Resources/EasyLevels"
        myPuzzles = Resources.LoadAll<GameObject>("EasyLevels");
        foreach (GameObject puzzle in myPuzzles)
        {
            numberOfEasyPuzzles += 1;
        }

        nextPuzzle = myPuzzles[levelcounter];
        loadNextPuzzle(nextPuzzle);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0) && haveSelected == false)
        {
            int layermask = 1 << 7; //will only find layer 7 when firing a ray
            RaycastHit hit;
            var ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layermask))
            {
                if (hit.collider != null)
                {
                    if(hit.collider.gameObject.tag == "Wall")
                    {
                        return;
                    }
                    else
                    {
                        //Debug.Log(hit.collider.gameObject.name);
                        objectToMove = hit.collider.gameObject;
                        haveSelected = true;
                    }

                }
            }
        }
        else if (Input.GetMouseButtonUp(0) && haveSelected == true)
        {
            objectToMove.GetComponent<CubeMovement>().snapTo();

        }
        else if (Input.GetMouseButton(0) && haveSelected == true)
        {
            float newZ = 0;

            int layermask = 1 << 8;
            RaycastHit hit;
            var ray = cam.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit, Mathf.Infinity, layermask))
            {
                if (hit.collider != null)
                {
                    newZ = hit.distance;
                }
            }

            Vector3 mousePos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, newZ);
            
                 
          
            mouseWorldPos = cam.ScreenToWorldPoint(mousePos);
            //Debug.Log(mouseWorldPos);

           objectToMove.GetComponent<CubeMovement>().moveTo(mouseWorldPos, this.gameObject);

        }
       
        
    }
    

    public void removeObject()
    {
        objectToMove = null;
        haveSelected = false;
    }

    public void playerWonPuzzle()
    {
        Debug.Log("Player Won!");

        if (objectToMove)
        {
            removeObject();
        }

        if ((levelcounter + 1) == numberOfEasyPuzzles)
        {
            levelcounter = 0;
            nextPuzzle = myPuzzles[levelcounter];
        }
        else
        {
            levelcounter += 1;
            nextPuzzle = myPuzzles[levelcounter];
        }

        removeLastPuzzle();
        loadNextPuzzle(nextPuzzle);
    }

    void mainMenu()
    {
        //main menu launches here
        //simple play and exit button.
    }

    void loadNextPuzzle(GameObject nextPuzzle)
    {

        currentPuzzle = Instantiate(nextPuzzle, Vector3.zero, Quaternion.identity);
        //load the next level from here
        //for prototype just loop between 5 levels, easy to hard.
    }

    void removeLastPuzzle()
    {
        Destroy(currentPuzzle);
        //remove last level after player has Won!
    }

    public void resetPuzzle()
    {
        if (objectToMove)
        {
            removeObject();
        }

        removeLastPuzzle();

        loadNextPuzzle(nextPuzzle);
    }
}
