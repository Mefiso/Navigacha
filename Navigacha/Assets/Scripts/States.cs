using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;


public class HeroState
{
    public static MovableState movableState = new MovableState();
    public static ArtistState artistState = new ArtistState();
    public static IdleState idleState = new IdleState();

    virtual public void Update(in HeroController hero) { }
    virtual public void OnMouseOver(in HeroController hero) { }

}

public class MovableState : HeroState
{
    override public void Update(in HeroController hero)
    {

        if (hero.follow)
        {
            if (Input.GetMouseButtonUp(0))
            {
                hero.follow = false;
                hero.transform.position = Helpers.MapUtils.PositionToGrid(hero.transform.position);
                hero.state = HeroState.idleState;
                Vector2Int squareCoords = Helpers.MapUtils.WorldToSquareCoords(hero.transform.position);
                GameObject go = hero.currentStage.GetGameObjectInSquare(squareCoords);
                float delta = 0.0F;
                while (go && (go.tag.Equals("Enemy") || go.tag.Equals("Hero") || go.tag.Equals("Obstacle")))
                {
                    squareCoords = Helpers.MapUtils.WorldToSquareCoords(hero.transform.position) + new Vector2Int((int)Mathf.Cos(delta), (int)Mathf.Sin(delta));
                    delta += Mathf.PI / 2;
                    if (squareCoords.x >=0 && squareCoords.x < Helpers.MapUtils.COLS &&
                        squareCoords.y >=0 && squareCoords.y < Helpers.MapUtils.ROWS)
                    {
                        go = hero.currentStage.GetGameObjectInSquare(squareCoords);
                    }
                }
                hero.currentStage.AddToPosition(hero.gameObject, squareCoords);
                hero.transform.position = Helpers.MapUtils.SquareToWorldCoords(squareCoords.x, squareCoords.y);
            }
            else
            {
                hero.FollowMouse();
            }
        }
    }

    override public void OnMouseOver(in HeroController hero)
    {
        // DAOUD 1: is this cheaper than direct comparison?
        if (Input.GetMouseButtonDown(0))
        {
            hero.follow = true;
            hero.currentStage.RemoveObjectFromPosition(Helpers.MapUtils.WorldToSquareCoords(hero.transform.position));
        }
    }
}

public class ArtistState : HeroState
{
    override public void Update(in HeroController hero)
    {

    }

    override public void OnMouseOver(in HeroController hero)
    {
        if (Input.GetMouseButtonDown(0))
        {
            // TODO: Implement arts
            Debug.Log("Select an art and perform it");
            hero.state = HeroState.idleState;
        }
    }
}

public class IdleState : HeroState
{
    override public void Update(in HeroController hero)
    {

    }

    override public void OnMouseOver(in HeroController hero)
    {

    }
}
