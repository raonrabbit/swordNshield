using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClassSelectButton : MonoBehaviour
{
    [SerializeField] private Image slotImage;
    [SerializeField] private TMP_Text name;
    [SerializeField] private GameObject crown;

    public Image SlotImage => slotImage;
    public TMP_Text Name => name;
    public GameObject Crown => crown;

    public void SetMasterClient(bool isMasterClient)
    {
        crown.SetActive(isMasterClient);
    }
}
