part of 'unity_bloc.dart';

class UnityState extends Equatable {
  const UnityState(
      {required this.controller});

  const UnityState.initial()
      : controller = null;

  final UnityWidgetController? controller;

  @override
  List<Object?> get props => [
    controller
  ];
}