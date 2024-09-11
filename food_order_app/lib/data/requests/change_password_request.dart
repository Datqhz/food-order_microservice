class ChangePasswordRequest {
  String userId;
  String oldPassword;
  String newPassword;

  ChangePasswordRequest(
      {required this.userId,
      required this.oldPassword,
      required this.newPassword});

  Map<String, dynamic> toJson() {
    return <String, dynamic>{
      "userId": userId,
      "newPassword": newPassword,
      "oldPassword": oldPassword
    };
  }
}
