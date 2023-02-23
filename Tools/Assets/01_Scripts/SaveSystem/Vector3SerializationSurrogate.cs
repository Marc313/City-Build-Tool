using UnityEngine;
using System.Runtime.Serialization;

public class Vector3SerializationSurrogate : ISerializationSurrogate
{

    // Method called to serialize a Vector3 object
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {

        Vector3 vector = (Vector3)obj;
        info.AddValue("x", vector.x);
        info.AddValue("y", vector.y);
        info.AddValue("z", vector.z);
    }

    // Method called to deserialize a Vector3 object
    public object SetObjectData(object obj, SerializationInfo info,
                                       StreamingContext context, ISurrogateSelector selector)
    {

        Vector3 vector = (Vector3)obj;
        vector.x = (float)info.GetValue("x", typeof(float));
        vector.y = (float)info.GetValue("y", typeof(float));
        vector.z = (float)info.GetValue("z", typeof(float));
        obj = vector;
        return obj;
    }
}