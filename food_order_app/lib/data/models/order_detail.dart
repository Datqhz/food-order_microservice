import 'package:food_order_app/data/models/food.dart';

class OrderDetail {
  int id;
  int orderId;
  Food food;
  double price;
  int quantity;

  OrderDetail(
      {required this.id,
      required this.orderId,
      required this.food,
      required this.price,
      required this.quantity});

  factory OrderDetail.fromJson(Map<String, dynamic> json) {
    return OrderDetail(
        id: json['id'],
        orderId: json['orderId'],
        food: Food.fromJson(json['food']),
        price: json['price'] / 1.0,
        quantity: json['quantity']);
  }
}
