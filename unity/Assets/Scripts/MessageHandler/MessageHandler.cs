using AR;
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
        [SerializeField]
        ARFaceControl m_arFaceControl;

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
            motionSource.Process(message);
        }

        public void UpdateSyncedBlinkScale(string value)
        {
            motionSource.motionProcessor.SetSyncedBlinkScale(float.Parse(value));
        }

        public void UpdateBlinkScale(string value)
        {
            motionSource.motionProcessor.SetBlinkScale(float.Parse(value));
        }

        public void UpdatePupilScale(string value)
        {
            motionSource.motionProcessor.SetPupilScale(float.Parse(value));
        }

        public void UpdateEyebrowScale(string value)
        {
            motionSource.motionProcessor.SetEyebrowScale(float.Parse(value));
        }

        public void UpdateMouthXScale(string value)
        {
            motionSource.motionProcessor.SetMouthXScale(float.Parse(value));
        }

        public void UpdateMouthYScale(string value)
        {
            motionSource.motionProcessor.SetMouthYScale(float.Parse(value));
        }

        public void SetARFaceXOffset(string value)
        {
            m_arFaceControl.SetXOffset(float.Parse(value));
        }

        public void SetARFaceYOffset(string value)
        {
            m_arFaceControl.SetYOffset(float.Parse(value));
        }

        public void SetARFaceScale(string value)
        {
            m_arFaceControl.SetScale(float.Parse(value));
        }

        public void SetARFaceAsDefault(string _)
        {
            m_arFaceControl.SetControlAsDefault();
        }
    }
}