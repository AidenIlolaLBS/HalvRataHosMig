using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToe : MonoBehaviour
{
    public Collider mainCollider;
    public Collider[,] colliders = new Collider[3,3];

    public GameObject[] gameObjects = new GameObject[3];


    /// <summary>
    /// 1: Player. 2: Goblin
    /// </summary>
    int[,] markPlacements =  new int[3,3];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerMarkBoard(Collider collider)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (colliders[i, j] == collider)
                {
                    MarkBoard(i,j, 1);
                }
            }
        }
    }

    void GoblinMarkBoard()
    {

    }

    /// <summary>
    /// Object: 1:player, 2: goblin
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="objekt">1: player, 2:goblin </param>
    public void MarkBoard(int x, int y, int objekt)
    {        
        markPlacements[x, y] = objekt;
        Destroy(colliders[x, y]);

        if (CheckForWinner())
        {

        }
        else if (CheckForDraw())
        {

        }
    }

    bool CheckForWinner()
    {
        for (int i = 0; i < 3; i++)
        {
            if (markPlacements[i, 0] == markPlacements[i, 1] && markPlacements[i, 1] == markPlacements[i, 2])
            {
                return true;
            }
            if (markPlacements[0, i] == markPlacements[1, i] && markPlacements[1, i] == markPlacements[2, i])
            {
                return true;
            }
        }
        if (markPlacements[0, 0] == markPlacements[1, 1] && markPlacements[1, 1] == markPlacements[2, 2])
        {
            return true;
        }
        if (markPlacements[0, 2] == markPlacements[1, 1] && markPlacements[1, 1] == markPlacements[2, 0])
        {
            return true;
        }

        return false;
    }

    bool CheckForDraw()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (markPlacements[i,j] == 0)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
