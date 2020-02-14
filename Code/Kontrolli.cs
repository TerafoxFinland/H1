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
    //Prefabs 
    public GameObject laatta1;
    public GameObject laatta2;
    public GameObject laatta3;
    public GameObject laatta4;
    public GameObject laatta5;

    // Use this for initialization
    void Awake () {
        fRuudukko = new float[10,10];
        iRuudukko = new int[10, 10, 2];
       
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

    public void KirjoitaNappulat()
    {
        Debug.Log("KIRJOITTAMISTA PYYDETTY");
        //Start the coroutine
        StartCoroutine(CoroutineKirjoitaNappulat());
        //KirjoitaNappulat();
    }

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
                //Debug.Log("transform.position = " + transform.position);
                //s = s + iRuudukko[j, i, 0] + " ";
                //Debug.Log("Solun tieto 0: ruutu " + j + " " + i + " " + iRuudukko[j, i, 0]); // 
                //Debug.Log("Solun tieto 1: " + iRuudukko[j, i, 1]);
            }
        }
       

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(2);

        //After we have waited 2 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }

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
            int ix = pos.x + 5;
            int iy = pos.y + 5;
            Debug.Log("Nappulan " + n + " sijainti: " + pos + " tunnus " + id);
            Debug.Log("Nappulan " + n + " koordinaatit: x " + ix + " y " + iy);
            n++;
        }
    }

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

	// Update is called once per frame
	void Update () {

        //if (laattaValittu)
        //{
        //   // Debug.Log("Something selected");
        //}
	}
}
