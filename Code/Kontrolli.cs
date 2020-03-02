using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
using UnityEngine.Tilemaps;


public class Kontrolli : MonoBehaviour {
    public static Kontrolli instance = null;                //Static instance of GameManager which allows it to be accessed by any other script.
    [HideInInspector]
    public bool laattaValittu = false;
    [HideInInspector]
    public bool blokkiValittu = false;
    [HideInInspector]
    public bool swapped = false;
    [HideInInspector]    
    public GameObject[] nappulat;
    [HideInInspector]
    public float[,] fRuudukko;
    [HideInInspector]
    public int[,,] iRuudukko;
    int i, j, h;
    private Grid m_Grid;
    private Tilemap tilemap;
    Vector3 position;
    Vector3Int gridPos;
    [HideInInspector]
    public Vector3 swapPosition;
    [HideInInspector]
    public Vector3 swapTarget;
    [HideInInspector]
    public Vector3Int swapGridPos;
    //Prefabs 
    public GameObject laatta1;
    public GameObject laatta2;
    public GameObject laatta3;
    public GameObject laatta4;
    public GameObject laatta5;

    // Use this for initialization
    void Awake () {
        fRuudukko = new float[10,10];
        iRuudukko = new int[10, 10, 3]; //koordinaatit, id, tyyppi, merkattu = 1
       
        //Check if instance already exists
        if (instance == null)
            //if not, set instance to this
            instance = this;
        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        m_Grid = GameObject.Find("Grid").GetComponent<Grid>();
        tilemap = GameObject.Find("Base").GetComponent<Tilemap>();
    }

    //KÄYNNISTETÄÄN NAPPULOIDEN LUONTI
    public void KirjoitaNappulat()
    {
        Debug.Log("KIRJOITTAMISTA PYYDETTY");
        //Start the coroutine
        StartCoroutine(CoroutineKirjoitaNappulat());
        //KirjoitaNappulat();
    }

    //LUODAAN NAPPULAT
    IEnumerator CoroutineKirjoitaNappulat()
    {
        //testin ajaksi palataan heti
        //return;

        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        GameObject newObject;
        GameObject laatta;
        //string s = "";
        for (j = 0; j < 10; j++)
        {
            for (i = 0; i < 10; i++)
            {
                gridPos = new Vector3Int((j - 5),  (i - 5), 0);
                position = m_Grid.CellToWorld(gridPos);

                // Generate the new object
                int r = Random.Range(1, 5);
                laatta = laatta1;
                if (r == 1) laatta = laatta1;
                if (r == 2) laatta = laatta2;
                if (r == 3) laatta = laatta3;
                if (r == 4) laatta = laatta4;
                if (r == 5) laatta = laatta5;

                newObject = Instantiate<GameObject>(laatta);
                newObject.transform.position = position;
                Vector3Int cellPosition = tilemap.LocalToCell(newObject.transform.localPosition);
                newObject.transform.localPosition = tilemap.GetCellCenterLocal(cellPosition);

                iRuudukko[j, i, 1] = r; //kirjoitetaan tyyppi ruudukkoon

                //Debug.Log("transform.position = " + transform.position);
                //s = s + iRuudukko[j, i, 0] + " ";
                //Debug.Log("Solun tieto 0: ruutu " + j + " " + i + " " + iRuudukko[j, i, 0]); // 
                //Debug.Log("Solun tieto 1: " + iRuudukko[j, i, 1]);
            }
        }
       
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(2);
        //After we have waited 2 seconds print the time again.
        //Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }

    //TULOSTETAAN KONSOLILLE
    public void LueNappulat()
    {
        Debug.Log("LUETAAN NAPPULAT... ");
        int n = 1;

        //if (nappulat == null)
        {
            
            nappulat = GameObject.FindGameObjectsWithTag("Nappula");
        }
        foreach (GameObject go in nappulat)
        {
            Vector3Int pos = go.GetComponent<MoveSelectedInGridWithMouse>().gridPos;
            int id = go.GetComponent<MoveSelectedInGridWithMouse>().idNumber;
            int type = go.GetComponent<MoveSelectedInGridWithMouse>().type;
            int ix = pos.x + 5;
            int iy = pos.y + 5;
            Debug.Log("Nappulan " + n + " sijainti: " + pos + " tunnus " + id);
            Debug.Log("Nappulan " + n + " tyyppe: " +  type);
            Debug.Log("Nappulan " + n + " koordinaatit: x " + ix + " y " + iy);
            n++;
        }
    }

    //TULOSTETAAN KONSOLILLE
    public void LueTaulukko()
    {
        //testin ajaksi palataan heti
        //return;
        string s = "";
        for(j = 0; j < 10; j++)
        {
            for(i = 0; i < 10; i++)
            {
                //s = s + iRuudukko[j, i, 0] + " \r\n";
                //s = s + iRuudukko[j, i, 0] + " ";
                Debug.Log("Solun tieto 0: ruutu " + j + " " + i + " " +  iRuudukko[j, i, 0]); // 
                Debug.Log("Solun tieto 1: " + iRuudukko[j, i, 1]);
            }
        }
        //s = s.Replace("\n", System.Environment.NewLine);
        //s = s.Replace("\n", "A");
        //EditorGUIUtility.systemCopyBuffer = s;
        //Debug.Log("Solun tieto 0: " + s);
        print("Solun tieto 0: " + s);
    }

    //TÄMÄ METODI EI KÄYTÖSSÄ
    public void CheckMatches()
    {
        HashSet<GameObject> matchedTiles = new HashSet<GameObject>();
        for (int row = 0; row < 10; row++)
        {
            for (int column = 0; column < 10; column++)
            {
                GameObject current = GetGameObjectAt(column, row);

                List<GameObject> horizontalMatches = FindColumnMatchForTile(column, row, current);
                if (horizontalMatches.Count >= 2)
                {
                    matchedTiles.UnionWith(horizontalMatches);
                    matchedTiles.Add(current);
                }

                List<GameObject> verticalMatches = FindRowMatchForTile(column, row, current);
                if (verticalMatches.Count >= 2)
                {
                    matchedTiles.UnionWith(verticalMatches);
                    matchedTiles.Add(current);
                }
            }
        }

        foreach (GameObject go in matchedTiles)
        {
            //renderer.sprite = null;
            //go = null;
        }
        //Score += matchedTiles.Count;
        //return matchedTiles.Count > 0;
        //return matchedTiles.Count > 0;
    }

    //TÄMÄ METODI EI KÄYTÖSSÄ
    List<GameObject> FindColumnMatchForTile(int col, int row, GameObject go)
    {
        List<GameObject> result = new List<GameObject>();
        for (int i = col + 1; i < 10; i++)
        {
            GameObject nextColumn = GetGameObjectAt(i, row);
            if (nextColumn.gameObject != go)
            {
                break;
            }
            result.Add(nextColumn);
        }
        return result;
    }

    //TÄMÄ METODI EI KÄYTÖSSÄ
    List<GameObject> FindRowMatchForTile(int col, int row, GameObject go)
    {
        List<GameObject> result = new List<GameObject>();
        for (int i = row + 1; i < 10; i++)
        {
            GameObject nextRow = GetGameObjectAt(col, i);
            if (nextRow.gameObject != go)
            {
                break;
            }
            result.Add(nextRow);
        }
        return result;
    }

    //TÄMÄ METODI EI KÄYTÖSSÄ
    GameObject GetGameObjectAt(int column, int row)
    {
        GameObject tile = null;
        int n = 0;
        if (column < 0 || column >= 10
         || row < 0 || row >= 10)
            return null;
        
            nappulat = GameObject.FindGameObjectsWithTag("Nappula");
        foreach (GameObject go in nappulat)
        {
            Vector3Int pos = go.GetComponent<MoveSelectedInGridWithMouse>().gridPos;
            int id = go.GetComponent<MoveSelectedInGridWithMouse>().idNumber;
            int ix = pos.x + 5;
            int iy = pos.y + 5;
            Debug.Log("Nappulan " + n + " sijainti: " + pos + " tunnus " + id);
            Debug.Log("Nappulan " + n + " koordinaatit: x " + ix + " y " + iy);
            n++;
            if(iy == row && ix == column) {
                tile = go;
            //SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
            //return renderer;
            }
        }
        return tile;
    }

    // ETSITÄÄN (VÄHINTÄÄN) KOLME SAMAA VAAKA- JA PYSTYSUUNNASSA
    public void Find3Line()
    {
        //testin ajaksi palataan heti
        //return;
        //string s = "";
        int type;
        int type2;
        int type3;
        for (j = 0; j < 10; j++)
        {
            for (i = 0; i < 10; i++)
            {
                //TSEKKAA VIEREISET PYSTYSUUNNASSA JA MERKITSE
                if (i < 8)
                {
                    type =  iRuudukko[j, i, 1];
                    type2 = iRuudukko[j, i + 1, 1];
                    type3 = iRuudukko[j, i + 2, 1];
                    if (type == type2 && type2 == type3)
                    {
                        Debug.Log("Solun tyyppi: ruutu " + j + " " + i + " " + type); // 
                        //merkkaa solut
                        iRuudukko[j, i, 2] = 1;
                        iRuudukko[j, i + 1, 2] = 1;
                        iRuudukko[j, i + 2, 2] = 1;

                    }
                }
                //s = s + iRuudukko[j, i, 0] + " ";
                Debug.Log("Solu merkitty: " + iRuudukko[j, i, 2]);
            }
        }

        for (j = 0; j < 10; j++)
        {
            for (i = 0; i < 10; i++)
            {
                //TSEKKAA VIEREISET VAAKASUUNNASSA JA MERKITSE
                if (j < 8)
                {
                    type = iRuudukko[j, i, 1];
                    type2 = iRuudukko[j + 1, i, 1];
                    type3 = iRuudukko[j + 2, i, 1];
                    if (type == type2 && type2 == type3)
                    {
                        Debug.Log("Solun tyyppi: ruutu " + j + " " + i + " " + type); // 
                        //merkkaa solut
                        iRuudukko[j, i, 2] = 1;
                        iRuudukko[j + 1, i, 2] = 1;
                        iRuudukko[j + 2, i, 2] = 1;

                    }
                }
                //s = s + iRuudukko[j, i, 0] + " ";
                Debug.Log("Solu merkitty: " + iRuudukko[j, i, 2]);
            }
        }

        MarkCells();
    }

    public void MarkCells()
    {
        GameObject nappu;
        SpriteRenderer ren;
        Debug.Log("LUETAAN NAPPULAT... ");
        int n = 1;
        //if (nappulat == null)
        {

            nappulat = GameObject.FindGameObjectsWithTag("Nappula");
        }
        foreach (GameObject go in nappulat)
        {
            nappu = go;
            Vector3Int pos = go.GetComponent<MoveSelectedInGridWithMouse>().gridPos;
            //int id = go.GetComponent<MoveSelectedInGridWithMouse>().idNumber;
            //int type = go.GetComponent<MoveSelectedInGridWithMouse>().type;
            int ix = pos.x + 5;
            int iy = pos.y + 5;
            if(iRuudukko[ix, iy, 2] == 1) //merkitty paikka
            {
                ren = nappu.GetComponent<SpriteRenderer>();
                ren.color = Color.red;
            }
            if (iRuudukko[ix, iy, 2] == 2) //merkitty paikka
            {
                ren = nappu.GetComponent<SpriteRenderer>();
                ren.color = Color.green;
            }
            //Debug.Log("Nappulan " + n + " sijainti: " + pos + " tunnus " + id);
            //Debug.Log("Nappulan " + n + " tyyppe: " + type);
            //Debug.Log("Nappulan " + n + " koordinaatit: x " + ix + " y " + iy);
            n++;

        }

    }

    public void RemoveCells()
    {
        GameObject nappu;
       
        Debug.Log("LUETAAN NAPPULAT... ");
        int n = 1;
        //if (nappulat == null)
        {

            nappulat = GameObject.FindGameObjectsWithTag("Nappula");
        }
        foreach (GameObject go in nappulat)
        {
            nappu = go;
            Vector3Int pos = go.GetComponent<MoveSelectedInGridWithMouse>().gridPos;
            //int id = go.GetComponent<MoveSelectedInGridWithMouse>().idNumber;
            //int type = go.GetComponent<MoveSelectedInGridWithMouse>().type;
            int ix = pos.x + 5;
            int iy = pos.y + 5;
            if (iRuudukko[ix, iy, 2] == 1 || iRuudukko[ix, iy, 2] == 2) //merkitty paikka
            {
                Destroy(nappu);
            }
            //Debug.Log("Nappulan " + n + " sijainti: " + pos + " tunnus " + id);
            //Debug.Log("Nappulan " + n + " tyyppe: " + type);
            //Debug.Log("Nappulan " + n + " koordinaatit: x " + ix + " y " + iy);
            n++;
        }

    }

    // ETSITÄÄN POTENTIAALISET KUVIOT
    public void FindPotentials()
    {
        //testin ajaksi palataan heti
        //return;
        //string s = "";
        int type = 0;
        int type2 = 0;
        int type3 = 0;
        for (j = 0; j < 10; j++)
        {
            for (i = 0; i < 10; i++)
            {
                //TSEKKAA POTENTIAALISET VIEREISET PYSTYSUUNNASSA JA MERKITSE
                
                {
                    type = iRuudukko[j, i, 1];
                    //type2 = iRuudukko[j, i + 1, 1];
                    if (i < 7 && j < 7)
                    {
                        type3 = iRuudukko[j + 1, i + 1, 1];
                    
                    if (type == type3)
                    {
                        iRuudukko[j, i, 2] = 2;
                        //iRuudukko[j - 1, i + 1, 2] = 2;
                        iRuudukko[j + 1, i + 1, 2] = 2;

                    }
                    }
                }
                //s = s + iRuudukko[j, i, 0] + " ";
                //Debug.Log("Solu merkitty: " + iRuudukko[j, i, 2]);
            }
        }

        for (j = 0; j < 10; j++)
        {
            for (i = 0; i < 10; i++)
            {
                //TSEKKAA VIEREISET POTENTIAALISET VAAKASUUNNASSA JA MERKITSE
                //if (j < 7)
                
                    
                    if (i < 7 && j < 7)
                {
                    type = iRuudukko[j, i, 1];
                    type2 = iRuudukko[j + 1, i, 1];
                    {
                        type3 = iRuudukko[j + 2, i, 1];
                    }
                    if (type == type2 && type2 == type3)
                    {
                        Debug.Log("POTENTIAALI MERKITTY !: ruutu " + j + " " + i + " " + type); // 
                        //merkkaa solut
                        iRuudukko[j, i, 2] = 2;
                        iRuudukko[j + 1, i, 2] = 2;
                        iRuudukko[j + 2, i, 2] = 2;

                    }
                }
                //s = s + iRuudukko[j, i, 0] + " ";
                //Debug.Log("Solu merkitty: " + iRuudukko[j, i, 2]);
            }
        }

        MarkCells();
    }

    public void SwapTiles(Vector3 start, Vector3 target, Vector3Int swapGrid)
    {
        swapPosition = start;
        swapTarget = target;
        swapGridPos = swapGrid;
        Debug.Log("SWAP TARGET: " + swapTarget);
    }


    // Update is called once per frame
    void Update () {

        //if (laattaValittu)
        //{
        //   // Debug.Log("Something selected");
        //}
	}
}
