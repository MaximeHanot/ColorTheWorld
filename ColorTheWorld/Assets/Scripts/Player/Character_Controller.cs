using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Character_Motor))]
public class Character_Controller : MonoBehaviour
{

    [SerializeField] private float sprintMaxTime = 4f;
    private float sprintTimer = 0f;
    private float speed = 10f;
    [SerializeField] private float walkSpeed = 15f;
    [SerializeField] private float sprintSpeed = 15f;
    [SerializeField] private float climbSpeed = 4f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    
    private Character_Motor character_Motor;
    private Character_Manager character_Manager;

    bool stopMouseMoving = false;

    public LayerMask climbingMask;

    public Animator animator;

    private void Start()
    {
        character_Motor = GetComponent<Character_Motor>();
        character_Manager = GetComponent<Character_Manager>();
    }

    private void Update()
    {

        character_Manager.sprintAmount = sprintTimer / sprintMaxTime;

        SprintController();
        MoveController();
        ClimbController();
        JumpController();

        if (!stopMouseMoving)
        {
            RotationController();
            CameraRotationController();
        }

        ClampVerticalVelocity();

        if (character_Motor.IsGrounded())
            Debug.Log("<color=green>Grounded</color>");
        else
            Debug.Log("<color=red>NOT Grounded</color>");
    }

    void MoveController()
    {
        //Calculate movement velocity as a 3D vector
        float _xMov = Input.GetAxisRaw("Horizontal");
        float _zMov = Input.GetAxisRaw("Vertical");

        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;

        //Final movement vector
        Vector3 _velocity = (_movHorizontal + _movVertical).normalized * speed;

        if(_velocity.x != 0 || _velocity.z != 0)
        {
            animator.SetBool("isIdling", false);

            if(speed == walkSpeed)
            {
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", false);
            }
            else if (speed == sprintSpeed)
            {
                animator.SetBool("isRunning", true);
                animator.SetBool("isWalking", false);
            }
        }
        else
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isIdling", true);
        }

        //if (_velocity.y > 0)
        //    animator.SetBool("isJumping", true);
        //else
        //    animator.SetBool("isJumping", false);

        //if (_velocity.y < 0)
        //    animator.SetBool("isFalling", true);
        //else
        //    animator.SetBool("isFalling", false);

        //Apply movement
        character_Motor.Move(_velocity);
    }

    void ClimbController()
    {
        //Calculate movement velocity as a 3D vector
        float _xMov = Input.GetAxisRaw("Horizontal");
        float _zMov = Input.GetAxisRaw("Vertical");

        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.up * _zMov;

        //Final movement vector
        Vector3 _velocity = (_movHorizontal + _movVertical).normalized * climbSpeed;

        //Apply movement
        character_Motor.Climb(_velocity);
    }

    void RotationController()
    {
        //Calculate rotation as a 3D vector
        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * rotationSpeed;

        //Apply rotation
        character_Motor.Rotate(_rotation);
    }

    void CameraRotationController()
    {
        //Calculate camera rotation as a 3D vector
        float _xRot = Input.GetAxisRaw("Mouse Y");

        float _cameraRotation = _xRot * rotationSpeed;

        //Apply rotation
        character_Motor.RotateCamera(_cameraRotation);

    }

    void SprintController()
    {
        if (Input.GetButton("Sprint") && sprintTimer < sprintMaxTime)
        {
                sprintTimer += Time.deltaTime;
                speed = sprintSpeed;
        }
        else
        {
            speed = walkSpeed;
            sprintTimer -= Time.deltaTime / 4;
        }
        
        if (sprintTimer <= 0)
            sprintTimer = 0;
    }

    void JumpController()
    {   
        float _yBump = 0f;

        if(Input.GetButton("Jump"))
        {
            character_Motor.ClimbBoolean(false);

            if (character_Motor.IsGrounded() )
            {
                animator.SetBool("isJumping", true);
                _yBump = jumpForce;
            }
            else
                _yBump = 0f;
        }     

        character_Motor.Bump(_yBump);
    }

    void ClampVerticalVelocity()
    {
        if(character_Motor.rb.velocity.y > jumpForce)
        {
            character_Motor.rb.velocity = new Vector3( character_Motor.rb.velocity.x, jumpForce, character_Motor.rb.velocity.z ) ;
        }

        if (character_Motor.IsGrounded() == false && character_Motor.rb.velocity.y < 0)
        {
            Debug.Log("isFallig?");
            animator.SetBool("isFalling", true);
            animator.SetBool("isJumping", false); 
        }
        else
        {
            Debug.Log("isNotFallig");
            animator.SetBool("isFalling", false);
        }
    }

    public void StopMouseMovement()
    {
        stopMouseMoving = true;
        Cursor.visible = true;
    }

    public void ActiveMouseMovement()
    {
        stopMouseMoving = false;
        Cursor.visible = false;
    }
}