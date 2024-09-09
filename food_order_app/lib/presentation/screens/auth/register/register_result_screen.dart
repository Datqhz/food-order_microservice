import 'package:flutter/material.dart';
import 'package:food_order_app/core/constant.dart';
import 'package:food_order_app/presentation/screens/auth/login/login_screen.dart';

class RegisterResultScreen extends StatelessWidget {
  const RegisterResultScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Theme.of(context).primaryColorLight,
      body: SafeArea(
        child: Container(
          padding: EdgeInsets.symmetric(
              horizontal: Constant.padding_horizontal_1,
              vertical: Constant.padding_verticle_3),
          height: MediaQuery.of(context).size.height,
          width: MediaQuery.of(context).size.width,
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            crossAxisAlignment: CrossAxisAlignment.center,
            children: [
              Text(
                "Registation successful.",
                style: TextStyle(
                  color: Theme.of(context).primaryColorDark,
                  fontSize: Constant.font_size_heading_2,
                  fontWeight: Constant.font_weight_heading1,
                ),
              ),
              SizedBox(
                height: Constant.dimension_12,
              ),
              SizedBox(
                width: double.infinity,
                child: TextButton(
                  onPressed: () {
                    Navigator.pushReplacement(
                        context,
                        MaterialPageRoute(
                            builder: (context) => const LoginScreen()));
                  },
                  style: TextButton.styleFrom(
                      backgroundColor: Theme.of(context).primaryColorDark,
                      foregroundColor: Theme.of(context).primaryColorLight,
                      shape: RoundedRectangleBorder(
                          borderRadius:
                              BorderRadius.circular(Constant.dimension_100),
                          side: BorderSide(
                              color: Theme.of(context).primaryColorDark))),
                  child: const Text("Return to login"),
                ),
              )
            ],
          ),
        ),
      ),
    );
  }
}
