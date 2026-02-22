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
        public float maxFuel=100;
        private float currentFuel;
        public float burnRate=1;
        public float acceleration;
        [Range(0f,2f)]
        public float friction;
    
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            currentFuel = maxFuel;
        }
    
        private void FixedUpdate()
        {
            Debug.Log("Velocity " + Mathf.Round(rb.linearVelocity.magnitude));
            Debug.Log("Fuel "+   Mathf.Round(currentFuel));
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
            float currentThrust = thrust;
            currentMaxSpeed = maxSpeed;

            if (Input.GetKey(KeyCode.W))
            {
                thrustInput = 1f;
            }
            if (Input.GetKey(KeyCode.S))
            {
                thrustInput = -1f;
            }

            if (Input.GetKey(KeyCode.Space) && currentFuel > 0f) 
            {
                thrustInput = 1f;
                currentThrust = thrust * (1 + acceleration / 100f);
                currentMaxSpeed = maxSpeed * 4f;
                currentFuel -= burnRate * Time.deltaTime;
            }
            else if(!(Input.GetKey(KeyCode.Space)&&currentFuel < maxFuel) || currentFuel<=0f)
            {
                currentFuel += burnRate * Time.fixedDeltaTime;
            }
            currentFuel = Mathf.Clamp(currentFuel, 0f, maxFuel);
            
            HandleBackward();
            if(!LimitSpeed())
                rb.linearVelocity += (Vector2)transform.up * (thrustInput * currentThrust * Time.fixedDeltaTime);
            
        }

        void HandleBackward()
        {
            if (Vector2.Dot(rb.linearVelocity.normalized, transform.up) < 0)
            {
                currentMaxSpeed = maxSpeed * (1-debuffBackDir/100f);
            }
        }
        
        bool LimitSpeed()
        {
            if(rb.linearVelocity.magnitude > currentMaxSpeed)
            {
                rb.linearDamping = friction;
                return true;
            }

            rb.linearDamping = 0;
            return false;

        }
    }
}
