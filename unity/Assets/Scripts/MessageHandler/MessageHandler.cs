using System.Collections.Generic;
using Avatar;
using Avatar.Impl;
using Avatar.Interface;
using Data;
using Newtonsoft.Json;
using UnityEngine;

namespace MessageHandler
{
    public class MessageHandler : MonoBehaviour
    {
        [SerializeField]
        AvatarDownloader m_avatarDownloader;
        [SerializeField]
        AvatarManager m_avatarManager;
        public Motion.MotionSource.MotionSource motionSource;

        void Start()
        {
            m_avatarDownloader = FindObjectOfType<AvatarDownloader>();
            m_avatarManager = FindObjectOfType<AvatarManager>();
            motionSource = FindObjectOfType<Motion.MotionSource.MotionSource>();
        }

        public void LoadAvatar(string message)
        {
            var obj = JsonConvert.DeserializeObject<LoadAvatarMessage>(message);
            m_avatarDownloader.DownloadAvatar(
                obj!.avatarCollectionId,
                obj!.metadataAssetUri,
                obj!.tokenId,
                obj!.tokenAssetUri  
            );
        }

        public void SelectAvatar(string message)
        {
            var obj = JsonConvert.DeserializeObject<SelectAvatarMessage>(message);
            m_avatarManager.SelectAvatar(
                obj!.avatarCollectionId,
                obj!.tokenId
            );
        }

        public void SwitchMode(string _)
        {
            m_avatarManager.SwitchMode();
        }

        public void ProcessCapturedResult(string message)
        {
            motionSource.ProcessCapturedResult(message);
        }
    }
}