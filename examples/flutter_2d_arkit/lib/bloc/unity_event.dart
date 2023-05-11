part of 'unity_bloc.dart';

abstract class UnityEvent {}

enum UnityEventTopic {
  UnityCreated,
  LoadAvatar,
  LoadAvatarList,
  SelectAvatar,
  SwitchMode,
  ProcessCapturedResult,
  UpdateSyncedBlinkScale,
  UpdateBlinkScale,
  UpdatePupilScale,
  UpdateEyebrowScale,
  UpdateMouthXScale,
  UpdateMouthYScale,
  SetARFaceXOffset,
  SetARFaceYOffset,
  SetARFaceScale,
  None
}

enum CalibrationType {
  SyncedBlink,
  Blink,
  Eyebrow,
  Pupil,
  MouthX,
  MouthY
}

enum ARFaceControlType {
  XOffset,
  YOffset,
  Scale
}

extension UnityEventName on UnityEventTopic {
  String get topicName {
    switch (this) {
      case UnityEventTopic.UnityCreated:
        return 'UnityCreated';
      case UnityEventTopic.LoadAvatar:
        return 'LoadAvatar';
      case UnityEventTopic.LoadAvatarList:
        return 'LoadAvatarList';
      case UnityEventTopic.SelectAvatar:
        return 'SelectAvatar';
      case UnityEventTopic.SwitchMode:
        return 'SwitchMode';
      case UnityEventTopic.ProcessCapturedResult:
        return 'ProcessCapturedResult';
      case UnityEventTopic.UpdateSyncedBlinkScale:
        return 'UpdateSyncedBlinkScale';
      case UnityEventTopic.UpdateBlinkScale:
        return 'UpdateBlinkScale';
      case UnityEventTopic.UpdateEyebrowScale:
        return 'UpdateEyebrowScale';
      case UnityEventTopic.UpdatePupilScale:
        return 'UpdatePupilScale';
      case UnityEventTopic.UpdateMouthXScale:
        return 'UpdateMouthXScale';
      case UnityEventTopic.UpdateMouthYScale:
        return 'UpdateMouthYScale';
      case UnityEventTopic.SetARFaceXOffset:
        return 'SetARFaceXOffset';
      case UnityEventTopic.SetARFaceYOffset:
        return 'SetARFaceYOffset';
      case UnityEventTopic.SetARFaceScale:
        return 'SetARFaceScale';
      case UnityEventTopic.None:
        return '';
      default:
        return '';
    }
  }
}

class UnityLoadAvatarEvent extends UnityEvent {
  final num avatarCollectionId;
  final String metadataAssetUri;
  final String tokenId;
  final String tokenAssetUri;

  UnityLoadAvatarEvent({
    required this.avatarCollectionId,
    required this.metadataAssetUri,
    required this.tokenId,
    required this.tokenAssetUri
  });
}

class UnitySelectAvatarEvent extends UnityEvent {
  final num avatarCollectionId;
  final String tokenId;

  UnitySelectAvatarEvent({
    required this.avatarCollectionId,
    required this.tokenId
  });
}

class UnityInitializedEvent extends UnityEvent {
  final UnityWidgetController controller;

  UnityInitializedEvent({
    required this.controller
  });
}

class UnitySwitchModeEvent extends UnityEvent {

  UnitySwitchModeEvent();
}

class UnityMotionCapturedEvent extends UnityEvent {
  final ARKitData arKitData;

  UnityMotionCapturedEvent({
    required this.arKitData
  });
}

class UnityMessageArrivedEvent extends UnityEvent {
  final String message;

  UnityMessageArrivedEvent({
    required this.message
  });
}

class UnityCalibrationEvent extends UnityEvent {
  final CalibrationType type;
  final double value;

  UnityCalibrationEvent({
    required this.type,
    required this.value
  });
}

class UnityARFaceControlEvent extends UnityEvent {
  final ARFaceControlType type;
  final double value;

  UnityARFaceControlEvent({
    required this.type,
    required this.value
  });
}