import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_spinkit/flutter_spinkit.dart';
import 'package:food_order_app/core/constant.dart';
import 'package:food_order_app/core/global_val.dart';
import 'package:food_order_app/core/snackbar.dart';
import 'package:food_order_app/core/stream/change_stream.dart';
import 'package:food_order_app/data/models/brief_user.dart';
import 'package:food_order_app/data/models/order.dart';
import 'package:food_order_app/data/models/order_detail.dart';
import 'package:food_order_app/data/requests/update_order_request.dart';
import 'package:food_order_app/presentation/widgets/order_detail_item.dart';
import 'package:food_order_app/repositories/order_detail_repository.dart';
import 'package:food_order_app/repositories/order_repository.dart';
import 'package:intl/intl.dart';

class OrderDetailScreen extends StatefulWidget {
  OrderDetailScreen({super.key, required this.orderId, required this.stream});
  int orderId;
  ChangeStream stream;

  @override
  State<OrderDetailScreen> createState() => _OrderDetailScreenState();
}

class _OrderDetailScreenState extends State<OrderDetailScreen> {
  final ValueNotifier<Order?> _order = ValueNotifier(null);

  final ValueNotifier<List<OrderDetail>?> _details = ValueNotifier(null);
  final ValueNotifier<bool> _isLoading = ValueNotifier(true);
  final ValueNotifier<double> _total = ValueNotifier(0.0);

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

  String getStatusAsString(int orderStatus) {
    switch (orderStatus) {
      case 2:
        return "Preparing";
      case 3:
        return "Delivery";
      case 4:
        return "Received";
      case 5:
        return "Cancelled";
      default:
        return "Initialize";
    }
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
    super.initState();
    fetchData();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        backgroundColor: Theme.of(context).primaryColorLight,
        title: Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            Text(
              "Order detail",
              style: TextStyle(
                color: Theme.of(context).primaryColorDark,
                fontSize: Constant.font_size_4,
                fontWeight: Constant.font_weight_heading2,
              ),
            ),
            Row(
              children: [
                Icon(
                  CupertinoIcons.circle_fill,
                  color: Constant.colour_red,
                  size: Constant.dimension_12,
                ),
                SizedBox(
                  width: Constant.dimension_4,
                ),
                ValueListenableBuilder(
                    valueListenable: _isLoading,
                    builder: (context, value, child) {
                      if (value) {
                        return const SizedBox();
                      } else {
                        return Text(
                          getStatusAsString(_order.value!.orderStatus),
                          style: TextStyle(
                            color: Theme.of(context).primaryColorDark,
                            fontSize: Constant.font_size_3,
                            fontWeight: Constant.font_weight_nomal,
                          ),
                        );
                      }
                    }),
              ],
            )
          ],
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
                return Stack(
                  children: [
                    Column(
                      children: [
                        SingleChildScrollView(
                          child: Column(
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
                                  (index) => OrderDetailItem(
                                      detail: _details.value![index])),
                              Row(
                                mainAxisAlignment:
                                    MainAxisAlignment.spaceBetween,
                                children: [
                                  Text(
                                    "Total",
                                    style: TextStyle(
                                        fontSize: Constant.font_size_4,
                                        fontWeight:
                                            Constant.font_weight_heading1,
                                        color:
                                            Theme.of(context).primaryColorDark),
                                  ),
                                  Text(
                                    NumberFormat.currency(
                                            locale: "vi_VN", symbol: '₫')
                                        .format(_total.value),
                                    style: TextStyle(
                                        fontSize: Constant.font_size_4,
                                        fontWeight:
                                            Constant.font_weight_heading1,
                                        color:
                                            Theme.of(context).primaryColorDark),
                                  ),
                                ],
                              )
                            ],
                          ),
                        ),
                      ],
                    ),
                    if (_order.value!.orderStatus < 4) ...[
                      Positioned(
                        bottom: 10,
                        left: 0,
                        right: 0,
                        child: SizedBox(
                            width: MediaQuery.of(context).size.width / 1.5,
                            child: GlobalVariable.currentUser!.role ==
                                    "MERCHANT"
                                ? TextButton(
                                    onPressed: () async {
                                      var request = UpdateOrderRequest(
                                        orderId: widget.orderId,
                                      );
                                      var result = await OrderRepository()
                                          .update(request, context);
                                      if (result) {
                                        showSnackBar(context,
                                            "Update status successful");
                                        widget.stream.notifyChange();
                                        Navigator.pop(context);
                                      }
                                    },
                                    style: TextButton.styleFrom(
                                        backgroundColor:
                                            Theme.of(context).primaryColorDark,
                                        shape: RoundedRectangleBorder(
                                            borderRadius: BorderRadius.circular(
                                                Constant.dimension_14))),
                                    child: Text(
                                      getStatusAsString(
                                          _order.value!.orderStatus + 1),
                                      style: TextStyle(
                                          fontSize: Constant.font_size_3,
                                          fontWeight:
                                              Constant.font_weight_heading1,
                                          color: Theme.of(context)
                                              .primaryColorLight),
                                    ),
                                  )
                                : (_order.value!.orderStatus == 2
                                    ? TextButton(
                                        onPressed: () async {
                                          var request = UpdateOrderRequest(
                                              orderId: widget.orderId,
                                              cancellation: true);
                                          var result = await OrderRepository()
                                              .update(request, context);
                                          if (result) {
                                            showSnackBar(context,
                                                "Update status successful");
                                            widget.stream.notifyChange();
                                            Navigator.pop(context);
                                          }
                                        },
                                        style: TextButton.styleFrom(
                                            backgroundColor: Theme.of(context)
                                                .primaryColorLight,
                                            shape: RoundedRectangleBorder(
                                                borderRadius:
                                                    BorderRadius.circular(
                                                        Constant.dimension_14),
                                                side: BorderSide(
                                                    color:
                                                        Constant.colour_red))),
                                        child: Text(
                                          "Cancel",
                                          style: TextStyle(
                                              fontSize: Constant.font_size_3,
                                              fontWeight:
                                                  Constant.font_weight_heading1,
                                              color: Constant.colour_red),
                                        ),
                                      )
                                    : const SizedBox())),
                      )
                    ]
                  ],
                );
              }
            }),
      )),
    );
  }
}
