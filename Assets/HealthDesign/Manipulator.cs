using UnityEngine;
using System.Collections;

public class Manipulator : MonoBehaviour {

    private static Ray ScreenPointToRay { get { return Camera.main.ScreenPointToRay(Input.mousePosition); } }
    private Vector3 _offset;

    void OnMouseDown() {
        _offset = transform.position - ScreenPointToRay.origin;
    }

    void OnMouseDrag() {
        var ray = ScreenPointToRay;
        transform.position = ScreenPointToRay.origin + _offset;
    }
}
