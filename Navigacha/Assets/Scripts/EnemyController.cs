using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, Helpers.IUnit
{

    public float hp;
    public Map stage;

    private CombatController combatController;
    private bool canReceiveDamage = false;
    private const float damageTickThreshold = 0.2F;
    private float damageTickTimer = 0.0F;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0.0F)
        {
            // TODO: Trigger death animation
            combatController.enemies.Remove(this);
            stage.RemoveObjectFromPosition(Helpers.MapUtils.WorldToSquareCoords(transform.position));
            Destroy(this.gameObject);
        }

        if (!canReceiveDamage)
        {
            damageTickTimer += Time.deltaTime;
            if (damageTickTimer >= damageTickThreshold)
            {
                damageTickTimer = 0.0F;
                canReceiveDamage = true;
            }
        }
    }

    public void SetCombatController(CombatController c)
    {
        combatController = c;
        combatController.enemies.Add(this);
    }

    public void TakeBasicAttack(Vector3 direction, Class heroClass)
    {
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
            case Class.SPELLSLINGER:

            default:
                break;
        }
    }

    public void TakeDamage(float damage)
    {
        if (canReceiveDamage)
        {
            hp -= damage;
            canReceiveDamage = false;
        }
    }

}
