using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.Jobs;
using Unity.Collections;


public class Map : MonoBehaviour
{
    [Header("Stages")]
    public GameObject obstaclePrefab;
    public GameObject hazardPrefab;
    public GameObject enemyPrefab;
    public GameObject heroSpawnPrefab;

    [HideInInspector]
    public int remainingWaves = 0;


    GameObject[,] map = new GameObject[Helpers.MapUtils.ROWS, Helpers.MapUtils.COLS];
    Stage stage;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadStage()
    {
        using (StreamReader sr = new StreamReader(Application.dataPath + "/Dungeons/gg/" + gameObject.name + ".json"))
        {
            string line = sr.ReadLine();
            stage = JsonUtility.FromJson<Stage>(line);

        }
    }

    public void GenerateMap(CombatController combatController)
    {
        combatController.enemies.Clear();
        int heroCount = 0;
        remainingWaves = stage.GetWavesNumber();
        foreach (var square in stage.squares)
        {
            Vector2Int position = square.Key;
            GameObject go = obstaclePrefab;
            if (square.Value[0].Equals('H'))
            {
                // TODO: Select correct interactable prefab
                go = Instantiate(hazardPrefab, this.transform);
                go.name = "Interactable";
                // TODO: Add appropriate components to go
            }
            else if (square.Value[0].Equals('O'))
            {
                go = Instantiate(obstaclePrefab, this.transform);
                go.name = "Obstacle";
            }
            else if (square.Value[0].Equals('E'))
            {
                // TODO: Select correct enemy prefab
                go = Instantiate(enemyPrefab, this.transform);
                EnemyController e = go.GetComponent<EnemyController>();
                e.stage = this;
                e.SetCombatController(combatController);
                // TODO: Add appropriate components to go
            }
            else if (square.Value[0].Equals('P'))
            {
                go = combatController.heroes[heroCount++].gameObject;
                go.GetComponent<HeroController>().currentStage = this;
                go.SetActive(true);
            }
            go.transform.position = Helpers.MapUtils.SquareToWorldCoords(position.x, position.y);
            map[position.y, position.x] = go;
        }
        combatController.gameObject.SetActive(true);
    }

    public void SpawnEnemies(CombatController combatController)
    {
        combatController.enemies.Clear();
        foreach (var square in stage.squares)
        {
            Vector2Int position = square.Key;
            GameObject go = obstaclePrefab;
            if (square.Value[0].Equals('E'))
            {
                // TODO: Select correct enemy prefab
                go = Instantiate(enemyPrefab, this.transform);
                EnemyController e = go.GetComponent<EnemyController>();
                e.stage = this;
                e.SetCombatController(combatController);
                // TODO: Add appropriate components to go
            }
            go.transform.position = Helpers.MapUtils.SquareToWorldCoords(position.x, position.y);

            GameObject occupiedBy = GetGameObjectInSquare(position);
            float delta = 0.0F;
            while (occupiedBy && (occupiedBy.tag.Equals("Hero")))
            {
                position = Helpers.MapUtils.WorldToSquareCoords(go.transform.position) + new Vector2Int((int)Mathf.Cos(delta), (int)Mathf.Sin(delta));
                delta += Mathf.PI / 2;
                if (position.x >= 0 && position.x < Helpers.MapUtils.COLS &&
                    position.y >= 0 && position.y < Helpers.MapUtils.ROWS)
                {
                    occupiedBy = GetGameObjectInSquare(position);
                }
            }
            map[position.y, position.x] = occupiedBy;
            go.transform.position = Helpers.MapUtils.SquareToWorldCoords(position.x, position.y);
        }
        combatController.gameObject.SetActive(true);
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

    public void ConnectWith(int sID) => stage.ConnectWith(sID);

    public List<int> GetConnections() => stage.GetConnections();

    public bool IsEntrance() => stage.IsEntrance();
}
