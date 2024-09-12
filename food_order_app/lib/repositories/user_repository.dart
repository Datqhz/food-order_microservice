import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:food_order_app/core/global_val.dart';
import 'package:food_order_app/core/jwt_decode.dart';
import 'package:food_order_app/core/snackbar.dart';
import 'package:food_order_app/data/models/dto/merchant_with_paging.dart';
import 'package:food_order_app/data/models/user.dart';
import 'package:food_order_app/data/requests/update_user_request.dart';
import 'package:http/http.dart';
import 'package:logger/logger.dart';

class UserRepository {
  Future<User?> getUserInfoById(BuildContext context) async {
    Map<String, String> headers = {
      'Content-Type': 'application/json; charset=UTF-8',
      'Authorization': 'Bearer ${GlobalVariable.loginResponse!.accessToken}'
    };
    try {
      var response = await get(
        Uri.parse(
            '${GlobalVariable.requestUrlPrefix}/api/v1/customer/${JWTHelper.getCurrentUid(GlobalVariable.loginResponse!.accessToken)}'),
        headers: headers,
      );

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

  Future<List<User>?> getAllMerchants(BuildContext context, {int? page, int? maxPerPage}) async {
    Map<String, String> headers = {
      'Content-Type': 'application/json; charset=UTF-8',
      'Authorization': 'Bearer ${GlobalVariable.loginResponse!.accessToken}'
    };
    try {
      var uri = '${GlobalVariable.requestUrlPrefix}/api/v1/customer/all-merchants';
      if(page!= null && maxPerPage!= null){
        uri += '?Page=$page&MaxPerPage=$maxPerPage';
      }
      var response = await get(
        Uri.parse(
            uri),
        headers: headers,
      );

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

  Future<List<User>?> getAllUsers(BuildContext context) async {
    Map<String, String> headers = {
      'Content-Type': 'application/json; charset=UTF-8',
      'Authorization': 'Bearer ${GlobalVariable.loginResponse!.accessToken}'
    };
    try {
      var response = await get(
        Uri.parse('${GlobalVariable.requestUrlPrefix}/api/v1/customer'),
        headers: headers,
      );

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

  Future<bool> update(
    UpdateUserRequest request,
    BuildContext context,
  ) async {
    final logger = Logger();

    Map<String, String> headers = {
      'Content-Type': 'application/json; charset=UTF-8',
      'Authorization': 'Bearer ${GlobalVariable.loginResponse!.accessToken}'
    };
    try {
      var response = await put(
          Uri.parse('${GlobalVariable.requestUrlPrefix}/api/v1/customer'),
          headers: headers,
          body: jsonEncode(request.toJson()));

      Map<String, dynamic> responseBody = json.decode(response.body);
      var statusCode = responseBody["statusCode"];
      switch (statusCode) {
        case 200:
          GlobalVariable.currentUser?.displayName = request.displayName;
          GlobalVariable.currentUser?.phoneNumber = request.phoneNumber;
          return true;
        case 400:
        case 403:
        case 404:
          showSnackBar(context, responseBody['statusText']);
          return false;
        case 500:
          showSnackBar(context, "Can't update infomation");
          logger.e(responseBody['errorMessage']);
          return false;
        default:
          return false;
      }
    } catch (e) {
      logger.e(e.toString());
      showSnackBar(context, "Some error arise in process");
      return false;
    }
  }

  Future<MerchantsWithPaging?> searchMerchantsByName(String keyword, BuildContext context, {int? page, int? maxPerPage}) async {
    Map<String, String> headers = {
      'Content-Type': 'application/json; charset=UTF-8',
      'Authorization': 'Bearer ${GlobalVariable.loginResponse!.accessToken}'
    };
    try {
      var uri = '${GlobalVariable.requestUrlPrefix}/api/v1/customer/search-by-name?Keyword=$keyword';
      if(page!= null && maxPerPage!= null){
        uri += '&Page=$page&MaxPerPage=$maxPerPage';
      }
      var response = await get(
        Uri.parse(
            uri),
        headers: headers,
      );

      Map<String, dynamic> responseBody = json.decode(response.body);
      var statusCode = responseBody["statusCode"];
      if (statusCode == 200) {
        return MerchantsWithPaging.fromJson(responseBody);
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
}
