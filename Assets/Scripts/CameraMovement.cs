using System.Collections;
using UnityEngine;

//Is moving objects down to simulate a camera scrolling
public class CameraMovement : MonoBehaviour
{
    public bool _currentlyMoving = false;
    
    Vector3 _destination;
    Vector3 _startPosition;

    float _movingDuration;
    float _distance; 

    private void Start()
    {
        GameManager._instance._CameraMovementObjects.Add(this); 

        _movingDuration = GameManager._instance._camMovementDuration;
        _distance = GameManager._instance._camMoveDistance; 

        _destination = new Vector3(0, -_distance, 0);
        _startPosition = transform.position;
    }

    public void StartMovingCamera()
    {
        if(!_currentlyMoving)
            StartCoroutine(MoveCamera(_movingDuration));
    }


    IEnumerator MoveCamera(float duration)
    {
        _currentlyMoving = true;
        float startTime = Time.time; // Zeitpunkt, zu dem die Bewegung begonnen hat
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration; // Wert zwischen 0 und 1, abhängig von der vergangenen Zeit
            transform.position = Vector3.Lerp(_startPosition, _startPosition + _destination, t);

            // Hier kannst du auch eine Check-Distanz verwenden, wenn du möchtest.
            // if (Vector3.Distance(transform.position, _startPosition + _destination) <= _offset)

            elapsedTime = Time.time - startTime; // Aktualisiere die vergangene Zeit
            yield return null;
        }

        // Stelle sicher, dass das Ziel erreicht wird.
        transform.position = _startPosition + _destination;

        _currentlyMoving = false;
        _startPosition = transform.position;

        yield return null;
    }
    /*
    IEnumerator MoveCamera(float duration)
    {
        _currentlyMoving = true; 
        float elapsedTime = 0;
        while(transform.position != _startPosition + _destination)
        {
            transform.position = Vector3.Lerp(transform.position, _startPosition + _destination, elapsedTime / duration);
            
            if(Vector3.Distance(transform.position, _startPosition + _destination) <= _offset) 
            {
                transform.position = _startPosition + _destination; 
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _currentlyMoving = false;
        elapsedTime = 0; 
        _startPosition = transform.position; 

        yield return null;
    }*/
}
