using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSelectedWithMouse : MonoBehaviour {
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
    // Use this for initialization
    void Awake () {
        go = GameObject.Find("Handler");
        kontrolli = go.GetComponent<Kontrolli>();
        targetPos = transform.position;
    }
    void OnMouseDown()
    {
        startTime = Time.time;
        Debug.Log("START TIME " + startTime);

        if (!kontrolli.laattaValittu && !clicked)
        {
            Debug.Log("VALINTA TEHTY");
            clicked = true;
            Debug.Log("Clicked " + clicked);
            selected = true;
            kontrolli.laattaValittu = true;
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
            Debug.Log("Kohde saavutettu");
            kontrolli.laattaValittu = false;
            clicked = false;
        }
    }
}
