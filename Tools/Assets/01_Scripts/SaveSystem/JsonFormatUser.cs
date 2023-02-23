using System.Runtime.Serialization.Formatters.Binary;

public class JsonFormatUser : IFormatter
{
    private BinaryFormatter formatter;

    public JsonFormatUser()
    {
        formatter = new BinaryFormatter();
    }

    public void Deserialize()
    {
        //formatter.Deserialize();
    }

    public void Serialize()
    {
        //formatter.Serialize();
    }
}
