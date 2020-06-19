using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Character_Motor : MonoBehaviour
{
    [SerializeField] private GameObject camAxis;
    [SerializeField] private bool TPS;
    [SerializeField] private bool invertCameraAxis = true;
    [SerializeField] private GameObject cameraFPS;
    [SerializeField] private GameObject cameraTPS;

    private Vector3 velocity = Vector3.zero;
    private Vector3 climb = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float bump = 0f;

    private float cameraRotationX = 0f;
    private float currentCameraRotationX = 0f;
    [SerializeField] private Vector2 cameraRotationLimit = new Vector2(-40f, 85f);

    [HideInInspector] public Rigidbody rb;
    private CapsuleCollider col;
    float distToGround;

    public PhysicMaterial physicsMaterial;

    public bool isClimbing = false;

    private void Start()
    {
        col = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();

        distToGround = col.bounds.extents.y;

        SetCameraView();
    }

    private void FixedUpdate()
    {

        if (!isClimbing )
        {
            EnableGravity();
            PerformMovement();
            PerformRotation();
            PerformCameraRotation();
        }
        else if (isClimbing )
        {
            DisableGravity();
            PerformClimbing();
        }

        PerformJump();
        SetPhysicsMaterial();
    }

    public void Move(Vector3 _velocity) { velocity = _velocity; }
    public void Climb(Vector3 _climb) { climb = _climb; }
    public void ClimbBoolean(bool canClimb) {isClimbing = canClimb; }
    public void Rotate(Vector3 _rotation) { rotation = _rotation; }
    public void Bump(float _bump) { bump = _bump; }
    public void RotateCamera(float _cameraRotationX) { cameraRotationX = _cameraRotationX; }

    void PerformMovement()
    {
        if( velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }

    void PerformClimbing()
    {
        if (velocity != Vector3.zero)
        {            
            rb.MovePosition(rb.position + climb * Time.fixedDeltaTime);
        }
    }

    void PerformJump()
    {
        rb.AddForce(Vector3.up * bump, ForceMode.Impulse);
    }
    
    void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation) );
    }

    void PerformCameraRotation()
    {
        if (camAxis != null)
        {
            if (invertCameraAxis)
                currentCameraRotationX += cameraRotationX;
            else if (!invertCameraAxis)
                currentCameraRotationX -= cameraRotationX;

            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, cameraRotationLimit.x, cameraRotationLimit.y);

            camAxis.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        }
    }

    void SetCameraView()
    {
        if (TPS && cameraTPS != null)
        {
            cameraTPS.SetActive(true);
            cameraFPS.SetActive(false);
        }
        else if (!TPS && cameraFPS != null)
        {
            cameraFPS.SetActive(true);
            cameraTPS.SetActive(false);
        }
        else
            Debug.LogError("<color=red><b>There is no camera gameobject set in 'Character_Motor' script (Player)</b></color>");
    }

    public bool IsGrounded()
    {
        RaycastHit hit;
        return Physics.SphereCast(transform.position, col.radius, -Vector3.up, out hit, ( distToGround / 2 ) + 0.04f);
        //return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.04f); //Verifier sur plus de points        
    }

    void SetPhysicsMaterial()
    {
        if(IsGrounded())
        {
            transform.GetComponent<Collider>().material = null;
        }
        else
        {
            transform.GetComponent<Collider>().material = physicsMaterial;
        }
    }

    public void DisableGravity()
    {
        rb.useGravity = false;
    }

    public void EnableGravity()
    {
        rb.useGravity = true;
    }
}