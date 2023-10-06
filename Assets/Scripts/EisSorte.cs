using System.Collections.Generic;
using UnityEngine;

public class EisSorte : MonoBehaviour
{
    [SerializeField] List<GameObject> _blobAccessoires = new List<GameObject>();
    [SerializeField] GameObject _parentObject;
    [SerializeField] bool _isBlob;


    GameObject _objectBase; 
    GameObject _objectDecoration; 
    public enum Sorte { Erdbeere, Vanille, Minze, Blaubeere }
    public Sorte _sortenIndex; 
    int _randomSortenIndex;

    private void Start()
    {
        SetSorte();
    }

    void SetSorte()
    {
        _sortenIndex = (Sorte)_randomSortenIndex;
        InstantiateFallingObject();
    }

    //Füllt das GameObject im GameManager
    void InstantiateFallingObject()
    {
        Destroy(_objectBase);
        foreach(GameObject obj in _blobAccessoires)
        {
            obj.SetActive(false);
        }

        if (_isBlob)
        {
            _objectBase = Instantiate(GameManager._instance._blobVersions[(int)_sortenIndex]);
            int randomNumber = Random.Range(0, 4);
            _blobAccessoires[randomNumber].SetActive(true);

            _objectBase.transform.SetParent(_parentObject.transform);

            _objectBase.transform.position = _parentObject.transform.position;

            gameObject.GetComponent<BlobMovement>().SetMeshRenderer(_objectBase);
            gameObject.GetComponent<BlobMovement>().ResetColorAfterRespawn(); 
        }
        else
        {
            _objectBase = Instantiate(GameManager._instance._futterVersions[(int)_sortenIndex]);

            _objectBase.transform.SetParent(_parentObject.transform);
            _objectBase.transform.position = _parentObject.transform.position;
        }
    }

    //Wird in FutterSpawner beim Spawn ausgeführt
    public int RandomSorte()
    {
        _randomSortenIndex = Random.Range(0, 4);
        SetSorte();
        return _randomSortenIndex;
    }

    public string GetSorte() { return _sortenIndex.ToString(); }
}
