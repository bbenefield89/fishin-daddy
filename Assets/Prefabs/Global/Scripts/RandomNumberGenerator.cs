using UnityEngine;

[System.Serializable]
public class RandomNumberGenerator
{
    [SerializeField] private float min;
    [SerializeField] private float max;

    public float Generate()
    {
        return Random.Range(min, max + 1);
    }

    public static float Generate(float min, float max)
    {
        return Random.Range(min, max + 1);
    }

    public static bool TruthyFalsyGenerator()
    {
        return Random.Range(0, 2) == 1;
    }
}
