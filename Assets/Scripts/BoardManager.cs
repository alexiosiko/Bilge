using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoardManager : MonoBehaviour
{
    private string[] popNames = {"Pop1", "Pop2", "Pop3"};
    public float riseSpeed = 0.05f;
    public GameObject[] pieces;
    private GameManager gameManager;
    private SoundManager soundManager;
    public Transform water;
    void Awake()
    {
        water = GameObject.Find("Water").transform;
        gameManager = GameObject.FindObjectOfType<GameManager>();
        soundManager = GameObject.FindObjectOfType<SoundManager>();
    }
    public void Start()
    {
        soundManager.PlayAudio("BackgroundAudio");
        FillBoard();
    }
    void Update()
    {
        // Board
        if (Input.GetKeyDown(KeyCode.C))
        {
            Clear();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Rise());
        }

        // Inputs
        if (Input.GetKeyDown(KeyCode.F))
        {
            FillBoard();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            DeleteBoard();
        }
    }
    public void Clear()
    {
        // Clear list
        List<GameObject> pieces = new List<GameObject>();

        // Add vertical
        for (int x = 0; x < 6; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                GameObject first = Physics2D.Raycast(new Vector2(x, y), Vector2.zero).collider.gameObject;
                GameObject second = Physics2D.Raycast(new Vector2(x, y + 1), Vector2.zero).collider.gameObject;
                GameObject third = Physics2D.Raycast(new Vector2(x, y + 2), Vector2.zero).collider.gameObject;

                // If one of the pieces is a crab continue
                if (first.CompareTag("Crab") || second.CompareTag("Crab") || third.CompareTag("Crab"))
                    continue;

                // If 3 in a line, add to pieces[]
                if (first.CompareTag(second.tag) && second.CompareTag(third.tag))
                {
                    pieces.Add(first);
                    pieces.Add(second);
                    pieces.Add(third);
                }
            }
        }

        // Add Horizontal
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 12; y++)
            {
                GameObject first = Physics2D.Raycast(new Vector2(x, y), Vector2.zero).collider.gameObject;
                GameObject second = Physics2D.Raycast(new Vector2(x + 1, y), Vector2.zero).collider.gameObject;
                GameObject third = Physics2D.Raycast(new Vector2(x + 2, y), Vector2.zero).collider.gameObject;

                // If one of the pieces is a crab continue
                if (first.CompareTag("Crab") || second.CompareTag("Crab") || third.CompareTag("Crab"))
                    continue;

                // Points
                gameManager.points += pieces.Count * 4;

                // If 3 in a line, add to pieces[]
                if (first.CompareTag(second.tag) && second.CompareTag(third.tag))
                {
                    pieces.Add(first);
                    pieces.Add(second);
                    pieces.Add(third);
                }
            }
        }
        
        // Check crabs
        for (int x = 0; x < 6; x++)
        {
            for (int y = 0; y < 12; y++)
            {
                GameObject piece = Physics2D.Raycast(new Vector2(x, y), Vector2.zero).collider.gameObject;
                if (piece.CompareTag("Crab"))
                {
                    if (water.position.y - piece.transform.position.y <= -6.5f)
                        pieces.Add(piece);
                }
            }
        }

        // If no collapses -> then return true as in done
        if (pieces.Count == 0)
        {
            // Stop audio cause stopped risen
            soundManager.StopAudio("Rise");

            // Change box color back to normal
            gameManager.boxSprite.color = new Color(1, 1, 1, 1f);
            gameManager.waiting = false;
            return;
        }

        // Adjust points
        Debug.Log(pieces.Count);
        gameManager.points += PointCalculations.CalculatePoint(pieces.Count);

        // Set water
        //gameManager.SetWater();

        // Delete
        foreach (GameObject a in pieces)
        {
            if (a != null)
            {
                if (a.CompareTag("Crab"))
                    ScareCrab(a);
                else
                {
                    soundManager.PlayAudio( popNames[ Random.Range(0, popNames.Length) ]);
                    a.GetComponent<BoxCollider2D>().enabled = false;
                    a.transform.DOScale(Vector3.zero, 0.2f);
                    StartCoroutine(DestroyPiece(a, 0.2f));
                }
            }
        }

        // Fill
        Invoke("FillBoard", 0.2f);
        return;

    }
    IEnumerator DestroyPiece(GameObject a, float time)
    {
        yield return new WaitForSeconds(time);
        if (a != null) Destroy(a);
    }

    IEnumerator Rise()
    {
        // Keep rising one spot up until can't anymore
        soundManager.PlayAudio("Rise");
        bool done = false;
        while (done == false)
        {
            done = true;
            yield return new WaitForSeconds(riseSpeed);
            for (int x = 0; x < 6; x++)
                for (int y = -12; y < 11 ; y++)
                {
                    RaycastHit2D piece = Physics2D.Raycast(new Vector2(x, y), Vector2.zero);
                    if (piece.collider != null) // Hit
                        if (Physics2D.Raycast(new Vector2(x, y + 1), Vector2.zero).collider == null) // Above empty
                        {
                            piece.collider.gameObject.transform.DOMove(new Vector3(x, y + 1, 0), riseSpeed);
                            done = false;
                        }
                }
        }
        // After risen -> clear again
        Clear();
    }
    void ScareCrab(GameObject crab)
    {
        // Disable box collider
        crab.GetComponent<BoxCollider2D>().enabled = false;

        // Random move to the left or right
        float leftOrRight = 7 * (Random.value > 0.5f ? 1 : -1);
        crab.transform.DOMoveX(crab.transform.position.x + leftOrRight, 1.5f);

        // Audio
        soundManager.PlayAudio("CrabWalk");

        // Animate
        crab.GetComponent<Animator>().Play("crabwiggle");
        
        // Late destroy
        StartCoroutine(DestroyPiece(crab, 1.5f));
    }
    public void FillBoard()
    {
        Transform piecesParent = GameObject.Find("Pieces").transform;
        for (int x = 0; x < 6; x++)
        {
            for (int y = 0; y < 12; y++)
            {

                if (Physics2D.Raycast(new Vector2(x, y), Vector2.zero).collider == null) 
                {
                    int index;
                    // pieces.Length - 1 because last is crab -> don't include it
                    if (Random.value < 0.005f)
                        index = pieces.Length - 1;
                    else // pieces.Length - 1 because last is crab -> don't include it [0, 5)
                        index = Random.Range(0, pieces.Length - 1);

                    Instantiate<GameObject>(pieces[index], new Vector3(x, -y, 0), Quaternion.identity).transform.parent = piecesParent;
                }
            }
        }

        // Rise
        StartCoroutine(Rise());
    }
    public void DeleteBoard()
    {
        Transform piecesParent = GameObject.Find("Pieces").transform;
        foreach (Transform child in piecesParent)
        {
            Destroy(child.gameObject);   
        }
    }
}
