import 'dart:math';

import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_spinkit/flutter_spinkit.dart';
import 'package:food_order_app/core/constant.dart';
import 'package:food_order_app/core/global_val.dart';
import 'package:food_order_app/core/snackbar.dart';
import 'package:food_order_app/data/models/order_detail.dart';
import 'package:food_order_app/data/models/shipping_info.dart';
import 'package:food_order_app/data/requests/update_order_with_shipping_info_request.dart';
import 'package:food_order_app/presentation/screens/eater/fill_shipping_info.dart';
import 'package:food_order_app/presentation/widgets/order_detail_item.dart';
import 'package:food_order_app/repositories/order_detail_repository.dart';
import 'package:food_order_app/repositories/order_repository.dart';
import 'package:intl/intl.dart';

class PrepareOrderScreen extends StatefulWidget {
  PrepareOrderScreen({super.key, required this.orderId});
  int orderId;

  @override
  State<PrepareOrderScreen> createState() => _PrepareOrderScreenState();
}

class _PrepareOrderScreenState extends State<PrepareOrderScreen> {
  final ValueNotifier<List<OrderDetail>?> _orderDetails = ValueNotifier(null);
  final _isLoading = ValueNotifier(true);
  final ValueNotifier<ShippingInfo> _shippingInfo = ValueNotifier(ShippingInfo(
      shippingAddress: "Tp.HCM",
      shippingPhoneNumber: GlobalVariable.currentUser!.phoneNumber));
  final _shippingFee = 15000 + Random().nextDouble() * (15000);

  Future<void> _fetchData() async {
    _isLoading.value = true;
    _orderDetails.value = await OrderDetailRepository()
        .getAllOrderDetailByOrderId(widget.orderId, context);
    _isLoading.value = false;
  }

  @override
  void initState() {
    // TODO: implement initState
    super.initState();
    _fetchData();
  }

  double _calcSubstantial() {
    var total = 0.0;
    for (var detail in _orderDetails.value!) {
      total += detail.price * detail.quantity;
    }
    return total;
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Theme.of(context).primaryColorLight,
      body: SafeArea(
        child: SizedBox(
          height: MediaQuery.of(context).size.height,
          width: MediaQuery.of(context).size.width,
          child: Stack(
            children: [
              Padding(
                padding: EdgeInsets.symmetric(
                    horizontal: Constant.padding_horizontal_3),
                child: SingleChildScrollView(
                  child: Column(
                    children: [
                      SizedBox(
                        height: Constant.dimension_50,
                      ),
                      SizedBox(
                        height: Constant.dimension_14,
                      ),
                      GestureDetector(
                        onTap: () async {
                          var shippingInfo = await Navigator.push(
                              context,
                              MaterialPageRoute(
                                  builder: (context) =>
                                      FillShippingInfoScreen()));
                          if (shippingInfo != null) {
                            _shippingInfo.value = shippingInfo;
                          }
                        },
                        child: Container(
                          height: 80,
                          decoration: BoxDecoration(
                              color: Colors.white,
                              border: Border(
                                  bottom: BorderSide(
                                      color: Constant.colour_low_grey,
                                      width: 0.8))),
                          child: Row(
                            crossAxisAlignment: CrossAxisAlignment.center,
                            children: [
                              const Icon(
                                Icons.location_on_sharp,
                                color: Colors.red,
                                size: 30,
                              ),
                              const SizedBox(
                                width: 8,
                              ),
                              ValueListenableBuilder(
                                  valueListenable: _shippingInfo,
                                  builder: (context, value, child) {
                                    return Column(
                                      crossAxisAlignment:
                                          CrossAxisAlignment.start,
                                      mainAxisAlignment:
                                          MainAxisAlignment.center,
                                      children: [
                                        Text(
                                          value.shippingPhoneNumber,
                                          maxLines: 1,
                                          style: const TextStyle(
                                              fontSize: 16.0,
                                              fontWeight: FontWeight.bold,
                                              color:
                                                  Color.fromRGBO(49, 49, 49, 1),
                                              overflow: TextOverflow.ellipsis),
                                        ),
                                        Text(
                                          value.shippingAddress,
                                          maxLines: 1,
                                          style: const TextStyle(
                                              fontSize: 14.0,
                                              fontWeight: FontWeight.w400,
                                              color:
                                                  Color.fromRGBO(49, 49, 49, 1),
                                              overflow: TextOverflow.ellipsis),
                                        )
                                      ],
                                    );
                                  }),
                              const Expanded(
                                  child: SizedBox(
                                width: 12,
                              )),
                              const Icon(
                                Icons.chevron_right,
                                color: Colors.red,
                                size: 30,
                              ),
                            ],
                          ),
                        ),
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
                              if (_orderDetails.value == null) {
                                return Text(
                                  "Can't load details",
                                  style: TextStyle(
                                      color: Theme.of(context).primaryColorDark,
                                      fontSize: Constant.font_size_4,
                                      fontWeight:
                                          Constant.font_weight_heading2),
                                );
                              } else {
                                return Column(
                                  children: List.generate(
                                    _orderDetails.value!.length,
                                    (index) => OrderDetailItem(
                                      detail: _orderDetails.value![index],
                                    ),
                                  ),
                                );
                              }
                            }
                          })
                    ],
                  ),
                ),
              ),
              // appbar
              Positioned(
                  left: 0,
                  right: 0,
                  top: 0,
                  height: 50,
                  child: Row(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      Text(
                        "Order",
                        style: TextStyle(
                            fontSize: Constant.font_size_4,
                            fontWeight: Constant.font_weight_heading2,
                            color: Theme.of(context).primaryColorDark),
                      )
                    ],
                  )),
              Positioned(
                top: 0,
                left: 20,
                height: 50,
                child: GestureDetector(
                  onTap: () {
                    Navigator.pop(context);
                  },
                  child: const Row(
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: [
                      Icon(
                        CupertinoIcons.xmark,
                        color: Colors.black,
                        size: 22,
                      ),
                    ],
                  ),
                ),
              ),
              // substantial
              Positioned(
                bottom: 0,
                left: 0,
                right: 0,
                child: Container(
                  padding: EdgeInsets.symmetric(
                      horizontal: Constant.padding_horizontal_3,
                      vertical: Constant.padding_verticle_1),
                  decoration: BoxDecoration(
                    border: Border(
                        top: BorderSide(
                            color: Constant.colour_low_black, width: 0.2)),
                  ),
                  child: Column(
                    children: [
                      Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: [
                          const Text(
                            'Substantial',
                            maxLines: 1,
                            style: TextStyle(
                                fontSize: 16.0,
                                fontWeight: FontWeight.bold,
                                color: Color.fromRGBO(49, 49, 49, 1),
                                overflow: TextOverflow.ellipsis),
                          ),
                          ValueListenableBuilder(
                            valueListenable: _isLoading,
                            builder: (context, value, child) {
                              return Text(
                                value
                                    ? "..."
                                    : NumberFormat.currency(
                                            locale: 'vi_VN', symbol: '₫')
                                        .format(_calcSubstantial()),
                                maxLines: 1,
                                style: const TextStyle(
                                  fontSize: 16.0,
                                  fontWeight: FontWeight.bold,
                                  color: Color.fromRGBO(49, 49, 49, 1),
                                  overflow: TextOverflow.ellipsis,
                                ),
                              );
                            },
                          ),
                        ],
                      ),
                      SizedBox(height: Constant.dimension_8),
                      Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: [
                          const Text(
                            'Shipping fee',
                            maxLines: 1,
                            style: TextStyle(
                                fontSize: 16.0,
                                fontWeight: FontWeight.bold,
                                color: Color.fromRGBO(49, 49, 49, 1),
                                overflow: TextOverflow.ellipsis),
                          ),
                          Text(
                            NumberFormat.currency(locale: 'vi_VN', symbol: '₫')
                                .format(_shippingFee),
                            maxLines: 1,
                            style: const TextStyle(
                              fontSize: 16.0,
                              fontWeight: FontWeight.bold,
                              color: Color.fromRGBO(49, 49, 49, 1),
                              overflow: TextOverflow.ellipsis,
                            ),
                          ),
                        ],
                      ),
                      SizedBox(height: Constant.dimension_8),
                      Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: [
                          const Text(
                            'Total',
                            maxLines: 1,
                            style: TextStyle(
                                fontSize: 20.0,
                                fontWeight: FontWeight.bold,
                                color: Color.fromRGBO(49, 49, 49, 1),
                                overflow: TextOverflow.ellipsis),
                          ),
                          ValueListenableBuilder(
                            valueListenable: _isLoading,
                            builder: (context, value, child) {
                              return Text(
                                value
                                    ? "..."
                                    : NumberFormat.currency(
                                            locale: 'vi_VN', symbol: '₫')
                                        .format(
                                            _calcSubstantial() + _shippingFee),
                                maxLines: 1,
                                style: const TextStyle(
                                  fontSize: 16.0,
                                  fontWeight: FontWeight.bold,
                                  color: Color.fromRGBO(49, 49, 49, 1),
                                  overflow: TextOverflow.ellipsis,
                                ),
                              );
                            },
                          ),
                        ],
                      ),
                      SizedBox(
                        height: Constant.dimension_14,
                      ),
                      SizedBox(
                        width: MediaQuery.of(context).size.width / 1.5,
                        child: TextButton(
                          onPressed: () async {
                            var request = UpdateOrderWithShippingInfoRequest(
                                orderId: widget.orderId,
                                shippingAddress:
                                    _shippingInfo.value.shippingAddress,
                                shippingFee: 15000,
                                shippingPhoneNumber:
                                    _shippingInfo.value.shippingPhoneNumber);
                            var result = await OrderRepository()
                                .updateWithShippingInfo(request, context);
                            if (result) {
                              showSnackBar(context, "Order successful");
                              Navigator.pop(context, true);
                            }
                          },
                          style: TextButton.styleFrom(
                              backgroundColor:
                                  Theme.of(context).primaryColorDark,
                              shape: RoundedRectangleBorder(
                                  borderRadius: BorderRadius.circular(
                                      Constant.dimension_14))),
                          child: Text(
                            "Checkout",
                            style: TextStyle(
                                fontSize: Constant.font_size_3,
                                fontWeight: Constant.font_weight_heading1,
                                color: Theme.of(context).primaryColorLight),
                          ),
                        ),
                      )
                    ],
                  ),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
