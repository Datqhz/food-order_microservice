class Order {
  int id;
  DateTime? orderedDate;
  int orderStatus;
  String eaterId;
  String merchantId;

  Order(
      {required this.id,
      this.orderedDate,
      required this.orderStatus,
      required this.eaterId,
      required this.merchantId});

  factory Order.fromJson(Map<String, dynamic> json) {
    return Order(
        id: json['id'],
        orderStatus: json['orderStatus'],
        eaterId: json['eaterId'],
        merchantId: json['merchantId'],
        orderedDate: json['orderedDate'] != null
            ? DateTime.parse(json['orderedDate'])
            : null);
  }
}
