class UpdateOrderRequest {
  int orderId;
  bool? cancellation;

  UpdateOrderRequest({required this.orderId, this.cancellation});
  Map<String, dynamic> toJson() {
    return <String, dynamic>{"orderId": orderId, "cancellation": cancellation};
  }
}
