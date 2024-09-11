import 'dart:async';

class ChangeStream{
  final StreamController _controller = StreamController.broadcast();
  Stream get stream => _controller.stream;
  
  void notifyChange(){
    _controller.add(null);
  }
  void dispose(){
    _controller.close();
  }
}