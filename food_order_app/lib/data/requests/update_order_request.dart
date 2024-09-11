class UpdateOrderRequest {
  int orderId;
  int orderStatus;

  UpdateOrderRequest({required this.orderId, required this.orderStatus});
  Map<String, dynamic> toJson() {
    return <String, dynamic>{"orderId": orderId, "orderStatus": orderStatus};
  }
}
