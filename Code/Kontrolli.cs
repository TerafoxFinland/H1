using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

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
    }

    void Helou()
    {
       

        //if (nappulat == null)
        //{
        //    nappulat = GameObject.FindGameObjectsWithTag("Nappula");
        //}
        //LueNappulat();
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
            Debug.Log("Nappulan sijainti: " + pos + " " + n);
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
                Debug.Log("Solun tieto 0: " + iRuudukko[j, i, 0]); // + j + " " + i); 
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
