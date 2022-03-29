using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{

    public LayerMask blockingLayer;

    // --- Hero stats ---
    [Header("Hero stats")]
    public float speed;
    public Class heroClass;

    [HideInInspector]
    public bool follow = false;

    // --- States ---
    public HeroState state = HeroState.idleState;

    // --- Utils ---
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;


    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        state.Update(this);
    }

    private void OnMouseOver() => state.OnMouseOver(this);

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Vector3.Distance(collision.transform.position, transform.position) < 0.5f)
        {
            Helpers.IUnit unit = collision.gameObject.GetComponent<Helpers.IUnit>();
            if (unit != null)
            {
                unit.TakeBasicAttack(1, collision.contacts[0].normal, heroClass);
            }
        }
    }

    public void FollowMouse ()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // TODO: Replace this for actual outer walls
        if (mousePos.x < -3 || mousePos.x > 4)
        {
            mousePos.x = transform.position.x;
        }
         if (Mathf.Abs(mousePos.y) > 6)
        {
            mousePos.y = transform.position.y;
        }

        Vector3 newPos = new Vector3(mousePos.x, mousePos.y, transform.position.z);
        RaycastHit2D hit = Physics2D.Linecast(transform.position, newPos, blockingLayer);
        while (hit.transform)
        {
            float hitHDirection = Mathf.Abs(Vector2.Dot(hit.normal, new Vector2(1, 0)));
            if (hitHDirection > 0)
                newPos.x = transform.position.x;
            else
                newPos.y = transform.position.y;
            hit = Physics2D.Linecast(transform.position, newPos, blockingLayer);
        }
        rb2D.MovePosition(newPos);

    }


}
