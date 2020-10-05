using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    [Header("States")]
    public bool isPlayer1;
    public bool isAlive = true;
    private string inputKey = "_1";

    [Header("Horizontal Movement")]
    [SerializeField] private float horizontalForce = 10f;
    private float maxRotation = 15f;
    public float rotationSpeed = 10f;

    [Header("Turbo")]
    [SerializeField] private float turboForce = 15f;
    private float timeSinceTurbo = 0f;
    [SerializeField] private float turboStartForce = 0f;
    [SerializeField] private float turboCooldown;
    [SerializeField] private float turboOff;

    [Header("Break")]
    [SerializeField] private float breakForce = 10f;
    private float baseLinearDrag;
    [SerializeField] private float breakLinearDrag = 0f;

    [Header("Straff")]
    [SerializeField] private float straffForce = 0f;
    [SerializeField] private float straffCooldown = 0f;
    [SerializeField] private float straffMass = 0f;

    [Header("Fuel")]
    public float turboFuel;
    [SerializeField] private float maxFuelValue = 100f;
    [SerializeField] private float fuelDecayTickSpeed = 0.1f;
    [SerializeField] private float fuelDecayValue = 0.5f;
    [SerializeField] private float straffFuelCost = 10f;
    private float y = 0f;

    [Header("Restriction")]
    public bool canMoveHorizontal;
    public bool canTurbo;
    public bool canBreak;
    public bool canStraff;

    [Header("Debug")]
    [SerializeField] [Range(0, 1)] private int breakControlType = 0;
    [SerializeField] private bool infiniteFuel = false;
    public bool isTurbo;
    public bool asReleaseTurbo = false;
    public bool isBreak;
    public bool isStraffLeft;
    public bool isStaffRight;
    public bool isStraffing;

    [Header("Components")]
    [HideInInspector] public Rigidbody2D playerRb;
    [HideInInspector] public TrailRenderer playerTrail;

    private void Awake()
    {
        //Get les components
        playerRb = GetComponent<Rigidbody2D>();
        baseLinearDrag = playerRb.drag;
        playerTrail = GetComponentInChildren<TrailRenderer>();


        //Pour get les bons input
        if (isPlayer1)
            inputKey = "_1";
        else
            inputKey = "_2";
    }

    private void FixedUpdate()
    {
        GetInput();
        TrailManager();

        if (infiniteFuel && turboFuel <= 0)
            turboFuel = maxFuelValue;

        if(transform.position.y <= -3.5)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Récupère les inputs de la manette
    /// </summary>
    private void GetInput()
    {
        //Déplacement Horizontal
        float horizontal = Input.GetAxisRaw("LeftJoystickHorizontal" + inputKey);

        if (horizontal != 0 && canMoveHorizontal)
        {
            playerRb.AddForce(new Vector2(horizontal, 0).normalized * horizontalForce, ForceMode2D.Force);
        }

        CarRotation(horizontal);

        //Turbo
        isTurbo = Input.GetButton("AButton" + inputKey);
        //asReleaseTurbo = Input.GetButtonUp("AButton" + inputKey);

        //if (asReleaseTurbo && canTurbo)
          //  StartCoroutine(TurboAntiSpam());

        if (isTurbo && canTurbo && turboFuel > 0 && !isBreak)
        {
            Turbo();

            timeSinceTurbo += Time.fixedDeltaTime;
        }
        else
        {
            timeSinceTurbo = 0f;
        }


        //Break
        if (breakControlType == 0)
            //Le PJ freine quand le joueur appuie sur B
            isBreak = Input.GetButton("BButton" + inputKey);
        else
        {
            //le PJ freine quand le joueur met le stick vers le bas
            float vertical = Input.GetAxis("LeftJoystickVertical" + inputKey);
            
            if (vertical > 0)
                isBreak = true;
            else
                isBreak = false;
        }

        if (isBreak && canBreak && !isTurbo)
        {
            Break();
            playerRb.drag = breakLinearDrag;
        }
        else playerRb.drag = baseLinearDrag;

        //Straff
        isStraffLeft = Input.GetButtonDown("LeftBumper" + inputKey);
        isStaffRight = Input.GetButtonDown("RightBumper" + inputKey);

        if ((isStaffRight || isStraffLeft) && !isStraffing && canStraff && turboFuel > 0)
        {
            StartCoroutine(Straff());
        }
    }

    /// <summary>
    /// Gestion du déplacement et de la consommation quand le joueur utilise le turbo
    /// </summary>
    private void Turbo()
    {
        if(timeSinceTurbo == 0)
        {
            playerRb.AddForce(Vector2.up * turboStartForce, ForceMode2D.Impulse);
        }

        playerRb.AddForce(Vector2.up * turboForce, ForceMode2D.Force);

        if (y < fuelDecayTickSpeed)
            y += Time.fixedDeltaTime;
        else
        {
            FuelConsomption();
            y = 0;
        }
            
    }

    /// <summary>
    /// Tout ce qui touche au freinage
    /// </summary>
    private void Break()
    {
        playerRb.AddForce(Vector2.down * breakForce, ForceMode2D.Force);
    }

    /// <summary>
    /// Pour la consommation de la jauge de fuel
    /// </summary>
    private void FuelConsomption()
    {
        turboFuel -= fuelDecayValue;
    }

    /// <summary>
    /// Pour les gains de turbo
    /// </summary>
    /// <param name="fuelGained"></param>
    public void FuelGain(float fuelGained)
    {
        turboFuel += fuelGained;

        if(turboFuel >= maxFuelValue)
        {
            turboFuel = maxFuelValue;
        }
    }

    /// <summary>
    /// Tilt la voiture quand tu tourne
    /// </summary>
    public void CarRotation(float horizontal)
    {
        float angle = 0f;

        if (horizontal == 0)
        {
            angle = Mathf.LerpAngle(transform.eulerAngles.z, 0f, rotationSpeed * Time.deltaTime);
        }
        else if(horizontal > 0)
        {
            angle = Mathf.LerpAngle(transform.eulerAngles.z, -maxRotation, rotationSpeed * Time.deltaTime);
        }
        else if(horizontal < 0)
        {
            angle = Mathf.LerpAngle(transform.eulerAngles.z, maxRotation, rotationSpeed * Time.deltaTime);
        }
        
        transform.eulerAngles = new Vector3(0f, 0f, angle);
    }

    /// <summary>
    /// Applique les changements sur le trail
    /// </summary>
    private void TrailManager()
    {
        //Impact du frein sur la trail 
        if (!isBreak && !isStraffing && playerRb.velocity != Vector2.zero)
        {
            playerTrail.emitting = true;
        }
        else
        {
            playerTrail.emitting = false;
        }

        //Impact du turbo sur la trail
        if (!isTurbo)
        {
            playerTrail.widthMultiplier = 0.3f;
        }
        else
        {
            playerTrail.widthMultiplier = 0.5f;
        }
    }

    /// <summary>
    /// Tout ce qui concerne la méca de Straff
    /// </summary>
    /// <returns></returns>
    private IEnumerator Straff()
    {
        isStraffing = true;

        turboFuel -= straffFuelCost;

        GetComponent<EchoScript>().canEcho = true;
        playerRb.mass = straffMass;

        canMoveHorizontal = false;
        canTurbo = false;
        canBreak = false;

        playerRb.velocity = Vector2.zero;

        if (isStraffLeft && !isStaffRight)
        {
            playerRb.AddForce(Vector2.left * straffForce, ForceMode2D.Impulse);
            transform.eulerAngles = new Vector3(0f, 0f, maxRotation);
        }
        else if  (isStaffRight && !isStraffLeft)
        {
            playerRb.AddForce(Vector2.right * straffForce, ForceMode2D.Impulse);
            transform.eulerAngles = new Vector3(0f, 0f, -maxRotation);
        }

        yield return new WaitForSecondsRealtime(straffCooldown);

        GetComponent<EchoScript>().canEcho = false;
        playerRb.mass = 1f;

        canMoveHorizontal = true;
        canTurbo = true;
        canBreak = true;

        isStraffing = false;
    }

    public IEnumerator TurboOff()
    {
        canTurbo = false;

        yield return new WaitForSeconds(turboOff);

        canTurbo = true;
    }

    /*private IEnumerator TurboAntiSpam()
    {
        canTurbo = false;

        yield return new WaitForSecondsRealtime(turboCooldown);

        canTurbo = true;
    }*/

}
