using UnityEngine;

public class EisSorte : MonoBehaviour
{
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
        Destroy(_objectDecoration);
        if (_isBlob)
        {
            _objectBase = Instantiate(GameManager._instance._blobVersions[(int)_sortenIndex]);
            _objectDecoration = Instantiate(GameManager._instance._blobAccessoires[Random.Range(0, 4)]);

            _objectBase.transform.SetParent(_parentObject.transform);
            _objectDecoration.transform.SetParent(_parentObject.transform);

            _objectBase.transform.position = _parentObject.transform.position;
            _objectDecoration.transform.position = _parentObject.transform.position;

            if(this.gameObject.GetComponent<BlobMovement>() != null)
                this.gameObject.GetComponent<BlobMovement>().SetMeshRenderer(_objectBase); 
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
