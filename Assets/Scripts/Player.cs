using System;
using UnityEngine;

[RequireComponent(typeof(PlayersAttributeComponent))]
public class Player : SpaceshipBase
{
    public Action onSpacePressed; 

    [Header("Setup your player")]
    [SerializeField] float thrustSpeed = 3f;
    [SerializeField] float rotationSpeed = 1f;

    AudioSource audioSource;
    PlayersAttributeComponent playersAttributeComponent;
	Camera mainCamera;

	bool disabledMovement = false;

    protected override void Awake()
    {
        base.Awake();

        playersAttributeComponent = GetComponent<PlayersAttributeComponent>(); 
        audioSource = GetComponent<AudioSource>();
		mainCamera = Camera.main;

		if (!playersAttributeComponent || !audioSource || !mainCamera)
        {
            Debug.LogError("[PlayerScript::Awake] Something went wrong, check on: " + name);
        }

        playersAttributeComponent.OnZeroHealth += ZeroHealth;
    }
    protected override void Start()
    {
        base.Start();

        HandleThrustAudio(false);
    }

    void Update()
    {
        if (disabledMovement || Time.timeScale == 0)
        {
            return;
        }

        Move();
        Rotate();

        HandleShootInput();
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

	void KeepPlayerInBounds()
	{
		Vector3 position = transform.position;
		Vector3 screenBoundsMin = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
		Vector3 screenBoundsMax = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));

		position.x = Mathf.Clamp(position.x, screenBoundsMin.x, screenBoundsMax.x);
		position.y = Mathf.Clamp(position.y, screenBoundsMin.y, screenBoundsMax.y);

		transform.position = position;
	}

    void HandleShootInput()
    {
		if (Input.GetKey(KeyCode.Space))
		{
			onSpacePressed?.Invoke();
			foreach (Gun gun in guns)
			{
				gun.Shoot();
			}
		}
	}

	public PlayersAttributeComponent GetAttributeComponent()
    {
        return playersAttributeComponent;
    }

    public void AddGun()
    {
        bool allGunsActivated = true;

        foreach (Gun gun in guns)
        {
            if (!gun.IsActivate())
            {
                gun.ActiveSwitch(true);
                allGunsActivated = false;
                break;
            }
        }

        if (allGunsActivated)
        {
            foreach (Gun gun in guns)
            {
                gun.IncreaseFireRate(0.03f);
            }
        }
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

    public void ChangeShip(Player newShip)
    {
        SetIsAbandoned(true);
        SetCollision(false);
        newShip.SetIsAbandoned(false);
    }

    public void SetCollision(bool value)
    {
        myCollider.enabled = value;
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
