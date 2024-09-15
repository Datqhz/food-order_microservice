class UpdateOrderWithShippingInfoRequest {
  int orderId;
  String shippingAddress;
  double shippingFee;
  String shippingPhoneNumber;

  UpdateOrderWithShippingInfoRequest({required this.orderId, required this.shippingAddress, required this.shippingFee, required this.shippingPhoneNumber});
  Map<String, dynamic> toJson() {
    return <String, dynamic>{"orderId": orderId,
  "shippingFee": shippingFee,
  "shippingAddress": shippingAddress,
  "shippingPhoneNumber": shippingPhoneNumber};
  }
}
