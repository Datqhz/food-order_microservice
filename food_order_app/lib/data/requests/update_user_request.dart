class UpdateUserRequest {
  String id;
  String displayName;
  String phoneNumber;
  String avatar;
  UpdateUserRequest(
      {required this.id, required this.displayName, required this.phoneNumber, required this.avatar});

  Map<String, dynamic> toJson() {
    return <String, dynamic>{
      "id": id,
      "displayName": displayName,
      "phoneNumber": phoneNumber,
      "avatar": avatar
    };
  }
}
