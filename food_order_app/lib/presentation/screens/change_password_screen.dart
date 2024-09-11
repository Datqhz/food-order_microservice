import 'package:flutter/material.dart';
import 'package:food_order_app/core/constant.dart';
import 'package:food_order_app/core/global_val.dart';
import 'package:food_order_app/core/jwt_decode.dart';
import 'package:food_order_app/core/provider/login_state_provider.dart';
import 'package:food_order_app/core/snackbar.dart';
import 'package:food_order_app/data/requests/change_password_request.dart';
import 'package:food_order_app/data/requests/login_request.dart';
import 'package:food_order_app/repositories/auth_repository.dart';
import 'package:food_order_app/repositories/user_repository.dart';
import 'package:provider/provider.dart';

class ChangePasswordScreen extends StatefulWidget {
  const ChangePasswordScreen({super.key});

  @override
  State<ChangePasswordScreen> createState() => _ChangePasswordScreenState();
}

class _ChangePasswordScreenState extends State<ChangePasswordScreen> {
  final _formKey = GlobalKey<FormState>();
  final _oldPasswordController = TextEditingController();
  final _newPasswordController = TextEditingController();
  late AuthRepository repository;

  @override
  void initState() {
    // TODO: implement initState
    repository = new AuthRepository();
    super.initState();
  }

  @override
  void dispose() {
    // TODO: implement dispose
    _oldPasswordController.dispose();
    _newPasswordController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Theme.of(context).primaryColorLight,
      appBar: AppBar(
        backgroundColor: Theme.of(context).primaryColorLight,
        title: Text(
          "Change password",
          style: TextStyle(
              fontSize: Constant.font_size_3,
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
                      controller: _oldPasswordController,
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
                        hintText: "Current password",
                        hintStyle: TextStyle(
                            color: Constant.colour_grey,
                            fontSize: Constant.font_size_2,
                            fontWeight: Constant.font_weight_nomal),
                        labelText: "Current password",
                        labelStyle: TextStyle(
                            color: Constant.colour_grey,
                            fontWeight: Constant.font_weight_nomal),
                      ),
                      validator: (value) {
                        if (value!.isEmpty) {
                          return "Current password is required";
                        } else if (value.trim().length > 100 &&
                            value.trim().length < 5) {
                          return "Current password must be between 8 and 16 characters";
                        }
                      },
                    ),
                    SizedBox(
                      height: Constant.dimension_12,
                    ),
                    TextFormField(
                      controller: _newPasswordController,
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
                        hintText: "New password",
                        hintStyle: TextStyle(
                            color: Constant.colour_grey,
                            fontSize: Constant.font_size_2,
                            fontWeight: Constant.font_weight_nomal),
                        labelText: "New password",
                        labelStyle: TextStyle(
                            color: Constant.colour_grey,
                            fontWeight: Constant.font_weight_nomal),
                      ),
                      validator: (value) {
                        if (value!.isEmpty) {
                          return "New password is required";
                        } else if (value.trim().length > 16 &&
                            value.trim().length < 8) {
                          return "New password must be between 8 and 16 characters";
                        }
                      },
                    )
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
                        if (_formKey.currentState!.validate()) {
                          final oldPassword =
                              _oldPasswordController.text.trim();
                          final newPassword =
                              _newPasswordController.text.trim();
                          var request = ChangePasswordRequest(
                              oldPassword: oldPassword,
                              newPassword: newPassword,
                              userId: GlobalVariable.currentUser!.id);
                          var result =
                              await repository.changePassword(request, context);
                          if (result) {
                            showSnackBar(context, "Change password successful");
                            Navigator.pop(context);
                          }
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
