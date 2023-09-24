using System.Collections.Generic;
using UnityEngine.SceneManagement; 
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    [Header("----------Futter und Blob Farben------------------------------------------------------------------------------------------------------------------")]
    [Tooltip("Farben des Futters und Blobs")]
    public Color _erdbeereColor = new Color(0.99f, 0.47f, 0.99f);
    public Color _vanilleColor = new Color(0.9f, 0.8f, 0.4f);
    public Color _minzeColor = new Color(0.48f, 0.91f, 0.51f);
    public Color _blaubeereColor = new Color(0.136f, 0.124f, 0.613f);

    [Header("----------Performance Variablen------------------------------------------------------------------------------------------------------------------")]
    [Tooltip("Wie viele Futtereinheiten sind maximal gleichzeitig da?")]
    public int _maxFutterInScene;
    [Tooltip("Wie viele Blobs sind maximal gleichzeitig da?")]
    public int _maxBlobsInScene;

    [Header("----------Menu------------------------------------------------------------------------------------------------------------------")]
    [Tooltip("Variable für Pause.")]
    public bool _pause; 
    [Tooltip("Die Zeit wird um diesen Faktor verlangsamt.")]
    public float _slowMotionFactor; 

    [Header("----------Game Variablen------------------------------------------------------------------------------------------------------------------")]
    [Tooltip("Wie oft kommt Futter vom Himmel, alle X Sekunden:")]
    public int _futterSpawnRate;
    [Tooltip("Wie oft kommen Blops vom Himmel, alle X Sekunden:")]
    public int _blopSpawnRate;
    [Tooltip("Wie viel Futter braucht Blob um solid zu werden:")]
    public int _amountOfFutterReq;

    [Header("----------Explosionen------------------------------------------------------------------------------------------------------------------")]
    [Tooltip("Stärke der Explosion, wenn ein Blob zweimal das Falsche isst.")]
    public float _explosionForce = 10f;
    [Tooltip("Radius der Explosion, wenn ein Blob zweimal das Falsche isst.")]
    public float _explosionRadius = 10f;
    [Tooltip("Upforce der Explosion, wenn ein Blob zweimal das Falsche isst.")]
    public float _explosionUpforce = 0.5f;

    [Tooltip("Multiplikator, um Blobspawnrate zu erhöhen. Standardwert ist 1.")]
    public float _blobTimeMultiplicator = 2;
    [Tooltip("Multiplikator, um Futterspawnrate zu erhöhen. Standardwert ist 1.")]
    public float _futterTimeMultiplicator = 1;

    [Tooltip("Verknüpfung zur Main Camera")]
    [HideInInspector] public CameraShake cameraShake;
    [Tooltip("Stärke des Kamera-Shakes.")]
    public float _cameraShakeStrength; 
    [Tooltip("Roughness des Kamera-Shakes.")]
    public float _cameraShakeRoughness;
    [Tooltip("Stärke des Kamera-Shakes.")]
    public float _cameraShakeFadeIn;
    [Tooltip("Roughness des Kamera-Shakes.")]
    public float _cameraShakeFadeOut;
    [Tooltip("Position Influence.")]
    public Vector3 _cameraShakePositionInfluence;
    [Tooltip("Rotation Influence.")]
    public Vector3 _cameraShakeRotationInfluence;

    [Header("----------Mesh Variationen -------------------------------------------------------------------------------------------------------------------")]
    [Header("Reihenfolge: Erdbeere, Vanille, Minze, Blaubeere")]
    [Tooltip("Meshes zum Zusammenstellen.")]
    public List<GameObject> _blobVersions = new List<GameObject>();
    public List<GameObject> _blobAccessoires = new List<GameObject>();
    public List<GameObject> _futterVersions = new List<GameObject>();
    
    [Header("----------Camera Movement------------------------------------------------------------------------------------------------------------------")]
    [Tooltip("Liste von Objekten, die sich bewegen sollen und das CameraMovement Script enthalten.")]
    public List<CameraMovement> _CameraMovementObjects = new List<CameraMovement>();

    [Tooltip("Y-Wert, an dem die Kamera sich bewegen soll.")]
    public float _cameraMovingLine;

    [Tooltip("Wie weit sich die Kamera bewegen soll.")]
    public float _camDistance;

    [Tooltip("Goals, für Triggern von der Kamera.")]
    public GameObject _currentGoal;
    public float _currentGoalYOffset = 1;
    public float _currentGoalY;

    [Tooltip("Goal Prefab, um neuen zu spawnen.")]
    public GameObject _GoalPrefab;
   
    [Tooltip("Duration des Kamera Movements in seconds")]
    public float _camMovementDuration;

    [Tooltip("Wie weit sich die Kamera bewegen soll.")]
    public float _camMoveDistance;
 
    [Header("----------Live Debug Mode------------------------------------------------------------------------------------------------------------------")]
    [Tooltip("Ermöglicht realtime Tests. Unbeidngt ausschalten beim Builden! Mögliche Änderungen: _futterSpeedValue")]
    public bool _debugMode;
    [Tooltip("Ändert sich nur bei aktiviertem debugMode. Fallgeschwindigkeit des Futters zwischen 0 und 5.")]
    [Range(0, 5)] public float _futterSpeedValue;
    [Tooltip("Ändert sich nur bei aktiviertem debugMode. Fallgeschwindigkeit des Blobs zwischen 0 und 5.")]
    [Range(0, 5)] public float _blobSpeedValue;
    [Tooltip("Alphawert vom Grid.")]
    [Range(0, 1)] public float _gridAlpha;

    float _maxGoalSpawnTimer;
    float _goalTimer = 0;
   // [NonSerialized] public bool _countingGoalTimer = false; 

    [Header("----------Verknüpfte Objekte oder Variablen------------------------------------------------------------------------------------------------------------------")]
    [Tooltip("NICHT ÄNDERN! Ebene, auf der das Grid, der Regen und die Blobs sind.")]
    public float _mainZAxis;

    [Tooltip("NICHT ÄNDERN! Ebene, auf der die Kamera ist.")]
    public float _cameraZAxis;

    [Tooltip("Referenz zum Spawner. Dort werden alle gespawnten Futtereinheiten und Blobs gespawnt.")]
    public GameObject _spawner;

    [Tooltip("Referenz zum Container für die Freezed Blops")]
    public GameObject _freezedBlopParent;

    [Tooltip("Referenz zum freezed Blop Prefab")]
    public GameObject _solidFreezedBlopPrefab;

    [Tooltip("Goalsystem Verknüpfung.")]
    public GameObject _goalSystem;

    [Tooltip("Referenz zum GridSystem. Dort werden alle Gridspezifischen Dinge durchgeführt.")]
    public GameObject GridSystem;

    [Tooltip("Verknüpft ist Menu, das Parent für das Settings Menü")]
    public GameObject _menuSettings; 

    [Tooltip("Verknüpft ist EndScreen, das Endmenü")]
    public GameObject _endScreen; 
   
    [Tooltip("Highscore")]
    public Text _highscoreUI;
    public Text _highscoreUIEndanzeige;
    [Tooltip("Current Score")]
    public Text _currentScoreUI;
    int _currentScore = 0;
    int _highscore;

    [Tooltip("Wie viel darf daneben gehen")]
    public int _maxLosePoints;
    int _currentLosePoints = 0;

    
    [Header("----------Grid Settings------------------------------------------------------------------------------------------------------------------")]
    [Tooltip("Linke untere Ecke")]
    public Vector2 _startCoordinatesGrid = new Vector2(-3, -6);
    [Tooltip("Rechte obere Ecke")]
    public Vector2 _endCoordinatesGrid = new Vector2(2.5f, 0);
    [Tooltip("Y-Wert, oberer Rand, wo die Futtereinheiten spawnen.")]
    public float _spawnLine;
    [Tooltip("Menge an Reihen vom Grid")]
    public int _rowAmount;
    [Tooltip("Höhe & Breite eines Tiles")]
    public float _gridSize;
    [Tooltip("Dichte des Grids")]
    [HideInInspector]public Vector2 _gridDepth = new Vector2(20,20);


    private void Awake()
    {
        if(_instance == null)
            _instance = this;
        else 
            Destroy(this);

        SetStartVariables(); 
    }

    //Set Variables at beginning of the game
    void SetStartVariables()
    {
        _menuSettings.SetActive(false);
        _endScreen.SetActive(false);
        _currentScore = 0; 
        _currentScoreUI.text = _highscore.ToString();
        _highscore = PlayerPrefs.GetInt("highscore");
        _highscoreUI.text = _highscore.ToString();
        _spawner.GetComponent<FutterSpawner>().MaxEinheiten(_maxFutterInScene, _maxBlobsInScene);
        cameraShake = Camera.main.GetComponent<CameraShake>();
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, _cameraZAxis);
    }

    public void ReplaceBlopObject(GameObject blop)
    {
        GameObject newFreezedBlop = Instantiate(_solidFreezedBlopPrefab, _freezedBlopParent.transform);
        newFreezedBlop.transform.position = blop.transform.position;
        blop.gameObject.GetComponent<BlobMovement>().ResetBlob(); 
    }

    public void MovingPlatformsDown()
    {
        foreach(CameraMovement camObj in _CameraMovementObjects)
            camObj.StartMovingCamera();
    }


    //Method to add Score
    public void AddScore(int score, int multiplicator)
    {
        _currentScore += score * multiplicator;
        _currentScoreUI.text = _currentScore.ToString();
        if (_currentScore >= _highscore)
            _highscore = _currentScore;
        SaveHighscore(_highscore); 
    }

    public void AddLosePoint()
    {
        _currentLosePoints++; 

        if(_currentLosePoints >= _maxLosePoints)
        {
            EndScreenGame(); 
        }
    }

    //Method to save the highscore 
    void SaveHighscore(int highscore) 
    { 
        _highscoreUI.text = highscore.ToString();
        _highscoreUIEndanzeige.text = _currentScoreUI.text;
        PlayerPrefs.SetInt("highscore", highscore);
    }

    //Deletes ALL saved Data and resets the highscore and score 
    public void DeleteSaveData()
    {
        Debug.Log("Delete all Data");
        _highscore = 0; 
        _currentScore = 0; 

        _highscoreUI.text = _highscore.ToString();
        _currentScoreUI.text = _highscore.ToString();
        PlayerPrefs.DeleteAll();
    }
    public int _soManySolidUpstairs; 
    public int _amountOfBlopsOverGoal;
    public bool _countingGoalTimer = false; 

    private void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            Time.timeScale = _slowMotionFactor;
        }
        if(Input.GetKeyUp(KeyCode.M))
        {
            Time.timeScale = 1; 
        }

        if (_soManySolidUpstairs >= _amountOfBlopsOverGoal)
        {
            _soManySolidUpstairs = 0; 
            _countingGoalTimer = true;
            MovingPlatformsDown();
            _spawner.GetComponent<FutterSpawner>().FreezeBlops(); 
        }

        if (_countingGoalTimer)
        {
            _goalTimer += Time.deltaTime;
        } 

        if (_goalTimer >= _maxGoalSpawnTimer)
        {
            _goalTimer = 0;
            _goalSystem.GetComponent<GoalSystem>().SpawnNewGoal(); 
            _countingGoalTimer = false;
        }
    }

    public void PauseGame()
    {
        _menuSettings.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        _menuSettings.SetActive(false);
        Time.timeScale = 1;
    }

    public void SlowMotion(bool active)
    {
        if (active)
            Time.timeScale = _slowMotionFactor;
        else
            Time.timeScale = 1;
    }

    public void EndScreenGame()
    {
        _endScreen.SetActive(true);
    }

    public void QuitApplication()
    {
        #if UNITY_EDITOR
                // Application.Quit() does not work in the editor so
                // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
    public void ResetGame()
    {
        SceneManager.LoadScene(1);
    }
}