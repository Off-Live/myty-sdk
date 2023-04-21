using System.Collections;
using Avatar;
using UnityEngine;
using UnityEngine.Networking;

public class AvatarLoader : MonoBehaviour
{
    [SerializeField] AvatarManager m_avatarManager;

    public void LoadAvatar(
        long avatarCollectionId,
        string metadataAssetUri,
        string tokenId,
        string tokenAssetUri)
    {
        if (!m_avatarManager.IsAvatarExists(avatarCollectionId))
        {
            StartCoroutine(LoadTemplate(avatarCollectionId, metadataAssetUri, tokenId, tokenAssetUri));
        }
        else if (!m_avatarManager.IsTokenExists(avatarCollectionId, tokenId))
        {
            StartCoroutine(LoadToken(avatarCollectionId, tokenId, tokenAssetUri));
        }
    }

    private IEnumerator LoadTemplate(
        long avatarCollectionId,
        string metadataAssetUri,
        string tokenId,
        string tokenAssetUri
    )
    {
        using (UnityWebRequest uwr = UnityWebRequest.Get(metadataAssetUri))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.Success)
            {
                var bytes = uwr.downloadHandler.data;

                m_avatarManager.AddAvatarObject(avatarCollectionId, bytes);

                if (!m_avatarManager.IsTokenExists(avatarCollectionId, tokenId))
                {
                    yield return LoadToken(avatarCollectionId, tokenId, tokenAssetUri);
                }
            }
            else
            {
                Debug.LogWarning($"Failed to Load asset from ${metadataAssetUri}");
            }
        }
    }

    private IEnumerator LoadToken(
        long avatarCollectionId,
        string tokenId,
        string tokenAssetUri
    )
    {
        using (UnityWebRequest uwr = UnityWebRequest.Get(tokenAssetUri))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.Success)
            {
                var bytes = uwr.downloadHandler.data;

                m_avatarManager.AddToken(avatarCollectionId, tokenId, bytes);
            }
            else
            {
                Debug.LogWarning($"Failed to Load asset from ${tokenAssetUri}");
            }
        }
    }
}