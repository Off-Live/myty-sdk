using System;
using UnityEngine;

namespace MessageHandler
{
    public class MessageHandler : MonoBehaviour
    {
        public AvatarLoader avatarLoader;
        public Avatar3DLoader avatar3DLoader;
        private void Start()
        {
            avatarLoader = FindObjectOfType<AvatarLoader>();
            avatar3DLoader = FindObjectOfType<Avatar3DLoader>();
        }

        public void SelectAvatar(string tokenID)
        {
            avatarLoader.SelectAvatar(tokenID);
        }

        public void SetARMode(bool flag)
        {
            avatarLoader.SetARMode(flag);
        }

        public void Load3DAvatar()
        {
            avatar3DLoader.LoadAvatar();
        }
    }
}