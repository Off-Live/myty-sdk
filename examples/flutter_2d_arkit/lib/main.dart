import 'package:arkit_plugin/arkit_plugin.dart';
import 'package:flutter/material.dart';
import 'package:flutter_2d_arkit/bloc/unity_bloc.dart';
import 'package:flutter_2d_arkit/components/avatar_button.dart';
import 'package:flutter_2d_arkit/components/myty_avatar_widget.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

void main() {
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create: (_) => UnityBloc(),
      child: MaterialApp(
        home: Scaffold(
          appBar: AppBar(title: const Text('MYTY Example')),
          body: const ExampleApp(),
        ),
      ),
    );
  }
}

class ExampleApp extends StatefulWidget {
  const ExampleApp({super.key});

  @override
  State<ExampleApp> createState() => _ExampleAppState();
}

class _ExampleAppState extends State<ExampleApp> {
  late ARKitController arKitController;
  bool _isARMode = false;

  ARKitNode? node;

  @override
  Widget build(BuildContext context) {
    return Stack(
      children: [
        const MYTYAvatarWidget(),
        Positioned(
          top: 20,
          right: 10,
          child: Switch(
            value: _isARMode,
            onChanged: (flag) {
              setState(() {
                _isARMode = !_isARMode;
                context
                    .read<UnityBloc>()
                    .add(UnitySetARModeEvent(isARMode: _isARMode));
              });
            },
          ),
        ),
        BlocSelector<UnityBloc, UnityState, List<String>>(
          selector: (state) => state.loadedAvatars,
          builder: ((context, avatars) =>
            Positioned(
              left: 10,
              right: 10,
              bottom: 30,
              child: Center(
                child: Wrap(
                  alignment: WrapAlignment.center,
                  children: avatars
                      .map((e) => AvatarButton(
                            collectionAddress:
                                "0xbc4ca0eda7647a8ab7c2061c2e118a18a936f13d",
                            tokenID: e,
                          ))
                      .toList(),
                ),
              ),
            )
          ),
        )
      ],
    );
  }
}
