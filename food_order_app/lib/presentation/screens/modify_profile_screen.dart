
import 'package:flutter/material.dart';
import 'package:food_order_app/core/constant.dart';
import 'package:food_order_app/core/global_val.dart';
import 'package:food_order_app/core/snackbar.dart';
import 'package:food_order_app/core/stream/change_stream.dart';
import 'package:food_order_app/data/requests/update_user_request.dart';
import 'package:food_order_app/repositories/user_repository.dart';

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
                        var result = await UserRepository().update(
                            UpdateUserRequest(
                                id: GlobalVariable.currentUser!.id,
                                displayName: displayName,
                                phoneNumber: phone),
                            context);
                        if (result) {
                          showSnackBar(context, "Update successful");
                          GlobalVariable.currentUser!.displayName = displayName;
                          GlobalVariable.currentUser!.phoneNumber = phone;
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
