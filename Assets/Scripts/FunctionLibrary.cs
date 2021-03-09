using static UnityEngine.Mathf;
using UnityEngine;

public static class FunctionLibrary
{
    public delegate Vector3 Function(float u, float v, float t);

    public enum FunctionName
    {
        Wave,
        MultiWave,
        Ripple,
        Sphere,
        Torus,
    }

    private static readonly Function[] functions = {Wave, MultiWave, Ripple, Sphere, Torus};

    public static Function GetFunction(FunctionName name)
    {
        return functions[(int) name];
    }

    public static FunctionName GetNextFunctionName(FunctionName name)
    {
        if ((int) name < functions.Length - 1)
        {
            return name + 1;
        }
        else
        {
            return 0;
        }
    }

    public static FunctionName GetRandomFunctionNameOtherThan(FunctionName name)
    {
        var choice = (FunctionName) Random.Range(1, functions.Length);
        return choice == name ? 0 : choice;
    }

    public static Vector3 Morph(float u, float v, float t, Function from, Function to, float progress)
    {
        // return Vector3.Lerp(from(u, v, t), to(u, v, t), progress);
        return Vector3.LerpUnclamped(from(u, v, t), to(u, v, t), SmoothStep(0f, 1f, progress));
    }

    public static Vector3 Wave(float u, float v, float t)
    {
        Vector3 p;
        p.x = u;
        p.y = Sin(PI * (u + v + t));
        p.z = v;
        return p;
    }

    public static Vector3 MultiWave(float u, float v, float t)
    {
        Vector3 p;
        p.x = u;
        p.y = Sin(PI * (u + 0.5f + t));
        p.y += 0.5f * Sin(2f * PI * (v + t));
        p.y += Sin(PI * (u + v + 0.25f * t));
        p.y *= 1f / 2.5f;
        p.z = v;
        return p;
    }

    public static Vector3 Ripple(float u, float v, float t)
    {
        float d = Sqrt(u * u + v * v);
        Vector3 p;
        p.x = u;
        p.y = Sin(4f * (PI * d - t));
        p.y /= 1f + 10f * d;
        p.z = v;
        return p;
    }

    public static Vector3 Sphere(float u, float v, float t)
    {
        float r = 0.9f + 0.1f * Sin(PI * (6f * u + 4f * v + t)); //Cos(0.5f * PI * t);
        float s = r * Cos(PI * 0.5f * v);
        Vector3 p;
        p.x = s * Sin(PI * u);
        p.y = r * Sin(PI * 0.5f * v);
        p.z = s * Cos(PI * u);
        return p;
    }

    public static Vector3 Torus(float u, float v, float t)
    {
        float r1 = 0.6f + 0.1f * Sin(PI * (6f * u + 0.5f * t)); //0.75f;
        float r2 = 0.15f + 0.05f * Sin(PI * (8f * u + 4f * v + 2f * t)); //0.25f;
        float s = r1 + r2 * Cos(PI * v);
        Vector3 p;
        p.x = s * Sin(PI * u);
        p.y = r2 * Sin(PI * v);
        p.z = s * Cos(PI * u);
        return p;
    }
}