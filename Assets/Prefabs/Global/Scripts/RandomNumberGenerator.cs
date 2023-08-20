using UnityEngine;

[System.Serializable]
public class RandomNumberGenerator
{
    [SerializeField] private float min = 0f;
    [SerializeField] private float max = 1f;

    public float Generate()
    {
        return Random.Range(min, max + 1f);
    }

    public static float Generate(float min, float max)
    {
        return Random.Range(min, max + 1f);
    }

    public static bool TruthyFalsyGenerator()
    {
        return Random.Range(0f, 2f) == 1f;
    }
}
