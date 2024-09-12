import 'package:flutter/material.dart';
import 'package:flutter_spinkit/flutter_spinkit.dart';
import 'package:food_order_app/core/constant.dart';
import 'package:food_order_app/core/global_val.dart';
import 'package:food_order_app/core/jwt_decode.dart';
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
  final ValueNotifier<List<Order>?> _orders = ValueNotifier(null);
  final _isLoading = ValueNotifier(false);

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
      padding: EdgeInsets.only(
          top: Constant.padding_verticle_4,
          bottom: Constant.padding_verticle_5,
          left: Constant.padding_horizontal_2,
          right: Constant.padding_horizontal_2),
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
            ValueListenableBuilder(
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
            )
          ],
        ),
      ),
    );
  }
}
