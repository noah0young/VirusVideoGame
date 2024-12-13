using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D myRigidbody;
    private Animator myAnim;
    private SpriteRenderer mySprite;
    //private Collider2D myJumpCollider;
    private Collider2D myCollider;
    [Header("Movement")]
    private Vector2 platformVelocity = new Vector2(0,0);
    private List<Rigidbody2D> stadingOnRigidbodies = new List<Rigidbody2D>();
    private bool canMove = true;
    public float speed = 5f;
    private Door doorPath;
    //private int doorNum = -1;
    private Vector3 lastCheckPointPos;
    [Header("Jump")]
    public LayerMask GroundLayer;
    public float jumpCastHeight = 1f;
    public float deathCastHeight = 0f;
    public float jumpSpeed = 5f;
    public float jumpTimeLength = 1f;
    private bool inAir = true;
    public float upGravity = 1;
    public float downGravity = 1.3f;
    public float terminalVelocity = -8f;
    [Header("Jump Extras")]
    private bool jumped = false;
    public float extraJumpSecs = .1f;
    private float remainingExtraJumpSecs = 0;
    public float quickJumpSecs = .1f;
    private float remianingQuickJumpSecs = 0;
    // The num of frames after a jump press where jump will start again
    [Header("Trash Can")]
    private bool canShoot = true;
    private bool facingRight = true;
    public GameObject trashCan;
    public float shootSpeed;
    public float shootRecharge;
    [Header("HP")]
    public int maxHP = 2;
    public static bool[] hasHearts = new bool[4];
    private int curHP;
    private HPSystem hpSystem;
    public float pushBackSpeed = 3f;
    public float pushBackBonusHeight = 2f;
    public float pushBackTime = .3f;
    public float invinsibleFlashTime = .1f;
    public float invinsiblityTime = .5f;
    private bool isInvinsible = false;
    [Header("Encryption")]
    private EncryptedTilemap[] encryptedTilemaps;
    public enum EncState { NONE, PURPLE, GREEN };
    private EncState encState = EncState.NONE;
    private bool purpleAllowed = false;
    private bool greenAllowed = false;
    private bool canEncrypt = true;
    [Header("Double Jump")]
    private bool hasDoubleJump = false;
    private bool canDoubleJump = false;
    public float doubleJumpSpeed = 5;
    [Header("Color")]
    public Color defaultColor = Color.white;
    public Color[] encryptedColor;
    [Header("Camera")]
    public Camera mainCamera;
    public Camera trashCamera;
    [Header("Polish")]
    //private CameraEffects cameraEffects;
    public AudioSource dmgSound;
    public AudioSource jumpSound;
    public AudioSource trashShootSound;
    public AudioSource landSound;
    public AudioSource gotSomethingSound;
    public AudioSource gotHeartSound;
    public AudioSource backgroundMusic;
    public ParticleSystem dmgParticles;
    public GameObject deathEffect;
    [Header("Input")]
    private float curDirection;
    private bool curPressedJump;
    private bool curPressedDJump;
    private bool curPressedShoot;
    private bool curReleasedJump;
    [Header("Mobile Controls")]
    public MobileControls controls;
    private bool usingMobileControls = false;
    // Start is called before the first frame update

    void Start()
    {
        resetCamera();
        isInvinsible = false;
        curHP = maxHP;
        hpSystem = GameObject.Find("HPCanvas").GetComponent<HPSystem>();
        resetDoorPath();
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        mySprite = GetComponentInChildren<SpriteRenderer>();
        encryptedTilemaps = setArrayUsingLayer<EncryptedTilemap>("EncryptedTilemap");
        hpSystem.setMaxHP(maxHP);
        myCollider = GetComponent<BoxCollider2D>();

        // Loads Data if continuing
        if (LevelManager.loadData)
        {
            LoadData(LevelManager.LoadPlayerPos(), LevelManager.LoadHasPurpleEnc(), LevelManager.LoadHasGreenEnc(), LevelManager.LoadHasDoubleJump(), LevelManager.LoadMaxHP(), LevelManager.LoadHasHearts(), LevelManager.LoadCurHP(), LevelManager.LoadEncState());
        }

        lastCheckPointPos = transform.position;

        LevelManager.PrintWelcome();
    }

    // Update is called once per frame
    private void Update()
    {
        saveInput();
		//StartCoroutine(resetInputAfterFrame());
		if (canMove)
		{
			openDoor();
			if (!inAir && LevelManager.hasLoadedScene())
			{
				toggleEncryption();
			}
		}
	}

    private void saveInput()
    {
        curPressedJump = pressedJump() || curPressedJump;
        if (hasDoubleJump)
        {
            curPressedDJump = pressedJump() || curPressedDJump;
        }
        curReleasedJump = releasedJump() || curReleasedJump;
        curPressedShoot = pressedShoot() || curPressedShoot;
        curDirection = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        updateJumpExtras();
        if (canMove)
        {
            setInAir();
            checkBeingCrushed();
            shoot();
            move();
            jump();
            /*openDoor();
            if (!inAir && LevelManager.hasLoadedScene())
            {
                toggleEncryption();
            }*/
            if (stadingOnRigidbodies.Count > 0)
            {
                // The first moving platform is always used until it is removed
                platformVelocity = stadingOnRigidbodies[0].velocity;
                // This adds all parent velocities as well
                Rigidbody2D stadingOnRigidbodyParent = stadingOnRigidbodies[0].transform.parent.GetComponent<Rigidbody2D>();
                while (stadingOnRigidbodyParent != null)
                {
                    platformVelocity += stadingOnRigidbodyParent.velocity;
                    stadingOnRigidbodyParent = stadingOnRigidbodyParent.transform.parent.GetComponent<Rigidbody2D>();
                }
            }
            LimitVelocity();
        }
    }

    private void LimitVelocity()
    {
        Vector2 velocity = myRigidbody.velocity;
        if (velocity.y < terminalVelocity)
        {
            velocity.y = terminalVelocity;
        }
        myRigidbody.velocity = velocity;
    }

    /*private void jumpOrOpenDoor()
    {
        if ((Input.GetKeyDown("w") || Input.GetKeyDown("up")) && !inAir)
        {
            if (doorPath.Equals(""))
            {
                jump();
            }
            else
            {
                openDoor();
            }
        }
    }*/

    private void openDoor()
    {
        if ((Input.GetKeyDown("w") || Input.GetKeyDown("up")) && !inAir && LevelManager.hasLoadedScene())
        {
            if (doorPath != null)
            {
                LevelManager.EnteredDoorMsg();
                moveToScene(doorPath);
                StartCoroutine(setLocToDoorAndResetEnc(doorPath.transform.position, doorPath.connectionNum));
            }
        }
    }

    private void moveToScene(Door door)
    {
        LevelManager.SwitchCurScene(door.nextScene);
        if (door.nextScene.Equals("Trash"))
        {
            switchToTrashCamera();
        }
        else
        {
            resetCamera();
        }
    }

    private  void jump()
    {
        if (shouldJump())
        {
            Vector2 velocity = myRigidbody.velocity;
            jumped = true;
            canDoubleJump = true;
            velocity.y = jumpSpeed;
            myRigidbody.velocity = velocity;
            StartCoroutine(jumpGravity());
            jumpSound.Play();

            curPressedDJump = false;
        }
        else if (shouldDoubleJump())
        {
            jumpSound.Play();
            canDoubleJump = false;
            Vector2 velocity = myRigidbody.velocity;
            velocity.y = doubleJumpSpeed;
            myRigidbody.velocity = velocity;
            StartCoroutine(jumpGravity());
        }
    }

    private bool pressedJump()
    {
        if (!usingMobileControls) {
            return (Input.GetKeyDown("j") || Input.GetKeyDown("x"));
        }
        else
        {
            return controls.justPressedJump();
        }
    }

    private bool shouldJump()
    {
        if (curPressedJump && !jumped && inAir && remainingExtraJumpSecs > 0)
        {
            remainingExtraJumpSecs = 0;
            remianingQuickJumpSecs = 0;
            curPressedJump = false;
            return true;
        }

        if (remianingQuickJumpSecs > 0 && !inAir)
        {
            remianingQuickJumpSecs = 0;
            return true;
        }

        if (curPressedJump)//(Input.GetKeyDown("j") || Input.GetKeyDown("x"))
        {
            curPressedJump = false;
            if (!inAir) {
                remianingQuickJumpSecs = 0;
                return true;
            }
            else
            {
                remianingQuickJumpSecs = quickJumpSecs;
            }
        }
        return false;
    }

    private bool shouldDoubleJump()
    {
        if (canDoubleJump && hasDoubleJump && curPressedDJump)
        {
            curPressedDJump = false;
            return true;
        }
        return false;
    }

    public void getDoubleJump()
    {
        hasDoubleJump = true;
    }

    private void updateJumpExtras()
    {
        if (remianingQuickJumpSecs > 0)
        {
            remianingQuickJumpSecs -= Time.fixedDeltaTime;//Time.deltaTime;
        }
        if (remainingExtraJumpSecs > 0)
        {
            remainingExtraJumpSecs -= Time.fixedDeltaTime;//Time.deltaTime;
        }
    }

    private bool releasedJump()
    {
        if (!usingMobileControls)
        {
            return Input.GetKeyUp("j") || Input.GetKeyUp("x");
        }
        else
        {
            return controls.justReleasedJump();
        }
    }

    private IEnumerator jumpGravity()
    {
        bool isSecondJump = !canDoubleJump;
        myRigidbody.gravityScale = upGravity;
        float timePassed = 0;
        yield return new WaitUntil(() => {
            if (releasedJump())//curReleasedJump)
            {
                //Debug.Log("Jump Stopped");
                //curReleasedJump = false;
                return true;
            }
            timePassed += Time.deltaTime;
            return timePassed >= jumpTimeLength;
        });
        if (isSecondJump || canDoubleJump)
        {
            myRigidbody.gravityScale = downGravity;
            yield return new WaitUntil(() => !inAir);
            myRigidbody.gravityScale = upGravity;
        }
    }

    private bool pressedShoot()
    { 
        if (!usingMobileControls)
        {
            return (Input.GetKeyDown("k") || Input.GetKeyDown("z"));
        }
        else
        {
            return controls.justPressedShoot();
        }
    }

    private void shoot()
    {
        if (curPressedShoot && canShoot)
        {
            curPressedShoot = false;
            trashShootSound.Play();
            myAnim.SetTrigger("Shooting");
            canShoot = false;
            StartCoroutine(projectileRecharge());
            GameObject projectile = Instantiate(trashCan);
            projectile.transform.position = transform.position;
            float projectileSpeed = shootSpeed;
            if (!facingRight)
            {
                projectileSpeed = -projectileSpeed;
                projectile.transform.localScale = new Vector3(-projectile.transform.localScale.x,
                                                               projectile.transform.localScale.y,
                                                               projectile.transform.localScale.z);
            }
            else
            {
                projectile.transform.localScale = new Vector3(projectile.transform.localScale.x,
                                                              projectile.transform.localScale.y,
                                                              projectile.transform.localScale.z);
            }   
            projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed, 0);
        }
    }

    private IEnumerator projectileRecharge()
    {
        yield return new WaitForSeconds(shootRecharge);
        canShoot = true;
    }

    private void move()
    {
        Vector2 velocity = myRigidbody.velocity;// + platformVelocity;
        float direction = 0f;
        if (!usingMobileControls)
        {
            direction = curDirection;//Input.GetAxis("Horizontal");
        }
        else
        {
            direction = controls.horizontal();
        }
        if (direction != 0)
        {
            myAnim.SetBool("walking", true);
            if (direction < 0)
            {
                // Facing Left
                mySprite.flipX = true;
                facingRight = false;
                /*Vector3 spriteScale = mySprite.transform.localScale;
                facingRight = false;
                spriteScale.x = -Mathf.Abs(spriteScale.x);
                mySprite.transform.localScale = spriteScale;*/
            }
            else
            {
                // Facing Right
                mySprite.flipX = false;
                facingRight = true;
                /*Vector3 spriteScale = mySprite.transform.localScale;
                facingRight = true;
                spriteScale.x = Mathf.Abs(spriteScale.x);
                mySprite.transform.localScale = spriteScale;*/
            }
        }
        else
        {
            myAnim.SetBool("walking", false);
        }
        velocity.x = direction * speed + platformVelocity.x;
        if (platformVelocity.y < 0)
        {
            velocity.y += platformVelocity.y;
            // This only applies when moving the player down with a platform
            // (Unity's collision system will automatically bring the player up)
        }
        myRigidbody.velocity = velocity;
    }

    /*private void accMove()
    {
        // Adds acceleration
        float direction = Input.GetAxis("Horizontal");
        float mass = myRigidbody.mass;
        
        // Checks if the player is over max speed
        Vector2 velocity = myRigidbody.velocity;
        if (velocity.x > maxSpeed)
        {
            velocity.x = maxSpeed;
        }
        else if (velocity.x < -maxSpeed)
        {
            velocity.x = -maxSpeed;
        }

        // Checks if the player is not moving and tries to stop them
        myRigidbody.AddForce(new Vector2(acceleration * direction * mass, 0));
        if (direction == 0f && velocity.x != 0)
        {
            float reverseDirection = (-Mathf.Abs(velocity.x) / velocity.x);
            myRigidbody.AddForce(new Vector2(slowDown * reverseDirection * mass, 0));
            velocity = myRigidbody.velocity;
            if ((reverseDirection > 0 && velocity.x > 0) || (reverseDirection < 0 && velocity.x < 0))
            {
                velocity.x = 0;
            }
        }

        myRigidbody.velocity = velocity;
    }*/

    public void setInAir()//)bool a)
    {
        //RaycastHit2D[] castHits = new RaycastHit2D[1];
        //Vector2 boxCastSize = myCollider.bounds.size;
        //boxCastSize.x -= .1f;
        bool prevInAir = inAir;
        RaycastHit2D castHit = Physics2D.BoxCast(myCollider.bounds.center, myCollider.bounds.size, 0, Vector2.down, jumpCastHeight, GroundLayer);
        
        inAir = castHit.collider == null;

        //float debugYHeight = myCollider.bounds.center.y * .5f;

        //myJumpCollider.Cast(Vector2.down, castHits, jumpCastHeight) <= 0;
        if (!inAir)
            {
                jumped = false;
            if (prevInAir)
            {
                landSound.Play();
            }
            }
            else if (!prevInAir)
            {
                remainingExtraJumpSecs = extraJumpSecs;
            
        }
        myAnim.SetBool("inAir", inAir);
    }

    public void checkBeingCrushed()
    {
        Vector2 boxCastSize = myCollider.bounds.size;
        boxCastSize.x -= .1f;
        RaycastHit2D castHit = Physics2D.BoxCast(myCollider.bounds.center, boxCastSize, 0, Vector2.up, deathCastHeight, GroundLayer);

        bool somethingAbove = castHit.collider != null;

        if (!inAir && somethingAbove)
        {
            StartCoroutine(death());
        }
    }

    public void getPurpleEncryptionKey()
    {
        purpleAllowed = true;
    }

    public void getGreenEncryptionKey()
    {
        greenAllowed = true;
    }

    private void NextEncState()
    {
        if (encState == EncState.NONE)
        {
            if (purpleAllowed)
            {
                encState = EncState.PURPLE;
            }
            else if (greenAllowed)
            {
                encState = EncState.GREEN;
            }
        }
        else if (encState == EncState.PURPLE)
        {
            encState = EncState.NONE;
            if (greenAllowed)
            {
                encState = EncState.GREEN;
            }
        }
        else if (encState == EncState.GREEN)
        {
            encState = EncState.NONE;
        }
    }

    public void setEncState(EncState e)
    {
        if (encState != EncState.NONE)
        {
            foreach (EncryptedTilemap eT in encryptedTilemaps)
            {
                eT.toggleEncryption(encState);
            }
        }
        encState = e;
        if (encState != EncState.NONE)
        {
            foreach (EncryptedTilemap eT in encryptedTilemaps)
            {
                eT.toggleEncryption(encState);
            }
        }
        SetColor();
    }

    public void toggleEncryption()
    {
        if (Input.GetKeyDown("e") && canEncrypt && !inAir)
        {
            if (encState != EncState.NONE)
            {
                foreach (EncryptedTilemap eT in encryptedTilemaps)
                {
                    eT.toggleEncryption(encState);
                }
            }
            NextEncState();
            if (encState != EncState.NONE)
            {
                foreach (EncryptedTilemap eT in encryptedTilemaps)
                {
                    eT.toggleEncryption(encState);
                }
            }
            SetColor();
        }
    }

    private void SetColor()
    {
        mySprite.color = defaultColor;
        if (encState == EncState.PURPLE)
        {
            mySprite.color = encryptedColor[0];
        }
        else if (encState == EncState.GREEN)
        {
            mySprite.color = encryptedColor[1];
        }
    }

    public void setDoorPath(Door door)
    {
        doorPath = door;
        if (door.connectionNum == -1)
        {
            Debug.LogWarning("Please set connectionNum to a positive number that matches with a door in the next scene");
        }
    }

    public void resetDoorPath()
    {
        doorPath = null;
    }

    private IEnumerator setLocToDoorAndResetEnc(Vector2 origDoorPos, int connectionNum)
    {
        yield return new WaitUntil(() => LevelManager.hasLoadedScene());
        // Prints welcome message
        LevelManager.PrintCD();
        LevelManager.PrintWelcome();
        //Sets Enc State to the old one
        encryptedTilemaps = setArrayUsingLayer<EncryptedTilemap>("EncryptedTilemap");
        EncState oldEncState = encState;
        encState = EncState.NONE;
        setEncState(oldEncState);
        // Sets location based on the location of the other door with the same connection number
        GameObject[] doorsObjs = GameObject.FindGameObjectsWithTag("Door");
        foreach(GameObject doorObj in doorsObjs)
        {
            Door door = doorObj.GetComponent<Door>();
            Vector2 newPosition = doorObj.transform.position;
            if (newPosition != origDoorPos && door.connectionNum == connectionNum)
            {
                transform.position = newPosition;
                lastCheckPointPos = newPosition;
                doorPath = door;
            }
        }
    }

    private T[] setArrayUsingLayer<T>(string tag)
    {
        GameObject[] objsWithTag = GameObject.FindGameObjectsWithTag(tag);
        T[] resultArray = new T[objsWithTag.Length];
        for (int i = 0; i < objsWithTag.Length; i++)
        {
            resultArray[i] = objsWithTag[i].GetComponent<T>();
        }
        return resultArray;
    }
    
    public void setCanEncrypt(bool temp)
    {
        canEncrypt = temp;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("DoubleJumpSphere"))
        {
            StartCoroutine(playGotSomethingSound());
            hasDoubleJump = true;
            Destroy(collision.gameObject);
            LevelManager.GotItem("Double Jump");
            StartCoroutine(LevelManager.PauseAndSpeak("You have just found the Double Jump Sphere. Now you can use a jump while in the air!"));
            //StartCoroutine(LevelManager.PauseAndSpeak("Game has been saved!"));
            Save();
        }
        else if (collision.transform.CompareTag("PurpleEncKey"))
        {
            StartCoroutine(playGotSomethingSound());
            purpleAllowed = true;
            Destroy(collision.gameObject);
            LevelManager.GotItem("Purple Encryption Key");
            StartCoroutine(LevelManager.PauseAndSpeak("You have just found the Purple Encryption Key. Now you can use the e key to use purple encryption on yourself!"));
            //StartCoroutine(LevelManager.PauseAndSpeak("Game has been saved!"));
            Save();
        }
        else if (collision.transform.CompareTag("GreenEncKey"))
        {
            StartCoroutine(playGotSomethingSound());
            greenAllowed = true;
            Destroy(collision.gameObject);
            LevelManager.GotItem("Green Encryption Key");
            StartCoroutine(LevelManager.PauseAndSpeak("You have just found the Green Encryption Key. Now you can use the e key to use green encryption on yourself!"));
            //StartCoroutine(LevelManager.PauseAndSpeak("Game has been saved!"));
            Save();
        }
        else if (collision.transform.CompareTag("Enemy") || collision.transform.CompareTag("Boss"))
        {
            takeDmg();
            StartCoroutine(invinsiblePushBack(collision.transform.position));
        }
        else if (collision.transform.CompareTag("Lava") || collision.transform.CompareTag("TrashBoss"))
        {
            StartCoroutine(death());
        }
        else if (collision.collider.CompareTag("Heart"))
        {
            gotHeartSound.Play();
            heal1HP();
            Destroy(collision.collider.gameObject);
            LevelManager.GotItem("1 HP");
        }
        else if (collision.transform.CompareTag("HeartContainer"))
        {
            StartCoroutine(playGotSomethingSound());
            incMaxHP();
            Destroy(collision.gameObject);
            LevelManager.GotItem("HP Up");
            StartCoroutine(LevelManager.PauseAndSpeak("Your max health has increased by 1"));
            hasHearts[collision.gameObject.GetComponent<HasHeart>().heartNum] = true;
            //StartCoroutine(LevelManager.PauseAndSpeak("Game has been saved!"));
            Save();
        }
        else if (collision.transform.CompareTag("MovingPlatform") || collision.transform.CompareTag("Lights"))
        {
            Rigidbody2D collisionRigidbody = collision.transform.GetComponent<Rigidbody2D>();
            if (collisionRigidbody != null)
            {
                stadingOnRigidbodies.Add(collisionRigidbody);
            }
        }
    }

    private IEnumerator playGotSomethingSound()
    {
        gotSomethingSound.Play();
        backgroundMusic.Pause();
        yield return new WaitUntil(() => !gotSomethingSound.isPlaying);
        backgroundMusic.Play();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("MovingPlatform") || collision.transform.CompareTag("Lights"))
        {
            Rigidbody2D collisionRigidbody = collision.transform.GetComponent<Rigidbody2D>();
            if (collisionRigidbody != null)
            {
                platformVelocity = new Vector2(0, 0);
                stadingOnRigidbodies.Remove(collisionRigidbody);
            }
        }
    }

    public void incMaxHP()
    {
        maxHP++;
        curHP++;
        hpSystem.setMaxHP(maxHP);
        hpSystem.setRemainingHearts(curHP);
    }

    public void takeDmg()
    {
        if (!isInvinsible)
        {
            curHP--;
            if (curHP <= 0)
            {
                StartCoroutine(death());
                //LoadGameOver();
            }
            else
            {
                hpSystem.setRemainingHearts(curHP);
                dmgSound.Play();
                dmgParticles.Play();
                CameraEffects.screenShake();
                CameraEffects.stopAndSlowDown(.05f, .3f, .3f);
            }
        }
    }

    private IEnumerator death()
    {
        GameObject dEffect = Instantiate(deathEffect);
        dEffect.transform.position = transform.position;
        mySprite.gameObject.SetActive(false);
        freezePlayerPos();
        myCollider.enabled = false;
        CameraEffects.stopAndSlowDown(.05f, .3f, .3f);
        yield return new WaitForSeconds(1.5f);
        LoadGameOver();
    }

    public void freezePlayerPos()
    {
        myRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private IEnumerator invinsiblePushBack(Vector3 enemyPos)
    {
        canMove = false;
        StartCoroutine(invinsiblity());
        Vector2 velocity = (transform.position - enemyPos).normalized * pushBackSpeed;
        velocity.y += pushBackBonusHeight;
        myRigidbody.velocity = velocity;
        yield return new WaitForSeconds(pushBackTime);
        canMove = true;
    }

    private IEnumerator invinsiblity()
    {
        isInvinsible = true;
        float timePassed = 0;
        while (timePassed < invinsiblityTime)
        {
            mySprite.enabled = false;
            yield return new WaitForSeconds(invinsibleFlashTime);
            timePassed += invinsibleFlashTime;
            mySprite.enabled = true;
            yield return new WaitForSeconds(invinsibleFlashTime);
            timePassed += invinsibleFlashTime;
        }
        isInvinsible = false;
    }

    public void heal1HP()
    {
        curHP++;
        if (curHP > maxHP)
        {
            curHP = maxHP;
        }
        hpSystem.setRemainingHearts(curHP);
    }

    public void Save()
    {
        LevelManager.Save(lastCheckPointPos, purpleAllowed, greenAllowed, hasDoubleJump, hasHearts, curHP, encState);
    }

    public void SaveAndQuitButton()
    {
        Save();
        Time.timeScale = 1;
        LevelManager.LoadTitle();
    }

    public void LoadData(Vector3 position, bool hasPurpleEnc, bool hasGreenEnc, bool hasDoubleJump, int mHP, bool[] hHearts, int cHP, EncState e)
    {
        transform.position = position;
        purpleAllowed = hasPurpleEnc;
        greenAllowed = hasGreenEnc;
        this.hasDoubleJump = hasDoubleJump;
        hasHearts = hHearts;
        maxHP = mHP;
        curHP = cHP;
        hpSystem.setMaxHP(maxHP);
        hpSystem.setRemainingHearts(curHP);
        setEncState(e);
    }

    public void LoadGameOver()
    {
        curHP = maxHP;
        Save();
        LevelManager.LoadGameOver();
    }

    public void switchToTrashCamera()
    {
        //mainCamera.SetActive(false);
        //trashCamera.SetActive(true);
        //cameraEffects = trashCamera.GetComponent<CameraEffects>();
        CameraEffects.setCamera(trashCamera);
    }

    public void resetCamera()
    {
        //mainCamera.SetActive(true);
        //trashCamera.SetActive(false);
        //cameraEffects = mainCamera.GetComponent<CameraEffects>();
        if (LevelManager.trashIsLoaded())
        {
            switchToTrashCamera();
        }
        else
        {
            CameraEffects.setCamera(mainCamera);
        }
    }
}
