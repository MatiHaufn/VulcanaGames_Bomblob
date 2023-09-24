using UnityEngine;

public class FloorTracker : MonoBehaviour
{ 
    bool _standing;
    public bool StandingOnFloor() { return _standing; }

    private void OnTriggerStay(Collider other)
    {
<<<<<<< Updated upstream
        if (other.gameObject.tag == "Floor" || other.gameObject.tag == "SolidBlock" || other.gameObject.tag == "Player")
=======
        if (other.gameObject.tag == "Floor" || other.gameObject.tag == "SolidBlock" || other.gameObject.tag == "FreezedSolidBlop")
>>>>>>> Stashed changes
        {
            _standing = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
<<<<<<< Updated upstream
        if (other.gameObject.tag == "Floor" || other.gameObject.tag == "SolidBlock" || other.gameObject.tag == "Player")
=======
        if (other.gameObject.tag == "Floor" || other.gameObject.tag == "SolidBlock" || other.gameObject.tag == "FreezedSolidBlop")
>>>>>>> Stashed changes
            _standing = false; 
    }
}
