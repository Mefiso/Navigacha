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
    public StageMap currentStage;

    // --- States ---
    public HeroState state = HeroState.idleState;

    // --- Utils ---
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private bool readyToDamage = false;

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
        if (Vector3.Distance(collision.transform.position, transform.position) < 0.5f * Helpers.MapUtils.SQUARE_SIZE)
        {
            Helpers.IUnit unit = collision.gameObject.GetComponent<Helpers.IUnit>();
            if (unit != null)
            {
                readyToDamage = true;
                unit.TakeBasicAttack(collision.contacts[0].normal * Helpers.MapUtils.SQUARE_SIZE, heroClass);
            }
        }
        else if (readyToDamage)
        {
            Helpers.IUnit unit = collision.gameObject.GetComponent<Helpers.IUnit>();
            if (unit != null)
            {
                readyToDamage = false;
                unit.TakeDamage(1);
            }
        }
    }

    public void FollowMouse ()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // TODO: Replace this for actual outer walls
        if (mousePos.x < Helpers.MapUtils.L_BOUNDARY || mousePos.x > Helpers.MapUtils.R_BOUNDARY)
        {
            mousePos.x = transform.position.x;
        }
         if (mousePos.y < Helpers.MapUtils.B_BOUNDARY || mousePos.y > Helpers.MapUtils.T_BOUNDARY)
        {
            mousePos.y = transform.position.y;
        }

        Vector3 newPos = new Vector3(mousePos.x, mousePos.y, transform.position.z);
        RaycastHit2D hit = Physics2D.Linecast(transform.position, newPos, blockingLayer);
        while (hit.transform)
        {
            float hitHDirection = Mathf.Abs(Vector2.Dot(hit.normal, Vector2.right));
            if (hitHDirection > 0)
                newPos.x = transform.position.x;
            else
                newPos.y = transform.position.y;
            hit = Physics2D.Linecast(transform.position, newPos, blockingLayer);
        }
        rb2D.MovePosition(newPos);

    }


}
