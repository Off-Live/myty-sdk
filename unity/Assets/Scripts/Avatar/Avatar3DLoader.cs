using System.Collections;
using System.Collections.Generic;
using System.IO;
using MYTYKit.AvatarImporter;
using UnityEngine;

public class Avatar3DLoader : MonoBehaviour
{
    [SerializeField]
    MYTY3DAvatarImporter m_importer;
    
    private byte[] m_cloneXMainBody;
    private string m_cloneXMetadata;
    
    void Start()
    {
        m_cloneXMainBody = File.ReadAllBytes(Application.streamingAssetsPath + "/3DAssets/CloneX/f_character_neutral_neutral.glb");
        m_cloneXMetadata = File.ReadAllText(Application.streamingAssetsPath + "/3DAssets/CloneX/ExportedMetadata/CloneX_Female_SuitGeo.json");
    }

    public void LoadAvatar()
    {
        m_importer.LoadMainbody(
            m_cloneXMainBody,
            m_cloneXMetadata,
            LoadAvatarCallback
        );
    }

    private void LoadAvatarCallback(GameObject avatar)
    {
        Debug.Log(avatar);
    }
}
