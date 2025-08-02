using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;
using UnityEngine.Profiling;
using System.ComponentModel;
using TMPro;

public class LevelGeneration : MonoBehaviour
{
    [Header("Levels with exits")]
    [SerializeField] private GameObject[] up;
    [SerializeField] private GameObject[] down;
    [SerializeField] private GameObject[] left;
    [SerializeField] private GameObject[] right;
    [SerializeField] private GameObject[] upDown;
    [SerializeField] private GameObject[] upLeft;
    [SerializeField] private GameObject[] upRight;
    [SerializeField] private GameObject[] downLeft;
    [SerializeField] private GameObject[] downRight;
    [SerializeField] private GameObject[] leftRight;
    [SerializeField] private GameObject[] upDownLeft;
    [SerializeField] private GameObject[] upDownRight;
    [SerializeField] private GameObject[] upLeftRight;
    [SerializeField] private GameObject[] downLeftRight;
    [SerializeField] private GameObject[] all;
    private GameObject[][] combinationsArray;

    [Header("Level Generation parameters")]
    [SerializeField] private int roomsInLevel = 3;
    [SerializeField] private int gridSize = 5;
    [SerializeField] private GameObject hub;
    [SerializeField] private GameObject emptyRoom;
    [SerializeField] private GameObject winRoom;
    private int roomDistanceTracker = 0;
    private Vector2Int furthestRoomPosition = Vector2Int.zero;
    private int hubPosition = 0;

    private GameObject[,] levelGrid;
    private Vector2Int[] adjacentRooms;
    private int roomOffset = 150;
    private int iterationCounter = 0;
    private int gridBuffer = 2;

    [SerializeField] private float futureRoomWeight = 1f;
    [SerializeField] private float futureRoomWeightDecrease = 0.05f;
    
    [SerializeField] private NavMeshSurface navMesh;
    private Quaternion roomRotation;

    [Header("UI")]
    private GameObject[,] mapGrid;
    [SerializeField] private Vector2 startPosition;
    [SerializeField] private GameObject roomTile;
    [SerializeField] private GameObject linkTile;
    [SerializeField] private float bufferSize;
    [SerializeField] private float tileSize;
    [SerializeField] private float linkTileOffset;
    [SerializeField] private Vector2 mapCorrection;
    private GameObject canvas;
    [SerializeField] private GameObject loopyTile;
    [SerializeField] private Sprite bossTile;
    [SerializeField] private Sprite completeTile;
    [SerializeField] private Sprite startTile;

    [Header("Scrolls")]
    private ScrollManager scrollManager;
    [SerializeField] private int scrollFrequency = 3;
    private int clearedRooms = 0;

    [Header("Audio")]
    [SerializeField] private AudioSource nonCombatMusic;
    [SerializeField] private AudioSource combatMusic;
    private float musicVolume;

    private void Awake()
    {
        adjacentRooms = new Vector2Int[4];
        adjacentRooms[0] = new Vector2Int(0, -1);
        adjacentRooms[1] = new Vector2Int(0, 1);
        adjacentRooms[2] = new Vector2Int(1, 0);
        adjacentRooms[3] = new Vector2Int(-1, 0);

        levelGrid = new GameObject[gridSize, gridSize];
        mapGrid = new GameObject[gridSize, gridSize];

        combinationsArray = new GameObject[][] 
        {
            right,
            left,
            leftRight,
            down,
            downRight,
            downLeft,
            downLeftRight,
            up,
            upRight,
            upLeft,
            upLeftRight,
            upDown,
            upDownRight,
            upDownLeft,
            all
        };

        musicVolume = nonCombatMusic.volume;
        
    }

    private void Start()
    {
        //setting up hub area
        //hubPosition = (int)(gridSize * 0.5f);
        hubPosition = 2;
        levelGrid[hubPosition, hubPosition] = Instantiate(hub, 
            new Vector3(-1200, 0, -1200), 
            hub.transform.rotation);
        RoomData hubData = levelGrid[hubPosition, hubPosition].GetComponent<RoomData>();
        hubData.entrance = new List<Teleport>
        {
            hubData.exits[1],
            hubData.exits[0],
            hubData.exits[3],
            hubData.exits[2]
        };
        hubData.levelGeneration = this;
        hubData.gridPosition = new Vector2(hubPosition, hubPosition);
        hubData.startRoom = true;
        canvas = GameObject.Find("NonPlayerElements");

        scrollManager = GameObject.Find("Player").GetComponent<ScrollManager>();

        GenerateRooms();
        LinkTeleporters();

        BuildMap();
        loopyTile = Instantiate(loopyTile,
                    new Vector3(startPosition.x + ((tileSize + bufferSize) * hubData.gridPosition.x + 15),
                    startPosition.y + ((tileSize + bufferSize) * hubData.gridPosition.y-33), 
                    0),
                    Quaternion.identity);
        loopyTile.transform.SetParent(canvas.transform.parent, true);
        UpdateLoopyPosition(hubPosition, hubPosition);

        navMesh.BuildNavMesh();

        //Destroy(this.gameObject);
    }

    /// <summary>
    /// Generating rooms in based on max grid size and max rooms
    /// </summary>
    private void GenerateRooms()
    {
        Profiler.BeginSample("Room Gen");
        int roomCount = 0;
        while (roomCount < roomsInLevel)
        {
            for (int x = gridBuffer; x < gridSize - gridBuffer; x++)
            {
                for (int y = gridBuffer; y < gridSize - gridBuffer; y++)
                {
                    RoomData roomData;
                    if (levelGrid[x, y] != null)
                    {
                        roomData = levelGrid[x, y].GetComponent<RoomData>();
                        
                    }
                    else
                        continue;

                    //iterating through each exit direction
                    for (int i = 0; i < adjacentRooms.Length; i++)
                    {
                        int adjacentRoomX = x + adjacentRooms[i].x;
                        int adjacentRoomY = y + adjacentRooms[i].y;
                        if (roomData.exits[i])
                        {
                            if (levelGrid[adjacentRoomX, adjacentRoomY] == null)
                            {
                                levelGrid[adjacentRoomX, adjacentRoomY] =
                                    Instantiate(SelectRoom(adjacentRoomX, adjacentRoomY),
                                    new Vector3(roomOffset * (adjacentRoomX - gridSize * 0.5f),
                                    0,
                                    roomOffset * (adjacentRoomY - gridSize * 0.5f)),
                                    roomRotation);

                                levelGrid[adjacentRoomX, adjacentRoomY].name =
                                    (adjacentRoomX).ToString() + " " +
                                    (adjacentRoomY).ToString() + " " +
                                    levelGrid[adjacentRoomX, adjacentRoomY].name;

                                RoomData nextRoom = levelGrid[adjacentRoomX, adjacentRoomY].GetComponent<RoomData>();
                                nextRoom.gridPosition = new Vector2(adjacentRoomX, adjacentRoomY);
                                nextRoom.levelGeneration = this;

                                int currentRoomDistance = (Math.Abs(hubPosition - adjacentRoomX) + Math.Abs(hubPosition - adjacentRoomY));
                                if (roomDistanceTracker <= currentRoomDistance)
                                {
                                    roomDistanceTracker = currentRoomDistance;
                                    furthestRoomPosition = new Vector2Int(adjacentRoomX, adjacentRoomY);
                                }

                                roomCount++;
                                iterationCounter = 0;

                                //without this, all loops will continue to execute before while loop checks if the room count is met
                                if (roomCount > roomsInLevel)
                                    return;
                            }
                        }
                    }
                }
            }

            //contingency in case no more rooms can be generated
            iterationCounter++;
            if (iterationCounter < gridSize * gridSize)
                break;
        }
        CreateWinRoom();
        Profiler.EndSample();
    }

    private void LinkTeleporters()
    {
        for (int x = gridBuffer - 1; x < gridSize - gridBuffer + 1; x++)
        {
            for (int y = gridBuffer - 1; y < gridSize - gridBuffer + 1; y++)
            {
                RoomData roomData;
                if (levelGrid[x, y] != null)
                    roomData = levelGrid[x, y].GetComponent<RoomData>();
                else
                    continue;

                //iterating through exit directions (0=Up, 1=Down, 2=Left, 3=Right)
                for (int i = 0; i < roomData.exits.Length; i++)
                {
                    if (levelGrid[x + adjacentRooms[i].x, y + adjacentRooms[i].y] != null)
                    {
                        RoomData nextRoomData = levelGrid[x + adjacentRooms[i].x, y + adjacentRooms[i].y].GetComponent<RoomData>();
                        if (nextRoomData.entrance[i] != null && roomData.exits[i])
                            roomData.exits[i].teleportDestination = nextRoomData.entrance[i].gameObject.transform;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Returns a random room with exits based on adjacent rooms.
    /// Uses RNG if there is no room generated yet in a given direction.
    /// </summary>
    private GameObject SelectRoom(int x, int y)
    {
        int binaryCombination = 0;
        int binaryTracker = 1000;
        bool[] adjacentRoomsEntrances = new bool[adjacentRooms.Length];
        for (int i = 0; i < adjacentRooms.Length; i++, binaryTracker = (int)(binaryTracker * 0.1))
        {
            
            if (levelGrid[x + adjacentRooms[i].x, y + adjacentRooms[i].y] == null)
            {
                float temp = Random.Range(0f, 1f);//temp is the chance to have an exit in direction need to change later
                if (temp < futureRoomWeight)
                {
                    adjacentRoomsEntrances[i] = true;
                    futureRoomWeight -= futureRoomWeightDecrease;
                    binaryCombination += binaryTracker;
                }
                else
                    adjacentRoomsEntrances[i] = false;

                continue;
            }
            if (!levelGrid[x + adjacentRooms[i].x, y + adjacentRooms[i].y].TryGetComponent<RoomData>(out RoomData nextRoomData))
            {
                adjacentRoomsEntrances[i] = false;
                continue;
            }
            if (nextRoomData.entrance[i] == null)
            {
                adjacentRoomsEntrances[i] = false;
                continue;
            }
            else
            {
                adjacentRoomsEntrances[i] = true;
                binaryCombination += binaryTracker;
            }
        }

        int combinationIndex = Convert.ToInt16(binaryCombination.ToString(), 2) - 1;
        int randomRoom = Random.Range(0, combinationsArray[combinationIndex].Length);

        roomRotation = combinationsArray[combinationIndex][randomRoom].transform.rotation;
        return combinationsArray[combinationIndex][randomRoom];
    }

    private void CreateWinRoom()
    {
        Destroy(levelGrid[furthestRoomPosition.x, furthestRoomPosition.y]);

        levelGrid[furthestRoomPosition.x, furthestRoomPosition.y] =
            Instantiate(winRoom,
            new Vector3(roomOffset * (furthestRoomPosition.x - gridSize * 0.5f),
            0,
            roomOffset * (furthestRoomPosition.y - gridSize * 0.5f)),
            winRoom.transform.rotation);

        levelGrid[furthestRoomPosition.x, furthestRoomPosition.y].name =
            (furthestRoomPosition.x).ToString() + " " +
            (furthestRoomPosition.y).ToString() + " " +
            levelGrid[furthestRoomPosition.x, furthestRoomPosition.y].name;

        if(levelGrid[furthestRoomPosition.x, furthestRoomPosition.y].TryGetComponent<RoomData>(out RoomData roomData))
        {
            for (int i = 0; i < adjacentRooms.Length; i++)
            {
                //catching potential missing refference
                if(levelGrid[furthestRoomPosition.x + adjacentRooms[i].x, furthestRoomPosition.y + adjacentRooms[i].y] != null)
                {
                    if (levelGrid[furthestRoomPosition.x + adjacentRooms[i].x, furthestRoomPosition.y + adjacentRooms[i].y].TryGetComponent<RoomData>(out RoomData nextRoomData))
                    {
                        if (nextRoomData.entrance[i] != null)
                        {
                            roomData.exits[i].teleportDestination = nextRoomData.entrance[i].gameObject.transform;
                            nextRoomData.entrance[i].teleportDestination = roomData.exits[i].gameObject.transform;
                            roomData.gridPosition = new Vector2(furthestRoomPosition.x, furthestRoomPosition.y);
                            roomData.levelGeneration = this;
                            roomData.bossRoom = true;
                        }
                    }
                }
            }
        }
    }

    public void RoomCleared()
    {
        clearedRooms++;
        if(clearedRooms >= scrollFrequency)
        {
            clearedRooms = 0;
            scrollManager.EnableChoiceScrolls();
        }
    }

    #region Map Ui
    private void BuildMap()
    {
        for(int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                //creating rooms
                if (levelGrid[x, y] == null || levelGrid[x, y] == emptyRoom) 
                    continue;

                GameObject tile = Instantiate(roomTile,
                    new Vector3(startPosition.x + ((tileSize + bufferSize) * x),
                    startPosition.y + ((tileSize + bufferSize) * y), 
                    0),
                    Quaternion.identity);

                tile.transform.SetParent(canvas.transform, true);

                mapGrid[x, y] = tile;

                //creating room links
                RoomData roomData = levelGrid[x, y].GetComponent<RoomData>();

                if (roomData.startRoom)
                    mapGrid[x, y].GetComponent<UnityEngine.UI.Image>().sprite = startTile;
                else if(roomData.bossRoom)
                    mapGrid[x, y].GetComponent<UnityEngine.UI.Image>().sprite = bossTile;

                for (int i = 0; i < adjacentRooms.Length; i++)
                {
                    if (roomData.exits[i] != null)
                    {
                        if (roomData.exits[i].GetComponent<Teleport>().teleportDestination != null)
                        {
                            GameObject link = Instantiate(linkTile,
                                new Vector3(startPosition.x + x * (tileSize + bufferSize) + (adjacentRooms[i].x * linkTileOffset),
                                startPosition.y + y * (tileSize + bufferSize) + (adjacentRooms[i].y * linkTileOffset),
                                0),
                                Quaternion.identity);

                            link.transform.SetParent(canvas.transform, true);
                        }
                    }
                }
            }
        }
    }

    //to do: move to separate object to reduce ram needed
    public void UpdateLoopyPosition(float x, float y)
    {
        canvas.GetComponent<RectTransform>().position = new Vector3(
            startPosition.x + mapCorrection.x + ((hubPosition - x) * (tileSize + bufferSize)),
            startPosition.x + mapCorrection.y + ((hubPosition - y) * (tileSize + bufferSize)),
            0);

        int castX = (int)x;
        int castY = (int)y;

        RoomData roomData = levelGrid[castX, castY].GetComponent<RoomData>();

        if (!roomData.startRoom && !roomData.bossRoom)
            mapGrid[castX, castY].GetComponent<UnityEngine.UI.Image>().sprite = completeTile;

        canvas.GetComponentInParent<UpdateCanvas>().PageTurnAnimation();
    }
    #endregion

    #region Music
    public void TurnOnCombatMusic()
    {
        combatMusic.volume = musicVolume;
        nonCombatMusic.volume = 0;
        Debug.Log("on");
    }

    public void TurnOffCombatMusic()
    {
        combatMusic.volume = 0;
        nonCombatMusic.volume = musicVolume;
        Debug.Log("off");
    }

    #endregion
}