using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public GameObject hazardPrefab;
    public GameObject enemyPrefab;
    public GameObject interactablePrefab;

    private GameObject currentPrefab;
    // Start is called before the first frame update
    void Start()
    {
        currentPrefab = obstaclePrefab;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(currentPrefab, Helpers.MapUtils.PositionToGrid(Camera.main.ScreenToWorldPoint(Input.mousePosition)), Quaternion.identity);
        }
    }
}
