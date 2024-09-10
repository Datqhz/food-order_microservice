class Role {
  String id;
  String roleName;

  Role({required this.id, required this.roleName});

  factory Role.fromJson(Map<String, dynamic> json) {
    return Role(
      id: json["id"],
      roleName: json["roleName"],
    );
  }
}
