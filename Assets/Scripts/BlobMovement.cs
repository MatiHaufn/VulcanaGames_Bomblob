using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX; 

public class BlobMovement : MonoBehaviour
{
    public bool _isDragged = false;
    public bool _standing; 
    public bool _solid = false;
    public bool _justHitByExplosion = false;

    [SerializeField] GameObject _roundBlob; 
    [SerializeField] GameObject _edgeBlob;

    [SerializeField] GameObject _floorTracker;
    [SerializeField] float _maxGetSolidTimer = 1.5f;

    [SerializeField] MeshRenderer _meshRendererRoundBlop;
    [SerializeField] MeshRenderer _meshRendererEdgyBlop;
    [SerializeField] Material _currentBodyMaterial;
    Rigidbody _blobRigidbody; 

    RigidbodyConstraints rbc_normalState; 

    Quaternion _startRotation; 
    Vector3 _lastValidPosition;

    Color _blobColor;
    Color _sortenFarbe = Color.white;

    //Explosion
    float _explosionForce = 10f;
    float _explosionRadius = 10f;
    float _explosionUpforce = 0.5f;
    Vector3 _explosionPosition; 

    float _colorIntensityFactor = 0;
    float _zPosition;
    float _fallSpeed;
    float _startMass; 
    float _solidMass = 3;
    int _collectedCorrectColor;
    int _currentScore = 10;
    int _scoreMultiplicator = 1;
    bool _dropped = false;
    bool _justSpawned = true;
    bool _standingOverGoal = false;

    private void Start()
    {
        _blobRigidbody = GetComponent<Rigidbody>();

        _fallSpeed = GameManager._instance._blobSpeedValue;
        _explosionForce = GameManager._instance._explosionForce;
        _explosionRadius = GameManager._instance._explosionRadius;
        _explosionUpforce = GameManager._instance._explosionUpforce;

        rbc_normalState = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
        
        _startRotation = transform.rotation;
        _startMass = _blobRigidbody.mass; 

        SetSpawnVariables(); 
        SetColor(GetComponent<EisSorte>()._sortenIndex.ToString(), _collectedCorrectColor);
    }

    IEnumerator StandUp()
    {
        _blobRigidbody.velocity = Vector3.zero;
        _blobRigidbody.angularVelocity = Vector3.zero;
        _blobRigidbody.isKinematic = true;

        float elapsedTime = 0;
        float speed = 2; 
        Quaternion destination = new Quaternion(0, 0, 0, 1);

        while (transform.rotation != destination)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, destination, elapsedTime * speed);
            elapsedTime += Time.deltaTime; 
            yield return null; 
        }

        _blobRigidbody.isKinematic = false;
        yield return null; 
    }

    void SetSpawnVariables()
    {
        _collectedCorrectColor = 1;
        _solid = false;
        _isDragged = false;
        _justSpawned = true;

        _roundBlob.SetActive(true);
        _edgeBlob.SetActive(false);

        _blobRigidbody.constraints = rbc_normalState;
        _blobRigidbody.velocity = Vector3.zero;
        _blobRigidbody.angularVelocity = Vector3.zero;
        _blobRigidbody.mass = _startMass;
        _blobRigidbody.isKinematic = false;

        transform.rotation = _startRotation;

        _zPosition = GameManager._instance._mainZAxis;
        transform.position = new Vector3(transform.position.x, transform.position.y, _zPosition);
    }


    Vector3 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z + _zPosition));
    }

    private void OnMouseDown()
    {
        if (!_solid)
        {
            _justSpawned = false; 
            _isDragged = true;
        }
    }
    private void OnMouseDrag()
    {
        if (_isDragged)
        {
            StartCoroutine(StandUp()); 
            transform.position = GetMouseWorldPosition();
        }
    }

    private void OnMouseUp()
    {
        if (!_solid)
        {
            _isDragged = false;
            _dropped = true;

            _lastValidPosition = new Vector3(GetMouseWorldPosition().x, GetMouseWorldPosition().y, _zPosition);
            _blobRigidbody.mass = _startMass;
            _blobRigidbody.isKinematic = false;

            transform.position = new Vector3(_lastValidPosition.x, _lastValidPosition.y, _zPosition);
        }
    }
    
    private void Update()
    {
        _blobRigidbody.drag = _blobRigidbody.mass / _fallSpeed;
        _standing = _floorTracker.GetComponent<FloorTracker>().StandingOnFloor();

        if (_standing)
        {
            _justSpawned = false;
            
            //Ab hier: Wenn dieser Blop solid ist und über der Goallinie steht, soll die Kamerabewegung getriggert werden 
            //Kamerabewegung selbst ist in CameraMovement.cs, im GameManager.cs wird es ausgelöst wegen Variablen 
            if (!_standingOverGoal && _solid && transform.position.y >= GameManager._instance._currentGoal.transform.position.y)
            {
                _standingOverGoal = true;
                GameManager._instance._soManySolidUpstairs++;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Is eating one Futter
        if (!_justSpawned && !_solid)
        {
            if(other.gameObject.tag == "Futter")
            {
                if(this.GetComponent<EisSorte>()._sortenIndex == other.gameObject.GetComponent<EisSorte>()._sortenIndex)
                {
                    GameManager._instance._currentLosePoints--;
                    GameManager._instance._deadZoneSprite.GetComponent<DeadState>().SetNewPosition();
                    _collectedCorrectColor++; 
                    GameManager._instance.AddScore(_currentScore, _scoreMultiplicator);
                    SetColor(other.GetComponent<EisSorte>().GetSorte(), _collectedCorrectColor);
                }
                else
                {
                    _collectedCorrectColor--;
                    /*
                    if (_collectedCorrectColor < 1)
                    {
                        SetColor(other.GetComponent<EisSorte>().GetSorte(), _collectedCorrectColor);
                    }*/
                    if (_collectedCorrectColor < 0)
                    {
                        Explode();
                        Debug.Log("explode durch Futter");
                    }
                }
                other.gameObject.GetComponent<FutterMovement>().SetFutterInvisible();
            }
            
            if (other.gameObject.tag == "FloorTracker" && other.gameObject.transform.parent.gameObject.GetComponent<BlobMovement>()._solid && transform.position.y < other.gameObject.transform.position.y)
            {
                Explode();
                Debug.Log("explode floor tracker");
            }
        }


        /*
        //Snap into grid
        if (other.gameObject.tag == "GridCell" && !other.gameObject.GetComponent<GridTile>()._blockedTile)
        {
            if (_dropped)
            {
                Vector3 otherPosition = other.gameObject.GetComponent<GridTile>().GetSnapPosition();
                _lastValidPosition = new Vector3(otherPosition.x, otherPosition.y, _zPosition);
                _blobRigidbody.mass = _startMass;
                _blobRigidbody.isKinematic = false;
                _dropped = false;
            }

            if (_isDragged)
            {
                _lastValidPosition = other.gameObject.GetComponent<GridTile>().GetSnapPosition();
            }
        }*/
       
        if(other.gameObject.tag == "OutOfBounds")
        {
            if (!_solid)
            {
                GameManager._instance.AddLosePoint();
                Explode();
                Debug.Log("explode OutofBounds");
            }
            
            ResetBlob(); 
        }
    }

    //DIEEES
    void Explode()
    {
        GameManager._instance._myVFXPlayer.GetComponent<VFXTrigger>().PlayExplosionEffect(transform.position);
        Collider[] colliders = Physics.OverlapSphere(_explosionPosition, _explosionRadius);

        GameManager._instance.cameraShake.ShakeCamera(); 

        foreach(Collider hit in colliders)
        {
            Rigidbody rigidbody = hit.GetComponent<Rigidbody>();
                
            if (rigidbody != null) {
                rigidbody.AddExplosionForce(_explosionForce, _explosionPosition, _explosionRadius, _explosionUpforce, ForceMode.Impulse);
            }
        }
        ResetBlob(); 
    }

    public void SetMeshRenderer(GameObject blopSorte)
    {
        _meshRendererRoundBlop = blopSorte.GetComponent<MeshRenderer>();

        for (int i = 0; i < _meshRendererRoundBlop.materials.Length; i++)
        {
            Material material = _meshRendererRoundBlop.materials[i];
            if (material.HasProperty("_BodyColor"))
            {
                _currentBodyMaterial = material;
                _currentBodyMaterial.SetColor("_BodyColor", _sortenFarbe);
                _currentBodyMaterial.SetFloat("_ColorIntensity", _colorIntensityFactor);
            }
        }
    }

    void SetColor(string sorte, int intensity)
    {
        //Je nach Sorte wird die Farbe ausgesucht
        if (sorte == "Erdbeere")
        {
            _sortenFarbe = GameManager._instance._erdbeereColor;
        }
        else if (sorte == "Vanille")
        {
            _sortenFarbe = GameManager._instance._vanilleColor;
        }
        else if (sorte == "Minze")
        {
            _sortenFarbe = GameManager._instance._minzeColor;
        }
        else if (sorte == "Blaubeere")
        {
            _sortenFarbe = GameManager._instance._blaubeereColor;
        }

        //Je nach eingesammeltem Futter wird die Intensit�t der Farbe ge�ndert
        if (intensity == 0)
        {
            _colorIntensityFactor = 0.5f;
        }
        else if (intensity == 1)
        {
            _colorIntensityFactor = .60f;
            _currentScore = 100;
        }
        else if (intensity == 2)
        {
            _colorIntensityFactor = .70f;
            _currentScore = 150;
        }
        else if (intensity == 3)
        {
            _colorIntensityFactor = .85f;
            _currentScore = 200;
        }
        else if (intensity >= 4)
        {
            _colorIntensityFactor = 1f;
            _currentScore = 300;
        }

        //Wenn genug Futter gesammelt wurde, wird der Blob zum Block
        if (intensity >= GameManager._instance._amountOfFutterReq + 1) //+1 weil 1 das niedrigste ist 
        {
            StartCoroutine(GetSolidPufferTime(_maxGetSolidTimer));
        }

        _currentBodyMaterial.SetColor("_BodyColor", _sortenFarbe);
        _currentBodyMaterial.SetFloat("_ColorIntensity", _colorIntensityFactor);
    }

    IEnumerator GetSolidPufferTime(float timer)
    {
        float elapsedTime = 0;
        while (elapsedTime <= timer)
        {
            _currentBodyMaterial.SetFloat("_switchingColor", 1);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0;
        GetSolid();
        yield return null;
    }
    void GetSolid()
    {
        GameManager._instance._myVFXPlayer.GetComponent<VFXTrigger>().PlayGetSolidEffect(transform.position);
        _currentBodyMaterial.SetFloat("_switchingColor", 0);
        _meshRendererEdgyBlop.material.SetFloat("_ColorIntensity", 0.5f);
        _meshRendererEdgyBlop.material.SetColor("_BodyColor", _sortenFarbe);
        _blobRigidbody.mass = _solidMass;
        _blobRigidbody.isKinematic = false;
        _solid = true;

        _roundBlob.SetActive(false);
        _edgeBlob.SetActive(true);
        _isDragged = false;
    }

    public void ResetColorAfterRespawn()
    {
        Debug.Log(GetComponent<EisSorte>()._sortenIndex.ToString());
        SetColor(GetComponent<EisSorte>()._sortenIndex.ToString(), _collectedCorrectColor);
    }

    public void ResetBlob()
    { 
        SetSpawnVariables();
        gameObject.SetActive(false);
    }
}