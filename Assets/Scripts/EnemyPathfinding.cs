using UnityEngine;
using Pathfinding;
using System;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyPathfinding : MonoBehaviour
{
    public GameObject Target;

    public float updateRate = 2f;

    private Seeker seeker;
    private Rigidbody2D rb;

    public Path path;
    public float speed = 300f;
    public ForceMode2D fMode;

    [HideInInspector]
    public bool pathIsEnded = false;

    public float nextWavepointDistance = 3;

    private int currentWavepont = 0;

    [SerializeField] GameObject leftWall;
    [SerializeField] GameObject rightWall;
    WallCheck leftWallCheck;
    WallCheck rightWallCheck;

    [SerializeField] float viewRadius = 5f;

    [SerializeField] GameObject groundCheck;
    EnemyGroundCheck groundChecker;

    CooldownManager cm;
    CooldownData jumpWait;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }

    // Start is called before the first frame update
    void Start()
    {
        cm = CooldownManager.Instance;
        jumpWait = cm.FindCooldown(CooldownData.CooldownType.ENEMYJUMP);
        leftWallCheck = leftWall.GetComponent<WallCheck>();
        rightWallCheck = rightWall.GetComponent<WallCheck>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        groundChecker = groundCheck.GetComponent<EnemyGroundCheck>();

        if (Target == null)
        {
            return;
        }

        seeker.StartPath(transform.position, Target.transform.position, OnPathComplete);

        StartCoroutine(UpdatePath());
    }

    private IEnumerator UpdatePath()
    {
        if (Target == null)
        {
            yield return false;
        }
        seeker.StartPath(transform.position, Target.transform.position, OnPathComplete);

        yield return new WaitForSeconds(1f / updateRate);

        StartCoroutine(UpdatePath());
    }

    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWavepont = 0;
        }

    }

    bool playerOnRight;
    [SerializeField] float jumpForce;

    private void Jump() 
    {
        if (!jumpWait.IsReady())
            return;
       
        if (Target.transform.position.x > this.transform.position.x)
            playerOnRight = true;

        else
            playerOnRight = false;


        if (leftWallCheck.isTouchingWall && groundChecker.enemyIsGrounded && path.vectorPath[currentWavepont].x - transform.position.x < 0 && groundChecker.canJump) 
        {
            jumpWait.timer = 0;
            Vector2 v = new Vector2(0, jumpForce);
            Debug.Log(v);
            rb.AddForce(v, ForceMode2D.Impulse);
            groundChecker.enemyIsGrounded = false;

        }

        if (rightWallCheck.isTouchingWall && groundChecker.enemyIsGrounded && path.vectorPath[currentWavepont].x - transform.position.x > 0 && groundChecker.canJump)
        {
            jumpWait.timer = 0;
            Vector2 v = new Vector2(0, jumpForce);
            Debug.Log(v);
            rb.AddForce(v, ForceMode2D.Impulse);
            groundChecker.enemyIsGrounded = false;
        }
    }

    //Gets stuck on top of platforms often if player is close by so this is a fix
    [SerializeField] float extraForce = 0.05f;

    private void FixedUpdate()
    {
        Debug.Log(jumpWait);
        if (Target == null)
        {
            return;
        }

        if (path == null)
        {
            return;
        }

        if (currentWavepont >= path.vectorPath.Count)
        {
            if (pathIsEnded)
            {
                return;
            }
            else
            {
                pathIsEnded = true;
                return;
            }
        }

        pathIsEnded = false;

        if(Vector3.Distance(transform.position, Target.transform.position) > viewRadius || !jumpWait.IsReady())
            return;

        float currentExtraForce = extraForce;

        Vector3 dir = (path.vectorPath[currentWavepont] - transform.position).normalized;

        if(path.vectorPath[currentWavepont].x - transform.position.x < 0) 
        {
            currentExtraForce *= -1;
        }


        dir = new Vector3(dir.x + currentExtraForce, dir.y, dir.z);

        dir *= speed * Time.fixedDeltaTime;

        if (leftWallCheck.isTouchingWall && path.vectorPath[currentWavepont].x - transform.position.x > 0)
            rb.velocity = new Vector2(0, rb.velocity.y);

        else if (rightWallCheck.isTouchingWall && path.vectorPath[currentWavepont].x - transform.position.x < 0)
            rb.velocity = new Vector2(0, rb.velocity.y);

        else
            rb.AddForce(dir, fMode);

        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWavepont]);

        if (dist < nextWavepointDistance)
        {
            currentWavepont++;
            return;
        }

        Jump();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(path.vectorPath[currentWavepont].x - transform.position.x);
    }
}