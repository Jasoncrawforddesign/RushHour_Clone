using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorPoints : MonoBehaviour
{
    public bool isOccupied = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool available ()
    {
        Debug.Log("checked is available");
        if (isOccupied)
            return true;
        else 
            return false;
    }
}
