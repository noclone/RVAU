using ExitGames.Client.Photon;
using UnityEngine;

public class TypesRegistry : MonoBehaviour
{
    void Start()
    {
        PhotonPeer.RegisterType(typeof(SerializableColor), (byte)'C', SerializableColor.Serialize, SerializableColor.Deserialize);
    }
}