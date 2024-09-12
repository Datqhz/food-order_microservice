import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:food_order_app/core/constant.dart';
import 'package:food_order_app/core/utils/string_format.dart';
import 'package:food_order_app/data/models/role.dart';
import 'package:food_order_app/data/requests/register_request.dart';
import 'package:food_order_app/presentation/screens/auth/register/register_result_screen.dart';
import 'package:food_order_app/repositories/auth_repository.dart';
import 'package:food_order_app/repositories/role_repository.dart';

class RegisterScreen extends StatefulWidget {
  const RegisterScreen({super.key});

  @override
  State<RegisterScreen> createState() => _RegisterScreenState();
}

class _RegisterScreenState extends State<RegisterScreen> {
  final _formKey = GlobalKey<FormState>();
  final _displayNameController = TextEditingController();
  final _phoneController = TextEditingController();
  final _userNameController = TextEditingController();
  final _passwordController = TextEditingController();
  final ValueNotifier<String?> _roleSelected = ValueNotifier(null);
  List<Role>? _roles = [];

  Future<void> _fetchRoleData() async {
    _roles = await RoleRepository().getAllRole(context);
    setState(() {});
  }

  @override
  void initState() {
    // TODO: implement initState
    super.initState();
    _fetchRoleData();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Theme.of(context).primaryColorLight,
      appBar: AppBar(
        backgroundColor: Theme.of(context).primaryColorLight,
        title: Text(
          "Sign up",
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
                    Text(
                      "Sign up",
                      style: TextStyle(
                          color: Theme.of(context).primaryColorDark,
                          fontSize: Constant.font_size_heading_1,
                          fontWeight: Constant.font_weight_heading1),
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
                    ),
                    DropdownButtonFormField<String>(
                      dropdownColor: Colors.white,
                      style: const TextStyle(
                        color: Color.fromRGBO(49, 49, 49, 1),
                      ),
                      icon: const Icon(
                        CupertinoIcons.chevron_down,
                        size: 14,
                        color: Color.fromRGBO(118, 118, 118, 1),
                      ),
                      decoration: const InputDecoration(
                        border: UnderlineInputBorder(
                          borderSide: BorderSide(
                              color: Color.fromRGBO(118, 118, 118, 1),
                              width: 1), // Màu viền khi không được chọn
                        ),
                        errorBorder: UnderlineInputBorder(
                          borderSide: BorderSide(
                            color: Color.fromRGBO(182, 0, 0, 1),
                            width: 1,
                          ),
                        ),
                        focusedErrorBorder: UnderlineInputBorder(
                          borderSide: BorderSide(
                            color: Color.fromRGBO(182, 0, 0, 1),
                            width: 1,
                          ),
                        ),
                        errorStyle:
                            TextStyle(color: Color.fromRGBO(182, 0, 0, 1)),
                      ),
                      value: _roleSelected.value != null
                          ? capitalize(_roleSelected.value!)
                          : null,
                      items: _roles!
                          .map((item) => DropdownMenuItem<String>(
                                value: item.roleName,
                                child: Text(
                                  capitalize(item.roleName),
                                ),
                              ))
                          .toList(),
                      onChanged: (value) {
                        _roleSelected.value = value;
                      },
                      validator: (value) {
                        if (value == null || value.isEmpty) {
                          return 'Please select an option';
                        }
                        return null;
                      },
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
                        final userName = _userNameController.text.trim();
                        final password = _passwordController.text.trim();
                        Role? role;
                        for (var e in _roles!) {
                          if (e.roleName == _roleSelected.value) {
                            role = e;
                          }
                        }
                        var result = await AuthRepository().register(
                            RegisterRequest(
                                displayName: displayName,
                                userName: userName,
                                password: password,
                                role: role!.roleName,
                                phoneNumber: phone),
                            context);
                        if (result) {
                          Navigator.pushReplacement(
                              context,
                              MaterialPageRoute(
                                  builder: (context) =>
                                      const RegisterResultScreen()));
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
