import 'package:food_order_app/data/models/food.dart';
import 'package:food_order_app/data/models/paging.dart';

class FoodsWithPaging {
  List<Food> foods;
  Paging paging;

  FoodsWithPaging({required this.foods, required this.paging});

  factory FoodsWithPaging.fromJson(Map<String, dynamic> json) {
    List<dynamic> foods = json['data'];
    return FoodsWithPaging(
        foods: foods.map((e) => Food.fromJson(e)).toList(),
        paging: Paging.fromJson(json['paging']));
  }
  FoodsWithPaging clone() {
    return FoodsWithPaging(foods: foods, paging: paging);
  }
}
