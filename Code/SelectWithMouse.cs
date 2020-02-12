using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectWithMouse : MonoBehaviour {

    [HideInInspector]
    public bool selected = false;
    private GameObject go;
    private Kontrolli kontrolli;



	// Use this for initialization
	void Start () {
		go = GameObject.Find("Handler");
        kontrolli = go.GetComponent<Kontrolli>();

    }
    void OnMouseEnter() { Debug.Log("I am over something"); }

    void OnMouseDown() { 
        if (!kontrolli.laattaValittu)
        {
            selected = true;
            kontrolli.laattaValittu = true;
            Debug.Log(gameObject + " selected");
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
