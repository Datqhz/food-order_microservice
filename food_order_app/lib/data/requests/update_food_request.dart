class UpdateFoodRequest {
  int id;
  String name;
  String imageUrl;
  String describe;
  double price;

  UpdateFoodRequest(
      {required this.name,
      required this.imageUrl,
      required this.describe,
      required this.price,
      required this.id});

  Map<String, dynamic> toJson() {
    return <String, dynamic>{
      "id": id,
      "name": name,
      "imageUrl": imageUrl,
      "describe": describe,
      "price": price
    };
  }
}
