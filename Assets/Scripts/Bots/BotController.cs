using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class BotController : MonoBehaviour
{
    // Event for script communication
    public delegate void EnemyCollision();
    public static event EnemyCollision EnemyOnGround;

    private bool poweredUp = false, isGrounded = false, parachuteOpened = false;
    private Rigidbody rgbd;
    private int powerProb = 10;
    private Transform player;

    public Animator animator;
    public GameObject parachute, powerUpParticles;
    [HideInInspector] public bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        rgbd = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        InvokeRepeating("PowerUp", 1, 4);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -23.5f, 23.5f);
        pos.z = Mathf.Clamp(pos.z, -428, -380);

        transform.position = pos;

        if (isAttacking)
        {
            transform.position = Vector3.MoveTowards(transform.position, 
                new Vector3(player.position.x, transform.position.y, player.position.z), 12 * Time.deltaTime);
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

            rgbd.drag = 0.25f;
            parachute.GetComponent<Animator>().SetBool("Open", true);
            GetComponent<CapsuleCollider>().enabled = false;
            isAttacking = true;

            StartCoroutine(CloseParachute());
        }
    }

    IEnumerator CloseParachute()
    {
        // Play parachute closing animation after 3s
        yield return new WaitForSeconds(3);
        
        // Return the drag to normal value
        rgbd.drag = 0.055f;
        parachute.GetComponent<Animator>().SetBool("Open", false);
        GetComponent<CapsuleCollider>().enabled = true;
        parachuteOpened = false;

        // Deactivate the parachute object after 0.5s
        yield return new WaitForSeconds(0.5f);
        parachute.SetActive(false);

        InvokeRepeating("PowerUp", 1, 4);
    }

    /// <summary>
    /// This method will, based on probability, power up the enemy players boosting their fall speed for 2.25 seconds
    /// </summary>
    private void PowerUp()
    {
        if (Random.Range(1, powerProb) == 1 && !poweredUp && !parachuteOpened && !isGrounded && SceneManager.GetActiveScene().buildIndex != 1)
        {

            CancelInvoke("PowerUp");

            // Provide some feedback with particle effects
            var parts = Instantiate(powerUpParticles, Vector3.zero, Quaternion.Euler(0, 0, 0), transform);
            parts.transform.localPosition = Vector3.zero;

            // Increasing the fall speed and probability of the enemy getting power up
            poweredUp = true;
            isAttacking = true;
            rgbd.drag = 0.035f;
            powerProb += 2;

            Invoke("CancelAttack", 6f);
            Invoke("CancelPower", 3f);
        }
    }

    /// <summary>
    /// This method will erase the effects of the power up after 2.25 seconds of bot being powered up
    /// </summary>
    private void CancelPower()
    {
        poweredUp = false;
        rgbd.drag = 0.055f;

        InvokeRepeating("PowerUp", 1, 3);
    }

    private void CancelAttack()
    {
        isAttacking = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            GameObject.Find("SceneManager").GetComponent<SceneController>().LoosingScreenShow();
            // checking and enable EnemyonGround event
            if (EnemyOnGround != null)
            {
                //EnemyOnGround();
            }
            isGrounded = true;
            animator.SetTrigger("Landed");
        }
    }

    // if player reach first then all enemies die
    private void OnEnable()
    {
        PlayerController.groundCollision += DestroyAllEnemy;
    }

    public void DestroyAllEnemy()
    {
        gameObject.SetActive(false);
    }

    public void DisableAttacking()
    {
        isAttacking = false;
    }
}