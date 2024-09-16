import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:food_order_app/core/global_val.dart';
import 'package:food_order_app/core/snackbar.dart';
import 'package:food_order_app/data/models/dto/food_with_paging.dart';
import 'package:food_order_app/data/models/food.dart';
import 'package:food_order_app/data/requests/create_food_request.dart';
import 'package:food_order_app/data/requests/update_food_request.dart';
import 'package:http/http.dart';

class FoodRepository {
  Future<List<Food>?> getAllFoodsByMerchantId(
      String merchantId, int sortBy, BuildContext context) async {
    Map<String, String> headers = {
      'Content-Type': 'application/json; charset=UTF-8',
      'Authorization': 'Bearer ${GlobalVariable.loginResponse!.accessToken}'
    };
    try {
      var response = await get(
          Uri.parse(
              '${GlobalVariable.requestUrlPrefix}/api/v1/food/get-by-user?UserId=$merchantId&SortBy=$sortBy'),
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

  Future<bool> create(CreateFoodRequest request, BuildContext context) async {
    Map<String, String> headers = {
      'Content-Type': 'application/json; charset=UTF-8',
      'Authorization': 'Bearer ${GlobalVariable.loginResponse!.accessToken}'
    };
    try {
      var response = await post(
          Uri.parse('${GlobalVariable.requestUrlPrefix}/api/v1/food'),
          headers: headers,
          body: jsonEncode(request.toJson()));

      Map<String, dynamic> responseBody = json.decode(response.body);
      var statusCode = responseBody["statusCode"];
      if (statusCode == 201) {
        return true;
      }
      showSnackBar(context, "Update failed");
      print(responseBody["errorMessage"]);
      return false;
    } catch (e) {
      print(e.toString());
      showSnackBar(context, "Update failed");
      return false;
    }
  }

  Future<bool> update(UpdateFoodRequest request, BuildContext context) async {
    Map<String, String> headers = {
      'Content-Type': 'application/json; charset=UTF-8',
      'Authorization': 'Bearer ${GlobalVariable.loginResponse!.accessToken}'
    };
    try {
      var response = await put(
          Uri.parse('${GlobalVariable.requestUrlPrefix}/api/v1/food'),
          headers: headers,
          body: jsonEncode(request.toJson()));

      Map<String, dynamic> responseBody = json.decode(response.body);
      var statusCode = responseBody["statusCode"];
      if (statusCode == 200) {
        return true;
      }
      showSnackBar(context, "Update failed");
      print(responseBody["errorMessage"]);
      return false;
    } catch (e) {
      print(e.toString());
      showSnackBar(context, "Update failed");
      return false;
    }
  }

  Future<bool> deleteFood(int id, BuildContext context) async {
    Map<String, String> headers = {
      'Content-Type': 'application/json; charset=UTF-8',
      'Authorization': 'Bearer ${GlobalVariable.loginResponse!.accessToken}'
    };
    try {
      var response = await delete(
          Uri.parse('${GlobalVariable.requestUrlPrefix}/api/v1/food/$id'),
          headers: headers);

      Map<String, dynamic> responseBody = json.decode(response.body);
      var statusCode = responseBody["statusCode"];
      if (statusCode == 200) {
        return true;
      }
      showSnackBar(context, "Delete failed");
      print(responseBody["errorMessage"]);
      return false;
    } catch (e) {
      print(e.toString());
      showSnackBar(context, "Delete failed");
      return false;
    }
  }
   Future<FoodsWithPaging?> searchFoodsByName(
      String keyword, int page, BuildContext context) async {
    Map<String, String> headers = {
      'Content-Type': 'application/json; charset=UTF-8',
      'Authorization': 'Bearer ${GlobalVariable.loginResponse!.accessToken}'
    };
    try {
      var response = await get(
          Uri.parse(
              '${GlobalVariable.requestUrlPrefix}/api/v1/food/search-by-name?Keyword=$keyword&Page=$page&MaxPerPage=1'),
          headers: headers);

      Map<String, dynamic> responseBody = json.decode(response.body);
      var statusCode = responseBody["statusCode"];
      if (statusCode == 200) {
        return FoodsWithPaging.fromJson(responseBody);
      }
      showSnackBar(context, "Get foods failed");
      print(responseBody["errorMessage"]);
      return null;
    } catch (e) {
      print(e.toString());
      showSnackBar(context, "Get foods failed");
      return null;
    }
  }
}
