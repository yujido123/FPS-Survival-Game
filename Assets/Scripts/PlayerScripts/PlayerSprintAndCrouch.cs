using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintAndCrouch : MonoBehaviour
{

    private PlayerMovement playerMovement;

    public float moveSpeed = 5f;
    public float sprintSpeed = 10f;
    public float crouchSpeed = 2f;

    private bool isCrouching;
    private float crouchHeight = 1f;
    private float standHeight = 1.6f;

    public Transform lookRoot;

    // foot step sound
    private PlayerFootStep playerFootStep;
    private float sprintVolume = 1f;
    private float crouchVolume = 0.1f;
    private float walkVolumeMin = 0.2f, walkVolumeMax = 0.6f;
    private float walkStepDistance = 0.4f;
    private float sprintStepDistance = 0.25f;
    private float crouchStepDistance = 0.5f;

    // display stamina UI
    private PlayerStats player_Stats;
    private float sprint_Value;
    public float maxStamina = 100f;
    public float sprint_Spend_Stamina = -30;
    public float stamina_Recover = 30;
    private bool isSprint;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();

        playerFootStep = GetComponentInChildren<PlayerFootStep>();

        player_Stats = GetComponent<PlayerStats>();
    }

    void Start()
    {
        playerFootStep.volumeMin = walkVolumeMin;
        playerFootStep.volumeMax = walkVolumeMax;
        playerFootStep.stepDistance = walkStepDistance;

        isSprint = false;
        sprint_Value = maxStamina;
    }


    void Update()
    {
        Sprint();
        Crouch();
        ApplyStamina();
    }

    void ApplyStamina()
    {
        float staminaIncrease = isSprint ? sprint_Spend_Stamina : stamina_Recover;
        sprint_Value += staminaIncrease * Time.deltaTime;
        if (sprint_Value >= maxStamina)
            sprint_Value = maxStamina;

        if (sprint_Value <= 0)
            sprint_Value = 0;

        player_Stats.DisplayStaminaStats(sprint_Value, maxStamina);
    }

    void Sprint()
    {
        if(
            !isCrouching && 
            Input.GetKeyDown(KeyCode.LeftShift) &&
            sprint_Value > 0)
        {
            playerMovement.speed = sprintSpeed;

            // when sprint, make distance and volume change
            playerFootStep.stepDistance = sprintStepDistance;
            playerFootStep.volumeMin = sprintVolume;
            playerFootStep.volumeMax = sprintVolume;

            isSprint = true;
        }
        if(sprint_Value <= 0 || (!isCrouching && Input.GetKeyUp(KeyCode.LeftShift)))
        {
            playerMovement.speed = moveSpeed;

            // when walk, recover distance and volume
            playerFootStep.stepDistance = walkStepDistance;
            playerFootStep.volumeMin = walkVolumeMin;
            playerFootStep.volumeMax = walkVolumeMax;

            isSprint = false;
        }
    }

    void Crouch()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (isCrouching)
            {
                isCrouching = false;
                playerMovement.speed = moveSpeed;
                lookRoot.localPosition = new Vector3(lookRoot.localPosition.x, standHeight, lookRoot.localPosition.z);

                // when walk, recover distance and volume
                playerFootStep.stepDistance = walkStepDistance;
                playerFootStep.volumeMin = walkVolumeMin;
                playerFootStep.volumeMax = walkVolumeMax;
            }
            else
            {
                isCrouching = true;
                playerMovement.speed = crouchSpeed;
                lookRoot.localPosition = new Vector3(lookRoot.localPosition.x, crouchHeight, lookRoot.localPosition.z);

                // when crouch, make distance and volume change
                playerFootStep.stepDistance = crouchStepDistance;
                playerFootStep.volumeMin = crouchVolume;
                playerFootStep.volumeMax = crouchVolume;
            }

        }
    }
}
