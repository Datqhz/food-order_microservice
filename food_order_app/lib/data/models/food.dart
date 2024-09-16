class Food {
  int id;
  String name;
  String imageUrl;
  String describe;
  double? price;
  String? userId;

  Food(
      {required this.id,
      required this.name,
      required this.imageUrl,
      required this.describe,
      required this.price,
      required this.userId});

  factory Food.fromJson(Map<String, dynamic> json) {
    return Food(
        id: json['id'],
        name: json['name'],
        imageUrl: json['imageUrl'],
        describe: json['describe'],
        price: json['price'] != null ? json['price'] / 1.0 : null,
        userId: json['userId'] ?? json['userId']);
  }
}
