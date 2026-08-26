using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DungeonMap : MonoBehaviour
{
    public CombatController combatController;
    public GameObject UI;
    public StageMap startingStage;

    private StageMap currentStage;
    // Helpers
    private bool inCombat = true;

    // Start is called before the first frame update
    void Start()
    {
        DeactivateAllSatges();
        StartDungeon();
        combatController.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(inCombat && combatController.loadedEnemies.Count == 0)
        {
            foreach (var hero in combatController.heroes)
            {
                hero.follow = false;
                hero.transform.position = Helpers.MapUtils.PositionToGrid(hero.transform.position);
                currentStage.AddToPosition(hero.gameObject, Helpers.MapUtils.WorldToSquareCoords(hero.transform.position));
            }
            combatController.gameObject.SetActive(false);
            if (--currentStage.remainingWaves <= 0)
            {
                // TODO: Stage selector
                foreach (var hero in combatController.heroes)
                {
                    hero.currentStage = null;
                }
                inCombat = false;
                if (currentStage.isExit)
                {
                    Debug.Log("Dungeon completed succesfully!");
                    return;
                }
                GameObject gridGO = UI.transform.Find("Grid").gameObject;
                foreach (StageMap id in currentStage.GetConnections())
                {
                    Button newButton = DefaultControls.CreateButton(new DefaultControls.Resources()).GetComponent<Button>();
                    newButton.transform.SetParent(gridGO.transform, false);
                    newButton.onClick.AddListener(() => OpenStage(id, gridGO));
                    Text t = newButton.GetComponentInChildren<Text>();
                    t.text = id.ToString();
                    t.fontSize = 32;
                }
            }
            else
            {
                currentStage.SpawnEnemies(combatController);
                combatController.gameObject.SetActive(true);
            }
        }
    }

    void DeactivateAllSatges()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform stageTransform = transform.GetChild(i);
            stageTransform.gameObject.SetActive(false);
            StageMap mapScript = stageTransform.GetComponent<StageMap>();
            mapScript.DeactivateContents();
        }
        foreach (var hero in combatController.heroes)
        {
            hero.gameObject.SetActive(false);
        }
    }

    void StartDungeon()
    {
        if (startingStage.IsEntrance())
        {
            currentStage = startingStage;
            startingStage.gameObject.SetActive(true);
            startingStage.GenerateMap(combatController);
        }

    }

    public void OpenStage(StageMap stageToOpen, GameObject buttonHolder)
    {
        for (int i = 0; i < buttonHolder.transform.childCount; ++i)
        {
            Destroy(buttonHolder.transform.GetChild(i));
        }

        currentStage.gameObject.SetActive(false);
        currentStage = stageToOpen;
        currentStage.gameObject.SetActive(true);
        currentStage.GenerateMap(combatController);
    }
}
