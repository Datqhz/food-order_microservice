import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:food_order_app/core/global_val.dart';
import 'package:food_order_app/core/snackbar.dart';
import 'package:food_order_app/data/requests/login_request.dart';
import 'package:food_order_app/data/responses/login_response.dart';
import 'package:http/http.dart';

class AuthRepository {
  Future<LoginResponse?> login(
      LoginRequest request, BuildContext context) async {
    Map<String, String> headers = {
      'Content-Type': 'application/json; charset=UTF-8',
    };
    try {
      var response = await post(
          Uri.parse('${GlobalVariable.requestUrlPrefix}/api/v1/auth/login'),
          headers: headers,
          body: jsonEncode(request.toJson()));

      Map<String, dynamic> responseBody = json.decode(response.body);
      var statusCode = responseBody["statusCode"];
      if (statusCode == 200) {
        GlobalVariable.loginResponse =
            LoginResponse.fromJson(responseBody['data']);
        return LoginResponse.fromJson(responseBody['data']);
      } else if (statusCode == 400) {
        showSnackBar(context, "Invalid infomation");
      } else if (statusCode == 404) {
        showSnackBar(context, "Username doesn't exits");
      } else {
        showSnackBar(context, "Can't authentication");
      }
      print(responseBody["errors"]);
      return null;
    } catch (e) {
      print(e.toString());
      showSnackBar(context, "Can't authentication");
      return null;
    }
  }
}
