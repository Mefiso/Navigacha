using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DungeonMap : MonoBehaviour
{
    public GameObject stagePrefab;
    public CombatController combatController;
    // TODO: replace for actual hero loading
    public HeroController[] heroes = new HeroController[4];

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
        // TODO: replace for actual hero loading
        combatController.heroes = new HeroController[4];
        heroes.CopyTo(combatController.heroes, 0);
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
            if (sMap.IsEntrance())
            {
                sMap.GenerateMap(combatController);
                currentStage = sMap;
                sMap.gameObject.SetActive(true);
            }

        }
    }

    public void OpenStage()
    {

    }
}
