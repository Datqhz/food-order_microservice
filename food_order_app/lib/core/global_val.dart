import 'package:food_order_app/data/models/user.dart';
import 'package:food_order_app/data/responses/login_response.dart';

class GlobalVariable {
  static const String requestUrlPrefix = "http://192.168.100.55:5084";
  static LoginResponse? loginResponse;
  static User? currentUser;
}
