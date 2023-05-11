import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import '../bloc/unity_bloc.dart';

class ControlPanel extends StatefulWidget {
  const ControlPanel({super.key});

  @override
  State<ControlPanel> createState() => _ControlPanelState();
}

class _ControlPanelState extends State<ControlPanel> {
  bool isPanelVisible = false;
  bool isCalibrationMode = true;

  double syncedBlinkValue = 50;
  double blinkValue = 100;
  double eyebrowValue = 100;
  double pupilValue = 100;
  double mouthXValue = 100;
  double mouthYValue = 100;

  double arFaceXOffsetValue = 0;
  double arFaceYOffsetValue = 0;
  double arFaceScaleValue = 1;

  @override
  Widget build(BuildContext context) {
    return SingleChildScrollView(
      child: Column(
        children: [
          ElevatedButton(
            child: isPanelVisible ? const Text('Close') : const Text('Control Panel'),
            onPressed: () {
              setState(() {
                isPanelVisible = !isPanelVisible;
              });
            },
          ),
          if (isPanelVisible) ...[
            SwitchListTile(
              title: Text('Mode: ${isCalibrationMode ? "Calibration" : "AR Control"}'),
              value: isCalibrationMode,
              onChanged: (value) {
                setState(() {
                  isCalibrationMode = value;
                });
              },
            ),
            if (isCalibrationMode) ...[
              Text('SyncedBlink: ${syncedBlinkValue.toStringAsFixed(0)}'),
              Slider(
                value: syncedBlinkValue,
                min: 0,
                max: 100,
                divisions: 100,
                onChanged: (newValue) {
                  context.read<UnityBloc>().add(UnityCalibrationEvent(
                      type: CalibrationType.SyncedBlink,
                      value: newValue
                  ));
                  setState(() {
                    syncedBlinkValue = newValue;
                  });
                },
              ),
              Text('Blink: ${blinkValue.toStringAsFixed(0)}'),
              Slider(
                value: blinkValue,
                min: 0,
                max: 200,
                divisions: 200,
                onChanged: (newValue) {
                  context.read<UnityBloc>().add(UnityCalibrationEvent(
                      type: CalibrationType.Blink,
                      value: newValue
                  ));
                  setState(() {
                    blinkValue = newValue;
                  });
                },
              ),
              Text('Eyebrow: ${eyebrowValue.toStringAsFixed(0)}'),
              Slider(
                value: eyebrowValue,
                min: 0,
                max: 200,
                divisions: 200,
                onChanged: (newValue) {
                  context.read<UnityBloc>().add(UnityCalibrationEvent(
                      type: CalibrationType.Eyebrow,
                      value: newValue
                  ));
                  setState(() {
                    eyebrowValue = newValue;
                  });
                },
              ),
              Text('Pupil: ${pupilValue.toStringAsFixed(0)}'),
              Slider(
                value: pupilValue,
                min: 0,
                max: 200,
                divisions: 200,
                onChanged: (newValue) {
                  context.read<UnityBloc>().add(UnityCalibrationEvent(
                      type: CalibrationType.Pupil,
                      value: newValue
                  ));
                  setState(() {
                    pupilValue = newValue;
                  });
                },
              ),
              Text('MouthX: ${mouthXValue.toStringAsFixed(0)}'),
              Slider(
                value: mouthXValue,
                min: 0,
                max: 200,
                divisions: 200,
                onChanged: (newValue) {
                  context.read<UnityBloc>().add(UnityCalibrationEvent(
                      type: CalibrationType.MouthX,
                      value: newValue
                  ));
                  setState(() {
                    mouthXValue = newValue;
                  });
                },
              ),
              Text('MouthY: ${mouthYValue.toStringAsFixed(0)}'),
              Slider(
                value: mouthYValue,
                min: 0,
                max: 200,
                divisions: 200,
                onChanged: (newValue) {
                  context.read<UnityBloc>().add(UnityCalibrationEvent(
                      type: CalibrationType.MouthY,
                      value: newValue
                  ));
                  setState(() {
                    mouthYValue = newValue;
                  });
                },
              ),
            ] else ...[
              Text('AR Face XOffset: ${arFaceXOffsetValue.toStringAsFixed(2)}'),
              Slider(
                value: arFaceXOffsetValue,
                min: -1,
                max: 1,
                divisions: 100,
                onChanged: (newValue) {
                  context.read<UnityBloc>().add(UnityARFaceControlEvent(
                      type: ARFaceControlType.XOffset,
                      value: newValue
                  ));
                  setState(() {
                    arFaceXOffsetValue = newValue;
                  });
                },
              ),
              Text('AR Face YOffset: ${arFaceYOffsetValue.toStringAsFixed(2)}'),
              Slider(
                value: arFaceYOffsetValue,
                min: -1,
                max: 1,
                divisions: 100,
                onChanged: (newValue) {
                  context.read<UnityBloc>().add(UnityARFaceControlEvent(
                      type: ARFaceControlType.YOffset,
                      value: newValue
                  ));
                  setState(() {
                    arFaceYOffsetValue = newValue;
                  });
                },
              ),
              Text('AR Face Scale: ${arFaceScaleValue.toStringAsFixed(2)}'),
              Slider(
                value: arFaceScaleValue,
                min: 0.5,
                max: 2,
                divisions: 150,
                onChanged: (newValue) {
                  context.read<UnityBloc>().add(UnityARFaceControlEvent(
                      type: ARFaceControlType.Scale,
                      value: newValue
                  ));
                  setState(() {
                    arFaceScaleValue = newValue;
                  });
                },
              ),
            ],
          ]
        ],
      ),
    );
  }
}