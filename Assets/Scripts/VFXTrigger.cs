using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXTrigger : MonoBehaviour
{
    [SerializeField] VisualEffectAsset _vfxExplosion, _vfxGetSolid;
    [SerializeField] VisualEffect _myVfx, _myIceTime;

    public void PlayExplosionEffect(Vector3 position)
    {
        _myVfx.enabled = true;
        transform.position = position;
        transform.position += Vector3.back; 
        _myVfx.visualEffectAsset = _vfxExplosion;
        _myVfx.Play();
    }

    public void PlayGetSolidEffect(Vector3 position)
    {
        _myVfx.enabled = true;
        transform.position = position;
        transform.position += Vector3.back; 
        _myVfx.visualEffectAsset = _vfxGetSolid;
        _myVfx.Play();
    }

    public void PlayCrystalizeEffect()
    {
        _myIceTime.enabled = true;
        _myIceTime.Play();
    }
}
