using MilkShake;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] Shaker _shaker;
    [SerializeField] ShakePreset _myShakerPreset;

    private void Start()
    {
        _shaker = GetComponent<Shaker>();
        _myShakerPreset.ShakeType = ShakeType.OneShot; 
        _myShakerPreset.Strength = GameManager._instance._cameraShakeStrength; 
        _myShakerPreset.Roughness = GameManager._instance._cameraShakeRoughness;
        _myShakerPreset.FadeIn = GameManager._instance._cameraShakeFadeIn; 
        _myShakerPreset.FadeOut = GameManager._instance._cameraShakeFadeOut; 
        _myShakerPreset.PositionInfluence = GameManager._instance._cameraShakePositionInfluence; 
        _myShakerPreset.RotationInfluence = GameManager._instance._cameraShakeRotationInfluence; 
    }
    public void ShakeCamera()
    {
        _shaker.Shake(_myShakerPreset);
    }
}
