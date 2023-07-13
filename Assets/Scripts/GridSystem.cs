using System.Collections.Generic;
using UnityEngine;


//Script to spawn the Grid for placing the not solid blobs
public class GridSystem : MonoBehaviour
{
    [SerializeField] GameObject _gridTile; //Loading grid tile Prefab 

    List<GameObject> _gridTiles = new List<GameObject>();   
    Vector2 _startCoordinatesGrid;
    Vector2 _endCoordinatesGrid;
    Vector2 _distanceCoordinates;
    Vector2 _gridDepth;
    float _maxRow; 
    float _gridSize; 
   
    void Start()
    {
        SetVariables();
        _maxRow = GameManager._instance._rowAmount; 
        _distanceCoordinates = _endCoordinatesGrid - _startCoordinatesGrid;
        _gridDepth = new Vector2(_distanceCoordinates.x / _gridSize, _distanceCoordinates.y / _gridSize );
        DrawGrid(); 
    }

    public void DrawGrid()
    {
        //Debug.Log("Created new Grid");
        SetVariables(); 
        foreach (GameObject tile in _gridTiles)
        {
            Destroy(tile);
        }
        _gridTiles.Clear();

        for (float x = 0; x <= _gridDepth.x; x += _gridSize)
        {
            for (float y = 0; y <= _maxRow; y += _gridSize)
            {

                if (_startCoordinatesGrid.x + x < _endCoordinatesGrid.x && _startCoordinatesGrid.y + y < _endCoordinatesGrid.y)
                {
                    GameObject newGridTile = Instantiate(_gridTile);
                    _gridTiles.Add(newGridTile);
                    newGridTile.transform.localScale = new Vector3(_gridSize, _gridSize, 0.01f);
                    newGridTile.transform.SetParent(this.transform);
                    newGridTile.transform.position = new Vector3(_startCoordinatesGrid.x + x, _startCoordinatesGrid.y + y, transform.position.z);
                }
                else
                    break;
            }
        }
    }
    void SetVariables()
    {
        this.transform.position = new Vector3(transform.position.x, transform.position.y, GameManager._instance._mainZAxis - 0.5f);
        _startCoordinatesGrid = GameManager._instance._startCoordinatesGrid;
        _endCoordinatesGrid = GameManager._instance._endCoordinatesGrid;
        _gridDepth = GameManager._instance._gridDepth;
        _gridSize = GameManager._instance._gridSize;
    }

}
