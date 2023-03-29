using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MYTYKit.AvatarImporter;
using MYTYKit.Components;
using UnityEngine;
using UnityEngine.Networking;

public class Avatar3DLoader : MonoBehaviour
{
    [SerializeField]
    MYTY3DAvatarImporter m_importer;
    [SerializeField]
    Material m_avatar3DMaterial;
    [SerializeField]
    Camera m_renderCamera;
    
    private byte[] m_cloneXMainBody;
    private string m_cloneXMetadata;
    private Dictionary<string, byte[]> m_cloneXTraitMap;

    HttpClient m_client;
    void Start()
    {
#if UNITY_EDITOR
        m_cloneXMainBody = File.ReadAllBytes(Application.streamingAssetsPath + "/3DAssets/CloneX/f_character_neutral_neutral.glb");
        m_cloneXMetadata = File.ReadAllText(Application.streamingAssetsPath + "/3DAssets/CloneX/ExportedMetadata/CloneX_Female_SuitGeo.json");
        m_cloneXTraitMap = new Dictionary<string, byte[]>
        {
            {"jacket", File.ReadAllBytes(Application.streamingAssetsPath + "/3DAssets/CloneX/f_rigged_bone_pfa_jckt.glb")},
            {"eyelash", File.ReadAllBytes(Application.streamingAssetsPath + "/3DAssets/CloneX/f_rigged_neutral_eyelashes.glb")},
            {"reptile", File.ReadAllBytes(Application.streamingAssetsPath + "/3DAssets/CloneX/f_rigged_reptile.glb")},
            {"smile", File.ReadAllBytes(Application.streamingAssetsPath + "/3DAssets/CloneX/f_rigged_tctcl_smile.glb")},
            {"thick", File.ReadAllBytes(Application.streamingAssetsPath + "/3DAssets/CloneX/f_rigged_thick.glb")},
            {"bwlcut", File.ReadAllBytes(Application.streamingAssetsPath + "/3DAssets/CloneX/f_rigged_wht_bwlcut.glb")},
        };
        Debug.Log("Editor");
#else
        Debug.Log("WebGL");
        StartCoroutine(LoadWebGLAssets());
#endif
    }

    private IEnumerator LoadWebGLAssets()
    {
        var appUrl = Application.absoluteURL;
        var origin = new System.Uri(appUrl).GetLeftPart(System.UriPartial.Authority);
        var url = origin + "/WebGL/StreamingAssets";
        
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url + "/3DAssets/CloneX/f_character_neutral_neutral.glb"))
        {
            yield return webRequest.SendWebRequest();

            Debug.Log("mainbody");
            m_cloneXMainBody = webRequest.downloadHandler.data;
        }

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url + "/3DAssets/CloneX/ExportedMetadata/CloneX_Female_SuitGeo.json"))
        {
            yield return webRequest.SendWebRequest();

            Debug.Log("mainbodyMeta");
            m_cloneXMetadata = webRequest.downloadHandler.text;
        }
        
        m_cloneXTraitMap = new();

        using (UnityWebRequest jacket = UnityWebRequest.Get(url + "/3DAssets/CloneX/f_rigged_bone_pfa_jckt.glb"))
        using (UnityWebRequest eyelash = UnityWebRequest.Get(url + "/3DAssets/CloneX/f_rigged_neutral_eyelashes.glb"))
        using (UnityWebRequest reptile = UnityWebRequest.Get(url + "/3DAssets/CloneX/f_rigged_reptile.glb"))
        using (UnityWebRequest smile = UnityWebRequest.Get(url + "/3DAssets/CloneX/f_rigged_tctcl_smile.glb"))
        using (UnityWebRequest thick = UnityWebRequest.Get(url + "/3DAssets/CloneX/f_rigged_thick.glb"))
        using (UnityWebRequest bwlcut = UnityWebRequest.Get(url + "/3DAssets/CloneX/f_rigged_wht_bwlcut.glb"))
        {
            yield return jacket.SendWebRequest();
            
            m_cloneXTraitMap["jacket"] = jacket.downloadHandler.data;
            
            yield return eyelash.SendWebRequest();
            
            m_cloneXTraitMap["eyelash"] = eyelash.downloadHandler.data;
            
            yield return reptile.SendWebRequest();
            
            m_cloneXTraitMap["reptile"] = reptile.downloadHandler.data;
            
            yield return smile.SendWebRequest();
            
            m_cloneXTraitMap["smile"] = smile.downloadHandler.data;
            
            yield return thick.SendWebRequest();
            
            m_cloneXTraitMap["thick"] = thick.downloadHandler.data;
            
            yield return bwlcut.SendWebRequest();
            
            m_cloneXTraitMap["bwlcut"] = bwlcut.downloadHandler.data;
        }

        Debug.Log("Done Downloading All Traits");
    }

    public void LoadAvatar()
    {
        m_importer.LoadMainbody(
            m_cloneXMainBody,
            m_cloneXMetadata,
            LoadAvatarCallback
        );
    }

    public void LoadTraits()
    {
        foreach (var pair in m_cloneXTraitMap)
        {
            m_importer.LoadTrait(pair.Value, pair.Key);
        }
    }

    private void LoadAvatarCallback(GameObject avatar)
    {
        Debug.Log("Avatar Loaded");
        var texture = new RenderTexture(1280, 720, 0, RenderTextureFormat.ARGB32);
        m_renderCamera.targetTexture = texture;
        m_avatar3DMaterial.mainTexture = texture;
    }
}
