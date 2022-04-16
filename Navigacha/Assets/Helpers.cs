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

[Serializable] public class DictStringAndInt : SerializableDictionary<string, SerializableList<int>> { }
[Serializable] public class DictVector2AndString : SerializableDictionary<Vector2, string> { }


namespace Helpers
{
    public interface IUnit
    {
        void TakeBasicAttack(float damage, Vector3 direction, Class heroClass);
    }

    class MapUtils
    {
        public const float SQUARE_SIZE = 1.0f;
        public const int L_BOUNDARY = -3;
        public const int R_BOUNDARY = 4;
        public const int V_BOUNDARY = 6;
        public const int MAX_ENEMIES = 10;

        static public Vector2 Square(int x, int y)
        {
            return new Vector2(L_BOUNDARY + (x + 0.5f) * SQUARE_SIZE,
                           V_BOUNDARY - (y + 0.5f) * SQUARE_SIZE);
        }

        static public Vector2 GetSquareIn(Vector2 position)
        {
            return new Vector2(Mathf.Floor((position.x - L_BOUNDARY) / SQUARE_SIZE),
                                Mathf.Floor((V_BOUNDARY - position.y) / SQUARE_SIZE));
        }

        static public Vector3 PositionToGrid (Vector3 position)
        {
            return new Vector3(Mathf.Floor(position.x) + 0.5f*SQUARE_SIZE,
                               Mathf.Floor(position.y) + 0.5f*SQUARE_SIZE,
                               0);
        }
    }
}