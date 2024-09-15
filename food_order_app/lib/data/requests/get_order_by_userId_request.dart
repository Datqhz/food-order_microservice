class GetOrderByUseridRequest {
  String? eaterId;
  String? merchantId;
  int orderStatus;
  int sortBy;
  GetOrderByUseridRequest({this.eaterId, this.merchantId, required this.orderStatus, required this.sortBy});
}