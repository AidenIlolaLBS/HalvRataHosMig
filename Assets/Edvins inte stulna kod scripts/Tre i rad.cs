using System.Collections;
using UnityEngine;

public class TicTacToe3D : MonoBehaviour
{
    private string[,] board = new string[3, 3]; // 3x3 grid för spelplanen
    private bool isPlayerXTurn = true; // Spelaren som är "X" börjar
    public GameObject xPrefab; // Prefab för "X"
    public GameObject oPrefab; // Prefab för "O"

    void Start()
    {
        ResetBoard();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Om spelaren klickar med musen
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                GameObject selectedTile = hit.collider.gameObject;
                Vector2Int tileIndex = GetTileIndex(selectedTile);

                if (board[tileIndex.x, tileIndex.y] == "")
                {
                    PlaceMark(tileIndex);
                    if (CheckForWinner())
                    {
                        Debug.Log("Vinnare: " + (isPlayerXTurn ? "O" : "X"));
                        ResetBoard();
                    }
                    else if (IsBoardFull())
                    {
                        Debug.Log("Det är oavgjort!");
                        ResetBoard();
                    }
                    else
                    {
                        isPlayerXTurn = !isPlayerXTurn;
                    }
                }
            }
        }
    }

    // Sätter markering på brädet och placerar prefab i världen
    void PlaceMark(Vector2Int tileIndex)
    {
        board[tileIndex.x, tileIndex.y] = isPlayerXTurn ? "X" : "O";
        GameObject prefabToInstantiate = isPlayerXTurn ? xPrefab : oPrefab;
        Instantiate(prefabToInstantiate, GetTilePosition(tileIndex), Quaternion.identity);
    }

    // Returnerar positionen för en tile i 3D-världen baserat på index
    Vector3 GetTilePosition(Vector2Int tileIndex)
    {
        return new Vector3(tileIndex.x * 2.0f, 0, tileIndex.y * 2.0f); // Justera detta baserat på layouten av dina kuber
    }

    // Returnerar index för tile som användaren klickade på
    Vector2Int GetTileIndex(GameObject tile)
    {
        int x = Mathf.RoundToInt(tile.transform.position.x / 2.0f);
        int y = Mathf.RoundToInt(tile.transform.position.z / 2.0f);
        return new Vector2Int(x, y);
    }

    // Återställer spelbrädet
    void ResetBoard()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                board[i, j] = "";
            }
        }
        isPlayerXTurn = true;
    }

    // Kollar om någon har vunnit
    bool CheckForWinner()
    {
        for (int i = 0; i < 3; i++)
        {
            if (board[i, 0] != "" && board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2]) return true; // Rader
            if (board[0, i] != "" && board[0, i] == board[1, i] && board[1, i] == board[2, i]) return true; // Kolumner
        }
        if (board[0, 0] != "" && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2]) return true; // Diagonal 1
        if (board[0, 2] != "" && board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0]) return true; // Diagonal 2
        return false;
    }

    // Kollar om hela spelbrädet är fyllt
    bool IsBoardFull()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] == "")
                {
                    return false;
                }
            }
        }
        return true;
    }
}