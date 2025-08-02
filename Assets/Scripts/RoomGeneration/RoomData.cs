using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    [TextArea(1, 2)]
    public string Description = "Exit Directions - Up, Down, Left, Right. Ensure there are 4 variables, they can be empty";
    public Teleport[] exits;
    [HideInInspector] public List<Teleport> entrance;
    [HideInInspector] public List<GameObject> enemySpawners;
    [HideInInspector] public int enemyCounter;
    public Vector2 gridPosition;
    public LevelGeneration levelGeneration;
    [SerializeField] private EnemyWaveManager enemyWave;

    [HideInInspector] public bool startRoom = false;
    [HideInInspector] public bool bossRoom = false;


    private void Awake()
    {
        enemySpawners = new List<GameObject>();
        foreach (EnemySpawner spawner in GetComponentsInChildren<EnemySpawner>())//make enemy folder to optimize
        {
            enemySpawners.Add(spawner.gameObject);
            spawner.gameObject.SetActive(false);
        }
        enemyCounter = enemySpawners.Count;

        entrance = new List<Teleport>
        {
            exits[1],
            exits[0],
            exits[3],
            exits[2]
        };
    }

    public void PlayerEntered()
    {
        if(enemyWave != null)
        {
            levelGeneration.TurnOnCombatMusic();
            enemyWave.StartDelayedSpawn();
            foreach (Teleport teleport in exits)
            {
                if (teleport != null)
                {
                    teleport.locked = true;
                }
            }
        }
        levelGeneration.UpdateLoopyPosition(gridPosition.x, gridPosition.y);
    }

    public void UnlockExits()
    {
        levelGeneration.TurnOffCombatMusic();
        levelGeneration.RoomCleared();
        foreach (Teleport teleport in exits)
        {
            if (teleport != null)
            {
                teleport.locked = false;
                enemyWave = null;
            }
        }
    }
}
