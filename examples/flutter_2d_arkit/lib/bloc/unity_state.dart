part of 'unity_bloc.dart';

class UnityState extends Equatable {
  const UnityState({
    required this.controller,
    required this.loadedAvatars,
  });

  UnityState.initial()
      : controller = null,
        loadedAvatars = <String>[];

  final UnityWidgetController? controller;
  final List<String> loadedAvatars;

  @override
  List<Object?> get props => [controller, loadedAvatars];

  UnityState copyWith(
      {UnityWidgetController? controller, List<String>? loadedAvatars}) {
    return UnityState(
      controller: controller ?? this.controller,
      loadedAvatars: loadedAvatars ?? this.loadedAvatars,
    );
  }
}
