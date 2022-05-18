using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour
{
    [Header("Dungeon")]
    public GameObject dungeonGO;

    public GameObject entrancePrefab;
    public GameObject exitPrefab;
    public GameObject roomPrefab;
    public GameObject stagePrefab;
    public GameObject inputWaves;
    public GameObject inputDungeon;

    [Header("Stages")]
    public GameObject obstaclePrefab;
    public GameObject hazardPrefab;
    public GameObject enemyPrefab;
    public GameObject heroSpawnPrefab;

    private GameObject currentPrefab;
    private string squareInfo;

    private Dungeon dungeon;
    private Dictionary<int, KeyValuePair<Stage, GameObject>> stages;

    private int currentStage;

    // Line rendering
    private Vector3 lineOrigin;
    private bool drawingLine = false;
    private int originStage;

    enum View
    {
        DUNGEON,
        STAGE,
        INPUT
    }
    View mode = View.DUNGEON;
    // Start is called before the first frame update
    void Start()
    {
        currentPrefab = roomPrefab;
        squareInfo = "O";
        dungeon = new Dungeon();
        dungeon.GenerateID();
        dungeon.SetPlaneAndDungeonType(1, 2);
        stages = new Dictionary<int, KeyValuePair<Stage, GameObject>>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (mode)
        {
            case View.DUNGEON:
                if (Input.GetKeyDown(KeyCode.E))
                    currentPrefab = entrancePrefab;
                else if (Input.GetKeyDown(KeyCode.R))
                    currentPrefab = roomPrefab;
                else if (Input.GetKeyDown(KeyCode.T))
                    currentPrefab = exitPrefab;
                else if (Input.GetKeyDown(KeyCode.S) && Input.GetKey(KeyCode.LeftControl))
                {
                    inputDungeon.SetActive(true);
                    mode = View.INPUT;
                    break;
                }
                else if (Input.GetMouseButtonDown(3))
                {
                    if (ActivateSelectedStage())
                        break;
                }

                if (Input.GetMouseButtonDown(0))
                    CreateNewStage();
                else if (Input.GetMouseButtonDown(1))
                    DeleteSelectedStage();
                else if (Input.GetMouseButtonDown(2))
                    Line();

                if (drawingLine)
                    Debug.DrawLine(lineOrigin, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                break;
            case View.STAGE:
                if (Input.GetKeyDown(KeyCode.O))
                {
                    currentPrefab = obstaclePrefab;
                    squareInfo = "O";
                }
                else if (Input.GetKeyDown(KeyCode.H))
                {
                    currentPrefab = hazardPrefab;
                    squareInfo = "H";
                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    currentPrefab = enemyPrefab;
                    squareInfo = "E";
                }
                else if (Input.GetKeyDown(KeyCode.P))
                {
                    currentPrefab = heroSpawnPrefab;
                    squareInfo = "P";
                }
                else if (Input.GetKeyDown(KeyCode.Escape))
                    ReturnToDungeon();
                else if (Input.GetKeyDown(KeyCode.S) && Input.GetKey(KeyCode.LeftControl))
                {
                    mode = View.INPUT;
                    SaveStage(currentStage);
                    break;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 pos = Helpers.MapUtils.PositionToGrid(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    GameObject go = Instantiate(currentPrefab, pos, Quaternion.identity);
                    go.transform.parent = stages[currentStage].Value.transform;
                    stages[currentStage].Key.AddSquare(Helpers.MapUtils.WorldToSquareCoords(pos), squareInfo);

                }
                else if (Input.GetMouseButtonDown(1))
                {
                    Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Collider2D col = Physics2D.OverlapBox(pos, Vector2.zero, 0.0f);
                    if (col)
                    {
                        stages[currentStage].Key.RemoveSquare(Helpers.MapUtils.WorldToSquareCoords(pos));
                        Destroy(col.gameObject);
                    }
                }

                break;
            default:
                break;
        }
    }

    bool ActivateSelectedStage()
    {
        Collider2D col = Physics2D.OverlapBox(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0.0f);
        if (col)
        {
            mode = View.STAGE;
            currentPrefab = obstaclePrefab;
            currentStage = int.Parse(col.name);
            // Activate stage
            dungeonGO.SetActive(false);
            stages[currentStage].Value.SetActive(true);
            return true;
        }
        return false;
    }

    void CreateNewStage()
    {
        Stage s = new Stage();
        s.GenerateID();
        s.SetDungeonID(dungeon.ID);
        currentStage = s.ID;
        inputWaves.SetActive(true);
        mode = View.INPUT;
        dungeon.AddStage(s);
        if (currentPrefab == entrancePrefab)
        {
            s.MakeEntrance();
        }
        else if (currentPrefab == exitPrefab)
        {
            s.MakeExit();
        }

        GameObject go = Instantiate(currentPrefab, Helpers.MapUtils.PositionToGrid(Camera.main.ScreenToWorldPoint(Input.mousePosition)), Quaternion.identity);
        go.transform.parent = dungeonGO.transform;
        go.name = s.ID.ToString();
        GameObject stageGO = Instantiate(stagePrefab);
        stageGO.name = s.ID.ToString();
        stageGO.SetActive(false);

        stages.Add(s.ID, new KeyValuePair<Stage, GameObject>(s, stageGO));
    }

    bool DeleteSelectedStage()
    {
        Collider2D col = Physics2D.OverlapBox(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0.0f);
        if (col)
        {
            if (col.tag.Equals("line"))
            {
                dungeon.RemoveConnection(col.name);
            }
            else
            {
                int ID = int.Parse(col.name);
                dungeon.RemoveStage(ID);
                Destroy(stages[ID].Value);
                stages.Remove(ID);
            }
            Destroy(col.gameObject);
            return true;
        }
        return false;
    }

    void ReturnToDungeon()
    {
        mode = View.DUNGEON;
        currentPrefab = roomPrefab;
        stages[currentStage].Value.SetActive(false);
        dungeonGO.SetActive(true);
    }

    public void ReadDungeonName()
    {
        dungeon.name = inputDungeon.GetComponentInChildren<InputField>().text;
        inputDungeon.GetComponentInChildren<InputField>().text = "";
        inputDungeon.SetActive(false);
        SaveDungeon();
    }

    public void ReadNumberOfWaves()
    {
        stages[currentStage].Key.SetWavesNumber(int.Parse(inputWaves.GetComponentInChildren<InputField>().text));
        inputWaves.GetComponentInChildren<InputField>().text = "";
        inputWaves.SetActive(false);
        mode = View.DUNGEON;
    }

    public void SaveDungeon()
    {
        string json = JsonUtility.ToJson(dungeon);
        string path = Application.dataPath + "/Dungeons/" + dungeon.name;
        Directory.CreateDirectory(path);
        using (StreamWriter sr = new StreamWriter(path + "/" + dungeon.name + ".json"))
        {
            sr.Write(json);
        }
        mode = View.DUNGEON;
    }

    public void SaveStage(int stage)
    {
        string json = JsonUtility.ToJson(stages[currentStage].Key);
        string path = Application.dataPath + "/Dungeons/" + dungeon.name;
        using (StreamWriter sr = new StreamWriter(path + "/" + stage.ToString() + ".json"))
        {
            sr.Write(json);
        }
        mode = View.STAGE;
    }

    public void Line()
    {
        Collider2D col = Physics2D.OverlapBox(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 0.0f);
        if (col)
        {
            if (!drawingLine)
            {
                originStage = int.Parse(col.name);
                lineOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                lineOrigin.z = -1;
                drawingLine = true;
            }
            else
            {
                drawingLine = false;
                GameObject myLine = new GameObject();
                myLine.name = UnityEngine.Random.Range(1, int.MaxValue).ToString();
                myLine.tag = "line";
                myLine.transform.parent = dungeonGO.transform;
                myLine.transform.position = lineOrigin;
                myLine.AddComponent<LineRenderer>();
                myLine.AddComponent<BoxCollider2D>();
                LineRenderer lr = myLine.GetComponent<LineRenderer>();
                BoxCollider2D bc = myLine.GetComponent<BoxCollider2D>();
                lr.startWidth = 0.05f;
                lr.endWidth = 0.05f;
                lr.SetPosition(0, lineOrigin);
                Vector3 lineDest = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                lineDest.z = -1;
                lr.SetPosition(1, lineDest);
                bc.offset = (lr.GetPosition(1) - lr.GetPosition(0)) / 2;

                int destinationStage = int.Parse(col.name);
                dungeon.GenerateConnection(myLine.name, originStage, destinationStage);
            }
        }

    }
}
