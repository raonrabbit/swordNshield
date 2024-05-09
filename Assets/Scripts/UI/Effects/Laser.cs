using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private LineRenderer line;
    [SerializeField] private GameObject EndVFX;


    public void SetLaserSize(float width = 0.5f, float length = 1.5f)
    {
        line.startWidth = width;
        line.endWidth = width;
        
        line.positionCount = 2;
        line.SetPosition(1, new Vector3(length, 0, 0));
        EndVFX.transform.position = new Vector3(0, length + transform.position.y, 0);
    }
}
