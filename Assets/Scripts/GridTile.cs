using UnityEngine;

//Each grid Tile has this script. It changes the color and make it possible, to place blops
public class GridTile : MonoBehaviour
{
    public Color _availableColor, _blockedColor, _neutralColor; //unselected = free tile, selected = red and blocked, neutral = almost invisible
    public bool _blockedTile;

    [SerializeField] Transform _snapPosition; 

    private void Start()
    {
        UpdateColor(); 
        GetComponent<SpriteRenderer>().color = _neutralColor;
    }

    private void Update()
    {
        if (GameManager._instance._debugMode)
        {
            UpdateColor(); 
        }
    }

    public void BlockTile(bool blocked)
    {
        _blockedTile = blocked;
        if (blocked)
        { 
            GetComponent<SpriteRenderer>().color = _blockedColor;
            GetComponent<Collider>().enabled = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = _availableColor;
            GetComponent<Collider>().enabled = true;
        }
    }
    
    public Vector2 GetSnapPosition() { return _snapPosition.position; } //Position for blobs to place
    
    //DebugMode Option to change the alpha during runtime
    void UpdateColor() { _availableColor = new Color(_availableColor.r, _availableColor.g, _availableColor.b, GameManager._instance._gridAlpha); }

}
