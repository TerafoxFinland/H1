using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MoveSelectedInGridWithMouse : MonoBehaviour 
{
    public int idNumber; //nappulan tunniste
    public int type; //nappulan tyyppi
    [HideInInspector]
    public bool selected = false;
    bool moved = false;
    bool clicked = false;
    private GameObject go;
    private Kontrolli kontrolli;
    float speed = 6f;
    Vector2 targetPos;
    Vector3 startPos;
    float startTime;
    float timeDifference;
    private Grid m_Grid;
    private Vector3 mousePosition;
    //private Vector3 world;
    [HideInInspector]
    public Vector3Int gridPos;
    [HideInInspector]
    public Vector3Int gridPos0;
    //Vector3 center;
    Tilemap tilemap;
    int ix, iy, fx, fy;
    SpriteRenderer sr;

    // Use this for initialization
    void Start () {
        go = GameObject.Find("Handler");
        kontrolli = go.GetComponent<Kontrolli>();
        sr = GetComponent<SpriteRenderer>();
        targetPos = transform.position;
        m_Grid = GameObject.Find("Grid").GetComponent<Grid>();
        tilemap = GameObject.Find("Base").GetComponent<Tilemap>();
        startTime = Time.time;
        Debug.Log("GAMEOBJECT ENABLED " + startTime + "POSITION " + targetPos);
        gridPos = m_Grid.WorldToCell(targetPos);
        Debug.Log("gridPos = " + gridPos);
        //Start the coroutine we define below named ExampleCoroutine.
        //StartCoroutine(ExampleCoroutine());
        //InitGame();
    }

    IEnumerator ExampleCoroutine()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(2);

        //After we have waited 2 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }

    void InitGame()
    {
        ix = gridPos.x + 5;
        iy = gridPos.y + 5;
        if (ix < 0) ix = 0;
        if (ix > 9) ix = 9;
        if (iy < 0) iy = 0;
        if (iy > 9) iy = 9;

        kontrolli.iRuudukko[ix, iy, 0] = idNumber;
        kontrolli.iRuudukko[ix, iy, 1] = type;
    }
    void OnMouseDown()
    {
        startTime = Time.time;
        //Debug.Log("START TIME " + startTime);

        if (!kontrolli.blokkiValittu && !clicked)
        {
            clicked = true;
            //Debug.Log("Clicked " + clicked);
            selected = true;
            //Debug.Log("VALINTA TEHTY selected " + selected );
            kontrolli.blokkiValittu = true;
            //Debug.Log(gameObject + " selected");
            startPos = transform.position;
            //Debug.Log("START POS " + startPos);
            gridPos0 = m_Grid.WorldToCell(startPos);
            sr.color = Color.cyan;
        }
        else
        {
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            gridPos = m_Grid.WorldToCell(targetPos);
            if (gridPos0 == gridPos) { Debug.Log("KOHTEET SAMAT");
                clicked = false;
                selected = false;
                kontrolli.blokkiValittu = false;
                sr.color = Color.white;
                return; }
        }
    }

    // Update is called once per frame
    void Update () 
    {
        if (m_Grid && Input.GetMouseButtonDown(0) && clicked)
        {
            timeDifference = Time.time - startTime;
            //Debug.Log("Time difference" + timeDifference);
            if (timeDifference > 0.1f)
            {
                targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //Debug.Log("Liikkeelle lähdetty" + targetPos);
                gridPos = m_Grid.WorldToCell(targetPos);
                if (gridPos0 == gridPos) return;
                else
                    moved = true;
                Debug.Log("gridPos != gridPos0");
            }
        }
        //if (transform.position != (Vector3) targetPos && selected && moved)
            if (selected && moved)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
        if (transform.position == (Vector3) targetPos && selected && moved)
        { //Debug.Log("Kohde saavutettu" + targetPos);
            moved = false;
            selected = false;
            kontrolli.blokkiValittu = false;
            clicked = false;
            sr.color = Color.white;
            //Kirjoita taulukkoon
            //KORJATTAVA KOORDINAATIT: LISÄTTÄVÄ ARVO, ETTEI MENE NEGATIIVISELLE
            //TARKISTUS MYÖS, ETTEI MENE YLI RAJAN!
            ix = gridPos.x + 5;
            iy = gridPos.y + 5;
            if (ix < 0) ix = 0;
            if (ix > 9) ix = 9;
            if (iy < 0) iy = 0;
            if (iy > 9) iy = 9;

            kontrolli.iRuudukko[ix, iy, 0] = idNumber;
            kontrolli.iRuudukko[ix, iy, 1] = type;
            //Keskitä ruudukkoon
            Vector3Int cellPosition = tilemap.LocalToCell(transform.localPosition);
            transform.localPosition = tilemap.GetCellCenterLocal(cellPosition);
        }
    }

    // Converts a Vector3Int to a [[Vector2Int]].
    //[MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
    //public static explicit operator Vector2Int(Vector3Int v)
    //{
    //    return new Vector2Int(v.x, v.y);
    //}
}
