using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DungeonMap : MonoBehaviour
{
    [Header("Dungeon")]

    public GameObject entrancePrefab;
    public GameObject exitPrefab;
    public GameObject roomPrefab;

    private Dungeon dungeon;
    // Start is called before the first frame update
    void Start()
    {
        using (StreamReader sr = new StreamReader(Application.dataPath + "/Dungeons/gg/gg.json"))
        {
            string line = sr.ReadLine();
            dungeon = JsonUtility.FromJson<Dungeon>(line);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
