class ModifyOrderdDetailRequest {
  int? orderDetailId;
  int? orderId;
  int? foodId;
  int quantity;
  double price;
  int feature;

  ModifyOrderdDetailRequest(
      {this.orderDetailId,
      this.orderId,
      this.foodId,
      required this.quantity,
      required this.price,
      required this.feature});
  Map<String, dynamic> toJson() {
    return <String, dynamic>{
      "orderDetailId": orderDetailId,
      "orderId": orderId,
      "foodId": foodId,
      "quantity": quantity,
      "price": price,
      "feature": feature
    };
  }
}
