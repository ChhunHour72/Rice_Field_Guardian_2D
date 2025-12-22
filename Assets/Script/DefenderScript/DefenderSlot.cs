
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Reflection.Emit;

public class DefenderSlot : MonoBehaviour
{
    public Sprite DefenderSprite;
    public GameObject DefenderObject;
    public Image icon;
    public TextMeshProUGUI labelText;

    private void OnValidate()
    {


        if(DefenderSprite)
        {
            icon.enabled = true;
            icon.sprite = DefenderSprite;
        } else
        {
            icon.enabled = false;
        }
    }
}
