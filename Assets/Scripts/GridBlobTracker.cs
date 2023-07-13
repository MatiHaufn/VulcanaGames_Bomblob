using UnityEngine;

//Script to check, if there is a Blop on this GridTile
public class GridBlobTracker : MonoBehaviour
{
    public int _blobInTouchCounter = 0;
    [SerializeField] GameObject _gridTile;

    private void Update()
    {
        if(_blobInTouchCounter >= 2)
        {
            _gridTile.GetComponent<GridTile>().BlockTile(true);
        }
        else
        {
            _gridTile.GetComponent<GridTile>().BlockTile(false);
        }
    }

    //Is it in contact with a Blop or a solid Block? 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "RoundBlob" || other.gameObject.tag == "SolidBlock")
        {
            _blobInTouchCounter++; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "RoundBlob" || other.gameObject.tag == "SolidBlock")
        {
            _blobInTouchCounter--;
        }
    }
}
