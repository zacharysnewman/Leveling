using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SetImageFill : MonoBehaviour
{
    private Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetFill(float fillAmount)
    {
        image.fillAmount = fillAmount;
    }
}
