import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:food_order_app/core/global_val.dart';
import 'package:food_order_app/core/snackbar.dart';
import 'package:food_order_app/data/models/role.dart';
import 'package:http/http.dart';

class RoleRepository {
  Future<List<Role>?> getAllRole(BuildContext context) async {
      Map<String, String> headers = {
      'Content-Type': 'application/json; charset=UTF-8',
    };
    try {
      var response = await get(Uri.parse("${GlobalVariable.requestUrlPrefix}/api/v1/role"), headers: headers);
      var responseBody = json.decode(response.body);
      var statusCode = responseBody['statusCode'];
      if(statusCode == 200){
        List<dynamic> roles = responseBody['data'];
        return roles.map((e) => Role.fromJson(e)).toList();
      }
      showSnackBar(context, responseBody['errorMessage']);
      return null;
    }catch (e){
      showSnackBar(context, "Some error arise in process");
      return null;
    }
  }
}