import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:food_order_app/core/constant.dart';
import 'package:food_order_app/core/global_val.dart';
import 'package:food_order_app/core/provider/login_state_provider.dart';
import 'package:food_order_app/repositories/auth_repository.dart';
import 'package:provider/provider.dart';

class SettingScreen extends StatelessWidget {
  const SettingScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        backgroundColor: Theme.of(context).primaryColorLight,
        title: Text(
          "Settings",
          style: TextStyle(
              fontSize: Constant.font_size_4,
              fontWeight: Constant.font_weight_heading2),
        ),
      ),
      backgroundColor: Theme.of(context).primaryColorLight,
      body: SafeArea(
        child: Container(
          padding: EdgeInsets.symmetric(
              horizontal: Constant.padding_horizontal_2,
              vertical: Constant.padding_verticle_3),
          child: TextButton(
            onPressed: () async {
              showDialog(
                context: context,
                builder: (context) => Dialog(
                  child: Container(
                    padding: EdgeInsets.all(Constant.dimension_8),
                    height: 120,
                    color: Theme.of(context).primaryColorDark,
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      children: [
                        Text(
                          "Do you want delete your account?",
                          style: TextStyle(
                              color: Theme.of(context).primaryColorLight,
                              fontSize: Constant.font_size_4,
                              fontWeight: Constant.font_weight_heading2),
                        ),
                        Row(
                          mainAxisAlignment: MainAxisAlignment.end,
                          children: [
                            TextButton(
                              onPressed: () async {
                                Navigator.pop(context);
                              },
                              style: TextButton.styleFrom(
                                  backgroundColor: Colors.transparent,
                                  shape: RoundedRectangleBorder(
                                    borderRadius: BorderRadius.circular(
                                        Constant.dimension_8),
                                  )),
                              child: Text(
                                "NO",
                                style: TextStyle(
                                    color: Theme.of(context).primaryColorLight),
                              ),
                            ),
                            TextButton(
                              onPressed: () async {
                                var result = await AuthRepository()
                                    .deleteAccount(context);
                                if (result) {
                                  GlobalVariable.currentUser = null;
                                  GlobalVariable.loginResponse = null;
                                  Navigator.pop(context);
                                  Navigator.pop(context);
                                  Provider.of<LoginStateProvider>(context,
                                          listen: false)
                                      .logout();
                                }
                              },
                              style: TextButton.styleFrom(
                                backgroundColor: Constant.colour_low_white,
                              ),
                              child: Text(
                                "YES",
                                style: TextStyle(
                                    color: Theme.of(context).primaryColorDark),
                              ),
                            ),
                          ],
                        )
                      ],
                    ),
                  ),
                ),
              );
            },
            style: TextButton.styleFrom(
                backgroundColor: Colors.transparent,
                shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(Constant.dimension_8),
                    side: BorderSide(color: Constant.colour_red))),
            child: Text(
              "Delete account",
              style: TextStyle(color: Constant.colour_red),
            ),
          ),
        ),
      ),
    );
  }
}
