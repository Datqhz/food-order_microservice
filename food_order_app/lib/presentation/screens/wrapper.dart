import 'package:flutter/material.dart';
import 'package:food_order_app/core/provider/login_state_provider.dart';
import 'package:food_order_app/presentation/screens/main_screen.dart';
import 'package:food_order_app/presentation/screens/onboarding_screen.dart';
import 'package:provider/provider.dart';

class Wrapper extends StatelessWidget {
  const Wrapper({super.key});

  @override
  Widget build(BuildContext context) {
    final loginState = Provider.of<LoginStateProvider>(context);
    return loginState.isLogin ? const MainScreen() : const OnboardingScreen();
  }
}
