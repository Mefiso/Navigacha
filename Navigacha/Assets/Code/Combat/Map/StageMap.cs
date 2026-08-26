using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.Jobs;
using Unity.Collections;
using System;

public class StageMap : MonoBehaviour
{
    [System.Serializable]
    public struct Wave
    {
        public List<EnemyController> enemies;
    }

    [Header("Configuration")]
    public bool isExit = false;
    public bool isEntrance = false;
    public List<StageMap> connections = new List<StageMap>();
    public List<GameObject> mapContents = new List<GameObject>();

    [Header("Enemies")]
    public List<Wave> waves = new List<Wave>();

    [HideInInspector]
    public int remainingWaves = 0;

    GameObject[,] map = new GameObject[Helpers.MapUtils.ROWS, Helpers.MapUtils.COLS];

    // Start is called before the first frame update
    void Start()
    {
        remainingWaves = waves.Count;
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void DeactivateContents()
    {
        foreach (var content in mapContents)
        {
            content.SetActive(false);
        }
        foreach (var wave in waves)
        {
            foreach (var enemy in wave.enemies)
            {
                enemy.gameObject.SetActive(false);
            }
        }
    }

    public void GenerateMap(CombatController combatController)
    {
        foreach (var content in mapContents)
        {
            content.transform.position = Helpers.MapUtils.PositionToGrid(content.transform.position);
            Vector2Int position = Helpers.MapUtils.WorldToSquareCoords(content.transform.position);
            map[position.y, position.x] = content;
            content.SetActive(true);
        }

        foreach (var hero in combatController.heroes)
        {
            hero.transform.position = Helpers.MapUtils.PositionToGrid(hero.transform.position);
            Vector2Int position = Helpers.MapUtils.WorldToSquareCoords(hero.transform.position);
            map[position.y, position.x] = hero.gameObject;
            hero.currentStage = this;
            hero.gameObject.SetActive(true);
        }

        SpawnEnemies(combatController);
    }

    public void SpawnEnemies(CombatController combatController)
    {
        combatController.loadedEnemies.Clear();
        foreach (var enemy in waves[waves.Count - remainingWaves].enemies)
        {
            enemy.transform.position = Helpers.MapUtils.PositionToGrid(enemy.transform.position);
            Vector2Int position = Helpers.MapUtils.WorldToSquareCoords(enemy.transform.position);
            enemy.SetCombatController(combatController);
            enemy.SetStage(this);
            map[position.y, position.x] = enemy.gameObject;
            enemy.gameObject.SetActive(true);
        }
    }

    public GameObject GetGameObjectInSquare (Vector2Int position)
    {
        return map[position.y, position.x];
    }

    public void AddToPosition(GameObject obj, Vector2Int position)
    {
        map[position.y, position.x] = obj;
    }

    public void RemoveObjectFromPosition(Vector2Int position)
    {
        map[position.y, position.x] = null;
    }

    public void Move(Vector2Int origin, Vector2Int destination)
    {
        map[destination.y, destination.x] = map[origin.y, origin.x];
        map[origin.y, origin.x] = null;
    }

    public List<StageMap> GetConnections() => connections;

    public bool IsEntrance() => isEntrance;
}
