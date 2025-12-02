using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class NetworkControl : MonoBehaviour
{
    public static NetworkControl Instance { get; private set; }
    public NetworkManager networkManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift)) 
        {
            if (Input.GetKeyDown(KeyCode.N)) 
            {
                NetworkManager.Singleton.StartClient();
            }
        }
    }
}
