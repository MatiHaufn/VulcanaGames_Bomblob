using UnityEngine;

public class EisSorte : MonoBehaviour
{
    [SerializeField] GameObject _parentObject;
    [SerializeField] bool _isBlob;
    GameObject _objectBase; 
    GameObject _objectDecoration; 
    public enum Sorte { Erdbeere, Vanille, Minze, Blaubeere }
    public Sorte _sortenIndex;

    Color _erdbeereColor;
    Color _vanilleColor;
    Color _minzeColor;
    Color _blaubeereColor;

    Color _currentColor;

    float _colorIntensity = 1; 
    int _randomSortenIndex;

    private void Start()
    {
        _erdbeereColor = new Color(0.99f, 0.47f, 0.99f);
        _vanilleColor = new Color(0.9f, 0.8f, 0.4f);
        _minzeColor = new Color(0.48f, 0.91f, 0.51f);
        _blaubeereColor = new Color(0.136f, 0.124f, 0.613f);
        SetSorte();
    }



    void SetSorte()
    {
        _sortenIndex = (Sorte)_randomSortenIndex;

        InstantiateFallingObject();

        switch (_sortenIndex)
        {
            case Sorte.Erdbeere:
                SetColor(_erdbeereColor);
                break;
            case Sorte.Vanille:
                SetColor(_vanilleColor);
                break;
            case Sorte.Minze:
                SetColor(_minzeColor);
                break;
            case Sorte.Blaubeere:
                SetColor(_blaubeereColor);
                break;
        }
    }


    //Fills GameObject from GameManager to this object.
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

            _colorIntensity = 0.25f;
        }
        else
        {
            _objectBase = Instantiate(GameManager._instance._futterVersions[(int)_sortenIndex]);

            _objectBase.transform.SetParent(_parentObject.transform);
            _objectBase.transform.position = _parentObject.transform.position;

            _colorIntensity = 1;
        }
    }

    void SetColor(Color color)
    {
        _currentColor = color;
        _objectBase.GetComponent<MeshRenderer>().material.color = _currentColor * _colorIntensity;
    }

    //Wird in FutterSpawner beim Spawn ausgeführt
    public int RandomSorte()
    {
        _randomSortenIndex = Random.Range(0, 4);
        SetSorte();
        return _randomSortenIndex;
    }

    public string GetColor() { return _sortenIndex.ToString(); }
}
