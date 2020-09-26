using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    //States
    public bool isPlayer1;
    public bool isAlive = true;
    private string inputKey = "_1";
    
    //Movement
    [SerializeField] private float horizontalForce = 10f;
    [SerializeField] private float turboForce = 15f;
    [SerializeField] private float breakForce = 10f;

    //Fuel
    public float turboFuel;
    [SerializeField] private float maxFuelValue = 100f;
    [SerializeField] private float fuelDecayTickSpeed = 0.1f;
    [SerializeField] private float fuelDecayValue = 0.5f;
    private float y = 0f;

    //Restrictions
    public bool canMoveHorizontal;
    public bool canTurbo;
    public bool canBreak;

    //Debug
    [SerializeField] [Range(0, 1)] private int breakControlType = 0;
    public bool isTurbo;
    public bool isBreak;

    //Components
    [HideInInspector] public Rigidbody2D playerRb;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();

        //Pour get les bons input
        if (isPlayer1)
            inputKey = "_1";
        else
            inputKey = "_2";
    }

    private void Update()
    {
        GetInput();
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

        //Turbo
        isTurbo = Input.GetButton("AButton" + inputKey);

        if (isTurbo && canTurbo && turboFuel > 0 && !isBreak)
            Turbo();

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
            Break();
    }

    /// <summary>
    /// Gestion du déplacement et de la consommation quand le joueur utilise le turbo
    /// </summary>
    private void Turbo()
    {
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

}
