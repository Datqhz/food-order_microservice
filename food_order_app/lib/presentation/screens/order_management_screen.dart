import 'package:flutter/material.dart';
import 'package:flutter_spinkit/flutter_spinkit.dart';
import 'package:food_order_app/core/constant.dart';
import 'package:food_order_app/core/global_val.dart';
import 'package:food_order_app/core/jwt_decode.dart';
import 'package:food_order_app/core/stream/change_stream.dart';
import 'package:food_order_app/data/models/order.dart';
import 'package:food_order_app/data/requests/get_order_by_userId_request.dart';
import 'package:food_order_app/presentation/widgets/order_item.dart';
import 'package:food_order_app/repositories/order_repository.dart';

class OrderManagementScreen extends StatefulWidget {
  const OrderManagementScreen({super.key});

  @override
  State<OrderManagementScreen> createState() => _OrderManagementScreenState();
}

class _OrderManagementScreenState extends State<OrderManagementScreen> {
  ValueNotifier<List<Order>?> _orders = ValueNotifier(null);
  final _isLoading = ValueNotifier(false);
  final ChangeStream _stream = ChangeStream();

  Future<void> fetchData() async {
    _isLoading.value = true;
    _orders.value = await OrderRepository().getAllOrdersByUserId(
        GetOrderByUseridRequest(
          merchantId: GlobalVariable.currentUser!.role == 'MERCHANT'
              ? JWTHelper.getCurrentUid(
                  GlobalVariable.loginResponse!.accessToken)
              : null,
          eaterId: GlobalVariable.currentUser!.role == 'EATER'
              ? JWTHelper.getCurrentUid(
                  GlobalVariable.loginResponse!.accessToken)
              : null,
        ),
        context);
    _isLoading.value = false;
  }

  void removeItem(int orderId) {
    var list = _orders.value;
    for (var i = 0; i < list!.length; i++) {
      if (list[i].id == orderId) {
        list.removeAt(i);
        break;
      }
    }
    _orders.value = list;
    _stream.notifyChange();
  }

  void updateItem(Order order) {
    var list = _orders.value;
    for (var i = 0; i < list!.length; i++) {
      if (list[i].id == order.id) {
        list.replaceRange(i, i + 1, [order]);
        break;
      }
    }
    _orders.value = list;
    _stream.notifyChange();
  }

  @override
  void initState() {
    // TODO: implement initState
    super.initState();
    fetchData();
  }

  @override
  Widget build(BuildContext context) {
    return Container(
      height: MediaQuery.of(context).size.height,
      width: MediaQuery.of(context).size.width,
      padding: EdgeInsets.symmetric(horizontal: Constant.padding_horizontal_2),
      child: SingleChildScrollView(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              "Orders",
              style: TextStyle(
                  color: Theme.of(context).primaryColorDark,
                  fontSize: Constant.font_size_4,
                  fontWeight: Constant.font_weight_heading2),
            ),
            SizedBox(
              height: Constant.dimension_14,
            ),
            StreamBuilder(
              stream: _stream.stream,
              builder: (context, snapshot) => ValueListenableBuilder(
                valueListenable: _isLoading,
                builder: (context, value, child) {
                  if (value) {
                    return Center(
                      child: SpinKitCircle(
                        color: Theme.of(context).primaryColorDark,
                        size: Constant.dimension_50,
                      ),
                    );
                  } else {
                    if (_orders.value == null) {
                      return Text(
                        "No order found",
                        style: TextStyle(
                            color: Theme.of(context).primaryColorDark,
                            fontSize: Constant.font_size_4,
                            fontWeight: Constant.font_weight_heading2),
                      );
                    } else {
                      return Column(
                        children: List.generate(
                          _orders.value!.length,
                          (index) => OrderItem(
                            order: _orders.value![index],
                          ),
                        ),
                      );
                    }
                  }
                },
              ),
            )
          ],
        ),
      ),
    );
  }
}
