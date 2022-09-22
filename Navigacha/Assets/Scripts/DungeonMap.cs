using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DungeonMap : MonoBehaviour
{
    public GameObject stagePrefab;
    public CombatController combatController;
    public GameObject UI;
    // TODO: replace for actual hero loading
    public HeroController[] heroes = new HeroController[4];

    private Dungeon dungeon;
    private Dictionary<int, Map> stages = new Dictionary<int, Map>();
    private Map currentStage;

    // Helpers
    private bool inCombat = true;
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
        GenerateStageConnections();
    }

    // Update is called once per frame
    void Update()
    {
        if(inCombat && combatController.enemies.Count == 0)
        {
            foreach (var hero in heroes)
            {
                hero.follow = false;
                hero.transform.position = Helpers.MapUtils.PositionToGrid(hero.transform.position);
                currentStage.AddToPosition(hero.gameObject, Helpers.MapUtils.WorldToSquareCoords(hero.transform.position));
            }
            combatController.gameObject.SetActive(false);
            if (--currentStage.remainingWaves <= 0)
            {
                // TODO: Stage selector
                inCombat = false;
                GameObject gridGO = UI.transform.Find("Grid").gameObject;
                foreach (int id in currentStage.GetConnections())
                {
                    GameObject newButton = DefaultControls.CreateButton(new DefaultControls.Resources());
                    newButton.transform.SetParent(gridGO.transform, false);
                    Text t = newButton.GetComponentInChildren<Text>();
                    t.text = id.ToString();
                    t.fontSize = 32;
                }
            }
            else
            {
                currentStage.SpawnEnemies(combatController);
            }
        }
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

    void GenerateStageConnections()
    {
        foreach (var c in dungeon.connections.Values)
        {
            stages[c.list[0]].ConnectWith(c.list[1]);
            stages[c.list[1]].ConnectWith(c.list[0]);
        }
    }
}
