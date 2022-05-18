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

    public void GenerateMap()
    {
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
                // TODO: Add appropriate components to go
            }
            else if (square.Value[0].Equals('P'))
            {
                // TODO: Program actual hero spawn
                go = Instantiate(heroSpawnPrefab, this.transform);
            }
            go.transform.position = Helpers.MapUtils.SquareToWorldCoords(position.x, position.y);
            map[position.y, position.x] = go;
        }
    }

    public bool IsEntrance() => stage.IsEntrance();
}
