using TMPro;
using UnityEngine;

public class AnimWindowReaction : MonoBehaviour
{
    public void OnEndEdit()
    {
        GetComponent<TMP_InputField>()?.onEndEdit?.Invoke(GetComponent<TMP_InputField>().text);
    }
}
