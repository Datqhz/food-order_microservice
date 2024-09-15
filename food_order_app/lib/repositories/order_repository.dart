import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:food_order_app/core/global_val.dart';
import 'package:food_order_app/core/snackbar.dart';
import 'package:food_order_app/data/models/order.dart';
import 'package:food_order_app/data/requests/get_order_by_userId_request.dart';
import 'package:food_order_app/data/requests/update_order_request.dart';
import 'package:food_order_app/data/requests/update_order_with_shipping_info_request.dart';
import 'package:http/http.dart';

class OrderRepository {
  Future<Order?> getOrderByEaterAndMerchant(
      GetOrderByUseridRequest request, BuildContext context) async {
    Map<String, String> headers = {
      'Content-Type': 'application/json; charset=UTF-8',
      'Authorization': 'Bearer ${GlobalVariable.loginResponse!.accessToken}'
    };
    try {
      var response = await get(
          Uri.parse(
              '${GlobalVariable.requestUrlPrefix}/api/v1/order/get-by-eater-merchant?EaterId=${request.eaterId}&MerchantId=${request.merchantId}'),
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

  Future<bool> update(UpdateOrderRequest request, BuildContext context) async {
    Map<String, String> headers = {
      'Content-Type': 'application/json; charset=UTF-8',
      'Authorization': 'Bearer ${GlobalVariable.loginResponse!.accessToken}'
    };
    try {
      var response = await put(
          Uri.parse('${GlobalVariable.requestUrlPrefix}/api/v1/order'),
          headers: headers,
          body: jsonEncode(request.toJson()));

      Map<String, dynamic> responseBody = json.decode(response.body);
      var statusCode = responseBody["statusCode"];
      if (statusCode == 200) {
        return true;
      }
      showSnackBar(context, "Update order failed");
      print(responseBody["errorMessage"]);
      return false;
    } catch (e) {
      print(e.toString());
      showSnackBar(context, "Update order failed");
      return false;
    }
  }

  Future<bool> updateWithShippingInfo(
      UpdateOrderWithShippingInfoRequest request, BuildContext context) async {
    Map<String, String> headers = {
      'Content-Type': 'application/json; charset=UTF-8',
      'Authorization': 'Bearer ${GlobalVariable.loginResponse!.accessToken}'
    };
    try {
      var response = await put(
          Uri.parse(
              '${GlobalVariable.requestUrlPrefix}/api/v1/order/update-shipping-info'),
          headers: headers,
          body: jsonEncode(request.toJson()));

      Map<String, dynamic> responseBody = json.decode(response.body);
      var statusCode = responseBody["statusCode"];
      if (statusCode == 200) {
        return true;
      }
      showSnackBar(context, "Update order failed");
      print(responseBody["errorMessage"]);
      return false;
    } catch (e) {
      print(e.toString());
      showSnackBar(context, "Update order failed");
      return false;
    }
  }

  Future<List<Order>?> getAllOrdersByUserId(
      GetOrderByUseridRequest request, BuildContext context) async {
    Map<String, String> headers = {
      'Content-Type': 'application/json; charset=UTF-8',
      'Authorization': 'Bearer ${GlobalVariable.loginResponse!.accessToken}'
    };
    try {
      var requestUrl =
          '${GlobalVariable.requestUrlPrefix}/api/v1/order/get-by-user?SortBy=${request.sortBy}&OrderStatus=${request.orderStatus}&';
      if (request.eaterId != null) {
        requestUrl += "EaterId=${request.eaterId}";
      } else {
        requestUrl += "MerchantId=${request.merchantId}";
      }
      var response = await get(Uri.parse(requestUrl), headers: headers);

      Map<String, dynamic> responseBody = json.decode(response.body);
      var statusCode = responseBody["statusCode"];
      if (statusCode == 200) {
        List<dynamic> orders = responseBody["data"];
        return orders.map((e) => Order.fromJson(e)).toList();
      }
      showSnackBar(context, "Get orders failed");
      print(responseBody["errorMessage"]);
      return null;
    } catch (e) {
      print(e.toString());
      showSnackBar(context, "Get orders failed");
      return null;
    }
  }

  Future<Order?> getOrderById(int orderId, BuildContext context) async {
    Map<String, String> headers = {
      'Content-Type': 'application/json; charset=UTF-8',
      'Authorization': 'Bearer ${GlobalVariable.loginResponse!.accessToken}'
    };
    try {
      var requestUrl =
          '${GlobalVariable.requestUrlPrefix}/api/v1/order/$orderId';

      var response = await get(Uri.parse(requestUrl), headers: headers);

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
