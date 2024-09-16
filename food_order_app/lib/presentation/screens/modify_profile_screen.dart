import 'dart:io';

import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:food_order_app/core/constant.dart';
import 'package:food_order_app/core/global_val.dart';
import 'package:food_order_app/core/snackbar.dart';
import 'package:food_order_app/core/stream/change_stream.dart';
import 'package:food_order_app/core/utils/image_helper.dart';
import 'package:food_order_app/data/requests/update_user_request.dart';
import 'package:food_order_app/repositories/user_repository.dart';
import 'package:image_picker/image_picker.dart';

class ModifyProfileScreen extends StatefulWidget {
  ModifyProfileScreen({super.key, required this.stream});
  ChangeStream stream;
  @override
  State<ModifyProfileScreen> createState() => _ModifyProfileScreenState();
}

class _ModifyProfileScreenState extends State<ModifyProfileScreen> {
  final _formKey = GlobalKey<FormState>();
  final _displayNameController = TextEditingController();
  final _phoneController = TextEditingController();
  final ValueNotifier<XFile?> _avatar = ValueNotifier(null);

  @override
  void initState() {
    // TODO: implement initState
    super.initState();
    _autoFill();
  }

  void _autoFill() {
    _displayNameController.text = GlobalVariable.currentUser!.displayName;
    _phoneController.text = GlobalVariable.currentUser!.phoneNumber;
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Theme.of(context).primaryColorLight,
      appBar: AppBar(
        backgroundColor: Theme.of(context).primaryColorLight,
        title: Text(
          "Update infomation",
          style: TextStyle(
              fontSize: Constant.font_size_2,
              fontWeight: Constant.font_weight_nomal,
              color: Theme.of(context).primaryColorDark),
        ),
      ),
      body: Stack(
        children: [
          Container(
            width: MediaQuery.of(context).size.width,
            height: MediaQuery.of(context).size.height,
            padding: EdgeInsets.symmetric(
                horizontal: Constant.padding_horizontal_2,
                vertical: Constant.padding_verticle_3),
            child: SingleChildScrollView(
              child: Form(
                key: _formKey,
                child: Column(
                  mainAxisAlignment: MainAxisAlignment.center,
                  mainAxisSize: MainAxisSize.max,
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Row(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        GestureDetector(
                          onTap: () async {
                            XFile? img = await ImagePicker().pickImage(
                                maxWidth: 1920,
                                maxHeight: 1080,
                                imageQuality: 100,
                                source: ImageSource.gallery);
                            if (img != null) {
                              _avatar.value = img;
                            }
                          },
                          child: SizedBox(
                            height: 100,
                            child: Stack(
                              children: [
                                ValueListenableBuilder(
                                    valueListenable: _avatar,
                                    builder: (context, value, child) {
                                      return CircleAvatar(
                                        backgroundColor: Colors.black,
                                        radius: 50,
                                        backgroundImage: value != null
                                            ? FileImage(File(value.path))
                                                as ImageProvider<Object>
                                            : NetworkImage(GlobalVariable
                                                .currentUser!.avatar),
                                      );
                                    }),
                                Positioned(
                                  bottom: 0,
                                  right: 16,
                                  child: Container(
                                    height: 20,
                                    width: 20,
                                    decoration: BoxDecoration(
                                        color: Colors.black,
                                        borderRadius: BorderRadius.circular(2)),
                                    child: const Icon(
                                      CupertinoIcons.pencil,
                                      size: 16,
                                      color: Color.fromRGBO(240, 240, 240, 1),
                                    ),
                                  ),
                                )
                              ],
                            ),
                          ),
                        ),
                      ],
                    ),
                    SizedBox(
                      height: Constant.dimension_12,
                    ),
                    TextFormField(
                      controller: _displayNameController,
                      decoration: InputDecoration(
                        enabledBorder: OutlineInputBorder(
                          borderSide: BorderSide(
                              color: Theme.of(context).primaryColorDark,
                              width: 1),
                        ),
                        focusedBorder: OutlineInputBorder(
                          borderSide:
                              BorderSide(color: Constant.colour_blue, width: 1),
                        ),
                        hintText: "DisplayName",
                        hintStyle: TextStyle(
                            color: Constant.colour_grey,
                            fontSize: Constant.font_size_2,
                            fontWeight: Constant.font_weight_nomal),
                        labelText: "DisplayName",
                        labelStyle: TextStyle(
                            color: Constant.colour_grey,
                            fontWeight: Constant.font_weight_nomal),
                      ),
                    ),
                    SizedBox(
                      height: Constant.dimension_12,
                    ),
                    TextFormField(
                      controller: _phoneController,
                      decoration: InputDecoration(
                        enabledBorder: OutlineInputBorder(
                          borderSide: BorderSide(
                              color: Theme.of(context).primaryColorDark,
                              width: 1),
                        ),
                        focusedBorder: OutlineInputBorder(
                          borderSide:
                              BorderSide(color: Constant.colour_blue, width: 1),
                        ),
                        hintText: "Phone",
                        hintStyle: TextStyle(
                            color: Constant.colour_grey,
                            fontSize: Constant.font_size_2,
                            fontWeight: Constant.font_weight_nomal),
                        labelText: "Phone",
                        labelStyle: TextStyle(
                            color: Constant.colour_grey,
                            fontWeight: Constant.font_weight_nomal),
                      ),
                    ),
                    SizedBox(
                      height: Constant.dimension_12,
                    ),
                  ],
                ),
              ),
            ),
          ),
          Positioned(
              left: 0,
              right: 0,
              bottom: 0,
              child: Padding(
                padding: EdgeInsets.symmetric(
                    horizontal: Constant.padding_horizontal_2,
                    vertical: Constant.padding_verticle_1),
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.end,
                  children: [
                    TextButton(
                      onPressed: () async {
                        final displayName = _displayNameController.text.trim();
                        final phone = _phoneController.text.trim();
                        String? avtUrl;
                        if (_avatar.value != null) {
                          avtUrl = await ImageHelper.uploadAvatarImage(_avatar.value);
                        } else {
                          avtUrl = GlobalVariable.currentUser!.avatar;
                        }
                        if (avtUrl == null) {
                          showSnackBar(context, "Update failed");
                          return;
                        }
                        var result = await UserRepository().update(
                            UpdateUserRequest(
                                id: GlobalVariable.currentUser!.id,
                                displayName: displayName,
                                phoneNumber: phone,
                                avatar: avtUrl),
                            context);
                        if (result) {
                          showSnackBar(context, "Update successful");
                          GlobalVariable.currentUser!.displayName = displayName;
                          GlobalVariable.currentUser!.phoneNumber = phone;
                          GlobalVariable.currentUser!.avatar = avtUrl;
                          widget.stream.notifyChange();
                          Navigator.pop(context);
                        }
                      },
                      style: TextButton.styleFrom(
                          backgroundColor: Theme.of(context).primaryColorDark,
                          foregroundColor: Theme.of(context).primaryColorLight,
                          shape: RoundedRectangleBorder(
                              borderRadius:
                                  BorderRadius.circular(Constant.dimension_100),
                              side: BorderSide(
                                  color: Theme.of(context).primaryColorDark))),
                      child: const Text("Done"),
                    )
                  ],
                ),
              ))
        ],
      ),
    );
  }
}
