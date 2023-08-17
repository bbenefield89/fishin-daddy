using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public static FishSpawner Instance { get; private set; }

    [SerializeField] private Transform bobberModel;
    [SerializeField] private float offset = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    
    public void Spawn()
    {
        Vector3 desiredPos = bobberModel.position + bobberModel.position.normalized * offset;

        Quaternion lookDir = Quaternion.LookRotation(bobberModel.position - desiredPos);
        transform.rotation = Quaternion.Euler(0f, lookDir.eulerAngles.y + 90, 0f);

        transform.position = desiredPos + transform.forward * -1;
    }
}
