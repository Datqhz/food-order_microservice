import 'package:flutter/material.dart';
import 'package:food_order_app/data/models/order_detail.dart';

class OrderDetailNotifier with ChangeNotifier {
  OrderDetail? _detail;

  get orderDetail => _detail;

  void change(OrderDetail? detail){
    _detail = detail;
    notifyListeners();
  }
}
