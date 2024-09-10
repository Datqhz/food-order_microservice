class User {
  String id;
  String displayName;
  String role;
  DateTime createdDate;
  bool isActive;
  String userName;
  String phoneNumber;

  User({
    required this.id,
    required this.displayName,
    required this.role,
    required this.createdDate,
    required this.isActive,
    required this.userName,
    required this.phoneNumber,
  });

  factory User.fromJson(Map<String, dynamic> json) {
    return User(
        id: json['id'],
        displayName: json['displayName'],
        role: json['role'],
        createdDate: DateTime.parse(json['createdDate']),
        isActive: json['isActive'],
        userName: json['userName'],
        phoneNumber: json['phoneNumber']);
  }
}
