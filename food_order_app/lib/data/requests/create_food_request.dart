class CreateFoodRequest {
  String name;
  String imageUrl;
  String describe;
  double price;
  String userId;

  CreateFoodRequest(
      {required this.name,
      required this.imageUrl,
      required this.describe,
      required this.price,
      required this.userId});

  Map<String, dynamic> toJson() {
    return <String, dynamic>{
      "name": name,
      "imageUrl": imageUrl,
      "describe": describe,
      "price": price,
      "userId": userId
    };
  }
}
