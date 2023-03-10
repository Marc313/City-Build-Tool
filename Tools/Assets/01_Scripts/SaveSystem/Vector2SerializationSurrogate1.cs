using UnityEngine;
using System.Runtime.Serialization;

public class Vector2SerializationSurrogate : ISerializationSurrogate
{

    // Method called to serialize a Vector3 object
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        Vector2 vector = (Vector2)obj;
        info.AddValue("x", vector.x);
        info.AddValue("y", vector.y);
    }

    // Method called to deserialize a Vector3 object
    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
        Vector2 vector = (Vector2)obj;
        vector.x = (float)info.GetValue("x", typeof(float));
        vector.y = (float)info.GetValue("y", typeof(float));
        obj = vector;
        return obj;
    }
}