using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyMovement : MonoBehaviour
{

    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private bool follow = false;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            follow = false;
            transform.position = new Vector3(Mathf.Floor(transform.position.x) + 0.5f,
                                            Mathf.Floor(transform.position.y) + 0.5f,
                                            transform.position.z);
        }

        if (follow)
        {
            FollowMouse();
        }
    }

    private void OnMouseOver()
    {
        // DAOUD 1: is this cheaper than direct comparison?
        if (Input.GetMouseButtonDown(0))
        {
            follow = true;
        }

    }

    void FollowMouse ()
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
        if (hit.transform == null)
            rb2D.MovePosition(newPos);
    }


}
