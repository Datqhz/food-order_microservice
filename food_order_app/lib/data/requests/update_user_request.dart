class UpdateUserRequest {
  String id;
  String displayName;
  String phoneNumber;

  UpdateUserRequest(
      {required this.id, required this.displayName, required this.phoneNumber});

  Map<String, dynamic> toJson() {
    return <String, dynamic>{
      "id": id,
      "displayName": displayName,
      "phoneNumber": phoneNumber
    };
  }
}
