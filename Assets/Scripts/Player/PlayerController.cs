using System.Collections;
using UnityEngine;
using EZCameraShake;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // some event to call on other scripts for methods
    public delegate void PlayerCollision();
    public static event PlayerCollision coinCollision;
    public static event PlayerCollision groundCollision;

    private CameraShakeInstance myShaker;
    private bool spawnParticles = true;

    public Animator animator;
    private Rigidbody _rigidBody;
    public float movementSpeed = 10f;
    private DynamicJoystick joyStick;
    public bool isGrounded, parachuteOpened;
    public Transform childPlayer;
    public GameObject parachute, landingParticles, powerUpParticles;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        joyStick = GameObject.FindWithTag("Joystick").GetComponent<DynamicJoystick>();
        myShaker = CameraShaker.Instance.StartShake(1.3f, 0.4f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        // Move the player normally when the player is within the range of x = (-23.5 and 23.5)
        if (transform.position.x > -23.5f && transform.position.x < 23.5f)
        {
            _rigidBody.velocity = new Vector3(joyStick.Horizontal * movementSpeed, _rigidBody.velocity.y
            , joyStick.Vertical * movementSpeed);
        }

        // When player goes outside of the range -23.5 and 23.5
        else if (transform.position.x <= -23.5f || transform.position.x >= 23.5f)
        {
            // Zero the velocity of the player
            _rigidBody.velocity = new Vector3(0, _rigidBody.velocity.y, 0);

            // Check if player is on the left most bound and player is trying to move the character to right side then move the character
            if (transform.position.x <= -23.5 && joyStick.Horizontal > 0)
            {
                _rigidBody.velocity = new Vector3(joyStick.Horizontal * movementSpeed, _rigidBody.velocity.y
                , joyStick.Vertical * movementSpeed);
            }

            // Check if player is on the right most bound and player is trying to move the character to left side then move the character
            else if (transform.position.x >= 23.5 && joyStick.Horizontal < 0)
            {
                _rigidBody.velocity = new Vector3(joyStick.Horizontal * movementSpeed, _rigidBody.velocity.y
                , joyStick.Vertical * movementSpeed);
            }
        }

        // Move the player normally when the player is within the range of x = (-23.5 and 23.5)
        if (transform.position.z < -380f && transform.position.z > -426f)
        {
            _rigidBody.velocity = new Vector3(joyStick.Horizontal * movementSpeed, _rigidBody.velocity.y
            , joyStick.Vertical * movementSpeed);
        }

        // When player goes outside of the range -23.5 and 23.5
        else if (transform.position.z >= -380f || transform.position.z <= -426f)
        {
            // Zero the velocity of the player
            _rigidBody.velocity = new Vector3(0, _rigidBody.velocity.y, 0);

            // Check if player is on the left most bound and player is trying to move the character to right side then move the character
            if (transform.position.z >= -380 && joyStick.Vertical < 0)
            {
                _rigidBody.velocity = new Vector3(joyStick.Horizontal * movementSpeed, _rigidBody.velocity.y
                , joyStick.Vertical * movementSpeed);
            }

            // Check if player is on the right most bound and player is trying to move the character to left side then move the character
            else if (transform.position.z <= -426 && joyStick.Vertical > 0)
            {
                _rigidBody.velocity = new Vector3(joyStick.Horizontal * movementSpeed, _rigidBody.velocity.y
                , joyStick.Vertical * movementSpeed);
            }
        }

        // When the player is on ground
        if (isGrounded)
        {
            animator.SetFloat("Walk", Mathf.Abs(joyStick.Horizontal));
            animator.SetFloat("Walk", Mathf.Abs(joyStick.Vertical));
            Vector3 movement = new Vector3(joyStick.Horizontal, 0, joyStick.Vertical);

            // for rotation in all direction
            if (movement != Vector3.zero)
            {
                childPlayer.rotation = Quaternion.LookRotation(movement);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // When the player lands on the platform, play the "Flying to Landing" animation
        if (collision.gameObject.tag == "Platform")
        {
            animator.SetTrigger("Landed");
            isGrounded = true;

            GameObject.Find("SceneManager").GetComponent<SceneController>().ShowCoins();

            myShaker.StartFadeOut(0.2f);
            CameraShaker.Instance.ShakeOnce(1.3f, 0.4f, 0.3f, 1f);

            if (spawnParticles)
            {
                var part = Instantiate(landingParticles);
                part.transform.SetParent(gameObject.transform);
                part.transform.localPosition = new Vector3(0, 0.25f, 0);
                spawnParticles = false;
            }
            GameObject.Find("SceneManager").GetComponent<SceneController>().WinningScreen();

            var enemies = GameObject.FindGameObjectsWithTag("Enemy");

            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].SetActive(false);
            }

            // checking and enable groundCollision enent
            if (groundCollision != null)
            {
                groundCollision();
            }
            
        }


        else if (collision.gameObject.tag == "Enemy")
        {
            // If the enemy is attacking then open players parachute
            if (collision.gameObject.GetComponent<BotController>().isAttacking)
            {
                OpenParachute();
                collision.gameObject.GetComponent<BotController>().DisableAttacking();
            }

            else
            {
                Time.timeScale = 1;
                DecreaseDrag();
                collision.gameObject.GetComponent<BotController>().OpenParachute();
                var coins = int.Parse(GameObject.Find("SceneManager").GetComponent<SceneController>().coinText.text) + 1;
                GameObject.Find("SceneManager").GetComponent<SceneController>().coinText.text = coins.ToString();
            }            
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coin")
        {
            // checking and enabling coin collision event
            if (coinCollision != null)
            {
                coinCollision();
            }
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Enemy" && SceneManager.GetActiveScene().buildIndex == 1)
        {
            GameObject.Find("SceneManager").GetComponent<Tutorial>().PlayTutorial();
            transform.Find("Player").GetComponent<BoxCollider>().enabled = false;
        }
    }

    /// <summary>
    /// This method will play the parachute opening animation and slow down the falling speed of the player for some time
    /// </summary>
    public void OpenParachute()
    {
        if (!isGrounded && !parachuteOpened)
        {
            // Cancel all invokes
            CancelInvoke();

            parachuteOpened = true;
            parachute.SetActive(true);

            if (GameController.sfx)
            {
                parachute.GetComponent<AudioSource>().Play();
            }

            _rigidBody.drag = 0.25f;
            parachute.GetComponent<Animator>().SetBool("Open", true);

            StartCoroutine(CloseParachute());
        }
    }

    IEnumerator CloseParachute()
    {
        // Play parachute closing animation after 3s
        yield return new WaitForSeconds(3);

        // Return the drag to normal value
        _rigidBody.drag = 0.05f;

        if (parachuteOpened)
        {
            parachute.GetComponent<Animator>().SetBool("Open", false);
            parachuteOpened = false;

            // Deactivate the parachute object after 0.5s
            yield return new WaitForSeconds(0.5f);
            parachute.SetActive(false);
        }
    }


    public void DecreaseDrag()
    {
        _rigidBody.drag = 0.035f;
        var parts = Instantiate(powerUpParticles, Vector3.zero, Quaternion.Euler(0, 0, 0), transform);
        parts.transform.localPosition = Vector3.zero;
        StartCoroutine(CloseParachute());
    }
}
