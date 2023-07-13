using Unity.VisualScripting;
using UnityEditor; 
using UnityEngine;

public class FutterMovement : MonoBehaviour
{
    Vector3 movement = Vector3.zero;
    float currentSpeed;
    float bottomLine; 

    private void Start()
    {
        bottomLine = GameManager._instance._startCoordinatesGrid.y; //StartCoordinatesGrid ist unten
        currentSpeed = GameManager._instance._futterSpeedValue;
        movement = new Vector3(0, -currentSpeed);
    }

    private void Update()
    {
        if (GameManager._instance._debugMode)
        {
            currentSpeed = GameManager._instance._futterSpeedValue;
            movement = new Vector3(0, -currentSpeed);
        }

        transform.position += movement * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == ("Floor") || other.gameObject.tag == ("OutOfBounds"))
            SetInvisible();
    }

    public void SetInvisible()
    {
        this.gameObject.SetActive(false);
    }
}
