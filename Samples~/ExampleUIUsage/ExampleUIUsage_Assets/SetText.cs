using TMPro;
using UnityEngine;

public class SetText : MonoBehaviour
{
    private TMP_Text text;

    private TMP_Text GetText()
    {
        if (!text)
        {
            text = GetComponent<TMP_Text>();
        }
        return text;
    }

    public void Set(bool value)
    {
        GetText().text = $"{value}";
    }

    public void Set(int value)
    {
        GetText().text = $"{value}";
    }
}
