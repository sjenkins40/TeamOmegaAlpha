using System;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                    // the world-relative desired move direction, calculated from the camForward and user input.
        private int health;
		public GameObject HUD;
		public Text countText;
		public bool dashpunch;
		public bool dashpunchState;
		public int dashTime;
		public bool dashpunchAvailable = true;
		private bool grounded;
		private bool doublejump;
        private bool firstjump;
        private int jumpsAvailable;
        public bool knockback;
        public int knockbackTime;
		int jumpstatus;
        public AudioClip DashClip;
        public AudioClip JumpClip;
        public AudioClip BackgroundClip;
        public AudioClip RobotClip;
        public AudioClip DamageClip;
        public AudioSource MusicSource;
        public AudioSource JumpSource;
        public AudioSource BackgroundSource;
        public AudioSource RobotSource;
        public AudioSource DamageSource;
        int numjumps;

        private void Start()
        {

            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<ThirdPersonCharacter>();
            health = 12;
            countText.text = "Health: " + health.ToString();
            MusicSource.clip = DashClip;
            JumpSource.clip = JumpClip;
            BackgroundSource.clip = BackgroundClip;
            RobotSource.clip = RobotClip;
            DamageSource.clip = DamageClip;
            BackgroundSource.Play();
            numjumps = 0;
        }


        private void Update()
        {
            if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

			if (!dashpunch)
			{
				dashpunch = CrossPlatformInputManager.GetButtonDown ("DashPunch");
            }
            if (m_Character.m_Animator.GetBool("OnGround"))
            {
                numjumps = 0;
            }
            if (Input.GetKeyDown("space"))
            {
                numjumps++;
                if (numjumps <= 2)
                {
                    JumpSource.Play();
                }
            }

        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
			float h;
			float v;
			bool crouch;

            // read inputs
			if (dashpunchState == false) {
				h = CrossPlatformInputManager.GetAxisRaw ("Horizontal");
				v = CrossPlatformInputManager.GetAxisRaw ("Vertical");
				crouch = Input.GetKey (KeyCode.C);
			} else {
				h = 0;
				v = 0;
				crouch = false;
			}

            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v*m_CamForward + h*m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v*Vector3.forward + h*Vector3.right;
            }

			m_Character.m_Animator.SetBool ("DashAvailable", dashpunchAvailable);
            
            jumpsAvailable = 0;
            if (m_Character.m_Animator.GetBool("DoubleJump") == false) {
                jumpsAvailable = 1;
                if (m_Character.m_Animator.GetBool ("FirstJump") == false) {
                    jumpsAvailable = 2;
                }
            }

            firstjump = m_Character.m_Animator.GetBool ("FirstJump");
			doublejump = m_Character.m_Animator.GetBool ("DoubleJump");

			if (dashpunchState == false && dashpunch == true && dashpunchAvailable == true) {
                MusicSource.Play();
                m_Character.m_Animator.SetBool ("DashPunch", true);
				m_Character.m_Animator.SetBool ("DashAvailable", false);
				dashpunchState = true;
				dashTime = 0;
				dashpunchAvailable = false;
				if (doublejump == true) {
					jumpstatus = 2;
				} else if (firstjump == true) {
                    jumpstatus = 1;
                } else {
					jumpstatus = 0;
				}
			} else if (dashpunchState == true && dashTime < 18 && (jumpstatus == 2 || (jumpstatus == 1 && doublejump == false) || (jumpstatus == 0 && firstjump == false))) {
				dashTime += 1;
			} else if (dashpunchState == true && (dashTime >= 18 || (jumpstatus == 0 && firstjump == true) || (jumpstatus == 1 && doublejump == true))) {
				m_Character.m_Animator.SetBool ("DashPunch", false);
				dashpunchState = false;
				dashTime = 0;
			}

            if (knockback == true) {
                knockbackTime += 1;
                if (knockbackTime == 8) {
                    knockback = false;
                    m_Character.m_Animator.SetBool("Knockback", false);
                    knockbackTime = 0;
                }
            }

			m_Character.m_Animator.SetInteger ("DashTime", dashTime);
			//countText.text = dashTime.ToString();

			grounded = m_Character.m_Animator.GetBool("OnGround");
			if (dashpunchState == false && dashpunchAvailable == false && grounded == true) {
				dashpunchAvailable = true;
			}

#if !MOBILE_INPUT
			// walk speed multiplier
	        if (Input.GetKey(KeyCode.RightShift)) m_Move *= 0.5f;
#endif

            // pass all parameters to the character control script
			m_Character.Move(m_Move, crouch, m_Jump);
            m_Jump = false;
			dashpunch = false;

			HUD.GetComponent<updateHUD>().updateHealth(health);
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                if (dashpunchState == false)
                {
                    health--;
                    DamageSource.Play();
                    countText.text = "Health: " + health.ToString();
                    if (health <= 0)
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    }
                } else
                {
                    //This is when the enemy and player collide while dashpunching. It destroys enemies.
                    Destroy(other.gameObject);
                    RobotSource.Play();

                    //begins knockback
                    knockback = true;
                    m_Character.m_Animator.SetBool("Knockback", true);
                    knockbackTime = 0;

                    //turns off dashpunch
                    m_Character.m_Animator.SetBool ("DashPunch", false);
                    dashpunchState = false;
                    dashTime = 0;

                    //makes dashpunch available again
                    dashpunchAvailable = true;
                    m_Character.m_Animator.SetBool ("DashAvailable", dashpunchAvailable);

                    //makes double jump available again
                    m_Character.m_Animator.SetBool("DoubleJump", false);
                    doublejump = false;
                    jumpstatus = 1;

                    //turns off first jump if applicable
                    m_Character.m_Animator.SetBool("FirstJump", true);
                    firstjump = true;
                }
            }
            if (other.gameObject.CompareTag("Bullet"))
            {
                if (dashpunchState == false)
                {
                    health--;
                    DamageSource.Play();
                    countText.text = "Health: " + health.ToString();
                }
                Destroy(other.gameObject);
                if (health <= 0)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
            if (other.gameObject.CompareTag("Death"))
            {
                health = 0;
                countText.text = "Health: " + health.ToString();
                if (health <= 0)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
        }
    }
}
