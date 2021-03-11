using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    Transform groundCheck;

    [SerializeField]
    LayerMask groundLayer;
    bool pressedJump = false;

    [SerializeField, Range(0,30)]
    float playerSpeed = 5f;

    [SerializeField, Range(5,20)]
    float gravityScale = 5;

    [SerializeField, Range(4,20)]
    float jumpVelocity = 3;

    public bool onGround = true;

    Vector3 moveVector;

    Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        Debug.Log(rigidbody.velocity);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))pressedJump = true;
        float horizontalMove = Input.GetAxis("Horizontal")*playerSpeed;
        float verticalMove = Input.GetAxis("Vertical")*playerSpeed;
        moveVector = new Vector3(horizontalMove,0,verticalMove).normalized;
    }

    void FixedUpdate() {
        if(Physics.CheckSphere(groundCheck.position,0.3f,groundLayer))onGround = true; else onGround = false;
        if(pressedJump && onGround)rigidbody.AddForce((moveVector+Vector3.up)*playerSpeed,ForceMode.Impulse);
       if(!onGround)moveVector.y += -gravityScale;
       Debug.Log(moveVector);
        rigidbody.velocity = (moveVector);
    }
}
