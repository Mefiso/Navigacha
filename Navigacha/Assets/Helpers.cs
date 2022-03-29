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

        static public Vector2 Square(int x, int y)
        {
            return new Vector2(L_BOUNDARY + (x + 0.5f) * SQUARE_SIZE,
                           V_BOUNDARY - (y + 0.5f) * SQUARE_SIZE);
        }
        static public Vector3 PositionToGrid (Vector3 position)
        {
            return new Vector3(Mathf.Floor(position.x) + 0.5f*SQUARE_SIZE,
                               Mathf.Floor(position.y) + 0.5f*SQUARE_SIZE,
                               0);
        }
    }
}