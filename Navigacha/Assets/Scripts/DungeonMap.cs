using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DungeonMap : MonoBehaviour
{
    public GameObject stagePrefab;

    private Dungeon dungeon;
    private Dictionary<int, Map> stages = new Dictionary<int, Map>();
    private Map currentStage;

    // Start is called before the first frame update
    void Start()
    {
        // TODO: replace for actual map to load
        using (StreamReader sr = new StreamReader(Application.dataPath + "/Dungeons/gg/gg.json"))
        {
            string line = sr.ReadLine();
            dungeon = JsonUtility.FromJson<Dungeon>(line);
        }
        LoadStages();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void LoadStages()
    {
        foreach (int sID in dungeon.stagesID)
        {
            GameObject s = Instantiate(stagePrefab);
            s.name = sID.ToString();
            Map sMap = s.GetComponent<Map>();
            sMap.LoadStage();
            stages.Add(sID, sMap);
            sMap.GenerateMap();
            if (sMap.IsEntrance())
            {
                currentStage = sMap;
                sMap.gameObject.SetActive(true);
            }

        }
    }

    public void OpenStage()
    {

    }
}
