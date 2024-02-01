using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Switch: MonoBehaviour
{
    public Image On;
    public Image Off;

    public void ON()
    {
        Off.gameObject.SetActive(true);
        On.gameObject.SetActive(false);
        
        PhotonView.Get(this).RPC("GameManager.instance.Toggle", RpcTarget.AllBuffered, this.GetInstanceID());
    }

    public void OFF()
    {
        Off.gameObject.SetActive(false);
        On.gameObject.SetActive(true);

         PhotonView.Get(this).RPC("GameManager.instance.Toggle", RpcTarget.AllBuffered, this.GetInstanceID());
    }
}