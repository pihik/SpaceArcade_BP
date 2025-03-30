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

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			clickEffect.Stop();
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePosition.z = 0f;
			clickEffect.transform.position = mousePosition;
			clickEffect.Play();
		}
	}
}
