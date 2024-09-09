import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:food_order_app/core/global_val.dart';
import 'package:food_order_app/core/jwt_decode.dart';
import 'package:food_order_app/core/snackbar.dart';
import 'package:food_order_app/data/models/user.dart';
import 'package:food_order_app/data/requests/login_request.dart';
import 'package:food_order_app/data/requests/update_user_request.dart';
import 'package:food_order_app/data/responses/login_response.dart';
import 'package:http/http.dart';

class AuthRepository {
  Future<User?> GetUserInfoById(
      LoginRequest request, BuildContext context) async {
    Map<String, String> headers = {
      'Content-Type': 'application/json; charset=UTF-8',
    };
    try {
      var response = await get(
          Uri.parse('${GlobalVariable.requestUrlPrefix}/api/v1/customer/${JWTHelper.getCurrentUid(GlobalVariable.loginResponse.accessToken)}'),
          headers: headers,);

      Map<String, dynamic> responseBody = json.decode(response.body);
      var statusCode = responseBody["statusCode"];
      if (statusCode == 200) {
        return User.fromJson(responseBody['data']);
      } else if (statusCode == 404) {
        showSnackBar(context, "User doesn't exits");
      }
      print(responseBody["errors"]);
      return null;
    } catch (e) {
      print(e.toString());
      showSnackBar(context, "Get user infomation failed");
      return null;
    }
  }

  Future<List<User>?> GetAllMerchants(
      LoginRequest request, BuildContext context) async {
    Map<String, String> headers = {
      'Content-Type': 'application/json; charset=UTF-8',
    };
    try {
      var response = await get(
          Uri.parse('${GlobalVariable.requestUrlPrefix}/api/v1/customer/all-merchants'),
          headers: headers,);

      Map<String, dynamic> responseBody = json.decode(response.body);
      var statusCode = responseBody["statusCode"];
      if (statusCode == 200) {
        List<dynamic> merchants = responseBody["data"];
        return merchants.map((e) => User.fromJson(e)).toList();
      }
      showSnackBar(context, responseBody["statusText"]);
      print(responseBody["errorMessage"]);
      return null;
    } catch (e) {
      print(e.toString());
      showSnackBar(context, "Get all merchants failed");
      return null;
    }
  }

  Future<List<User>?> GetAllUsers(
      LoginRequest request, BuildContext context) async {
    Map<String, String> headers = {
      'Content-Type': 'application/json; charset=UTF-8',
    };
    try {
      var response = await get(
          Uri.parse('${GlobalVariable.requestUrlPrefix}/api/v1/customer'),
          headers: headers,);

      Map<String, dynamic> responseBody = json.decode(response.body);
      var statusCode = responseBody["statusCode"];
      if (statusCode == 200) {
        List<dynamic> merchants = responseBody["data"];
        return merchants.map((e) => User.fromJson(e)).toList();
      }
      showSnackBar(context, responseBody["statusText"]);
      print(responseBody["errorMessage"]);
      return null;
    } catch (e) {
      print(e.toString());
      showSnackBar(context, "Get all merchants failed");
      return null;
    }
  }

  Future<LoginResponse?> update(
      UpdateUserRequest request, BuildContext context) async {
    Map<String, String> headers = {
      'Content-Type': 'application/json; charset=UTF-8',
    };
    try {
      var response = await put(
          Uri.parse('${GlobalVariable.requestUrlPrefix}/api/v1/customer'),
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
