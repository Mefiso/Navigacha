using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, Helpers.IUnit
{

    public float hp;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeBasicAttack(float damage, Vector3 direction, Class heroClass)
    {
        hp -= damage;
        switch (heroClass)
        {
            case Class.BLADE_MASTER:
                transform.position += direction;
                break;
            case Class.JUGGERNAUT:
                transform.position -= direction;
                break;
            default:
                break;
        }
    }

}
