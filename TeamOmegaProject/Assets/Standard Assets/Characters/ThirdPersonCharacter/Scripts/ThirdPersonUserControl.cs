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
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
        private int health;
        public Text countText;
		public bool dashpunch;
		public bool dashpunchState;
		public int dashTime;
		public bool dashpunchAvailable = true;
		private bool grounded;
		private bool doublejump;
		int jumpstatus;

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
			doublejump = m_Character.m_Animator.GetBool ("DoubleJump");

			if (dashpunchState == false && dashpunch == true && dashpunchAvailable == true) {
				m_Character.m_Animator.SetBool ("DashPunch", true);
				m_Character.m_Animator.SetBool ("DashAvailable", false);
				dashpunchState = true;
				dashTime = 0;
				dashpunchAvailable = false;
				if (doublejump == true) {
					jumpstatus = 1;
				} else {
					jumpstatus = 0;
				}
			} else if (dashpunchState == true && dashTime < 18 && (jumpstatus == 1 || (jumpstatus == 0 && doublejump == false))) {
				dashTime += 1;
			} else if (dashpunchState == true && (dashTime >= 18 || (jumpstatus == 0 && doublejump == true))) {
				m_Character.m_Animator.SetBool ("DashPunch", false);
				dashpunchState = false;
				dashTime = 0;
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
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                health--;
                countText.text = "Health: " + health.ToString();
                if (health <= 0)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
            if (other.gameObject.CompareTag("Bullet"))
            {
                health--;
                countText.text = "Health: " + health.ToString();
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
