using UnityEngine;
using UnityEngine.UI;

public class Switch: MonoBehaviour
{
    public Image On;
    public Image Off;

    public void ON()
    {
        Off.gameObject.SetActive(true);
        On.gameObject.SetActive(false);
        
        GameManager.instance.Toggle(this.GetInstanceID());
    }

    public void OFF()
    {
        Off.gameObject.SetActive(false);
        On.gameObject.SetActive(true);

        GameManager.instance.Toggle(this.GetInstanceID());
    }
}