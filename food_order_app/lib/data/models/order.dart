import 'package:food_order_app/data/models/brief_user.dart';

class Order {
  int id;
  DateTime? orderedDate;
  int orderStatus;
  String? eaterId;
  String? merchantId;
  BriefUser? eater;
  BriefUser? merchant;
  Order(
      {required this.id,
      this.orderedDate,
      required this.orderStatus,
      this.eaterId,
      this.merchantId,
      this.merchant,
      this.eater});

  factory Order.fromJson(Map<String, dynamic> json) {
    return Order(
        id: json['id'],
        orderStatus: json['orderStatus'],
        eaterId: json['eaterId'],
        merchantId: json['merchantId'],
        orderedDate: json['orderedDate'] != null
            ? DateTime.parse(json['orderedDate'])
            : null,
        eater: json['eater'] != null ? BriefUser.fromJson(json['eater']) : null,
        merchant: json['merchant'] != null
            ? BriefUser.fromJson(json['merchant'])
            : null);
  }
}
