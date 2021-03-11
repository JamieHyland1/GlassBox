using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class MoveController : MonoBehaviour
{
    [Range(1,15)]public float moveSpeed;

    [Range(5,20),SerializeField]private float runSpeed;
    [Range(1,15)]public float jumpVelocity;

    [SerializeField,Range(5f,10f)]private float DashDistance;

    public Vector3 Drag;

    public Animator animator;

    CharacterController controller;
    public Transform cam;

    public Transform groundCheck;

    [SerializeField,Range(0,-50f)]public float gravity = -9.8f;

    public bool isGrounded = true;

    [SerializeField]
    LayerMask groundLayer;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public Vector3 velocity;

   
    public Vector3 nextPos; 

    public Vector3 moveDirection;
  //  [SerializeField] ParticleSystem particle;

    bool coyoteJump = false;

    void Awake(){
        controller = this.GetComponent<CharacterController>();
      //  animator = this.GetComponent<Animator>();
        velocity = new Vector3();
      //  particle.Stop();
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

        isGrounded = Physics.CheckSphere(groundCheck.position,0.1f,groundLayer,QueryTriggerInteraction.Ignore);
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal,0f,vertical).normalized;

       if(isGrounded && coyoteJump){
           velocity.y = Mathf.Sqrt(jumpVelocity*-2f*gravity);
           Debug.Log("Coyote Jump!");
       }

        if(direction != Vector3.zero){
            
            if(!isGrounded){
                velocity.y += gravity*Time.deltaTime;
            }else{
                velocity.y = 0;
            }
            if(Input.GetKeyDown(KeyCode.LeftShift)){
                //animator.SetTrigger("Running");
               // particle.Play();

            }else if (Input.GetKeyUp(KeyCode.LeftShift)){
               // particle.Stop();
              //  animator.SetTrigger("Idle");
            }
            
            
            float targetAngle = Mathf.Atan2(direction.x,direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y,targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0,angle,0);
            moveDirection = Quaternion.Euler(0f,targetAngle,0f)*Vector3.forward;
             if(Input.GetKey(KeyCode.LeftShift)){
                 controller.Move(moveDirection.normalized*runSpeed*Time.deltaTime);
            }else{
                controller.Move(moveDirection.normalized*moveSpeed*Time.deltaTime);
            }
        }

        if(Input.GetButtonDown("Jump") && isGrounded){
            velocity.y = Mathf.Sqrt(jumpVelocity*-2f*gravity);
        }

         if(!isGrounded)velocity.y += gravity*Time.deltaTime;
        else if(isGrounded && velocity.y < 0)velocity.y=0;


        //Extrapolate the current position n frame to see if the player is touching the ground the next frame
        //this allows me to check if the user pressed jump n frames before the player hit the ground, and to store
        //that input to be run when the player does hit the ground
        nextPos = new Vector3();
        nextPos = transform.position;

        nextPos.x =  transform.position.x + moveDirection.x * 5;
        nextPos.y =  transform.position.y + velocity.y;
        nextPos.z =  transform.position.z + velocity.z * 5;

        
        controller.Move(velocity*Time.deltaTime);

    }
    private void OnDrawGizmos() {
        Gizmos.DrawSphere(nextPos,1f);
    }
}
