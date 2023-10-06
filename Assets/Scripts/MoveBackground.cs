using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    [SerializeField] GameObject _bgPartOne, _bgPartTwo, _bgPartThree;
    float _bgOneResetPos = -18;
    float _bgTwoResetPos = -56f;
    float _bgThreeResetPos = -72f;

    float _bgOnePos = 56.5f;
    float _bgTwoPos = 20f;
    float _bgThreePos = 3f;

    private void Update()
    {
        if(_bgPartOne.transform.position.y < _bgOneResetPos)
        {
            _bgPartOne.transform.position = new Vector2(0, _bgOnePos); 
        }
        if (_bgPartTwo.transform.position.y < _bgTwoResetPos)
        {
            _bgPartTwo.transform.position = new Vector2(0, _bgTwoPos);
        }
        if (_bgPartThree.transform.position.y < _bgThreeResetPos)
        {
            _bgPartThree.transform.position = new Vector2(0, _bgThreePos);
        }
    }

}
