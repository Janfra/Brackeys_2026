using UnityEngine;

public class Billboard : MonoBehaviour
{
    private static Camera camera;

    private void Awake()
    {
        camera = Camera.main;
    }

    private void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
