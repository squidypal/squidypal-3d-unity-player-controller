using UnityEngine;

public class VelocityBasedFOV : MonoBehaviour
{
    private Camera camera; 
    public Rigidbody playerRB; 
    public float baseFOV = 90f;
    public float maxFOV = 120f;
    public float sensitivity = 0.1f; 

    void Start()
    {
        camera = GetComponent<Camera>();
    }
    
    void Update()
    {
        if (camera != null && playerRB != null)
        {
            float velocityMagnitude = playerRB.velocity.magnitude;
            float additionalFOV = Mathf.Clamp(velocityMagnitude * sensitivity, 0, maxFOV - baseFOV);
            camera.fieldOfView = baseFOV + additionalFOV;
        }
    }
}