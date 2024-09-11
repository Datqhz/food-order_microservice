class BriefUser {
  String userId;
  String displayName;
  String phoneNumber;

  BriefUser(
      {required this.userId,
      required this.displayName,
      required this.phoneNumber});

  factory BriefUser.fromJson(Map<String, dynamic> json) {
    return BriefUser(
      userId: json['id'],
      displayName: json['displayName'],
      phoneNumber: json['phoneNumber'],
    );
  }
}
