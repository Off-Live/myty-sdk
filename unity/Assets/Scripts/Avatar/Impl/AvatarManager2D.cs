using System.Collections.Generic;
using Avatar.Interface;
using MYTYKit.AvatarImporter;
using UnityEngine;
using UnityEngine.Events;

namespace Avatar.Impl
{
    public class AvatarManager2D : AvatarManager
    {
        [SerializeField] Material m_fvAvatarMaterial;
        [SerializeField] Material m_arFaceMaterial;
        [SerializeField] MeshRenderer m_avatarRenderer;
        [SerializeField] GameObject m_arFacePlane;

        Dictionary<(long, string), byte[]> m_tokenAssetMap = new();
        Dictionary<long, AvatarObject> m_avatarObjectMap = new();

        public UnityEvent<long, string> avatarLoadedEvent;
        
        AvatarObject m_currentAvatar;
        bool m_isARMode;

        class AvatarObject
        {
            public GameObject avatar;
            public RenderTexture vrRenderTexture;
        }

        public override void LoadAvatar(long avatarCollectionId, string tokenId, byte[] metadata, byte[] tokenAsset)
        {
            if (metadata != null)
            {
                var avatar = new GameObject($"{avatarCollectionId}")
                {
                    transform =
                    {
                        parent = transform
                    }
                };
                var vrRenderTexture = new RenderTexture(1280, 720, 0, RenderTextureFormat.ARGB32);

                var masImporter = avatar.AddComponent<MASImporter>();
                masImporter.templateRoot = avatar.transform;
                masImporter.LoadCollectionMetadata(metadata);
            
                motionSource.motionTemplateMapperList.Add(masImporter.motionTemplateMapper);
                motionSource.UpdateMotionAndTemplates();

                var avatarRoot = new GameObject("AvatarRoot")
                {
                    transform =
                    {
                        parent = avatar.transform
                    }
                };
                masImporter.avatarRoot = avatarRoot.transform;

                avatar.GetComponentInChildren<Camera>().targetTexture = vrRenderTexture;

                m_avatarObjectMap[avatarCollectionId] = new AvatarObject
                    { avatar = avatar, vrRenderTexture = vrRenderTexture };
            }
            
            m_tokenAssetMap[(avatarCollectionId, tokenId)] = tokenAsset;
            avatarLoadedEvent.Invoke(avatarCollectionId, tokenId);
            SelectAvatar(avatarCollectionId, tokenId);
        }

        public override void SelectAvatar(long avatarCollectionId, string tokenId)
        {
            AvatarObject target;
            if (m_avatarObjectMap.TryGetValue(avatarCollectionId, out target))
            {
                byte[] tokenData;
                if (m_tokenAssetMap.TryGetValue((avatarCollectionId, tokenId), out tokenData))
                {
                    var importer = target.avatar.GetComponent<MASImporter>();

                    m_currentAvatar = target;
                    if (m_isARMode)
                    {
                        importer.SetARMode(false);
                    }
                    importer.UnloadAvatar();
                    importer.LoadAvatar(tokenData, tokenId);
                    ApplyARMode();
                }
                else
                {
                    Debug.LogWarning($"Unable to find avatar with Id : {avatarCollectionId}-{tokenId}");
                }
            }
            else
            {
                Debug.LogWarning($"Unable to find avatar with avatar collection Id : {avatarCollectionId}");
            }
        }

        public override void SwitchMode()
        {
            m_isARMode = !m_isARMode;
            ApplyARMode();
        }

        public override bool IsMetadataLoaded(long avatarCollectionId)
        {
            return m_avatarObjectMap.ContainsKey(avatarCollectionId);
        }

        public override bool IsAvatarLoaded(long avatarCollectionId, string tokenId)
        {
            return m_tokenAssetMap.ContainsKey((avatarCollectionId, tokenId));
        }
        
        private void ApplyARMode()
        {
            var importer = m_currentAvatar.avatar.GetComponent<MASImporter>();
            importer.SetARMode(m_isARMode);
            if (m_isARMode)
            {
                m_arFacePlane.SetActive(true);
                var arFaceTexture = importer.currentARCamera.targetTexture;
                importer.currentARCamera.clearFlags = CameraClearFlags.Color;
                importer.currentARCamera.backgroundColor = new Color(0, 0, 0, 0);
                m_arFaceMaterial.mainTexture = arFaceTexture;
                m_avatarRenderer.gameObject.SetActive(false);
            }
            else
            {
                m_arFacePlane.SetActive(false);
                m_avatarRenderer.gameObject.SetActive(true);
                m_fvAvatarMaterial.mainTexture = m_currentAvatar.vrRenderTexture;
                m_avatarRenderer.material = m_fvAvatarMaterial;
            }
        }
    }
}