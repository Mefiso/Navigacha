using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    public enum CombatPhase
    {
        MOVING_PHASE,
        ARTS_PHASE,
        ENEMY_PHASE,
        COMBAT_FINISHED
    }

    public HeroController[] heroes = new HeroController[4];
    public List<EnemyController> enemies;

    private CombatPhase phase;

    // Start is called before the first frame update

    void OnEnable()
    {
        Debug.Log("Starting combat");
        phase = CombatPhase.MOVING_PHASE;
        ChangeHeroesState(HeroState.movableState);
        Debug.Log("Moving phase");
    }

    // Update is called once per frame
    void Update()
    {
        switch (phase)
        {
            case CombatPhase.MOVING_PHASE:
                if (!AnyHeroAtState(HeroState.movableState))
                {
                    Debug.Log("Arts phase!");
                    phase = CombatPhase.ARTS_PHASE;
                    ChangeHeroesState(HeroState.artistState);
                }
                break;
            case CombatPhase.ARTS_PHASE:
                if (!AnyHeroAtState(HeroState.artistState))
                {
                    Debug.Log("Enemies' turn");
                    phase = CombatPhase.ENEMY_PHASE;
                    ChangeHeroesState(HeroState.idleState);
                }
                break;
            case CombatPhase.ENEMY_PHASE:
                Debug.Log("Enemies act!");
                phase = CombatPhase.MOVING_PHASE;
                ChangeHeroesState(HeroState.movableState);
                Debug.Log("Move phase!");
                break;
        }
    }

    bool AnyHeroAtState(HeroState state)
    {
        for (int i = 0; i < heroes.Length; ++i)
        {
            if (heroes[i].state == state)
            {
                return true;
            }
        }
        return false;
    }

    void ChangeHeroesState(HeroState state)
    {
        for (int i = 0; i < heroes.Length; ++i)
        {
            heroes[i].state = state;
        }
    }
}
