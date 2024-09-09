class LoginResponse {
  String accessToken;
  String scope;
  int expired;

  LoginResponse({
    required this.accessToken,
    required this.scope,
    required this.expired,
  });
  factory LoginResponse.fromJson(Map<String, dynamic> json) {
    return LoginResponse(
      accessToken: json["accessToken"],
      scope: json["scope"],
      expired: json["expired"],
    );
  }
}
