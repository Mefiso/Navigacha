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
                // TODO: hide magic numbers
                hero.transform.position = Helpers.MapUtils.PositionToGrid(hero.transform.position);
                hero.state = HeroState.idleState;
            } else
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
