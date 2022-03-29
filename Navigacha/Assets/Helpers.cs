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
}