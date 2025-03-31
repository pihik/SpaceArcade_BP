using UnityEngine;

public class MouseHandler : MonoBehaviour
{
	public static MouseHandler instance;

	#region Singleton
	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
			return;
		}
		instance = this;
		DontDestroyOnLoad(gameObject);
	}
	#endregion

	[SerializeField] ParticleSystem clickEffect;

	LayerMask asteroidLayer;

	void Start()
	{
		asteroidLayer = InGameHelper.instance.GetAsteroidLayer();
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			clickEffect.Stop();
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePosition.z = 0f;
			clickEffect.transform.position = mousePosition;
			clickEffect.Play();

			RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, asteroidLayer);

			if (Time.timeScale == 1 && hit && hit.collider.TryGetComponent(out Asteroid asteroidComp))
			{
				asteroidComp.OnClicked();
			}
		}
	}
}
