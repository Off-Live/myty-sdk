import 'package:cached_network_image/cached_network_image.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import '../bloc/unity_bloc.dart';

typedef AvatarSelectionEvent = void Function(
    String collectionAddress, String tokenID);

class AvatarButton extends StatefulWidget {
  final String collectionAddress;
  final String tokenID;

  const AvatarButton({
    super.key,
    required this.collectionAddress,
    required this.tokenID,
  });

  @override
  State<AvatarButton> createState() => _AvatarButtonState();
}

class _AvatarButtonState extends State<AvatarButton> {
  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: () {
        context.read<UnityBloc>().add(UnitySelectAvatarEvent(
              assetCollectionId: 0,
              tokenId: widget.tokenID,
            ));
      },
      child: CircleAvatar(
        backgroundColor: Colors.white,
        radius: 30,
        child: CachedNetworkImage(
          imageUrl:
              'https://resources.myty.space/v1/nft-image?contractaddress=${widget.collectionAddress}&tokenid=${widget.tokenID}&size=thumbnail',
          imageBuilder: (context, imageProvider) => CircleAvatar(
            radius: 30,
            backgroundImage: imageProvider,
          ),
          placeholder: (context, url) => const CircularProgressIndicator(),
          errorWidget: (context, url, error) => const Icon(Icons.error),
        ),
      ),
    );
  }
}
