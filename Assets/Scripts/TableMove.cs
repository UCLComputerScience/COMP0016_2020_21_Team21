using UnityEngine;


public class TableMove : MonoBehaviour
{
    private Vector3 newPosition;

    private void Start()
    {
        enabled = false;
    }
    public void OnTableMove()
    {
        newPosition = new Vector3(transform.localPosition.x, transform.localPosition.y,
            transform.localPosition.z - 0.32f);
        enabled = true;
    }

    void Update()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, newPosition, 0.03f);
        if(transform.localPosition == newPosition)
        {
            enabled = false;
        }
    }
}
