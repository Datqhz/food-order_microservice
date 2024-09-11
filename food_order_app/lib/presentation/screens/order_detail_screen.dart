import 'package:flutter/material.dart';
import 'package:flutter_spinkit/flutter_spinkit.dart';
import 'package:food_order_app/core/constant.dart';
import 'package:food_order_app/data/models/brief_user.dart';
import 'package:food_order_app/data/models/order.dart';
import 'package:food_order_app/data/models/order_detail.dart';
import 'package:food_order_app/presentation/widgets/order_detail_item.dart';
import 'package:food_order_app/repositories/order_detail_repository.dart';
import 'package:food_order_app/repositories/order_repository.dart';
import 'package:intl/intl.dart';

class OrderDetailScreen extends StatefulWidget {
  OrderDetailScreen({super.key, required this.orderId});
  int orderId;

  @override
  State<OrderDetailScreen> createState() => _OrderDetailScreenState();
}

class _OrderDetailScreenState extends State<OrderDetailScreen> {
  final ValueNotifier<Order?> _order = ValueNotifier(null);

  final ValueNotifier<List<OrderDetail>?> _details = ValueNotifier(null);
  final ValueNotifier<bool> _isLoading = ValueNotifier(true);
  final ValueNotifier<double> _total = ValueNotifier(0);

  Future<void> fetchData() async {
    _isLoading.value = true;
    _order.value =
        await OrderRepository().getOrderById(widget.orderId, context);
    if (_order.value != null) {
      _details.value = await OrderDetailRepository()
          .getAllOrderDetailByOrderId(widget.orderId, context);
    }
    var total = 0.0;
    for (var item in _details.value!) {
      total += item.price * item.quantity;
    }
    _total.value = total;
    _isLoading.value = false;
  }

  Widget _userInfo(BriefUser user, context) {
    return Column(
      children: [
        Row(
          children: [
            Text(
              "Name: ",
              style: TextStyle(
                  fontSize: Constant.font_size_3,
                  fontWeight: Constant.font_weight_heading2,
                  color: Theme.of(context).primaryColorDark),
            ),
            SizedBox(
              width: Constant.dimension_4,
            ),
            Text(
              user.displayName,
              style: TextStyle(
                  fontSize: Constant.font_size_3,
                  fontWeight: Constant.font_weight_heading2,
                  color: Theme.of(context).primaryColorDark),
            ),
          ],
        ),
        Row(
          children: [
            Text(
              "Phone: ",
              style: TextStyle(
                  fontSize: Constant.font_size_3,
                  fontWeight: Constant.font_weight_nomal,
                  color: Theme.of(context).primaryColorDark),
            ),
            SizedBox(
              width: Constant.dimension_4,
            ),
            Text(
              user.phoneNumber,
              style: TextStyle(
                  fontSize: Constant.font_size_3,
                  fontWeight: Constant.font_weight_heading2,
                  color: Theme.of(context).primaryColorDark),
            ),
          ],
        )
      ],
    );
  }

  @override
  void initState() {
    // TODO: implement initState
    super.initState();
    fetchData();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        backgroundColor: Theme.of(context).primaryColorLight,
        title: Text(
          "Order detail",
          style: TextStyle(
            color: Theme.of(context).primaryColorDark,
            fontSize: Constant.font_size_4,
            fontWeight: Constant.font_weight_heading2,
          ),
        ),
      ),
      backgroundColor: Theme.of(context).primaryColorLight,
      body: SafeArea(
          child: Container(
        padding:
            EdgeInsets.symmetric(horizontal: Constant.padding_horizontal_3),
        child: ValueListenableBuilder(
            valueListenable: _isLoading,
            builder: (context, value, snapshot) {
              if (value) {
                return Center(
                  child: SpinKitCircle(
                    color: Theme.of(context).primaryColorDark,
                    size: Constant.dimension_50,
                  ),
                );
              } else {
                return Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      "Eater",
                      style: TextStyle(
                          fontSize: Constant.font_size_3,
                          fontWeight: Constant.font_weight_heading1,
                          color: Theme.of(context).primaryColorDark),
                    ),
                    SizedBox(
                      height: Constant.dimension_4,
                    ),
                    _userInfo(_order.value!.eater!, context),
                    SizedBox(
                      height: Constant.dimension_12,
                    ),
                    Text(
                      "Merchant",
                      style: TextStyle(
                          fontSize: Constant.font_size_3,
                          fontWeight: Constant.font_weight_heading1,
                          color: Theme.of(context).primaryColorDark),
                    ),
                    SizedBox(
                      height: Constant.dimension_4,
                    ),
                    _userInfo(_order.value!.eater!, context),
                    SizedBox(
                      height: Constant.dimension_14,
                    ),
                    Text(
                      "Detail",
                      style: TextStyle(
                          fontSize: Constant.font_size_3,
                          fontWeight: Constant.font_weight_heading1,
                          color: Theme.of(context).primaryColorDark),
                    ),
                    SizedBox(
                      height: Constant.dimension_8,
                    ),
                    ...List.generate(
                        _details.value!.length,
                        (index) =>
                            OrderDetailItem(detail: _details.value![index])),
                    Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      children: [
                        Text(
                          "Total",
                          style: TextStyle(
                              fontSize: Constant.font_size_3,
                              fontWeight: Constant.font_weight_heading1,
                              color: Theme.of(context).primaryColorDark),
                        ),
                        Text(
                          NumberFormat.currency(locale: "vi_VN", symbol: '₫')
                              .format(_total),
                          style: TextStyle(
                              fontSize: Constant.font_size_3,
                              fontWeight: Constant.font_weight_heading1,
                              color: Theme.of(context).primaryColorDark),
                        ),
                      ],
                    )
                  ],
                );
              }
            }),
      )),
    );
  }
}
