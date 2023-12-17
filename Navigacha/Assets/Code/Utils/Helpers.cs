using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum Class
{
    BLADE_MASTER,
    JUGGERNAUT,
    SPELLSLINGER
}

[Serializable]
public class SerializableList<T>
{
    public List<T> list = new List<T>();
}

[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();

    [SerializeField]
    private List<TValue> values = new List<TValue>();

    // save the dictionary to lists
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    // load dictionary from lists
    public void OnAfterDeserialize()
    {
        this.Clear();

        if (keys.Count != values.Count)
            throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));

        for (int i = 0; i < keys.Count; i++)
            this.Add(keys[i], values[i]);
    }
}

[Serializable] public class DictStringAndIntList : SerializableDictionary<string, SerializableList<int>> { }
[Serializable] public class DictVector2AndString : SerializableDictionary<Vector2Int, string> { }

[Serializable]
public struct SerializablePair<TKey, TValue>
{
    public TKey key;
    public TValue value;
}


namespace Helpers
{
    public interface IUnit
    {
        void TakeBasicAttack(Vector3 direction, Class heroClass);
        void TakeDamage(float damage);
    }

    class MapUtils
    {
        public const float SQUARE_SIZE = 0.8f;
        public const float L_BOUNDARY = (-3.0f * SQUARE_SIZE);
        public const float R_BOUNDARY = (4.0f * SQUARE_SIZE);
        public const float T_BOUNDARY = (4.0f * SQUARE_SIZE);
        public const float B_BOUNDARY = (-5.0f * SQUARE_SIZE);
        public const int MAX_ENEMIES = 10;
        public const int ROWS = (int)((T_BOUNDARY - B_BOUNDARY) / SQUARE_SIZE);
        public const int COLS = (int)((R_BOUNDARY - L_BOUNDARY) / SQUARE_SIZE);
        public const int N_SQUARES = ROWS * COLS;

        // Returns the 2D world position of a square.
        static public Vector2 SquareToWorldCoords(int x, int y)
        {
            return new Vector2(L_BOUNDARY + (x + 0.5f) * SQUARE_SIZE,
                           T_BOUNDARY - (y + 0.5f) * SQUARE_SIZE);
        }

        // Gets the square corrdinates in a given position
        static public Vector2Int WorldToSquareCoords(Vector2 position)
        {
            return new Vector2Int((int)Mathf.Floor((position.x - L_BOUNDARY) / SQUARE_SIZE),
                                (int)Mathf.Floor((T_BOUNDARY - position.y) / SQUARE_SIZE));
        }

        // Centers a position in the corresponding square
        static public Vector3 PositionToGrid(Vector3 position)
        {
            float xShift = Mathf.Abs(L_BOUNDARY);
            float yShift = Mathf.Abs(B_BOUNDARY);
            Vector2 shiftedPosition = new Vector2(position.x + xShift, position.y + yShift);
            float closestXLine = shiftedPosition.x - (shiftedPosition.x % SQUARE_SIZE) - xShift;
            float closestYLine = shiftedPosition.y - (shiftedPosition.y % SQUARE_SIZE) - yShift;
            return new Vector3(closestXLine + 0.5f*SQUARE_SIZE,
                               closestYLine + 0.5f*SQUARE_SIZE,
                               0);
        }
    }
}