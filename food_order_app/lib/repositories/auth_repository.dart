import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:food_order_app/core/global_val.dart';
import 'package:food_order_app/core/snackbar.dart';
import 'package:food_order_app/data/requests/login_request.dart';
import 'package:food_order_app/data/requests/register_request.dart';
import 'package:food_order_app/data/responses/login_response.dart';
import 'package:http/http.dart';

class AuthRepository {
  Future<bool> login(LoginRequest request, BuildContext context) async {
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
        return true;
      } else if (statusCode == 400) {
        showSnackBar(context, "Invalid infomation");
      } else if (statusCode == 404) {
        showSnackBar(context, "Username doesn't exits");
      } else {
        showSnackBar(context, "Can't authentication");
      }
      print(responseBody["errors"]);
      return false;
    } catch (e) {
      print(e.toString());
      showSnackBar(context, "Can't authentication");
      return false;
    }
  }

  Future<bool> register(RegisterRequest request, BuildContext context) async {
    Map<String, String> headers = {
      'Content-Type': 'application/json; charset=UTF-8',
    };
    try {
      var response = await post(
          Uri.parse('${GlobalVariable.requestUrlPrefix}/api/v1/auth/register'),
          headers: headers,
          body: jsonEncode(request.toJson()));

      Map<String, dynamic> responseBody = json.decode(response.body);
      var statusCode = responseBody["statusCode"];
      switch (statusCode) {
        case 201:
          return true;
        case 400:
          showSnackBar(context, responseBody['statusText']);
          return false;
      }
      print(responseBody["errors"]);
      return false;
    } catch (e) {
      print(e.toString());
      showSnackBar(context, "Can't register");
      return false;
    }
  }
}
