using UnityEngine;

namespace Testing
{
    public class MovementTesting : MonoBehaviour
    {
        public float thrust = 10f;
        public float rotationSpeed = 180f;
        public float maxSpeed = 8f;
        [Range(0f,100f)]
        public int debuffBackDir;
        private float currentMaxSpeed;
        [Range(0f, 1f)]
        public float drift;    
        private Rigidbody2D rb;
    
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }
    
        private void FixedUpdate()
        {
            //Debug.Log("Velocity " + rb.linearVelocity.magnitude);
            LimitSpeed();
            HandleMovement();
            HandleRotation();
            ApplyDrift();
        }

        void ApplyDrift()
        {
            Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.linearVelocity, transform.up);
            Vector2 lateralVelocity = transform.right * Vector2.Dot(rb.linearVelocity, transform.right);

            
            rb.linearVelocity = forwardVelocity + lateralVelocity * drift;
            
        }
    
        void HandleRotation()
        {
            float rotationInput = 0f;
    
            if (Input.GetKey(KeyCode.Q))
                rotationInput = 1f;
            if (Input.GetKey(KeyCode.E))
                rotationInput = -1f;
    
            rb.angularVelocity = rotationInput * rotationSpeed;
        }
    
        void HandleMovement()
        {
            float thrustInput = 0f;
            

            if (Input.GetKey(KeyCode.W))
            {
                thrustInput = 1f;
                currentMaxSpeed = maxSpeed;
            }
            if (Input.GetKey(KeyCode.S))
            {
                thrustInput = -1f;
            }
            
            HandleBackward();
            rb.linearVelocity += (Vector2)transform.up * (thrustInput * thrust * Time.fixedDeltaTime);
        }

        void HandleBackward()
        {
            if (Vector2.Dot(rb.linearVelocity.normalized, transform.up) < 0)
            {
                currentMaxSpeed = maxSpeed * (1-debuffBackDir/100f);
            }
        }
    
        // ReSharper disable Unity.PerformanceAnalysis
        void LimitSpeed()
        {
            if(rb.linearVelocity.magnitude > currentMaxSpeed)
            {
                //Debug.Log("Normalized "+rb.linearVelocity.normalized);
                rb.linearVelocity = rb.linearVelocity.normalized * currentMaxSpeed;
            }
               
        }
    }
}
