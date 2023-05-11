import { createContext } from "react";
import { UnityConfig, useUnityContext } from "react-unity-webgl";
import { UnityContextHook } from "react-unity-webgl/distribution/types/unity-context-hook";

export interface MYTYSDKContext {
    unityContext: UnityContextHook,
    loadAvatar: (avatarCollectionId: number, metadataAssetUri: string, tokenId: string, tokenAssetUri: string) => void,
    selectAvatar: (avatarCollectionId: number, tokenId: string) => void,
    switchMode: () => void,
    processCapturedResult: (data: string) => void,
    takeScreenshot: (dataType?: string, quality?: number) => string | undefined
    updateCalibration: (type: CalibrationType, value: number) => void
    controlARFace: (type: ARFaceControlType, value: number) => void
}

export enum CalibrationType {
    SyncedBlink,
    Blink,
    Eyebrow,
    Pupil,
    MouthX,
    MouthY
}

export enum ARFaceControlType {
    XOffset,
    YOffset,
    Scale
}

const MESSAGE_HANDLER = 'MessageHandler'

const mytySDKContext = createContext({})
const MYTYSDKContextProvider = ({ config, children }: { config: UnityConfig, children: React.ReactNode }) => {
    const unityContext = useUnityContext(config);

    const loadAvatar = (avatarCollectionId: number, metadataAssetUri: string, tokenId: string, tokenAssetUri: string) => {
        unityContext.sendMessage(MESSAGE_HANDLER, "LoadAvatar", JSON.stringify({
            avatarCollectionId: avatarCollectionId,
            metadataAssetUri: metadataAssetUri,
            tokenId: tokenId,
            tokenAssetUri: tokenAssetUri
        }))
    }

    const selectAvatar = (avatarCollectionId: number, tokenId: string) => {
        unityContext.sendMessage(MESSAGE_HANDLER, "SelectAvatar", JSON.stringify({
            avatarCollectionId: avatarCollectionId,
            tokenId: tokenId
        }))
    }

    const switchMode = () => unityContext.sendMessage(MESSAGE_HANDLER, "SwitchMode", "_")

    const processCapturedResult = (data: string) => unityContext.sendMessage(MESSAGE_HANDLER, "ProcessCapturedResult", data)

    const takeScreenshot = unityContext.takeScreenshot

    const updateCalibration = (type: CalibrationType, value: number) => {
        switch (type) {
            case CalibrationType.SyncedBlink:
                unityContext.sendMessage(MESSAGE_HANDLER, "UpdateSyncedBlinkScale", value.toString())
                break
            case CalibrationType.Blink:
                unityContext.sendMessage(MESSAGE_HANDLER, "UpdateBlinkScale", value.toString())
                break
            case CalibrationType.Eyebrow:
                unityContext.sendMessage(MESSAGE_HANDLER, "UpdateEyebrowScale", value.toString())
                break
            case CalibrationType.Pupil:
                unityContext.sendMessage(MESSAGE_HANDLER, "UpdatePupilScale", value.toString())
                break
            case CalibrationType.MouthX:
                unityContext.sendMessage(MESSAGE_HANDLER, "UpdateMouthXScale", value.toString())
                break
            case CalibrationType.MouthY:
                unityContext.sendMessage(MESSAGE_HANDLER, "UpdateMouthYScale", value.toString())
                break
        }
    }

    const controlARFace = (type: ARFaceControlType, value: number) => {
        switch (type) {
            case ARFaceControlType.XOffset:
                unityContext.sendMessage(MESSAGE_HANDLER, "SetARFaceXOffset", value.toString())
                break
            case ARFaceControlType.YOffset:
                unityContext.sendMessage(MESSAGE_HANDLER, "SetARFaceYOffset", value.toString())
                break
            case ARFaceControlType.Scale:
                unityContext.sendMessage(MESSAGE_HANDLER, "SetARFaceScale", value.toString())
                break
        }
    }

    return <mytySDKContext.Provider value={{ unityContext, loadAvatar, selectAvatar, switchMode, processCapturedResult, takeScreenshot, updateCalibration, controlARFace }}>{children}</mytySDKContext.Provider>
}

export { mytySDKContext, MYTYSDKContextProvider }