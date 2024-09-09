import 'package:jwt_decoder/jwt_decoder.dart';

class JWTHelper {
  static Map<String, dynamic> decodeJwt(String jwt) {
    return JwtDecoder.decode(jwt);
  }

  static String getCurrentUid(String jwt) {
    var payload = decodeJwt(jwt);
    return payload["user_id"];
  }
}
