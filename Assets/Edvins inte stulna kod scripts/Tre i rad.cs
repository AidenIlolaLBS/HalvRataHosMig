using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class TocTacToe3DV2 : MonoBehaviour
{
    [SerializeField] private BoxCollider playingArea;
    [SerializeField] private GameObject prefabPlayerMarker;
    [SerializeField] private GameObject prefabBotMarker;

    // To spawn obj when you win/lose/tie
    [Header("Objects")]
    [SerializeField] SpawnObject WinObj;
    [SerializeField] SpawnObject LoseObj;
    [SerializeField] SpawnObject TieObj;

    [Header("Bot Settings")]
    [SerializeField] float robotMoveDelay;
    [SerializeField] private BotLevel botDifficulty;

    // Used for marker identification. Should be changed for something more defined. Example give marker's a component with an ID and access that. If you do that you need to change how CheckForWinner() checks
    private readonly string playerMarkerName = "PlayerMarker";
    private readonly string botMarkerName = "BotMarker";

    private float3 incrementalPlayingAreaSideLength;
    private float3 playingAreaCorner;

    private float3 mousePointOfHit;

    private bool playerCanPlaceMarker = true;
    private GameObject[,] board = new GameObject[3, 3];
    private int totalMarkersPlaced = 0;
    private GameObject placeTestingObject;

    enum BotLevel
    {
        Easy,
        Normal,
        Hard
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        placeTestingObject = Instantiate(prefabPlayerMarker, transform);
        placeTestingObject.SetActive(false);

        incrementalPlayingAreaSideLength = playingArea.bounds.size / 6;
        playingAreaCorner = playingArea.bounds.min;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && playerCanPlaceMarker)
            PlayerTurn();
    }

    void PlayerTurn()
    {
        if (IsMouseInPlayingArea() == false)
            return;
        PlaceMarker(GetMousePosToBoardPos(), prefabPlayerMarker, playerMarkerName);

        if (CheckForWinner())
        {
            Debug.Log("Player Won");
            
            // Obj spawns
            WinObj.SpawnPrefab();
            return;
        }

        if (totalMarkersPlaced == 9)
        {
            DrawEnding();
            return;
        }

        StartCoroutine(RobotMove());
    }

    IEnumerator RobotMove()
    {
        playerCanPlaceMarker = false;

        yield return new WaitForSeconds(robotMoveDelay);

        BotPlaceMarker(botDifficulty);
        playerCanPlaceMarker = true;

        if (CheckForWinner())
        {
            /*foreach (var VARIABLE in board)
            {
                if (VARIABLE == null)
                    Debug.Log("NULL, ");
                Debug.Log(VARIABLE.name+", ");
            }*/
            Debug.Log("Vinnare: Bot");

            // Obj spawns
            LoseObj.SpawnPrefab();

            ResetGame();
        }
        else if (totalMarkersPlaced == 9)
        {
            DrawEnding();
        }
    }

    void BotPlaceMarker(BotLevel botLevel)
    {
        int2 botPlacement = new int2(-1, -1);

        switch (botLevel)
        {
            case BotLevel.Easy:
                botPlacement = GetRandomEmptyTile();
                break;
            case BotLevel.Normal:
                if (board[1, 1] == null)
                {
                    botPlacement = new int2(1, 1);
                    break;
                }
                int2 winingMove = CheckForWiningMove(botMarkerName);
                if (winingMove.x == -1)
                    winingMove = CheckForWiningMove(playerMarkerName);

                if (winingMove.x != -1)
                {
                    botPlacement = winingMove;
                    break;
                }
                break;
            case BotLevel.Hard:


                if (board[1, 1] == null)
                {
                    botPlacement = new int2(1, 1);
                    break;
                }

                List<int2> availableCorners = new List<int2>();
                if (totalMarkersPlaced == 2)
                {
                    if (board[0, 1] != null)
                    { availableCorners.Add(new int2(0, 0)); availableCorners.Add(new int2(0, 2)); }
                    else if (board[1, 0] != null)
                    { availableCorners.Add(new int2(0, 0)); availableCorners.Add(new int2(2, 0)); }
                    else if (board[2, 1] != null)
                    { availableCorners.Add(new int2(2, 0)); availableCorners.Add(new int2(2, 2)); }
                    else if (board[1, 2] != null)
                    { availableCorners.Add(new int2(0, 2)); availableCorners.Add(new int2(2, 2)); }

                    botPlacement = availableCorners[Random.Range(0, availableCorners.Count)];
                    break;
                }

                if (board[0, 0] == null)
                    availableCorners.Add(new int2(0, 0));
                if (board[2, 2] == null)
                    availableCorners.Add(new int2(2, 2));
                if (board[2, 0] == null)
                    availableCorners.Add(new int2(2, 0));
                if (board[0, 2] == null)
                    availableCorners.Add(new int2(0, 2));

                if (totalMarkersPlaced == 1)
                {
                    botPlacement = availableCorners[Random.Range(0, availableCorners.Count)];
                    break;
                }

                winingMove = CheckForWiningMove(botMarkerName);
                if (winingMove.x == -1)
                    winingMove = CheckForWiningMove(playerMarkerName);

                if (winingMove.x != -1)
                {
                    botPlacement = winingMove;
                    break;
                }

                foreach (int2 corner in availableCorners)
                {
                    int2 tempCorner = corner;

                    tempCorner.x--;
                    if (tempCorner.x < 0)
                        tempCorner.x += 2;
                    if (board[tempCorner.x, tempCorner.y] != null)
                        continue;
                    tempCorner.x--;

                    tempCorner.y--;
                    if (tempCorner.y < 0)
                        tempCorner.y += 2;
                    if (board[tempCorner.x, tempCorner.y] != null)
                        continue;

                    botPlacement = corner;
                    break;

                }
                break;
        }

        if (botPlacement.x == -1)
            botPlacement = GetRandomEmptyTile();

        if (botPlacement.x == -1)
        { Debug.LogWarning("No space for bot to place a maker!!!"); return; }
        PlaceMarker(botPlacement, prefabBotMarker, botMarkerName);
    }

    int2 CheckForWiningMove(string id)
    {
        placeTestingObject.name = id;

        for (int x = 0; x < 3; x++)
            for (int y = 0; y < 3; y++)
            {
                if (board[x, y] != null)
                    continue;

                board[x, y] = placeTestingObject;
                if (CheckForWinner() == false)
                { board[x, y] = null; continue; }

                board[x, y] = null;
                return new int2(x, y);
            }

        return new int2(-1, -1);
    }

    int2 GetRandomEmptyTile()
    {
        if (totalMarkersPlaced >= 9)
            return new int2(-1, -1);

        int randomNum = Random.Range(0, 9 - totalMarkersPlaced);

        for (int x = 0; x < 3; x++)
            for (int y = 0; y < 3; y++)
            {
                if (board[x, y] != null)
                    continue;

                randomNum--;
                if (randomNum < 0)
                    return new int2(x, y);
            }

        return new int2(-1, -1);
    }

    void PlaceMarker(int2 pos, GameObject marker, string id)
    {
        if (board[pos.x, pos.y] != null)
            return;

        totalMarkersPlaced++;

        board[pos.x, pos.y] = Instantiate(marker, GetWorldPosition(pos), quaternion.identity, transform);
        board[pos.x, pos.y].name = id;
    }

    float3 GetWorldPosition(int2 pos)
    {
        return playingAreaCorner +
               new float3(incrementalPlayingAreaSideLength.x + incrementalPlayingAreaSideLength.x * 2 * pos.x,
                   incrementalPlayingAreaSideLength.y + incrementalPlayingAreaSideLength.y * 2 * pos.y, 0);
    }

    bool IsMouseInPlayingArea()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo) == false)
            return false;

        if (hitInfo.collider.gameObject != playingArea.gameObject)
            return false;

        mousePointOfHit = hitInfo.point;
        return true;
    }

    int2 GetMousePosToBoardPos()
    {
        int2 boardPos = new int2();

        for (int x = 1; x < 4; x++)
        {
            if (incrementalPlayingAreaSideLength.x * 2 * x + playingAreaCorner.x + 0.01f < mousePointOfHit.x)
                continue;

            boardPos.x = x - 1;
            break;
        }
        for (int y = 1; y < 4; y++)
        {
            if (incrementalPlayingAreaSideLength.y * 2 * y + playingAreaCorner.y + 0.01f < mousePointOfHit.y)
                continue;
            boardPos.y = y - 1;
            break;
        }
        return boardPos;
    }

    public void ResetGame()
    {
        totalMarkersPlaced = 0;

        foreach (GameObject marker in board)
            Destroy(marker);
    }

    public void RecountMarkers()
    {
        totalMarkersPlaced = 0;

        foreach (GameObject marker in board)
            if (marker != null)
                totalMarkersPlaced++;
    }

    void DrawEnding()
    {
        Debug.Log("Det är oavgjort!");
        
        // Obj spawns
        TieObj.SpawnPrefab();
        
        ResetGame();
    }
    bool CheckForWinner()
    {
        for (int i = 0; i < 3; i++)
        {
            if (board[i, 0] != null && board[i, 1] != null && board[i, 2] != null
                && board[i, 0].name == board[i, 1].name && board[i, 1].name == board[i, 2].name) return true; // Rader

            if (board[0, i] != null && board[1, i] != null && board[2, i] != null
                && board[0, i].name == board[1, i].name && board[1, i].name == board[2, i].name) return true; // Kolumner
        }
        if (board[0, 0] != null && board[1, 1] != null && board[2, 2] != null
            && board[0, 0].name == board[1, 1].name && board[1, 1].name == board[2, 2].name) return true; // Diagonal 1

        if (board[0, 2] != null && board[1, 1] != null && board[2, 0] != null &&
            board[0, 2].name == board[1, 1].name && board[1, 1].name == board[2, 0].name) return true; // Diagonal 2
        return false;
    }
}