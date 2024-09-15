import 'package:cloudinary_flutter/cloudinary_context.dart';
import 'package:cloudinary_url_gen/cloudinary.dart';
import 'package:flutter/material.dart';
import 'package:food_order_app/core/provider/login_state_provider.dart';
import 'package:food_order_app/presentation/screens/wrapper.dart';
import 'package:provider/provider.dart';

void main() {
  CloudinaryContext.cloudinary = 
        Cloudinary.fromCloudName(cloudName: "dyhjqna2u");
  runApp(ChangeNotifierProvider(
      create: (_) => LoginStateProvider(), child: const MyApp()));
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Flutter Demo',
      theme: ThemeData(
        colorScheme: ColorScheme.fromSeed(seedColor: Colors.deepPurple),
        primaryColorDark: Colors.black,
        primaryColorLight: Colors.white,
        useMaterial3: true,
      ),
      home: const Wrapper(),
      debugShowCheckedModeBanner: false,
    );
  }
}
