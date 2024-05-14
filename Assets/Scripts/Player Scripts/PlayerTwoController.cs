using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementState
{
    moving,knockback
};

public class PlayerTwoController : MonoBehaviour
{
    //Component
    Rigidbody2D rb2d;
    public playercombatState combatstate;


    //Player Attributes
    public float playerhealth;
    float maxplayerhealth = 100f;
    public float movespeed = 10f;
    public float jumpforce = 15f;
    public float jetforce = 5f;
    public float jetfuel;
    float maxjetfuel = 100f;
    public static bool isinstadead2 = false;
    //Jumping Requirements
    public LayerMask whatisGround;
    public float jumpingradius;
    public Transform groundCheck;
    public GameObject jetparticles;

    //Shooting Attributes
    float shootrate;
    float shootdelay = .5f;
    int bulletspawnnum;
    public float ammo;
    float maxammo;
    public float gunforce, rocketforce,recoilforce;
    public static float p1gunforce;

    //Shooting Requirements
    public GameObject[] gunsType = new GameObject[0];
    public GameObject[] bulletspawns = new GameObject[0];
    public GameObject bullet;
    public GameObject missile;

    //Melee Requirements
    public GameObject punch;
    Animator anim;
    public float attackspeed;


    //Shooting Buff
    public static bool isSniperTwo;
    public static bool isRocketTwo;

    //Knockback?
    public MovementState movestate;
    public GameObject hitParticles;
    public Vector3 offset;
    bool goleft, goright,onrecoil;
    float knocbackrate;
    public float knockbackdelay;

    //Audio
    public AudioClip playerhit, playershoot, playerjump, playerpickup, playerpunch;
    AudioSource audioSource;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        combatstate = playercombatState.unarmed; // unarmed at start
        shootrate = shootdelay;
        playerhealth = maxplayerhealth;
        jetfuel = maxjetfuel;
        movestate = MovementState.moving;
    }

    void Update()
    {
        if (Time.timeScale == 1)
        {
            //Enable disable audio
            if (SoundManagerMainMenu.isenabledaudio)
            {
                audioSource.enabled = true;
            }
            else if (!SoundManagerMainMenu.isenabledaudio)
            {
                audioSource.enabled = false;
            }

            //Set Values for UI
            UIManager.p2health = playerhealth;
            UIManager.p2jet = Mathf.Round(jetfuel);
            UIManager.p2ammo = ammo;

            gunforce = PlayerOneController.p2gunforce;


            Movement();

            if (combatstate == playercombatState.armed) //if armed pwede na mag shoot
            {
                Shooting();
            }

            if (combatstate == playercombatState.unarmed) // if unarmed disable somethings
            {
                PlayerOneController.p2gunforce = 10f;
                recoilforce = 5f;
                punch.SetActive(true);
                Melee();
                UIManager.p2gunimagetype = gunimagestypes.none; // disable ammo ui
                isSniperTwo = false;
                isRocketTwo = false;
                gunsType[0].SetActive(false);
                gunsType[1].SetActive(false);
                gunsType[2].SetActive(false);
                gunsType[3].SetActive(false);
                ammo = 0;
                bulletspawnnum = 0;
            }
        }
    }

    void Movement()
    {
        if (Time.timeScale == 1)
        {
            //Movement
            float horizontal = Input.GetAxis("PlayerTwo");
        if (movestate == MovementState.moving)
        {
            rb2d.velocity = new Vector2(horizontal * movespeed, rb2d.velocity.y);
        }
        else if(movestate == MovementState.knockback) // knocback the player
        {
            if (goleft)
            {
                rb2d.velocity = new Vector2(-gunforce, rb2d.velocity.y); // KNOCKBACK LEFT
            }
            else if (goright)
            {
                rb2d.velocity = new Vector2(gunforce, rb2d.velocity.y); // KNOCKBACK RIGHT
            }
            else if (onrecoil)
            {
                if (transform.rotation == Quaternion.Euler(0f, 0f, 0f))
                {
                    rb2d.velocity = new Vector2(-recoilforce, rb2d.velocity.y); // KNOCKBACK RIGHT
                }
                else
                {
                    rb2d.velocity = new Vector2(recoilforce, rb2d.velocity.y); // KNOCKBACK LEFT
                }
            }
            //make the knocback remove after some seconds
            knocbackrate += 1 * Time.fixedDeltaTime; 
            if(knocbackrate >= knockbackdelay)
            {
                knocbackrate = 0f;
                movestate = MovementState.moving;
                goleft = false;
                goleft = false;
                onrecoil = false;
            }
        }
        //Flip
            if (movestate == MovementState.moving)
            {
                if (horizontal > 0)
                {
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                }
                else if (horizontal < 0)
                {
                    transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                }
            }
            


            //Jumping
            bool isground = Physics2D.OverlapCircle(groundCheck.transform.position, jumpingradius, whatisGround);

            if (Input.GetKeyDown(KeyCode.UpArrow) && isground)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpforce);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow)) // just to play the jump sound
            {
                audioSource.PlayOneShot(playerjump);
            }
            //Flying
            else if (Input.GetKey(KeyCode.UpArrow) && jetfuel > 0)
            {
                jetfuel -= 20 * Time.deltaTime;
                rb2d.velocity = new Vector2(rb2d.velocity.x, jetforce);
                jetparticles.SetActive(true);
            }
            else
            {
                jetparticles.SetActive(false);
            }

            if (jetfuel == 0)
            {
                jetfuel = 0;
            }
            if (jetfuel >= 100)
            {
                jetfuel = 100;
            }
        }
    }

    void Melee()
    {
        if (Time.timeScale == 1)
        {
            anim.speed = attackspeed;
            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                audioSource.PlayOneShot(playerpunch);
                anim.SetTrigger("punchTwo");
            }
        }
    }

    void Shooting()
    {
        if (Time.timeScale == 1)
        {
            //Discard Item
            if (Input.GetKeyDown(KeyCode.RightAlt))
            {
                audioSource.PlayOneShot(playerpickup);
                combatstate = playercombatState.unarmed;
            }

            //Reload
            if (Input.GetKeyDown(KeyCode.RightControl) && ammo < maxammo)
            {
                audioSource.PlayOneShot(playerpickup);
                ammo = maxammo;
            }

            //Shooting
            if (Input.GetKey(KeyCode.RightShift) && ammo > 0)
            {
                shootrate += 1 * Time.deltaTime;
                if (shootrate >= shootdelay)//shoot once ever second
                {
                    audioSource.PlayOneShot(playershoot);
                    ammo -= 1;
                    shootrate = 0; //reset shoot
                    movestate = MovementState.knockback;
                    onrecoil = true;
                    if (isRocketTwo)
                    {
                        Instantiate(missile, bulletspawns[bulletspawnnum].transform.position, bulletspawns[bulletspawnnum].transform.rotation);
                    }
                    else
                    {
                        Instantiate(bullet, bulletspawns[bulletspawnnum].transform.position, bulletspawns[bulletspawnnum].transform.rotation);
                    }
                }
            }

            //Auto Remove Weapon when No more Ammo
            if (isRocketTwo)
            {
                if (ammo == 0)
                {
                    combatstate = playercombatState.unarmed;
                }

            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //Pickup Weapons
        if (col.tag == "assaultRifle" && combatstate == playercombatState.unarmed)//for assault rifle
        {
            audioSource.PlayOneShot(playerpickup);
            if (col.gameObject.layer == 10)
            {
                WeaponDisplaySpawner.spawnedweapon = false;
            }
            else if (col.gameObject.layer == 11)
            {
                WeaponDisplaySpawner.spawnedweapon2 = false;
            }
            WeaponDisplaySpawner.totaldisplays--; // decrease total display by one per get display
            UIManager.p2gunimagetype = gunimagestypes.ar; //enable ammo ui
            maxammo = 60f; // change maxammo
            ammo = maxammo;//change to maxammo
            bulletspawnnum = 0; //change bullet spawn
            shootdelay = .1f; // change shoot delay
            p1gunforce = 15f;// change the gun force;
            recoilforce = 13f;// change recoil
            combatstate = playercombatState.armed;
            gunsType[0].SetActive(true);
            Destroy(col.gameObject);
        }
        if (col.tag == "pistol" && combatstate == playercombatState.unarmed) //for pistol
        {
            audioSource.PlayOneShot(playerpickup);
            if (col.gameObject.layer == 10)
            {
                WeaponDisplaySpawner.spawnedweapon = false;
            }
            else if (col.gameObject.layer == 11)
            {
                WeaponDisplaySpawner.spawnedweapon2 = false;
            }
            WeaponDisplaySpawner.totaldisplays--; // decrease total display by one per get display
            UIManager.p2gunimagetype = gunimagestypes.pistol; //enable ammo ui
            maxammo = 36f; // change maxammo
            ammo = maxammo;//change to maxammo
            shootdelay = .3f;// change shoot delay
            bulletspawnnum = 1; //change bullet spawn
            p1gunforce = 12f;// change the gun force;
            recoilforce = 10f;// change recoil
            combatstate = playercombatState.armed;
            gunsType[1].SetActive(true);
            Destroy(col.gameObject);
        }
        if (col.tag == "sniper" && combatstate == playercombatState.unarmed) //for sniper
        {
            audioSource.PlayOneShot(playerpickup);
            if (col.gameObject.layer == 10)
            {
                WeaponDisplaySpawner.spawnedweapon = false;
            }
            else if (col.gameObject.layer == 11)
            {
                WeaponDisplaySpawner.spawnedweapon2 = false;
            }
            WeaponDisplaySpawner.totaldisplays--; // decrease total display by one per get display
            UIManager.p2gunimagetype = gunimagestypes.sniper; //enable ammo ui
            isSniperTwo = true;
            maxammo = 20f; // change maxammo
            ammo = maxammo;//change to maxammo
            shootdelay = .6f;// change shoot delay
            bulletspawnnum = 2;//change bullet spawn
            p1gunforce = 18f;// change the gun force;
            recoilforce = 16f;// change recoil
            combatstate = playercombatState.armed;
            gunsType[2].SetActive(true);
            Destroy(col.gameObject);
        }
        if (col.tag == "rocket" && combatstate == playercombatState.unarmed) //for rocket
        {
            audioSource.PlayOneShot(playerpickup);
            WeaponDisplaySpawner.spawnedspecial = false; // got the rocket
            UIManager.p2gunimagetype = gunimagestypes.rocket; //enable ammo ui
            isRocketTwo = true;
            maxammo = 10f; // change maxammo
            ammo = maxammo;//change to maxammo
            shootdelay = .8f;// change shoot delay
            bulletspawnnum = 3;//change bullet spawn
            p1gunforce = 30f;// change the gun force;
            recoilforce = 55f;// change recoil
            combatstate = playercombatState.armed;
            gunsType[3].SetActive(true);
            Destroy(col.gameObject);
        }

        //Recieve Fuel
        if (col.tag == "fuel")
        {
            audioSource.PlayOneShot(playerpickup);
            jetfuel += 20;//add jet fuel
            Destroy(col.gameObject);
        }

        //Getting hit
        if (col.tag == "bulletOne" || col.tag == "missile")
        {
            Instantiate(hitParticles, this.transform.position + offset, this.transform.rotation);
            audioSource.PlayOneShot(playerhit);
            CameraShake.ShakeShakeCamera(); // camera shake
            movestate = MovementState.knockback;
            if (col.gameObject.transform.rotation == Quaternion.Euler(0f, 0f, 0f))
            {
                goright = true;
            }
            else
            {
                goleft = true;
            }
            Destroy(col.gameObject);
            if (PlayerOneController.isSniperOne)
            {
                playerhealth-= 4;
            }
            else
            {
                playerhealth -= 3;
            }
            if (playerhealth <= 0)//dead
            {
                hoteldelluna();
            }
        }
        if (col.tag == "punchOne")//get punched
        {
            Instantiate(hitParticles, this.transform.position + offset, this.transform.rotation);
            audioSource.PlayOneShot(playerhit);
            CameraShake.ShakeShakeCamera(); // camera shake
            playerhealth-=2;
            movestate = MovementState.knockback;
            if (GameObject.FindGameObjectWithTag("playerOne").transform.rotation == Quaternion.Euler(0f, 0f, 0f))
            {
                goright = true;
            }
            else
            {
                goleft = true;
            }
            if (playerhealth <= 0) //dead
            {
                hoteldelluna();
            }
        }
        if (col.tag == "splashDamage")
        {
            Instantiate(hitParticles, this.transform.position + offset, this.transform.rotation);
            audioSource.PlayOneShot(playerhit);
            CameraShake.ShakeShakeCamera(); //shake camera
            rb2d.velocity = new Vector2(rb2d.velocity.x, rocketforce);
            playerhealth -= 4;
            if (playerhealth <= 0)//dead
            {
                hoteldelluna();
            }
        }
        if (col.tag == "lava")//instant death
        {
            Instantiate(hitParticles, this.transform.position + offset, this.transform.rotation);
            isinstadead2 = true;
            CameraShake.ShakeShakeCamera(); //shake camera
            hoteldelluna();
            audioSource.PlayOneShot(playerhit);
        }
    }

    void hoteldelluna() // let the player rest then reincarnate to a player with full stat basically means reset player
    {
        UIManager.p1score++;
        combatstate = playercombatState.unarmed;
        playerhealth = maxplayerhealth;
        gameObject.SetActive(false);
        jetfuel = maxjetfuel;
        movestate = MovementState.moving;
        anim.speed = attackspeed;
        punch.SetActive(true);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.transform.position, jumpingradius);
    }

    /*
    void OnBecameInvisible()
    {
        if(CameraController.camerasize == 15f)
        {
            CameraController.camerasize = 20f;
        }
        if (CameraController.camerasize == 20f)
        {
            CameraController.camerasize = 25f;
        }
        else if(CameraController.camerasize == 25f)
        {
            CameraController.camerasize = 27f;
        }
        else if (CameraController.camerasize == 27f)
        {
            CameraController.camerasize = 30f;
        }
        else if (CameraController.camerasize == 30f)
        {
            CameraController.camerasize = 32f;
        }
        else if (CameraController.camerasize == 32f)
        {
            CameraController.camerasize = 35f;
        }
    }*/
}
