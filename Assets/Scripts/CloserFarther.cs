using UnityEngine;

public class CloserFarther : MonoBehaviour
{
    [SerializeField] private float _step;

    public void Closer()
    {
        transform.position += transform.forward * _step;
    }
    
    public void Farther()
    {
        transform.position -= transform.forward * _step;
    }
}
