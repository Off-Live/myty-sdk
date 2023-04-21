import 'package:vector_math/vector_math_64.dart';

class LoadAvatarMessage {
  final num assetVersionId;
  final String templateAssetUri;
  final String tokenId;
  final String tokenAssetUri;

  const LoadAvatarMessage({
    required this.assetVersionId,
    required this.templateAssetUri,
    required this.tokenId,
    required this.tokenAssetUri
  });

  LoadAvatarMessage.fromJson(Map<String, dynamic> json)
      : assetVersionId = json['assetVersionId'],
        templateAssetUri = json['templateAssetUri'],
        tokenId = json['tokenId'],
        tokenAssetUri = json['tokenAssetUri'];

  Map<String, dynamic> toJson() {
    return {
      'assetVersionId': assetVersionId,
      'templateAssetUri': templateAssetUri,
      'tokenId': tokenId,
      'tokenAssetUri': tokenAssetUri
    };
  }
}

class SelectAvatarMessage {
  final num assetVersionId;
  final String tokenId;

  const SelectAvatarMessage({
    required this.assetVersionId,
    required this.tokenId,
  });

  SelectAvatarMessage.fromJson(Map<String, dynamic> json)
      : assetVersionId = json['assetVersionId'],
        tokenId = json['tokenId'];

  Map<String, dynamic> toJson() {
    return {
      'assetVersionId': assetVersionId,
      'tokenId': tokenId,
    };
  }
}

class ARKitBlendShape
{
  final num eyeBlinkLeft;
  final num eyeBlinkRight;
  final num mouthPucker;
  final num mouthStretchLeft;
  final num mouthSmileLeft;
  final num mouthStretchRight;
  final num mouthSmileRight;
  final num jawOpen;
  final num mouthClose;
  final num browDownLeft;
  final num browOuterUpLeft;
  final num browDownRight;
  final num browOuterUpRight;

  ARKitBlendShape({
    required this.eyeBlinkLeft,
    required this.eyeBlinkRight,
    required this.mouthPucker,
    required this.mouthStretchLeft,
    required this.mouthSmileLeft,
    required this.mouthStretchRight,
    required this.mouthSmileRight,
    required this.jawOpen,
    required this.mouthClose,
    required this.browDownLeft,
    required this.browOuterUpLeft,
    required this.browDownRight,
    required this.browOuterUpRight
  });

  Map<String, dynamic> toJson() {
    return {
      'eyeblinkLeft': eyeBlinkLeft,
      'eyeblinkRight': eyeBlinkRight,
      'mouthPucker': mouthPucker,
      'mouthStretchLeft': mouthStretchLeft,
      'mouthSmileLeft': mouthSmileLeft,
      'mouthStretchRight': mouthStretchRight,
      'mouthSmileRight': mouthSmileRight,
      'jawOpen': jawOpen,
      'mouthClose': mouthClose,
      'browDownLeft': browDownLeft,
      'browOuterUpLeft': browOuterUpLeft,
      'browDownRight': browDownRight,
      'browOuterUpRight': browOuterUpRight
    };
  }
}

class ARKitData {
  final Vector3 facePosition;
  final Vector3 faceScale;
  final Vector3 up;
  final Vector3 forward;
  final ARKitBlendShape blendshapes;

  ARKitData({
    required this.facePosition,
    required this.faceScale,
    required this.up,
    required this.forward,
    required this.blendshapes
  });

  Map<String, dynamic> toJson() {
    return {
      'facePosition': facePosition.toJson(),
      'faceScale': faceScale.toJson(),
      'up': up.toJson(),
      'forward': forward.toJson(),
      'blendshapes': blendshapes.toJson()
    };
  }
}

extension SerializedVector3 on Vector3 {
  Map<String, dynamic> toJson() {
    return {
      'x': x,
      'y': y,
      'z': z
    };
  }
}