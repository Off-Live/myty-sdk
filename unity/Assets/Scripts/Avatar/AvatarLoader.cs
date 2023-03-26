using System.Collections.Generic;
using System.IO;
using MYTYKit.AvatarImporter;
using MYTYKit.MotionTemplates;
using UnityEngine;

public class AvatarLoader : MonoBehaviour
{
    [SerializeField]
    MotionTemplateMapper m_motionTemplateMapper;
    
    RenderTexture m_vrRenderTexture;

    [SerializeField]
    Material m_fvAvatarMaterial;
    [SerializeField]
    Material m_arFaceMaterial;
    [SerializeField]
    Material m_arAvatarMaterial;
    [SerializeField]
    MeshRenderer m_avatarRenderer;
    [SerializeField]
    GameObject m_arFacePlane;
    
    void Start()
    {
        m_vrRenderTexture = new RenderTexture(1280, 720, 0, RenderTextureFormat.ARGB32);
        var templateBytes = File.ReadAllBytes(Application.streamingAssetsPath + "/ExampleAssets/GhostsProject/collection_mas_metadata.zip");
        var map = new Dictionary<string, byte[]>();
        map["1"] = File.ReadAllBytes(Application.streamingAssetsPath + "/ExampleAssets/GhostsProject/1.zip");
        map["2"] = File.ReadAllBytes(Application.streamingAssetsPath + "/ExampleAssets/GhostsProject/2.zip");
        map["3"] = File.ReadAllBytes(Application.streamingAssetsPath + "/ExampleAssets/GhostsProject/3.zip");
        LoadAvatar(templateBytes, "Ghosts", map);
    }

    GameObject m_avatar;
    Dictionary<string, byte[]> m_tokenMap;

    public void LoadAvatar(
        byte[] templateJson,
        string avatarName,
        Dictionary<string, byte[]> tokenMap)
    {
        m_avatar = new GameObject(avatarName)
        {
            transform =
            {
                parent = transform
            }
        };
        
        var masImporter = m_avatar.AddComponent<MASImporter>();
        masImporter.templateRoot = m_avatar.transform;
        masImporter.motionTemplateMapper = m_motionTemplateMapper;
        masImporter.LoadCollectionMetadata(templateJson);
        
        var avatarRoot = new GameObject("AvatarRoot")
        {
            transform =
            {
                parent = m_avatar.transform
            }
        };
        masImporter.avatarRoot = avatarRoot.transform;
        
        m_avatar.GetComponentInChildren<Camera>().targetTexture = m_vrRenderTexture;
        m_fvAvatarMaterial.mainTexture = m_vrRenderTexture;
        m_avatarRenderer.material = m_fvAvatarMaterial;
        
        SetARMode(false);
        
        m_tokenMap = tokenMap;
    }

    public void SelectAvatar(string tokenId)
    {
        var importer = m_avatar.GetComponent<MASImporter>();

        var time = Time.time;
        importer.UnloadAvatar();
        importer.LoadAvatar(m_tokenMap[tokenId], tokenId);
        Debug.Log($"Elapsed : {Time.time - time}");
    }

    public void SetARMode(bool flag)
    {
        var importer = m_avatar.GetComponent<MASImporter>();
        importer.SetARMode(flag);
        if (flag)
        {
            m_arFacePlane.SetActive(true);
            var arFaceTexture = importer.currentARCamera.targetTexture;
            m_arFaceMaterial.mainTexture = arFaceTexture;
            m_avatarRenderer.material = null;
        }
        else
        {
            m_arFacePlane.SetActive(false);
            m_avatarRenderer.material = m_fvAvatarMaterial;
        }
    }
}
