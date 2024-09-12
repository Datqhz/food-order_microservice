import 'dart:async';

import 'package:food_order_app/data/models/order.dart';

class ChangeStream{
  final StreamController _controller = StreamController.broadcast();
  late Order order;
  Stream get stream => _controller.stream;
  
  ChangeStream(order);
  void notifyChange(){
    _controller.add(null);
  }
  void dispose(){
    _controller.close();
  }
}