import 'dart:convert';

import 'package:arkit_plugin/arkit_plugin.dart';
import 'package:flutter/material.dart';
import 'package:flutter_2d_arkit/bloc/unity_bloc.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_unity_widget/flutter_unity_widget.dart';
import 'package:vector_math/vector_math_64.dart' as vm;

import '../model/unity_message.dart';

class MYTYAvatarWidget extends StatefulWidget {
  const MYTYAvatarWidget({super.key});

  @override
  State<StatefulWidget> createState() => _MYTYAvatarState();
}

class _MYTYAvatarState extends State<MYTYAvatarWidget> {
  late final ARKitController _arKitController;

  ARKitNode? node;

  @override
  Widget build(BuildContext context) {
    return Stack(
      children: [
        ARKitSceneView(
          configuration: ARKitConfiguration.faceTracking,
          onARKitViewCreated: onARKitViewCreated,
        ),
        UnityWidget(
          onUnityCreated: (controller) {
            context
                .read<UnityBloc>()
                .add(UnityInitializedEvent(controller: controller));
          },
          onUnityMessage: (message) {
            context
              .read<UnityBloc>()
              .add(UnityMessageArrivedEvent(message: message));
          },
        ),
      ],
    );
  }

  void onARKitViewCreated(ARKitController arKitController) {
    _arKitController = arKitController;
    _arKitController.onUpdateNodeForAnchor = handleUpdateAnchor;
  }

  void handleUpdateAnchor(ARKitAnchor anchor) async {
    if (anchor is ARKitFaceAnchor && mounted) {
      var upVector = anchor.transform.up;
      final faceMatrix = anchor.transform;

      final facePosition = vm.Vector3(faceMatrix.getColumn(3).x, faceMatrix.getColumn(3).y, faceMatrix.getColumn(3).z);

      final screenCoordinates = await worldToScreen(facePosition);

      if (!mounted) return;

      var screenWidth = MediaQuery.of(context).size.width;
      var screenHeight = MediaQuery.of(context).size.height;

      context.read<UnityBloc>().add(UnityMotionCapturedEvent(
          arKitData: ARKitData(
              facePosition: vm.Vector3(screenCoordinates.x / screenWidth, screenCoordinates.y / screenHeight, 0),
              faceScale: vm.Vector3(0.35 / facePosition.length, 0.35 / facePosition.length, 0.35 / facePosition.length),
              up: vm.Vector3(-upVector.x, upVector.y, upVector.z),
              forward: anchor.transform.forward,
              blendshapes: ARKitBlendShape(
                  browDownLeft: anchor.blendShapes['browDown_L'] ?? 0,
                  browDownRight: anchor.blendShapes['browDown_R'] ?? 0,
                  browOuterUpLeft: anchor.blendShapes['browOuterUp_L'] ?? 0,
                  browOuterUpRight: anchor.blendShapes['browOuterUp_R'] ?? 0,
                  eyeBlinkLeft: anchor.blendShapes['eyeBlink_L'] ?? 0,
                  eyeBlinkRight: anchor.blendShapes['eyeBlink_R'] ?? 0,
                  jawOpen: anchor.blendShapes['jawOpen'] ?? 0,
                  mouthClose: anchor.blendShapes['mouthClose'] ?? 0,
                  mouthPucker: anchor.blendShapes['mouthPucker'] ?? 0,
                  mouthSmileLeft: anchor.blendShapes['mouthSmile_L'] ?? 0,
                  mouthSmileRight: anchor.blendShapes['mouthSmile_R'] ?? 0,
                  mouthStretchLeft: anchor.blendShapes['mouthStretch_L'] ?? 0,
                  mouthStretchRight:
                  anchor.blendShapes['mouthStretch_R'] ?? 0))));
    }
  }

  Future<vm.Vector2> worldToScreen(vm.Vector3 worldPosition) async {
    final projectPoint = await _arKitController.projectPoint(worldPosition);

    return vm.Vector2(projectPoint!.x, projectPoint.y);
  }
}