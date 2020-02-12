using UnityEngine;
public class MoveToClickInput : MonoBehaviour
{
    float speed = 6f;
    Vector2 targetPos;
    private void Start()
    {
        targetPos = transform.position;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            targetPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if ((Vector2)transform.position != targetPos)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
    }
}