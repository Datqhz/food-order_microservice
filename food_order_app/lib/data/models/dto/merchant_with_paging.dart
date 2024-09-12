import 'package:food_order_app/data/models/paging.dart';
import 'package:food_order_app/data/models/user.dart';

class MerchantsWithPaging {
  List<User> users;
  Paging paging;

  MerchantsWithPaging({required this.users, required this.paging});

  factory MerchantsWithPaging.fromJson(Map<String, dynamic> json) {
    List<dynamic> merchants = json['data'];
    return MerchantsWithPaging(
        users: merchants.map((e) => User.fromJson(e)).toList(),
        paging: Paging.fromJson(json['paging']));
  }
  MerchantsWithPaging clone() {
    return MerchantsWithPaging(users: users, paging: paging);
  }
}
