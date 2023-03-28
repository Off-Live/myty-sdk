using System.Collections;
using Avatar;
using UnityEngine;
using UnityEngine.Networking;

public class AvatarLoader : MonoBehaviour
{
    [SerializeField] AvatarManager m_avatarManager;

    public void LoadAvatar(
        long assetVersionId,
        string templateAssetUri,
        string tokenId,
        string tokenAssetUri)
    {
        if (!m_avatarManager.IsAvatarExists(assetVersionId))
        {
            StartCoroutine(LoadTemplate(assetVersionId, templateAssetUri, tokenId, tokenAssetUri));
        }
        else if (!m_avatarManager.IsTokenExists(assetVersionId, tokenId))
        {
            StartCoroutine(LoadToken(assetVersionId, tokenId, tokenAssetUri));
        }
    }

    private IEnumerator LoadTemplate(
        long assetVersionId,
        string templateAssetUri,
        string tokenId,
        string tokenAssetUri
    )
    {
        using (UnityWebRequest uwr = UnityWebRequest.Get(templateAssetUri))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.Success)
            {
                var bytes = uwr.downloadHandler.data;

                m_avatarManager.AddAvatarObject(assetVersionId, bytes);

                if (!m_avatarManager.IsTokenExists(assetVersionId, tokenId))
                {
                    yield return LoadToken(assetVersionId, tokenId, tokenAssetUri);
                }
            }
            else
            {
                Debug.LogWarning($"Failed to Load asset from ${templateAssetUri}");
            }
        }
    }

    private IEnumerator LoadToken(
        long assetVersionId,
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

                m_avatarManager.AddToken(assetVersionId, tokenId, bytes);
            }
            else
            {
                Debug.LogWarning($"Failed to Load asset from ${tokenAssetUri}");
            }
        }
    }
}