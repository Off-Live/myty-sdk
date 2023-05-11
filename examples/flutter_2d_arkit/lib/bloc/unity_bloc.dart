import 'dart:convert';

import 'package:equatable/equatable.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_unity_widget/flutter_unity_widget.dart';

import '../model/unity_message.dart';

part 'unity_state.dart';

part 'unity_event.dart';

class UnityBloc extends Bloc<UnityEvent, UnityState> {
  // ignore: non_constant_identifier_names
  final String MessageHandler = 'MessageHandler';

  UnityBloc() : super(UnityState.initial()) {
    on<UnityInitializedEvent>(_registerController);
    on<UnityLoadAvatarEvent>(_loadAvatar);
    on<UnitySelectAvatarEvent>(_selectAvatar);
    on<UnitySwitchModeEvent>(_switchMode);
    on<UnityMotionCapturedEvent>(_motionCaptured);
    on<UnityMessageArrivedEvent>(_onMessageArrived);
    on<UnityCalibrationEvent>(_onCalibration);
    on<UnityARFaceControlEvent>(_onARFaceControl);
  }

  void _registerController(UnityInitializedEvent event, Emitter emit) {
    emit(state.copyWith(controller: event.controller));

    _postMessage(
      UnityEventTopic.LoadAvatar,
      const LoadAvatarMessage(
        avatarCollectionId: 0,
        metadataAssetUri:
            "https://10k-asset.s3.ap-southeast-1.amazonaws.com/mock/collection_mas_metadata.zip",
        tokenId: "1",
        tokenAssetUri:
            "https://10k-asset.s3.ap-southeast-1.amazonaws.com/mock/1.zip",
      ),
    );
  }

  void _loadAvatar(UnityLoadAvatarEvent event, _) {
    var topic = UnityEventTopic.LoadAvatar;

    _postMessage(
      topic,
      LoadAvatarMessage(
          avatarCollectionId: event.avatarCollectionId,
          metadataAssetUri: event.metadataAssetUri,
          tokenId: event.tokenId,
          tokenAssetUri: event.tokenAssetUri),
    );
  }

  void _selectAvatar(UnitySelectAvatarEvent event, _) {
    var topic = UnityEventTopic.SelectAvatar;

    _postMessage(
      topic,
      SelectAvatarMessage(
        avatarCollectionId: event.avatarCollectionId,
        tokenId: event.tokenId,
      ),
    );
  }

  void _switchMode(UnitySwitchModeEvent event, _) {
    var topic = UnityEventTopic.SwitchMode;

    _postMessageRaw(topic, "");
  }

  void _motionCaptured(UnityMotionCapturedEvent event, _) {
    var topic = UnityEventTopic.ProcessCapturedResult;

    _postMessage(topic, event.arKitData);
  }

  void _onCalibration(UnityCalibrationEvent event, _) {
    var topic = getCalibrationTopic(event.type);

    _postMessageRaw(topic, (event.value / 100).toStringAsFixed(2));
  }

  UnityEventTopic getCalibrationTopic(CalibrationType type) {
    switch(type) {
      case CalibrationType.SyncedBlink:
        return UnityEventTopic.UpdateSyncedBlinkScale;
      case CalibrationType.Blink:
        return UnityEventTopic.UpdateBlinkScale;
      case CalibrationType.Eyebrow:
        return UnityEventTopic.UpdateEyebrowScale;
      case CalibrationType.Pupil:
        return UnityEventTopic.UpdatePupilScale;
      case CalibrationType.MouthX:
        return UnityEventTopic.UpdateMouthXScale;
      case CalibrationType.MouthY:
        return UnityEventTopic.UpdateMouthYScale;
    }
  }

  void _onARFaceControl(UnityARFaceControlEvent event, _) {
    var topic = getARFaceControlTopic(event.type);

    _postMessageRaw(topic, (event.value).toStringAsFixed(2));
  }

  UnityEventTopic getARFaceControlTopic(ARFaceControlType type) {
    switch(type) {
      case ARFaceControlType.XOffset:
        return UnityEventTopic.SetARFaceXOffset;
      case ARFaceControlType.YOffset:
        return UnityEventTopic.SetARFaceYOffset;
      case ARFaceControlType.Scale:
        return UnityEventTopic.SetARFaceScale;
    }
  }

  void _onMessageArrived(UnityMessageArrivedEvent event, Emitter emit) {
    var decoded = jsonDecode(event.message);

    var currentAvatars = <String>[...state.loadedAvatars, decoded['tokenId']];
    emit(state.copyWith(loadedAvatars: currentAvatars));
  }

  void _postMessage(UnityEventTopic topic, Object obj) {
    state.controller?.postMessage(
      MessageHandler,
      topic.topicName,
      jsonEncode(obj),
    );
  }

  void _postMessageRaw(UnityEventTopic topic, String str) {
    state.controller?.postMessage(
      MessageHandler,
      topic.topicName,
      str,
    );
  }
}
