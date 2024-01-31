[System.Serializable]
public class SerializableColor
{
    public float r;
    public float g;
    public float b;

    public SerializableColor(float r, float g, float b)
    {
        this.r = r;
        this.g = g;
        this.b = b;
    }

    // serialization function
    public static byte[] Serialize(object customType)
    {
        SerializableColor color = (SerializableColor)customType;
        byte[] result = new byte[3];
        result[0] = (byte)(color.r * 255);
        result[1] = (byte)(color.g * 255);
        result[2] = (byte)(color.b * 255);
        return result;
    }

    // deserialization function
    public static object Deserialize(byte[] serializedCustomType)
    {
        float r = (float)serializedCustomType[0] / 255;
        float g = (float)serializedCustomType[1] / 255;
        float b = (float)serializedCustomType[2] / 255;
        return new SerializableColor(r, g, b);
    }
}