using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum ArtOrigin
{
    CASTER,
    TARGET,
    INTERACTABLE,
    EMPTY
}

/*
 * Effect convention
 * -----------------
 *
 * D = damage
 * H = heal
 * P = push
 * A = attract
 * B1 to B7 = buff
 * P = orb or hazard
 * X = destroy obstacle or Trap
 *
 */

class Art
{
    string name;
    string description;

    // Shape
    ArtOrigin origin;
    bool directional;
    int range;

    SerializableList<SerializablePair<string, SerializableList<float>>[]> effectMatrix;

    // Power source
    float[] statsDamage = new float[7];
    float[] statsBuff = new float[7];
    float[] statsHeal = new float[7];

    void Perform()
    {

    }
}