import 'package:flutter/material.dart';
import 'package:food_order_app/core/constant.dart';
import 'package:food_order_app/presentation/screens/auth/login/login_screen.dart';
import 'package:food_order_app/presentation/screens/auth/register/register_screen.dart';

class OnboardingScreen extends StatelessWidget {
  const OnboardingScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Theme.of(context).primaryColorLight,
      body: SafeArea(
          child: Container(
        padding: EdgeInsets.symmetric(
            horizontal: Constant.padding_horizontal_1,
            vertical: Constant.padding_verticle_3),
        width: MediaQuery.of(context).size.width,
        height: MediaQuery.of(context).size.height,
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            SizedBox(
                width: double.infinity,
                child: TextButton(
                  onPressed: () async {
                    await Navigator.push(
                        context,
                        MaterialPageRoute(
                            builder: (context) => const LoginScreen()));
                  },
                  style: TextButton.styleFrom(
                      backgroundColor: Theme.of(context).primaryColorLight,
                      foregroundColor: Theme.of(context).primaryColorDark,
                      shape: RoundedRectangleBorder(
                          borderRadius:
                              BorderRadius.circular(Constant.dimension_8),
                          side: BorderSide(
                              color: Theme.of(context).primaryColorDark))),
                  child: const Text("Sign in"),
                )),
            SizedBox(
                width: double.infinity,
                child: TextButton(
                  onPressed: () async {
                    await Navigator.push(
                        context,
                        MaterialPageRoute(
                            builder: (context) => const RegisterScreen()));
                  },
                  style: TextButton.styleFrom(
                      backgroundColor: Theme.of(context).primaryColorDark,
                      foregroundColor: Theme.of(context).primaryColorLight,
                      shape: RoundedRectangleBorder(
                          borderRadius:
                              BorderRadius.circular(Constant.dimension_8),
                          side: BorderSide(
                              color: Theme.of(context).primaryColorDark))),
                  child: const Text("Sign up"),
                ))
          ],
        ),
      )),
    );
  }
}
