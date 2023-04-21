using Data;
using FlutterUnityIntegration;
using Newtonsoft.Json;
using UnityEngine;

namespace MessageHandler
{
    public class FlutterMessageSender : MonoBehaviour
    {
        [SerializeField]
        UnityMessageManager m_unityMessageManager;

        public void AvatarLoaded(long avatarCollectionId, string tokenId)
        {
            m_unityMessageManager.SendMessageToFlutter(
                JsonConvert.SerializeObject(new SelectAvatarMessage
                {
                    avatarCollectionId = avatarCollectionId,
                    tokenId = tokenId
                })    
            );
        }
    }
}