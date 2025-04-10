using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToe : MonoBehaviour
{
    public Collider mainCollider;
    public Collider[,] colliders = new Collider[3,3];

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
                    //MarkBoard();
                }
            }
        }
    }

    void GoblinMarkBoard()
    {

    }

    public void MarkBoard(int x, int y, int objekt)
    {
        
                    markPlacements[x, y] = objekt;
                    Destroy(colliders[x, y]);
                
        CheckForWinner();
    }


    bool CheckForWinner()
    {
        for (int i = 0; i < 3; i++)
        {
            if (colliders[i, 0] != null && colliders[i, 1] != null && colliders[i, 2] != null
                && colliders[i, 0].name == colliders[i, 1].name && colliders[i, 1].name == colliders[i, 2].name) return true; // Rader

            if (colliders[0, i] != null && colliders[1, i] != null && colliders[2, i] != null
                && colliders[0, i].name == colliders[1, i].name && colliders[1, i].name == colliders[2, i].name) return true; // Kolumner
        }
        if (colliders[0, 0] != null && colliders[1, 1] != null && colliders[2, 2] != null
            && colliders[0, 0].name == colliders[1, 1].name && colliders[1, 1].name == colliders[2, 2].name) return true; // Diagonal 1

        if (colliders[0, 2] != null && colliders[1, 1] != null && colliders[2, 0] != null &&
            colliders[0, 2].name == colliders[1, 1].name && colliders[1, 1].name == colliders[2, 0].name) return true; // Diagonal 2
        return false;
    }
}
