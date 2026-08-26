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
 * I = orb or hazard (Interactable)
 * X = destroy obstacle or Trap
 *
 */

[Serializable]
class Art
{
    string name;
    string description;

    // Shape
    ArtOrigin origin;
    bool directional;
    int range;

    [SerializeField] SerializableList<SerializablePair<string, List<float>>[]> effectMatrix;

    // Power source
    [SerializeField] float[] statsDamage = new float[7];
    [SerializeField] float[] statsBuff = new float[7];
    [SerializeField] float[] statsHeal = new float[7];

    void Perform()
    {

    }
}