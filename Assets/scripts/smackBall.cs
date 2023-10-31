using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smackBall : MonoBehaviour
{
    public static Rigidbody2D rig;

    public Transform powerAngleRef;
    private SpriteRenderer powerAngleColorRef;
    private Quaternion angleStore;
    private Vector3 positionStore;

    private Color[] powerColors;
    public float[] powerLevels;
    private int currentPowerLevel = 0;
    private int currentRotationLevel = 0;

    private bool shotTaken = false;
    public Transform spriteApprox;
    private float rotationInputDelay = .1f;

    [System.NonSerialized] public bool grounded = true;

    public static int powerUpState = 0; //0 = nothing, 1 = fire
    public SpriteRenderer golfBallColorRef;
    private int rotationlevelStorage; //for sticky powerup handling

    public static int strokes = 0;
    public static bool invalidShot = false;

    public static bool inEvent = true;

    public PhysicsMaterial2D bouncy;
    private AudioSource swingSFX;

    //In the particular case in which the ball is stuck on a corner pixel and cannot sense ground, this allows a shot to happen anyway.
    private float stuckShotTimer = 3f;
    public static Vector3 resetPoint;

    private bool power2Standby = true;
    private bool power3Standby = true;

    public CompositeCollider2D waterRef;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        angleStore = powerAngleRef.rotation;
        powerAngleColorRef = powerAngleRef.GetComponentInChildren<SpriteRenderer>();

        powerColors = new Color[powerLevels.Length];

        for(int i = 0; i < powerLevels.Length; i++)
        {
            powerColors[i] = Color.Lerp(Color.yellow, Color.red, (float)i / (float)powerLevels.Length);
        }

        resetPoint = new Vector3(-11.75f, -10.5f, 0);

        swingSFX = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!inEvent)
        {
            powerAngleRef.rotation = angleStore;
            if (!shotTaken)
            {
                if (Input.GetAxisRaw("Vertical") > .5f)
                {
                    if (currentPowerLevel < powerLevels.Length - 1 && rotationInputDelay < 0f)
                    {
                        currentPowerLevel++;
                        powerAngleRef.Translate(0, 1, 0);
                        rotationInputDelay = .1f;
                    }
                }
                else if (Input.GetAxisRaw("Vertical") < -.5f)
                {
                    if (currentPowerLevel > 0 && rotationInputDelay < 0f)
                    {
                        currentPowerLevel--;
                        powerAngleRef.Translate(0, -1, 0);
                        rotationInputDelay = .1f;
                    }
                }
                else if (Input.GetAxisRaw("Horizontal") < -.5f)
                {
                    if (currentRotationLevel > ((powerUpState == 2) ? -11 : -12) && rotationInputDelay < 0f)
                    {
                        powerAngleRef.RotateAround(transform.position, Vector3.forward, 7.5f);
                        currentRotationLevel--;
                        rotationInputDelay = .1f;
                    }
                }
                else if (Input.GetAxisRaw("Horizontal") > .5f)
                {
                    if (currentRotationLevel < ((powerUpState == 2) ? 11 : 12) && rotationInputDelay < 0f)
                    {
                        powerAngleRef.RotateAround(transform.position, Vector3.forward, -7.5f);
                        currentRotationLevel++;
                        rotationInputDelay = .1f;
                    }
                }
                rotationInputDelay -= Time.deltaTime;
                if (Input.GetButtonDown("Jump"))
                {
                    positionStore = transform.position;
                    shotTaken = true;
                    rig.velocity = powerLevels[currentPowerLevel] * (powerAngleRef.position - transform.position).normalized;
                    strokes++;
                    powerAngleColorRef.enabled = false;
                    swingSFX.Play();
                    swingSFX.pitch = Random.Range(0.7f, 1.1f);

                    if (powerUpState == 2)
                    {
                        transform.up = Vector2.up;
                        rig.gravityScale = 1;
                    }
                }
            }
            if (Input.GetButtonDown("Cancel"))
            {
                transform.position = resetPoint;
                transform.rotation = Quaternion.identity;
                rig.velocity = Vector2.zero;
                powerAngleRef.up = Vector3.up;
                powerAngleRef.localPosition = Vector3.up * 3;
                currentPowerLevel = 0;
                currentRotationLevel = 0;
                rig.gravityScale = 1;
                strokes++;
            }
            if (Input.GetButtonDown("Submit"))
            {
                fullReset();
            }
            if ((rig.velocity.sqrMagnitude < 1 && grounded))
            {
                if(powerUpState == 2 && shotTaken)
                {
                    powerAngleRef.localPosition = Vector3.up * 3;
                    currentPowerLevel = 0;
                    currentRotationLevel = 0;
                }
                shotTaken = false;
                transform.position = spriteApprox.position;
                powerAngleColorRef.enabled = true;
            }
            if (rig.velocity.sqrMagnitude < 1 && shotTaken)
            {
                stuckShotTimer -= Time.deltaTime;
                if(stuckShotTimer < 0f)
                {
                    invalidShot = true;
                    stuckShotTimer = 3f;
                }
            }
            else
            {
                stuckShotTimer = 3f;
            }

            powerAngleColorRef.color = powerColors[currentPowerLevel];

            switch (powerUpState)
            {
                case 1:
                    golfBallColorRef.color = new Color(1f, 0.7f, 0.7f);
                    break;
                case 2:
                    golfBallColorRef.color = new Color(.527f, .055f, .841f);
                    powerAngleRef.up = powerAngleRef.position - transform.position;
                    bouncy.friction = 0.4f;
                    bouncy.bounciness = 0f;
                    if (power2Standby)
                    {
                        StartCoroutine(updateForPhysMat());
                        power2Standby = false;
                    }
                    break;
                case 3:
                    golfBallColorRef.color = new Color(.4475f, .8688f, .8867f);
                    bouncy.friction = 0.3f;
                    bouncy.bounciness = 0.4f;
                    if (power3Standby)
                    {
                        StartCoroutine(updateForPhysMat());
                        power3Standby = false;
                    }
                    break;
                default:
                    golfBallColorRef.color = Color.white;
                    bouncy.friction = 0.3f;
                    bouncy.bounciness = 0.4f;
                    break;
            }

            if(transform.position.x < cameraFollow.worldBounds[0] || transform.position.y < cameraFollow.worldBounds[1] || transform.position.x > cameraFollow.worldBounds[2] || transform.position.y > cameraFollow.worldBounds[3])
            {
                invalidShot = true;
                //Debug.Log("Outside of world bounds; invalidating shot..");
            }

            if (invalidShot)
            {
                transform.position = positionStore;
                rig.velocity = Vector2.zero;
                strokes++;
                invalidShot = false;
            }

            if(powerUpState == 3)
            {
                waterRef.isTrigger = false;
            }
            else
            {
                waterRef.isTrigger = true;
            }
        }
        
    }

    private void LateUpdate()
    {
        if (!inEvent)
        {
            angleStore = powerAngleRef.rotation;
            powerAngleRef.rotation = Quaternion.identity;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        grounded = true;
        //Debug.Log("GROUNDING");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        grounded = false;
        //Debug.Log("NOT GROUNDED");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (powerUpState == 2)
        {
            if (Mathf.Abs(collision.contacts[0].normal.x) == 1 || Mathf.Abs(collision.contacts[0].normal.y) == 1)
            {
                transform.up = collision.GetContact(0).normal;
                /*
                if(collision.contacts[0].normal.x == 1)
                {
                    transform.Rotate(0, 0, -90);
                }
                else if (collision.contacts[0].normal.x == -1)
                {
                    transform.Rotate(0, 0, 90);
                }
                else if (collision.contacts[0].normal.y == -1)
                {
                    transform.Rotate(0, 0, 180);
                }
                */
                rig.velocity = Vector2.zero;
                rig.gravityScale = 0;
            }
        }
    }
    private void OnTriggerStay2D()
    {
        if(currentPowerLevel == 0 && currentRotationLevel == 0)
        {
            powerAngleRef.localPosition = Vector3.up * 3;
            currentPowerLevel = 0;
            currentRotationLevel = 0;
        }
        grounded = true;
    }

    IEnumerator updateForPhysMat()
    {
        GetComponent<CircleCollider2D>().enabled = false;
        yield return new WaitForSeconds(Time.fixedDeltaTime);
        GetComponent<CircleCollider2D>().enabled = true;
    }

    public void fullReset()
    {
        strokes = 0;
        resetPoint = new Vector3(-11.75f, -10.5f, 0);
        transform.position = resetPoint;
        transform.rotation = Quaternion.identity;
        rig.velocity = Vector2.zero;
        powerAngleRef.up = Vector3.up;
        powerAngleRef.localPosition = Vector3.up * 3;
        currentPowerLevel = 0;
        currentRotationLevel = 0;

        bouncy.friction = 0.3f;
        bouncy.bounciness = 0.4f;
        
        power3Standby = true;
        power2Standby = true;
        StartCoroutine(updateForPhysMat());

        powerUpState = 0;
        
        cameraFollow.worldBounds[0] = -16;
        cameraFollow.worldBounds[1] = -16;
        cameraFollow.worldBounds[2] = 16;
        cameraFollow.worldBounds[3] = 24;

        positionStore = resetPoint;

        rig.gravityScale = 1;

        GetComponent<resetScript>().factoryReset();
    }
}
