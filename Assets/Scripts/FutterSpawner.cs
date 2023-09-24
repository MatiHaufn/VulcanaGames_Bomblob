using System.Collections.Generic;
using UnityEngine;

public class FutterSpawner : MonoBehaviour
{
    //FUTTER
    List<GameObject> _futterContainer = new List<GameObject>(); 
    [SerializeField] GameObject _futterPrefab;  
    [SerializeField] GameObject _futterParent; 

    float _futterSpawnTimer = 0; // futter fällt direkt am Anfang runter
    int _futterSpawnMaxTimer;
    int _maxFutter;
    float _futterTimeMultiplicator; 

    //BLOBS
    List<GameObject> _blobContainer = new List<GameObject>(); 
    [SerializeField] GameObject _blobPrefab;
    [SerializeField] GameObject _blopParent; 

    float _blobSpawnTimer = 100; // Blob fällt direkt am Anfang runter
    float _blobTimeMultiplicator;
    int _blobSpawnMaxTimer;
    int _maxBlobs;

    //GENERAL
    Vector3 _startPosition;
    float _xPosition;

    private void Start()
    {
        _futterTimeMultiplicator = GameManager._instance._futterTimeMultiplicator;
        _blobTimeMultiplicator = GameManager._instance._blobTimeMultiplicator;
        _futterSpawnMaxTimer = GameManager._instance._futterSpawnRate;
        _blobSpawnMaxTimer = GameManager._instance._blopSpawnRate; 
    }

    private void Update()
    {
        _futterSpawnTimer += Time.deltaTime * _futterTimeMultiplicator;
        if (_futterSpawnTimer >= _futterSpawnMaxTimer)
        {
            Spawner(_futterPrefab, _futterParent, _maxFutter, "newFutter", _futterContainer);
            _futterSpawnTimer = 0; 
        }

        _blobSpawnTimer += Time.deltaTime * _blobTimeMultiplicator;
        if (_blobSpawnTimer >= _blobSpawnMaxTimer)
        {
            Spawner(_blobPrefab, _blopParent, _maxBlobs, "newBlob", _blobContainer);
            _blobSpawnTimer = 0; 
        }
    }

    void Spawner(GameObject prefab, GameObject parent, int maxAmount, string objName, List<GameObject> container) 
    {
        if (container.Count < maxAmount)
        {
            GameObject newObj = Instantiate(prefab);

            newObj.name = objName;
            newObj.GetComponent<EisSorte>().RandomSorte(); //Sorte wird random berechnet
            newObj.transform.SetParent(parent.transform);
            container.Add(newObj);
            SetStartPosition(newObj);
            if (newObj.GetComponent<FutterMovement>() != null)
                newObj.GetComponent<FutterMovement>().ResetFutterPositionRotation();
        }
        else
        {
            for (int i = 0; i < maxAmount; i++)
            {
                if (container[i].gameObject.activeSelf == false)
                {
                    container[i].GetComponent<EisSorte>().RandomSorte();
                    SetVisible(container[i]);
                    break;
                }
            }
        }
    }
    public void MaxEinheiten(int maxFutter, int maxBlobs) { _maxFutter = maxFutter; _maxBlobs = maxBlobs; } //Übertrag von MaxEinheiten vom GameManager
    public void SetStartPosition(GameObject spawnEinheit)
    {
        _startPosition = new Vector3(RandomizedXPosition(), GameManager._instance._spawnLine, transform.position.z);
        spawnEinheit.transform.position = _startPosition;  
    }

    //SetVisible zum "Recyclen" von Futter und Blops, nachdem es unsichtbar gemacht wurde
    public void SetVisible(GameObject spawnEinheit)
    {
        spawnEinheit.gameObject.SetActive(true);
        SetStartPosition(spawnEinheit);
    }
    float RandomizedXPosition()
    {
        _xPosition = Random.Range(GameManager._instance._startCoordinatesGrid.x, GameManager._instance._endCoordinatesGrid.x);
        return _xPosition;
    }

}
