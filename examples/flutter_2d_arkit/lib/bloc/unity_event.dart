part of 'unity_bloc.dart';

abstract class UnityEvent {}

enum UnityEventTopic {
  UnityCreated,
  LoadAvatar,
  LoadAvatarList,
  SelectAvatar,
  SetARMode,
  ProcessCapturedResult,
  None
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
      case UnityEventTopic.SetARMode:
        return 'SetARMode';
      case UnityEventTopic.ProcessCapturedResult:
        return 'ProcessCapturedResult';
      case UnityEventTopic.None:
        return '';
      default:
        return '';
    }
  }
}

class UnityLoadAvatarEvent extends UnityEvent {
  final num assetCollectionId;
  final String metadataAssetUri;
  final String tokenId;
  final String tokenAssetUri;

  UnityLoadAvatarEvent({
    required this.assetCollectionId,
    required this.metadataAssetUri,
    required this.tokenId,
    required this.tokenAssetUri
  });
}

class UnitySelectAvatarEvent extends UnityEvent {
  final num assetCollectionId;
  final String tokenId;

  UnitySelectAvatarEvent({
    required this.assetCollectionId,
    required this.tokenId
  });
}

class UnityInitializedEvent extends UnityEvent {
  final UnityWidgetController controller;

  UnityInitializedEvent({
    required this.controller
  });
}

class UnitySetARModeEvent extends UnityEvent {
  final bool isARMode;

  UnitySetARModeEvent({
    required this.isARMode
  });
}

class UnityMotionCapturedEvent extends UnityEvent {
  final ARKitData arKitData;

  UnityMotionCapturedEvent({
    required this.arKitData
  });
}