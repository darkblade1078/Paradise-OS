using UnityEngine;
using UnityEngine.SceneManagement;
using Meta.XR.MRUtilityKit;

public class MRUKScanListener : MonoBehaviour
{
    [Header("Timeout Settings")]
    [SerializeField] private float timeoutSeconds = 5f;

    private MRUK mruk;
    private bool sceneLoaded = false;
    private float elapsedTime = 0f;

    void Start()
    {
        mruk = MRUK.Instance;
        
        if (mruk != null)
        {
            mruk.RoomCreatedEvent.AddListener(OnRoomCreated);
        }
        else
        {
            Debug.LogError("MRUK instance not found!");
        }
    }

    void Update()
    {
        if (!sceneLoaded)
        {
            elapsedTime += Time.deltaTime;
            
            if (elapsedTime >= timeoutSeconds)
            {
                Debug.Log("Timeout reached - no saved room data found. (You may want to handle this case differently)");
                // Optionally, handle timeout here (e.g., show message or reload scene)
            }
        }
    }

    void OnDestroy()
    {
        if (mruk != null)
        {
            mruk.RoomCreatedEvent.RemoveListener(OnRoomCreated);
        }
    }

    private void OnRoomCreated(MRUKRoom room)
    {
        if (sceneLoaded)
            return;
        
        Debug.Log($"Room loaded: {room.name}. Loading next scene...");
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        if (sceneLoaded)
            return;

        sceneLoaded = true;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }
}