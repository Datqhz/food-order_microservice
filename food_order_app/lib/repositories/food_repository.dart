import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:food_order_app/core/global_val.dart';
import 'package:food_order_app/core/snackbar.dart';
import 'package:food_order_app/data/models/food.dart';
import 'package:http/http.dart';

class FoodRepository {
  Future<List<Food>?> getAllFoodsByMerchantId(
      String merchantId, BuildContext context) async {
    Map<String, String> headers = {
      'Content-Type': 'application/json; charset=UTF-8',
      'Authorization': 'Bearer ${GlobalVariable.loginResponse!.accessToken}'
    };
    try {
      var response = await get(Uri.parse(
          '${GlobalVariable.requestUrlPrefix}/api/v1/food/get-by-user?userId=$merchantId'),
          headers: headers);

      Map<String, dynamic> responseBody = json.decode(response.body);
      var statusCode = responseBody["statusCode"];
      if (statusCode == 200) {
        List<dynamic> foods = responseBody["data"];
        return foods.map((e) => Food.fromJson(e)).toList();
      }
      showSnackBar(context, "Get foods by mechant failed");
      print(responseBody["errorMessage"]);
      return null;
    } catch (e) {
      print(e.toString());
      showSnackBar(context, "Get foods by merchant failed");
      return null;
    }
  }
}
