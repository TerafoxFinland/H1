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
    bool swapable = false;
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
    [HideInInspector]
    public Vector3Int gridPos2;
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
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            gridPos = m_Grid.WorldToCell(targetPos);
            clicked = true;
            //Debug.Log("Clicked " + clicked);
            selected = true;
            kontrolli.swapped = false;
            swapable = false;
            //Debug.Log("VALINTA TEHTY selected " + selected );
            kontrolli.blokkiValittu = true;
            Debug.Log(gameObject + " selected");
            startPos = transform.position;
            //Debug.Log("START POS " + startPos);
            sr.color = Color.cyan;
            //kontrolli.SwapTiles(startPos, targetPos);
            kontrolli.SwapTiles(targetPos, startPos, gridPos);

        }
        else
        {
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            gridPos = m_Grid.WorldToCell(targetPos);
            gridPos0 = m_Grid.WorldToCell(startPos);
            //kontrolli.SwapTiles(startPos, targetPos);
            //Debug.Log("SWAPATAAN!");
            //kontrolli.SwapTiles(targetPos, startPos, gridPos);
            //Invoke("Swap", 0.5f);
           
            swapable = true;
            Debug.Log("KOHDE SWAPATAAN!" + gameObject);
            //Swap(gameObject);

            if (gridPos0 == gridPos)
            {
                Debug.Log("KOHTEET SAMAT");
                clicked = false;
                selected = false;
                kontrolli.blokkiValittu = false;
                sr.color = Color.white;
                return; 
            }
            //Invoke("MoveTile", 0.1f);
        } //MoveTile();
    }

    // Update is called once per frame
    void Update()
    {
        MoveTile();
        if(!kontrolli.swapped)         Swap(gameObject);
        
    }

    public void MoveTile()
    {
        //StartCoroutine(MoveCo());
        MoveIt();
    }

    public void MoveIt()
  
    //IEnumerator MoveCo()
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



                if (gridPos0 == gridPos) return; // yield return new WaitForSeconds(0.1f); 
                else
                    moved = true;
                Debug.Log("gridPos != gridPos0");
            }
           
               
        }
        if (selected && moved)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
           
        }
       
            


        if (transform.position == (Vector3) targetPos && selected && moved)
        {
                Debug.Log("1. Kohde saavutettu" + targetPos);
            //if (!kontrolli.swapped) Swap(gameObject);
            //if (!kontrolli.swapped && kontrolli.swapPosition == transform.position) Swap();
            //Swap();
            moved = false;
            selected = false;
            kontrolli.blokkiValittu = false;
            clicked = false;
            sr.color = Color.white;
            //Swap();
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

          //  yield return new WaitForSeconds(2);
        }



    }

    public void Swap(GameObject go)
    {
        StartCoroutine(SwapCo());
        //SwapIt(go);
    }



   IEnumerator SwapCo()
   //public void SwapIt(GameObject go)
    {
       
        //if (transform.position == (Vector3) targetPos)
        gridPos2 = m_Grid.WorldToCell(go.transform.position);
        //if (gridPos2 == kontrolli.swapGridPos && swapable)
            if (swapable)
            {
            sr.color = Color.yellow;
            //SWAPATTAVA NAPPULA/TIILI
            //if (!selected && !kontrolli.swapped)
            if(!kontrolli.swapped)
            {
                Debug.Log("LÄHDETÄÄN SWAPPAAMAAN" + targetPos);
                //targetPos = kontrolli.swapPosition;
                //targetPos = kontrolli.swapTarget;
                //gridPos = kontrolli.swapGridPos;
                transform.position = Vector2.MoveTowards(transform.position, kontrolli.swapTarget, speed * Time.deltaTime);
                //transform.position = targetPos;
                //transform.position = kontrolli.swapTarget; 

            }
            if (transform.position == kontrolli.swapTarget)
            {
                Debug.Log("Swap Kohde saavutettu" + kontrolli.swapTarget);
                kontrolli.swapped = true;
                swapable = false;
                //moved = false;
                //selected = false;
                //kontrolli.blokkiValittu = false;
                //clicked = false;
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
            moved = false;
        }
      yield return new WaitForSeconds(0.01f);
    }

    // Converts a Vector3Int to a [[Vector2Int]].
    //[MethodImpl(MethodImplOptionsEx.AggressiveInlining)]
    //public static explicit operator Vector2Int(Vector3Int v)
    //{
    //    return new Vector2Int(v.x, v.y);
    //}
}
