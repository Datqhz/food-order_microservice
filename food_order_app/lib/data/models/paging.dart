class Paging {
  int totalItems;
  int totalPages;
  int pageNumber;
  int maxPerPage;

  Paging(
      {required this.totalItems,
      required this.totalPages,
      required this.pageNumber,
      required this.maxPerPage});

  factory Paging.fromJson(Map<String, dynamic> json) {
    return Paging(
        totalItems: json['totalItems'],
        totalPages: json['totalPages'],
        pageNumber: json['pageNumber'],
        maxPerPage: json['maxPerPage']);
  }
}
