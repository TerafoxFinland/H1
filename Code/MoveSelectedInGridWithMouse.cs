using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MoveSelectedInGridWithMouse : MonoBehaviour {
    public int idNumber; //nappulan tunniste
    [HideInInspector]
    public bool selected = false;
    bool moved = true;
    bool clicked = false;
    private GameObject go;
    private Kontrolli kontrolli;
    float speed = 6f;
    Vector2 targetPos;
    Vector2 startPos;
    float startTime;
    float timeDifference;
    private Grid m_Grid;
    [HideInInspector]
    public Vector3Int gridPos;
    //Vector3 center;
    Tilemap tilemap;

    // Use this for initialization
    void Start () {
        go = GameObject.Find("Handler");
        kontrolli = go.GetComponent<Kontrolli>();
        targetPos = transform.position;
        m_Grid = GameObject.Find("Gridi").GetComponent<Grid>();
        tilemap = GameObject.Find("BaseMatrix").GetComponent<Tilemap>();
    }
    void OnMouseDown()
    {
        startTime = Time.time;
        Debug.Log("START TIME " + startTime);

        if (!kontrolli.blokkiValittu && !clicked)
        {
            Debug.Log("VALINTA TEHTY");
            clicked = true;
            Debug.Log("Clicked " + clicked);
            selected = true;
            kontrolli.blokkiValittu = true;
            Debug.Log(gameObject + " selected");
            startPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("START POS " + startPos);
            
        }
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0) && clicked)
        {
            timeDifference = Time.time - startTime;
            Debug.Log("Time difference" + timeDifference);
            if (timeDifference > 0.1f)
            {
                targetPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Debug.Log("Liikkeelle lähdetty" + targetPos);
                moved = true;
            }
        }
        if ((Vector2)transform.position != targetPos && selected && moved)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
        if ((Vector2) transform.position == targetPos && moved)
        {
            moved = false;
            selected = false;
            Debug.Log("Kohde saavutettu" + targetPos);
            kontrolli.blokkiValittu = false;
            clicked = false;

            //Debug.Log("targetPos = " + targetPos);
            Vector3 world = Camera.main.ScreenToWorldPoint(targetPos);
            gridPos = m_Grid.WorldToCell(world);
            Debug.Log("gridPos = " + gridPos);

            //Kirjoita taulukkoon
            kontrolli.iRuudukko[gridPos.x, gridPos.y, 0] = idNumber;

            
            Vector3Int cellPosition = tilemap.LocalToCell(transform.localPosition);
            transform.localPosition = tilemap.GetCellCenterLocal(cellPosition);
            Debug.Log("transform.position = " + transform.position);
            
        }
    }
}
