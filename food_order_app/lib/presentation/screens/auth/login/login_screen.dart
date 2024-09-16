import 'package:flutter/material.dart';
import 'package:food_order_app/core/constant.dart';
import 'package:food_order_app/core/global_val.dart';
import 'package:food_order_app/core/jwt_decode.dart';
import 'package:food_order_app/core/provider/login_state_provider.dart';
import 'package:food_order_app/data/requests/login_request.dart';
import 'package:food_order_app/presentation/screens/auth/register/register_screen.dart';
import 'package:food_order_app/repositories/auth_repository.dart';
import 'package:food_order_app/repositories/user_repository.dart';
import 'package:provider/provider.dart';

class LoginScreen extends StatefulWidget {
  const LoginScreen({super.key});

  @override
  State<LoginScreen> createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  final _formKey = GlobalKey<FormState>();
  final _userNameController = TextEditingController();
  final _passwordController = TextEditingController();
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
    _userNameController.dispose();
    _passwordController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Theme.of(context).primaryColorLight,
      appBar: AppBar(
          backgroundColor: Theme.of(context).primaryColorLight,
          title: Row(
            children: [
              Text(
                "Sign in",
                style: TextStyle(
                    fontSize: Constant.font_size_2,
                    fontWeight: Constant.font_weight_nomal,
                    color: Theme.of(context).primaryColorDark),
              ),
              const Expanded(child: SizedBox()),
              GestureDetector(
                onTap: () => Navigator.pushReplacement(
                    context,
                    MaterialPageRoute(
                        builder: (context) => const RegisterScreen())),
                child: Text(
                  "Sign up",
                  style: TextStyle(
                      fontSize: Constant.font_size_3,
                      fontWeight: Constant.font_weight_heading2,
                      color: Theme.of(context).primaryColorDark),
                ),
              )
            ],
          )),
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
                    Text(
                      "Sign in",
                      style: TextStyle(
                          color: Theme.of(context).primaryColorDark,
                          fontSize: Constant.font_size_heading_1,
                          fontWeight: Constant.font_weight_heading1),
                    ),
                    SizedBox(
                      height: Constant.dimension_12,
                    ),
                    TextFormField(
                      controller: _userNameController,
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
                        hintText: "Username",
                        hintStyle: TextStyle(
                            color: Constant.colour_grey,
                            fontSize: Constant.font_size_2,
                            fontWeight: Constant.font_weight_nomal),
                        labelText: "Username",
                        labelStyle: TextStyle(
                            color: Constant.colour_grey,
                            fontWeight: Constant.font_weight_nomal),
                      ),
                      validator: (value) {
                        if (value!.isEmpty) {
                          return "Username is required";
                        } else if (value.trim().length > 100 &&
                            value.trim().length < 5) {
                          return "Username must be between 5 and 50 characters";
                        }
                        return null;
                      },
                    ),
                    SizedBox(
                      height: Constant.dimension_12,
                    ),
                    TextFormField(
                      controller: _passwordController,
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
                        hintText: "Password",
                        hintStyle: TextStyle(
                            color: Constant.colour_grey,
                            fontSize: Constant.font_size_2,
                            fontWeight: Constant.font_weight_nomal),
                        labelText: "Password",
                        labelStyle: TextStyle(
                            color: Constant.colour_grey,
                            fontWeight: Constant.font_weight_nomal),
                      ),
                      validator: (value) {
                        if (value!.isEmpty) {
                          return "Password is required";
                        } else if (value.trim().length > 16 &&
                            value.trim().length < 8) {
                          return "Password must be between 8 and 16 characters";
                        }
                        return null;
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
                          final username = _userNameController.text.trim();
                          final password = _passwordController.text.trim();
                          var request = LoginRequest(
                              username: username, password: password);
                          var loginResult =
                              await repository.login(request, context);
                          if (loginResult) {
                            GlobalVariable.currentUser = await UserRepository()
                                .getUserInfoById(
                                    JWTHelper.getCurrentUid(GlobalVariable
                                        .loginResponse!.accessToken),
                                    context);
                            Provider.of<LoginStateProvider>(context,
                                    listen: false)
                                .login();
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
