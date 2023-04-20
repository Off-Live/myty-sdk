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
    on<UnitySetARModeEvent>(_setARMode);
    on<UnityMotionCapturedEvent>(_motionCaptured);
    on<UnityMessageArrivedEvent>(_onMessageArrived);
  }

  void _registerController(UnityInitializedEvent event, Emitter emit) {
    emit(state.copyWith(controller: event.controller));

    for (var e in [0, 1, 2, 3, 4, 5]) {
      _postMessage(
        UnityEventTopic.LoadAvatar,
        LoadAvatarMessage(
          assetVersionId: 0,
          templateAssetUri:
              "https://10k-asset.s3.ap-southeast-1.amazonaws.com/mock/collection_mas_metadata.zip",
          tokenId: e.toString(),
          tokenAssetUri:
              "https://10k-asset.s3.ap-southeast-1.amazonaws.com/mock/${e.toString()}.zip",
        ),
      );
    }
  }

  void _loadAvatar(UnityLoadAvatarEvent event, _) {
    var topic = UnityEventTopic.LoadAvatar;

    _postMessage(
      topic,
      LoadAvatarMessage(
          assetVersionId: event.assetCollectionId,
          templateAssetUri: event.metadataAssetUri,
          tokenId: event.tokenId,
          tokenAssetUri: event.tokenAssetUri),
    );
  }

  void _selectAvatar(UnitySelectAvatarEvent event, _) {
    var topic = UnityEventTopic.SelectAvatar;

    _postMessage(
      topic,
      SelectAvatarMessage(
        assetVersionId: event.assetCollectionId,
        tokenId: event.tokenId,
      ),
    );
  }

  void _setARMode(UnitySetARModeEvent event, _) {
    var topic = UnityEventTopic.SetARMode;

    _postMessageRaw(topic, event.isARMode ? "true" : "false");
  }

  void _motionCaptured(UnityMotionCapturedEvent event, _) {
    var topic = UnityEventTopic.ProcessCapturedResult;

    _postMessage(topic, event.arKitData);
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
