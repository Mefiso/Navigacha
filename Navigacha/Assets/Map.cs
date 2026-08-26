using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
class Stage
{
    public int ID;

    int dungeonID;
    int nWaves;
    bool isExit = false;
    bool isEntrance = false;
    [SerializeField] DictVector2AndString squares =
        new DictVector2AndString();
    [SerializeField] SerializableList<int> enemyIDs = new SerializableList<int>();
    [SerializeField] SerializableList<int>[] waveSpawnPoints;
    List<int> connections = new List<int>();

    public void GenerateID()
    {
        ID = UnityEngine.Random.Range(1, int.MaxValue);
    }

    public void SetDungeonID(int dID)
    {
        dungeonID = dID;
    }

    public void SetWavesNumber(int n)
    {
        nWaves = n;
        waveSpawnPoints = new SerializableList<int>[nWaves];
    }

    public void MakeEntrance()
    {
        isEntrance = true;
    }

    public void MakeExit()
    {
        isExit = true;
    }

    public void AddSquare(Vector2 coords, string type)
    {
        squares.Add(coords, type);
    }

    public void RemoveSquare(Vector2 coords)
    {
        squares.Remove(coords);
    }

    public void ConnectWith(int sID)
    {
        connections.Add(sID);
    }

}

[Serializable]
class Dungeon
{
    public int ID;
    public string name;
    [SerializeField] SerializableList<int> stagesID = new SerializableList<int>();
    public int plane;
    public int dungeonType;
    [SerializeField] DictStringAndInt connections = new DictStringAndInt();

    public void GenerateID()
    {
        ID = UnityEngine.Random.Range(1, int.MaxValue);
    }
    public void SetPlaneAndDungeonType(int p, int dt)
    {
        plane = p;
        dungeonType = dt;
    }
    public void AddStage(Stage newStage)
    {
        stagesID.list.Add(newStage.ID);
    }
    public void RemoveStage(int id)
    {
        stagesID.list.Remove(id);
    }

    public void GenerateConnection(string name, int firstID, int secondID)
    {
        SerializableList<int> sl = new SerializableList<int>();
        sl.list.Add(firstID);
        sl.list.Add(secondID);
        connections.Add(name,  sl);
    }

    public void RemoveConnection (string name)
    {
        connections.Remove(name);
    }
}
