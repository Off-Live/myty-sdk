import { createContext } from "react";
import { UnityConfig, useUnityContext } from "react-unity-webgl";
import { UnityContextHook } from "react-unity-webgl/distribution/types/unity-context-hook";

export interface MYTYSDKContext {
    unityContext: UnityContextHook,
    loadAvatar: (assetId: number, templateUri: string, tokenId: string, tokenUri: string) => void,
    selectAvatar: (assetId: number, tokenId: string) => void,
    setARMode: (flag: string) => void,
    processCapturedResult: (data: string) => void,
    takeScreenshot: (dataType?: string, quality?: number) => string | undefined
}

const MESSAGE_HANDLER = 'MessageHandler'

const mytySDKContext = createContext({})
const MYTYSDKContextProvider = ({ config, children }: { config: UnityConfig, children: React.ReactNode }) => {
    const unityContext = useUnityContext(config);

    const loadAvatar = (assetId: number, templateUri: string, tokenId: string, tokenUri: string) => {
        unityContext.sendMessage(MESSAGE_HANDLER, "LoadAvatar", JSON.stringify({
            assetVersionId: assetId,
            templateAssetUri: templateUri,
            tokenId: tokenId,
            tokenAssetUri: tokenUri
        }))
    }

    const selectAvatar = (assetId: number, tokenId: string) => {
        unityContext.sendMessage(MESSAGE_HANDLER, "SelectAvatar", JSON.stringify({
            assetVersionId: assetId,
            tokenId: tokenId
        }))
    }

    const setARMode = (flag: string) => unityContext.sendMessage(MESSAGE_HANDLER, "SetARMode", flag)

    const processCapturedResult = (data: string) => unityContext.sendMessage(MESSAGE_HANDLER, "ProcessCapturedResult", data)

    const takeScreenshot = unityContext.takeScreenshot

    return <mytySDKContext.Provider value={{ unityContext, loadAvatar, selectAvatar, setARMode, processCapturedResult, takeScreenshot }}>{children}</mytySDKContext.Provider>
}

export { mytySDKContext, MYTYSDKContextProvider }