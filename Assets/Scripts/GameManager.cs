using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    BoardManager boardManager;
    public float pointsDegrade = 50;
    public float maxPoints = 40000;
    public float moveCost = 750;
    public float speed = 0.1f;
    [HideInInspector] public bool waiting = true;
    public float points = 4000;
    public SpriteRenderer boxSprite;
    private Transform water;
    public bool freeze = false;
    SoundManager soundManager;
    void Awake()
    {   
        soundManager = GameObject.FindObjectOfType<SoundManager>();
        water = GameObject.Find("Water").transform;
        boxSprite = GameObject.Find("Box").GetComponentInChildren<SpriteRenderer>();
        boardManager = FindObjectOfType<BoardManager>();
    }
    void Update()
    {
        // Points
        points -= Time.deltaTime * pointsDegrade;
        if (points < 10000)
            points = 10000;
        
        if (points > 20000)
        {
            Debug.Log("Win");
            //this.enabled = false;
        }

        // Update water position
        SetWater();
    }
    public void Click(Vector2 pos)
    {
        // If frozen (menu is open)
        if (freeze)
            return;

        // Waiting for animations to finish
        if (waiting == true)
            return;
        waiting = true;

        // Set box waiting color
        boxSprite.color = new Color(1, 1, 1, 0.3f);
        
        Transform first = Physics2D.Raycast(pos, Vector2.zero).collider.transform;
        Transform second;
        if (pos.x >= 5) // Get left
            second = Physics2D.Raycast(pos + Vector2.left, Vector2.zero).collider.transform;
        else // Get right
            second = Physics2D.Raycast(pos + Vector2.right, Vector2.zero).collider.transform;

        // If the other piece is crab, return
        if (second.CompareTag("Crab"))
        {
            second.GetComponent<Animator>().Play("crabwiggle");
            Reset();
            return;
        }

        // If either left or right is empty
        if (first == null || second == null)
        {
            Reset();
            return;
        }

        // Each click minus points
        points -= moveCost;

        // Set water
        //SetWater();
            
        // Tween
        first.DOMove(new Vector3(second.position.x, second.position.y, 0), speed);
        second.DOMove(new Vector3(first.position.x, first.position.y, 0), speed);

        // Audio
        soundManager.PlayAudio("MovePiece");


        // Valid move
        Debug.Log("Valid move");
        Invoke("Clear", speed);
        
    }
    public void SetWater()
    {
        // -6.5 to 5
        float h = points / maxPoints; // Get precentage
        h = h * (5.5f + 6.5f); // Scale with range [-6.5, 5.5]
        h = 5.5f - h; // Invert it so water starts from the top

        // Set position
        water.DOKill();
        water.DOMoveY(h, 1f); // = new Vector3(2.5f, h, 0); 
    }
    void Reset()
    {
        boxSprite.color = new Color(1, 1, 1, 1f);
        waiting = false;

        Debug.Log("Reset!");
    }
    void UnWait()
    {
        waiting = false;
    }
    void Clear()
    {
        boardManager.Clear();
    }
    public void Restart()
    {
        // Waiting and points
        waiting = true;
        points = 4000;

        // Board
        boardManager.DeleteBoard();
        boardManager.Start();

        // Set box waiting color
        boxSprite.color = new Color(1, 1, 1, 0.3f);

    }
    
    
}
