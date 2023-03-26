using System;
using UnityEngine;

namespace MessageHandler
{
    public class MessageHandler : MonoBehaviour
    {
        AvatarLoader m_avatarLoader;
        private void Start()
        {
            m_avatarLoader = FindObjectOfType<AvatarLoader>();
        }

        public void SelectAvatar(string tokenID)
        {
            m_avatarLoader.SelectAvatar(tokenID);
        }

        public void SetARMode(bool flag)
        {
            m_avatarLoader.SetARMode(flag);
        }
    }
}