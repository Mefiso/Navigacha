using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, Helpers.IUnit
{

    public float hp;
    public Map stage;

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
        Vector2Int origin = Helpers.MapUtils.WorldToSquareCoords(transform.position);
        switch (heroClass)
        {
            case Class.BLADE_MASTER:
                transform.position += direction;
                stage.Move(origin, Helpers.MapUtils.WorldToSquareCoords(transform.position));
                break;
            case Class.JUGGERNAUT:
                transform.position -= direction;
                stage.Move(origin, Helpers.MapUtils.WorldToSquareCoords(transform.position));
                break;
            default:
                break;
        }
    }

}
