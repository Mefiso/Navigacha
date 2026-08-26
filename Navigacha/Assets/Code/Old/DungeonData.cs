using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

// TODO: REMOVE?
//[Serializable]
//public struct Interactable
//{
//    public bool movable;
//    public int trigger; // -1> tactical, -2> art 1-infinity> chrono
//    public SerializablePair<string, float> effect;
//}

//[Serializable]
//class Stage
//{
//    public int ID;
//    public DictVector2AndString squares = new DictVector2AndString();

//    [SerializeField] int dungeonID;
//    [SerializeField] int nWaves;
//    [SerializeField] bool isExit = false;
//    [SerializeField] bool isEntrance = false;
//    [SerializeField] List<int> enemyIDs = new List<int>();
//    [SerializeField] List<int>[] waveSpawnPoints;
//    [SerializeField] List<Interactable> interactables = new List<Interactable>();
//    List<int> connections = new List<int>();

//    public void GenerateID()
//    {
//        ID = UnityEngine.Random.Range(1, int.MaxValue);
//    }

//    public void SetDungeonID(int dID)
//    {
//        dungeonID = dID;
//    }

//    public void SetWavesNumber(int n)
//    {
//        nWaves = n;
//        waveSpawnPoints = new List<int>[nWaves];
//    }

//    public int GetWavesNumber()
//    {
//        return nWaves;
//    }

//    public void MakeEntrance()
//    {
//        isEntrance = true;
//    }

//    public bool IsEntrance()
//    {
//        return isEntrance;
//    }

//    public void MakeExit()
//    {
//        isExit = true;
//    }

//    public bool IsExit()
//    {
//        return isExit;
//    }

//    public void AddSquare(Vector2Int coords, string type)
//    {
//        squares.Add(coords, type);
//        if (type.Equals("H"))
//        {
//            Interactable def = new Interactable
//            {
//                movable = false,
//                trigger = 1,
//                effect = new SerializablePair<string, float>
//                {
//                    key = "D",
//                    value = 10
//                }
//            };
//            interactables.Add(def);

//        }
//    }

//    public void RemoveSquare(Vector2Int coords)
//    {
//        squares.Remove(coords);
//    }

//    public void ConnectWith(int sID)
//    {
//        connections.Add(sID);
//    }

//    public List<int> GetConnections()
//    {
//        return connections;
//    }
//}

//[Serializable]
//class Dungeon
//{
//    public int ID;
//    public string name;
//    public List<int> stagesID = new List<int>();
//    public int plane;
//    public int dungeonType;
//    public DictStringAndIntList connections = new DictStringAndIntList();

//    public void GenerateID()
//    {
//        ID = UnityEngine.Random.Range(1, int.MaxValue);
//    }
//    public void SetPlaneAndDungeonType(int p, int dt)
//    {
//        plane = p;
//        dungeonType = dt;
//    }
//    public void AddStage(Stage newStage)
//    {
//        stagesID.Add(newStage.ID);
//    }
//    public void RemoveStage(int id)
//    {
//        stagesID.Remove(id);
//    }

//    public void GenerateConnection(string name, int firstID, int secondID)
//    {
//        SerializableList<int> sl = new SerializableList<int>();
//        sl.list.Add(firstID);
//        sl.list.Add(secondID);
//        connections.Add(name,  sl);
//    }

//    public void RemoveConnection (string name)
//    {
//        connections.Remove(name);
//    }
//}
