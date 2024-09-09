import 'package:flutter/material.dart';
import 'package:food_order_app/core/constant.dart';

class HomeScreen extends StatefulWidget {
  const HomeScreen({super.key});

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Theme.of(context).primaryColorLight,
      body: SafeArea(
        child: Container(
          width: MediaQuery.of(context).size.width,
          height: MediaQuery.of(context).size.height,
          padding:
              EdgeInsets.symmetric(horizontal: Constant.padding_horizontal_1),
          child: const SingleChildScrollView(
            child: Column(
              children: [
                Text("sss"),
                Text("sss"),
                Text("sss"),
                Text("sss"),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
