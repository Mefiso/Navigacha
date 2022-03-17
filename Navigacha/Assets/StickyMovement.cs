using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyMovement : MonoBehaviour
{
    private bool follow = false;
    // Start is called before the first frame update
    void Start()
    {

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
        // TODO: Replace magic numbers for global variables "map boundaries"
        if (mousePos.x < -3 || mousePos.x > 4)
        {
            mousePos.x = transform.position.x;
        }
        if (Mathf.Abs(mousePos.y) > 6)
        {
            mousePos.y = transform.position.y;
        }

        transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
    }
}
