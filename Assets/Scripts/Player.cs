using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(PlayersAttributeComponent))]
public class Player : SpaceshipBase
{
    public Action onSpacePressed; 

    [Header("Setup your player")]
    [SerializeField] float thrustSpeed = 3f;
    [SerializeField] float rotationSpeed = 1f;

    AudioSource audioSource;
    PlayersAttributeComponent playersAttributeComponent;

    bool disabledMovement = false;

    protected override void Awake()
    {
        base.Awake();

        playersAttributeComponent = GetComponent<PlayersAttributeComponent>(); 
        audioSource = GetComponent<AudioSource>();

        if (!playersAttributeComponent || !audioSource)
        {
            Debug.LogError("Something went wrong, check PlayerScript::Awake on: " + gameObject.name);
        }

        playersAttributeComponent.OnZeroHealth += ZeroHealth;
    }

    void Update()
    {
        if (disabledMovement)
        {
            return;
        }

        Move();
        Rotate();

        if (Input.GetKey(KeyCode.Space))
        {
            onSpacePressed?.Invoke();
            foreach (Gun gun in guns)
            {
                gun.Shoot();
            }
        }
    }

    void Move()
    {
        float verticalInputs = Input.GetAxis("Vertical") * thrustSpeed * Time.deltaTime; ;
        Vector3 thrust = new Vector3(0, verticalInputs, 0);

        HandleThrustAudio(verticalInputs > 0);
        transform.Translate(thrust, Space.Self);

        KeepPlayerInBounds();
    }

    void Rotate()
    {
        float horizontalInputs = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;

        transform.Rotate(0, 0, -horizontalInputs);
    }

    public PlayersAttributeComponent GetAttributeComponent()
    {
        return playersAttributeComponent;
    }

    public void AddGun()
    {
        foreach (Gun gun in guns)
        {
            if (!gun.IsActivate())
            {
                gun.ActiveSwitch(true);
                break;
            }
        }
    }

    void KeepPlayerInBounds()
    {
        Vector3 position = transform.position;
        Vector3 screenBoundsMin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 screenBoundsMax = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        position.x = Mathf.Clamp(position.x, screenBoundsMin.x, screenBoundsMax.x);
        position.y = Mathf.Clamp(position.y, screenBoundsMin.y, screenBoundsMax.y);

        transform.position = position;
    }

    public void SetIsAbandoned(bool isAbandoned)
    {
        if (!isAbandoned)
        {
            InGameHelper.instance.SetPlayer(this);
        }

        disabledMovement = isAbandoned;
        playersAttributeComponent.SetIsImmortal(isAbandoned);
        HandleThrustAudio(false);
    }

    void HandleThrustAudio(bool isMovingForward)
    {
        if (!audioSource.isPlaying && isMovingForward)
        {
            audioSource.Play();
        }
        else if (audioSource.isPlaying && !isMovingForward)
        {
            audioSource.Stop();
        }
    }

    protected override void ZeroHealth()
    {
        base.ZeroHealth();

        Time.timeScale = 0;
        PlayersAttributeComponent.OnPlayerDestroy?.Invoke();

        gameObject.SetActive(false);
        //Destroy(gameObject);
    }

    void OnDisable()
    {
        playersAttributeComponent.OnZeroHealth -= ZeroHealth;
    }
}
