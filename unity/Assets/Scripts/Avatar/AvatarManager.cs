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

        Dictionary<(long, string), byte[]> m_assetMap = new();
        Dictionary<long, AvatarObject> m_assetVersionObjectMap = new();
        
        public UnityEvent<long, string> avatarLoadedEvent; 
        
        AvatarObject m_currentAvatar;
        
        bool m_isARMode = false;

        class AvatarObject
        {
            public GameObject avatar;
            public RenderTexture vrRenderTexture;
        }

        public bool IsAvatarExists(long assetVersionId)
        {
            return m_assetVersionObjectMap.ContainsKey(assetVersionId);
        }

        public bool IsTokenExists(long assetVersionId, string tokenId)
        {
            return m_assetMap.ContainsKey((assetVersionId, tokenId));
        }

        public void AddAvatarObject(
            long assetVersionId,
            byte[] templateData)
        {
            var avatar = new GameObject($"{assetVersionId}")
            {
                transform =
                {
                    parent = transform
                }
            };
            var vrRenderTexture = new RenderTexture(1280, 720, 0, RenderTextureFormat.ARGB32);

            var masImporter = avatar.AddComponent<MASImporter>();
            masImporter.templateRoot = avatar.transform;
            masImporter.LoadCollectionMetadata(templateData);
            
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

            m_assetVersionObjectMap[assetVersionId] = new AvatarObject
                { avatar = avatar, vrRenderTexture = vrRenderTexture };
        }

        public void AddToken(
            long assetVersionId,
            string tokenId,
            byte[] tokenData
        )
        {
            m_assetMap[(assetVersionId, tokenId)] = tokenData;
            avatarLoadedEvent.Invoke(assetVersionId, tokenId);
        }

        public void SelectAvatar(long assetVersionId, string tokenId)
        {
            AvatarObject target;
            if (m_assetVersionObjectMap.TryGetValue(assetVersionId, out target))
            {
                byte[] tokenData;
                if (m_assetMap.TryGetValue((assetVersionId, tokenId), out tokenData))
                {
                    var importer = target.avatar.GetComponent<MASImporter>();

                    m_currentAvatar = target;
                    if (m_isARMode)
                    {
                        importer.SetARMode(false);
                    }
                    importer.UnloadAvatar();
                    importer.LoadAvatar(tokenData, tokenId);
                    importer.SetARMode(m_isARMode);
                    m_fvAvatarMaterial.mainTexture = target.vrRenderTexture;
                    m_avatarRenderer.material = m_fvAvatarMaterial;
                }
                else
                {
                    Debug.LogWarning($"Unable to find avatar with token Id : {assetVersionId}-{tokenId}");
                }
            }
            else
            {
                Debug.LogWarning($"Unable to find avatar with asset version : {assetVersionId}");
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