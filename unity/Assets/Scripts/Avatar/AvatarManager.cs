using System.Collections.Generic;
using MYTYKit.AvatarImporter;
using UnityEngine;
using UnityEngine.Events;

namespace Avatar
{
    public class AvatarManager : MonoBehaviour
    {
        [SerializeField] Material m_fvAvatarMaterial;
        [SerializeField] Material m_arFaceMaterial;
        [SerializeField] MeshRenderer m_avatarRenderer;
        [SerializeField] GameObject m_arFacePlane;
        public MotionSource.MotionSource motionSource;

        Dictionary<(long, string), byte[]> m_tokenDataMap = new();
        Dictionary<long, AvatarObject> m_avatarCollectionObjectMap = new();
        
        public UnityEvent<long, string> avatarLoadedEvent; 
        
        AvatarObject m_currentAvatar;
        
        bool m_isARMode = false;

        class AvatarObject
        {
            public GameObject avatar;
            public RenderTexture vrRenderTexture;
        }

        public bool IsAvatarExists(long avatarCollectionId)
        {
            return m_avatarCollectionObjectMap.ContainsKey(avatarCollectionId);
        }

        public bool IsTokenExists(long avatarCollectionId, string tokenId)
        {
            return m_tokenDataMap.ContainsKey((avatarCollectionId, tokenId));
        }

        public void AddAvatarObject(
            long avatarCollectionId,
            byte[] metadata)
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

            m_avatarCollectionObjectMap[avatarCollectionId] = new AvatarObject
                { avatar = avatar, vrRenderTexture = vrRenderTexture };
        }

        public void AddToken(
            long avatarCollectionId,
            string tokenId,
            byte[] tokenData
        )
        {
            m_tokenDataMap[(avatarCollectionId, tokenId)] = tokenData;
            avatarLoadedEvent.Invoke(avatarCollectionId, tokenId);
            SelectAvatar(avatarCollectionId, tokenId);
        }

        public void SelectAvatar(long avatarCollectionId, string tokenId)
        {
            AvatarObject target;
            if (m_avatarCollectionObjectMap.TryGetValue(avatarCollectionId, out target))
            {
                byte[] tokenData;
                if (m_tokenDataMap.TryGetValue((avatarCollectionId, tokenId), out tokenData))
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
                    m_fvAvatarMaterial.mainTexture = target.vrRenderTexture;
                    m_avatarRenderer.material = m_fvAvatarMaterial;
                }
                else
                {
                    Debug.LogWarning($"Unable to find avatar with token Id : {avatarCollectionId}-{tokenId}");
                }
            }
            else
            {
                Debug.LogWarning($"Unable to find avatar with asset version : {avatarCollectionId}");
            }
        }

        public void SetARMode(bool flag)
        {
            m_isARMode = flag;
            ApplyARMode();
        }

        public void ApplyARMode()
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
                m_avatarRenderer.material = m_fvAvatarMaterial;
            }
        }
    }
}