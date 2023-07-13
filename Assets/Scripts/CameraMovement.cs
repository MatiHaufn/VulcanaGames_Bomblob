using System.Collections;
using UnityEngine;

//Is moving objects down to simulate a camera scrolling
public class CameraMovement : MonoBehaviour
{
    Vector3 _destination;
    Vector3 _startPosition;

    float _movingDuration = 2000f;
    float _distance = 0.5f; 

    bool _currentlyMoving = false;
    float _offset = 0.01f;

    float _timer = 0;
    int _maxTimer = 10; 

    private void Start()
    {
        GameManager._instance._CameraMovementObjects.Add(this); 

        _movingDuration = GameManager._instance._camMovementDuration;
        _distance = GameManager._instance._camDistance; 

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
    }
}
