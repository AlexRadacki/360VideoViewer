using UnityEngine;

public static class StaticTools  {

    public static Vector2 Vector2FromString(string input, char seperator)
    {
        string[] vectorData = input.Split(seperator);
        return new Vector2(float.Parse(vectorData[0]), float.Parse(vectorData[1]));
    }

    public static Vector3 Vector3FromString(string input, char seperator)
    {
        string[] vectorData = input.Split(seperator);
        return new Vector3(float.Parse(vectorData[0]), float.Parse(vectorData[1]), float.Parse(vectorData[2]));
    }

    public static string StringFromVector2(Vector2 input)
    {
        string output = input.x + "," + input.y;
        return output;
    }

    public static string StringFromVector3(Vector3 input)
    {
        string output = input.x + "," + input.y + "," + input.z;
        return output;
    }

    public static float Map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }
}
