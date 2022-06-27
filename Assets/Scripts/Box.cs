using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    void Update()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos = new Vector3( (int)(pos.x + 0.5f), (int)(pos.y + 0.5f), 0);

        // If right side
        if (pos.x >= 5)
            transform.position = pos + new Vector3(-1, 0, 0);
        else // Else left side
            transform.position = pos;
    }
    
}
