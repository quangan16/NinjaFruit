using UnityEngine;


public class Blade : MonoBehaviour
{
    public static Blade Instance;
    private Camera mainCam;
    private Collider bladeCollider;
    public bool isSlicing = false;
    public Vector3 sliceDirection;
    private Rigidbody bladeRb;
    [SerializeField] private Vector3 previousPos;
    [SerializeField] private Vector3 currentPos;
    private float minVelocity = 0.01f;
    public TrailRenderer bladeTrail;
    public float sliceForce = 5.0f;
    private void Awake()
    {
        Instance = this;
        bladeRb = GetComponent<Rigidbody>();
        bladeCollider = GetComponent<CapsuleCollider>();
        mainCam = Camera.main;
    }

    private void OnEnable()
    {
        previousPos = transform.position;
        StopSlicing();
    }

    private void OnDisable()
    {
        StopSlicing();
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartSlice();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            StopSlicing();
        }
        else if(isSlicing)
        {
            OnSlicing();
        }
    }

    public void StartSlice()
    {
        currentPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        if (isSlicing == false)
        {
            isSlicing = true;
            bladeCollider.enabled = true;
            currentPos.z = -10.0f;
            transform.position = currentPos; 
            // bladeTrail.enabled = true;
          
            
            bladeTrail.Clear();
        }
       
    }

    public void StopSlicing()
    {
        if (isSlicing)
        {
            // bladeTrail.enabled = false;
            isSlicing = false;
            bladeCollider.enabled = false;
            bladeTrail.Clear();
        }
      
    }

    public void OnSlicing()
    {
        
        bladeCollider.enabled = true;
      
        currentPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        currentPos.z = -10.0f;
        transform.position = currentPos;
        sliceDirection = Vector3.zero;
        
        float velocity = sliceDirection.magnitude / Time.deltaTime;
        bladeCollider.enabled = velocity >= minVelocity;
        previousPos = currentPos;
        
    }
  
}
