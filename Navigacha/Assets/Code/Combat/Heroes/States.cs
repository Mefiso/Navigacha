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
                int lap = 0;
                while (go && (go.tag.Equals("Enemy") || go.tag.Equals("Hero") || go.tag.Equals("Obstacle")))
                {
                    int xShift = Mathf.CeilToInt(Mathf.Cos(delta)) + lap;
                    int yShift = Mathf.CeilToInt(Mathf.Sin(delta)) + lap;
                    squareCoords = Helpers.MapUtils.WorldToSquareCoords(hero.transform.position) + new Vector2Int(xShift, yShift);
                    if (squareCoords.x >= 0 && squareCoords.x < Helpers.MapUtils.COLS &&
                        squareCoords.y >= 0 && squareCoords.y < Helpers.MapUtils.ROWS)
                    {
                        go = hero.currentStage.GetGameObjectInSquare(squareCoords);
                    }

                    delta += Mathf.PI / 4;
                    if (delta == 2*Mathf.PI)
                    {
                        delta = 0.0F;
                        ++lap;
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
