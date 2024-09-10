import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:food_order_app/core/global_val.dart';
import 'package:food_order_app/core/snackbar.dart';
import 'package:food_order_app/data/models/order.dart';
import 'package:food_order_app/data/requests/get_order_by_userId_request.dart';
import 'package:http/http.dart';

class OrderRepository {

  Future<Order?> getOrderByEaterAndMerchant(GetOrderByUseridRequest request, BuildContext context) async {
     Map<String, String> headers = {
      'Content-Type': 'application/json; charset=UTF-8',
      'Authorization': 'Bearer ${GlobalVariable.loginResponse!.accessToken}'
    };
    try {
      var response = await get(Uri.parse(
          '${GlobalVariable.requestUrlPrefix}/api/v1/order/get-by-eater-merchant?eaterId=${request.eaterId}&merchantId=${request.merchantId}'),
          headers: headers);

      Map<String, dynamic> responseBody = json.decode(response.body);
      var statusCode = responseBody["statusCode"];
      if (statusCode == 200) {
        return Order.fromJson(responseBody['data']);
      }
      showSnackBar(context, "Get order failed");
      print(responseBody["errorMessage"]);
      return null;
    } catch (e) {
      print(e.toString());
      showSnackBar(context, "Get order failed");
      return null;
    }
  }
}