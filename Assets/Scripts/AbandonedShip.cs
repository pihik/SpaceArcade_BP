using UnityEditor.SceneManagement;
using UnityEngine;


[RequireComponent(typeof(Player))]
public class AbandonedShip : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 15f;

    Player playerScript;
    Player oldPlayer;

    void Start()
    {
        playerScript = GetComponent<Player>();

        playerScript.SetIsAbandoned(true);
        GameManager.instance.StopSpawning?.Invoke(float.MinValue);
    }

    void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        int collisionLayerIndex = collision.gameObject.layer;

        if ((InGameHelper.instance.GetPlayerLayer() & 1 << collisionLayerIndex) == 1 << collisionLayerIndex)
        {
            if (collision.TryGetComponent<Player>(out Player player))
            {
                oldPlayer = player;
                //activate UI
                NewShipActivation(); //only for testing .. destroy after
            }
        }
    }

    public void NewShipActivation() //this will called after pressing agreed button on UI
    {
        oldPlayer.SetIsAbandoned(true);
        playerScript.SetIsAbandoned(false);

        AstronautHelper instanceOfAH = AstronautHelper.instance;
        if (instanceOfAH)
        {
            instanceOfAH.ShowNewText();
        }
        // activate UI -> warning message (incomming enemies and asteroids)
        // after agreed -> activate spawners

        GameManager.instance.StartSpawning?.Invoke(2);

        Destroy(this);
    }
}
