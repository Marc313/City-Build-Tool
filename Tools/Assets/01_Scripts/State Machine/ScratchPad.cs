using System.Collections.Generic;

public class ScratchPad
{
    private Dictionary<string, object> storedData = new Dictionary<string, object>();

    public void Register(string _id, object _data)
    {
        if (storedData.ContainsKey(_id))
        {
            storedData[_id] = _data;
        }
        else
        {
            storedData.Add(_id, _data);
        }
    }

    public object Get(string _id)
    {
        object data;
        storedData.TryGetValue(_id, out data);
        return data;
    }
}