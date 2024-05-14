using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum playercombatState
{
    armed,unarmed
};

public class PlayerOneController : MonoBehaviour
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
    public static bool isinstadead = false;
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
    public static float p2gunforce;

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
    public static bool isSniperOne;
    public static bool isRocketOne;

    //Knockback?
    public MovementState movestate;
    public GameObject hitParticles;
    public Vector3 offset;
    bool goleft, goright,onrecoil;
    float knocbackrate;
    public float knockbackdelay;

    //Audio
    public AudioClip playerhit, playershoot, playerjump,playerpickup,playerpunch;
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
            UIManager.p1health = playerhealth;
            UIManager.p1jet = Mathf.Round(jetfuel);
            UIManager.p1ammo = ammo;

            gunforce = PlayerTwoController.p1gunforce;

            Movement();

            if (combatstate == playercombatState.armed) //if armed pwede na mag shoot
            {
                Shooting();
                punch.SetActive(false);
            }

            if (combatstate == playercombatState.unarmed) // if unarmed disable somethings
            {
                PlayerTwoController.p1gunforce = 10f;
                recoilforce = 5f;
                punch.SetActive(true);
                Melee();
                UIManager.p1gunimagetype = gunimagestypes.none; // disable ammo ui
                gunsType[0].SetActive(false);
                gunsType[1].SetActive(false);
                gunsType[2].SetActive(false);
                gunsType[3].SetActive(false);
                ammo = 0;
                bulletspawnnum = 0;
                isSniperOne = false;
                isRocketOne = false;
            }
        }
    }

    void Movement()
    {
        if (Time.timeScale == 1)
        {
            //Movement
            float horizontal = Input.GetAxisRaw("PlayerOne");

            if (movestate == MovementState.moving)
            {
                rb2d.velocity = new Vector2(horizontal * movespeed, rb2d.velocity.y);
            }
            else if (movestate == MovementState.knockback) // knocback the player
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
                        rb2d.velocity = new Vector2(-recoilforce, rb2d.velocity.y); // KNOCKBACK LEFT
                    }
                    else
                    {
                        rb2d.velocity = new Vector2(recoilforce, rb2d.velocity.y); // KNOCKBACK RIGHT
                    }
                }
                //make the knocback remove after some seconds
                knocbackrate += 1 * Time.fixedDeltaTime;
                if (knocbackrate >= knockbackdelay)
                {
                    knocbackrate = 0f;
                    movestate = MovementState.moving;
                    goleft = false;
                    onrecoil = false;
                    goleft = false;
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

            if (Input.GetKeyDown(KeyCode.W) && isground)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpforce);
            }
            if (Input.GetKeyDown(KeyCode.W)) // just to play the jump sound
            {
                audioSource.PlayOneShot(playerjump);
            }
            //Flying
            else if (Input.GetKey(KeyCode.W) && jetfuel > 0)
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                audioSource.PlayOneShot(playerpunch);
                anim.SetTrigger("punchOne");
            }
        }
    }

    void Shooting()
    {
        if (Time.timeScale == 1)
        {
            //Discard Item
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                audioSource.PlayOneShot(playerpickup);
                combatstate = playercombatState.unarmed;
            }

            //Reload
            if (Input.GetKeyDown(KeyCode.R))
            {
                audioSource.PlayOneShot(playerpickup);
                ammo = maxammo;
            }

            //Shooting
            if (Input.GetKey(KeyCode.Space) && ammo > 0)
            {
                shootrate += 1 * Time.deltaTime;
                if (shootrate >= shootdelay)//shoot once ever second
                {
                    audioSource.PlayOneShot(playershoot);
                    ammo -= 1;
                    movestate = MovementState.knockback;
                    onrecoil = true;
                    shootrate = 0; //reset shoot
                    if (isRocketOne)
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
            if (isRocketOne)
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
            UIManager.p1gunimagetype = gunimagestypes.ar;//enable ammo ui
            maxammo = 60f; // change maxammo
            ammo = maxammo;//change to maxammo
            bulletspawnnum = 0; //change bullet spawn
            shootdelay = .1f; // change shoot delay
            p2gunforce = 15f;// change the gun force;
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
            UIManager.p1gunimagetype = gunimagestypes.pistol;//enable ammo ui
            maxammo = 36f; // change maxammo
            ammo = maxammo;//change to maxammo
            shootdelay = .3f;// change shoot delay
            bulletspawnnum = 1; //change bullet spawn
            p2gunforce = 12f;// change the gun force;
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
            UIManager.p1gunimagetype = gunimagestypes.sniper;//enable ammo ui
            isSniperOne = true;
            maxammo = 20f; // change maxammo
            ammo = maxammo;//change to maxammo
            shootdelay = .6f;// change shoot delay
            bulletspawnnum = 2;//change bullet spawn
            p2gunforce = 18f;// change the gun force;
            recoilforce = 16f;// change recoil
            combatstate = playercombatState.armed;
            gunsType[2].SetActive(true);
            Destroy(col.gameObject);
        }
        if (col.tag == "rocket" && combatstate == playercombatState.unarmed) //for rocket
        {
            audioSource.PlayOneShot(playerpickup);
            WeaponDisplaySpawner.spawnedspecial = false; //got the rocket
            UIManager.p1gunimagetype = gunimagestypes.rocket;//enable ammo ui
            isRocketOne = true;
            maxammo = 10f; // change maxammo
            ammo = maxammo;//change to maxammo
            shootdelay = .8f;// change shoot delay
            bulletspawnnum = 3;//change bullet spawn
            p2gunforce = 300f;// change the gun force;
            recoilforce = 55f;// change recoil
            combatstate = playercombatState.armed;
            gunsType[3].SetActive(true);
            Destroy(col.gameObject);
        }

        //Recieve Fuel
        if(col.tag == "fuel")
        {
            audioSource.PlayOneShot(playerpickup);
            WeaponDisplaySpawner.spawnedfuel = false; //got the rocket
            jetfuel += 20;//add jet fuel
            Destroy(col.gameObject);
        }

        //Getting hit
        if (col.tag == "bulletTwo")
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
            if (PlayerTwoController.isSniperTwo)
            {
                playerhealth -= 4;
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
        if (col.tag == "punchTwo")//get punched
        {
            Instantiate(hitParticles, this.transform.position + offset, this.transform.rotation);
            audioSource.PlayOneShot(playerhit);
            CameraShake.ShakeShakeCamera(); // camera shake
            playerhealth-=2;
            movestate = MovementState.knockback;
            if (GameObject.FindGameObjectWithTag("playerTwo").transform.rotation == Quaternion.Euler(0f, 0f, 0f))
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
        if(col.tag == "lava")//instant death
        {
            Instantiate(hitParticles, this.transform.position + offset, this.transform.rotation);
            isinstadead = true;
            CameraShake.ShakeShakeCamera(); //shake camera
            hoteldelluna();
            audioSource.PlayOneShot(playerhit);
        }
    }

    void hoteldelluna() // let the player rest then reincarnate to a player with full stat
    {
        combatstate = playercombatState.unarmed;
        UIManager.p2score++;
        playerhealth = maxplayerhealth;
        gameObject.SetActive(false);
        jetfuel = maxjetfuel;
        movestate = MovementState.moving;
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
        if (CameraController.camerasize == 15f)
        {
            CameraController.camerasize = 20f;
        }
        if (CameraController.camerasize == 20f)
        {
            CameraController.camerasize = 25f;
        }
        else if (CameraController.camerasize == 25f)
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