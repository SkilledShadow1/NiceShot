using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour
{
    private static PlayerInput instance;

    public static PlayerInput Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(this.gameObject);

    }
    //Movement
    public float horizontalMovement;

    //Shooting
    private Shotgun shotgunScript;
    public bool pressingShoot;
    public GameObject[] guns;
    public float gunSelected = 0;
    [SerializeField] int startingGun = 0;

    //Jumping
    public bool pressingJump;

    private int latestPress;

    //Grappling
    public bool pressingGrapple;

    public bool jumped;

    // Start is called before the first frame update

    public void StartingValues() 
    {
    
        guns = GameObject.FindGameObjectsWithTag("Gun");
        Debug.Log(guns.Length);
        gunSelected = startingGun;

        StartingWeapon();

        latestPress = 0;
        //Jumping   
        pressingJump = false;

        jumped = false;

    }

    // Update is called once per frame
    void Update()
    {
        MovementInput();
        JumpingInput();
        ShootingInput();
        GrappleInput();
        ChangeWeapons();
    }

    private void MovementInput()
    {
        
         horizontalMovement = Input.GetAxisRaw("Horizontal");

        if(Input.GetButtonDown("PressingLeft"))
        {
            latestPress = -1;
        }

        if (Input.GetButtonDown("PressingRight")) 
        {
            latestPress = 1;
        }

        Debug.Log(latestPress);
        if (Input.GetAxisRaw("Horizontal") == 0) 
        {
            if(Input.GetAxisRaw("PressingRight") == 1 && Input.GetAxisRaw("PressingLeft") == 1) 
            {
                horizontalMovement = latestPress;
            }
        }
    }

    private void JumpingInput() 
    {
        if (Input.GetAxisRaw("Jump") == 1 && jumped == false)
        {
            pressingJump = true;

        }
        else 
        {
            pressingJump = false;
        }

        if (Input.GetAxisRaw("Jump") != 1 && jumped)
        {
            jumped = false;
        }
    }

    private void ShootingInput() 
    {
        if(gunSelected == 1) 
        {
            if(Input.GetKey(KeyCode.Mouse0))
            {
                pressingShoot = true;
            }
            else
            {
                pressingShoot = false;
            }
                
        }
        else 
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                pressingShoot = true;

            }
            else
            {
                pressingShoot = false;
            }
        }
    }

    private void StartingWeapon() 
    {
        for (int i = 0; i < guns.Length; i++)
        {
            if (i == gunSelected)
            {
                guns[i].SetActive(true);
            }
            else
            {
                guns[i].SetActive(false);
            }
        }
    }

    private void ChangeWeapons() 
    {
        if (Input.GetKeyDown(KeyCode.E) == true) 
        {
            gunSelected += 1;

            if (gunSelected + 1 > guns.Length)
                gunSelected = 0;

            for (int i = 0; i < guns.Length; i++)
            {
                if (i == gunSelected)
                {
                    guns[i].SetActive(true);
                }
                else
                {
                    guns[i].SetActive(false);
                }
            }
        }


    }

    private void GrappleInput() 
    {
        if (Input.GetAxis("Grapple") == 1) 
        {
            pressingGrapple = true;
        }
        else 
        {
            pressingGrapple = false;
        }
    }

    
}
