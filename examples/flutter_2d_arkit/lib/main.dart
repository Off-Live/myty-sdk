import 'package:arkit_plugin/arkit_plugin.dart';
import 'package:flutter/material.dart';
import 'package:vector_math/vector_math_64.dart' as vm;

void main() {
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'MYTY Example',
      theme: ThemeData(
        primarySwatch: Colors.blue,
      ),
      home: const FaceDetection(title: 'Face Detection'),
    );
  }
}

class FaceDetection extends StatefulWidget {
  const FaceDetection({super.key, required this.title});

  final String title;

  @override
  State<FaceDetection> createState() => _FaceDetectionState();
}

class _FaceDetectionState extends State<FaceDetection> {
  late ARKitController arKitController;

  ARKitNode? node;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text(widget.title)),
      body: Container(
        child: ARKitSceneView(
          configuration: ARKitConfiguration.faceTracking,
          onARKitViewCreated: onARKitViewCreated,
        ),
      ),
    );
  }

  void onARKitViewCreated(ARKitController arKitController) {
    this.arKitController = arKitController;
    this.arKitController.onAddNodeForAnchor = _handleAddAnchor;
    this.arKitController.onUpdateNodeForAnchor = _handleUpdateAnchor;
  }

  void _handleAddAnchor(ARKitAnchor anchor) {
    if (anchor is! ARKitFaceAnchor) {
      return;
    }
    final material = ARKitMaterial(fillMode: ARKitFillMode.lines);
    anchor.geometry.materials.value = [material];

    node = ARKitNode(geometry: anchor.geometry);
    arKitController.add(node!, parentNodeName: anchor.nodeName);
  }

  void _handleUpdateAnchor(ARKitAnchor anchor) async {
    if (anchor is ARKitFaceAnchor && mounted) {
      arKitController.updateFaceGeometry(node!, anchor.identifier);
      final projectionMatrix = await arKitController.cameraProjectionMatrix();
      final viewMatrix = await arKitController.pointOfViewTransform();
      print(projectPoint(projectionMatrix! * viewMatrix!, anchor.transform.getTranslation()));
    }
  }

  vm.Vector2 projectPoint(Matrix4 modelViewProjectionMatrix, vm.Vector3 point) {
    final projectedPoint = modelViewProjectionMatrix * vm.Vector4(point.x, point.y, point.z, 1.0);
    final double x = (projectedPoint.x / projectedPoint.w + 1.0) / 2.0;
    final double y = 1.0 - (projectedPoint.y / projectedPoint.w + 1.0) / 2.0;
    return vm.Vector2(x, y);
  }
}
