import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:food_order_app/core/global_val.dart';
import 'package:food_order_app/core/snackbar.dart';
import 'package:food_order_app/data/models/food.dart';
import 'package:food_order_app/data/models/order_detail.dart';
import 'package:food_order_app/data/requests/modify_orderd_detail_request.dart';
import 'package:http/http.dart';

class OrderDetailRepository {
  Future<List<OrderDetail>?> getAllOrderDetailByOrderId(
      int orderId, BuildContext context) async {
    Map<String, String> headers = {
      'Content-Type': 'application/json; charset=UTF-8',
      'Authorization': 'Bearer ${GlobalVariable.loginResponse!.accessToken}'
    };
    try {
      var response = await get(
          Uri.parse(
              '${GlobalVariable.requestUrlPrefix}/api/v1/order-detail/get-by-order/$orderId'),
          headers: headers);

      Map<String, dynamic> responseBody = json.decode(response.body);
      var statusCode = responseBody["statusCode"];
      if (statusCode == 200) {
        List<dynamic> foods = responseBody["data"];
        return foods.map((e) => OrderDetail.fromJson(e)).toList();
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

  Future<bool> modifyMultipleOrderDetails(
      List<ModifyOrderdDetailRequest> request, BuildContext context) async {
    Map<String, String> headers = {
      'Content-Type': 'application/json; charset=UTF-8',
      'Authorization': 'Bearer ${GlobalVariable.loginResponse!.accessToken}'
    };

    try {
      var response = await post(
          Uri.parse(
              "${GlobalVariable.requestUrlPrefix}/api/v1/order-detail/modify-multiple"),
          headers: headers,
          body: jsonEncode(request.map((detail) => detail.toJson()).toList()));
      var responseBody = json.decode(response.body);
      var statusCode = responseBody['statusCode'];
      switch (statusCode) {
        case 200:
          return true;
        default:
          showSnackBar(context, responseBody['statusText']);
          print(responseBody['errorMessage']);
          return false;
      }
    } catch (e) {
      print(e.toString());
      return false;
    }
  }
}
