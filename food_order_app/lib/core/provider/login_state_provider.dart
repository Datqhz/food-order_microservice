import 'package:flutter/material.dart';

class LoginStateProvider with ChangeNotifier{
  bool _isLogin = false;

  get isLogin => _isLogin;

  void login(){
    _isLogin = true;
    notifyListeners();
  }
  
  void logout(){
    _isLogin = false;
    notifyListeners();
  }
}