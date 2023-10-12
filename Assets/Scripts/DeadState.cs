using UnityEngine;

public class DeadState : MonoBehaviour
{
    [SerializeField] float _yStateOne, _yStateTwo, _yStateThree;
    RectTransform _myRecttransform;

    private void Start()
    {
        _myRecttransform = GetComponent<RectTransform>();
        _myRecttransform.localPosition = new Vector3(0, _yStateOne, 0);
    }   

    public void SetNewPosition()
    {
        if (GameManager._instance._currentLife >= 3)
            _myRecttransform.localPosition = new Vector3(0, _yStateOne, 0); 
        else if(GameManager._instance._currentLife == 2)
            _myRecttransform.localPosition = new Vector3(0, _yStateTwo, 0);
        else if (GameManager._instance._currentLife <= 1)
            _myRecttransform.localPosition = new Vector3(0, _yStateThree, 0);
    }
}
