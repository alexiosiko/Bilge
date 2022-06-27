using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    GameManager gameManager;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    void OnMouseDown()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos = new Vector3( (int)(pos.x + 0.5f), (int)(pos.y + 0.5f) );
        
        gameManager.Click(pos);
    }
}
