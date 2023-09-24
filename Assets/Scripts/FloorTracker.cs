using UnityEngine;

public class FloorTracker : MonoBehaviour
{ 
    bool _standing;
    public bool StandingOnFloor() { return _standing; }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Floor" || other.gameObject.tag == "SolidBlock")
        {
            _standing = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Floor" || other.gameObject.tag == "SolidBlock")
            _standing = false; 
    }
}
