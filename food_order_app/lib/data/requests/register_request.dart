class RegisterRequest {
  String displayName;
  String userName;
  String password;
  String clientId = "Mobile";
  String role;
  String phoneNumber;

  RegisterRequest(
      {required this.displayName,
      required this.userName,
      required this.password,
      required this.role,
      required this.phoneNumber});

  Map<String, dynamic> toJson(){
    return <String, dynamic> {
      "displayname" : displayName,
      "username": userName,
      "password": password,
      "clientId": clientId,
      "role": role,
      "phoneNumber": phoneNumber
    };
  }
}
