import 'package:flutter/cupertino.dart';
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

class _OrderManagementScreenState extends State<OrderManagementScreen>
    with SingleTickerProviderStateMixin {
  final ValueNotifier<List<Order>?> _orders = ValueNotifier(null);
  final _isLoading = ValueNotifier(false);
  final _sortBy = ValueNotifier(0);
  late TabController _tabBarController;
  final _changeStream = ChangeStream();

  Future<void> fetchData(int status) async {
    _isLoading.value = true;
    _orders.value == null;
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
            orderStatus: status,
            sortBy: _sortBy.value),
        context);
    _isLoading.value = false;
  }

  Future<void> fetchByStatus() async {
    switch (_tabBarController.index) {
      case 1:
        await fetchData(3);
        break;
      case 2:
        await fetchData(4);
        break;
      case 3:
        await fetchData(5);
        break;
      default:
        await fetchData(2);
    }
  }

  @override
  void initState() {
    // TODO: implement initState
    super.initState();
    _tabBarController = TabController(length: 4, vsync: this);
    _tabBarController.addListener(() async {
      await fetchByStatus();
    });
    fetchData(2);
  }

  @override
  Widget build(BuildContext context) {
    return Container(
      height: MediaQuery.of(context).size.height,
      width: MediaQuery.of(context).size.width,
      padding: EdgeInsets.only(
          top: Constant.padding_verticle_4,
          left: Constant.padding_horizontal_2,
          right: Constant.padding_horizontal_2),
      child: Stack(
        children: [
          Container(
            padding: const EdgeInsets.only(top: 46),
            child: SingleChildScrollView(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Row(
                    children: [
                      Text(
                        "Orders",
                        style: TextStyle(
                            color: Theme.of(context).primaryColorDark,
                            fontSize: Constant.font_size_4,
                            fontWeight: Constant.font_weight_heading2),
                      ),
                      const Expanded(child: SizedBox()),
                      ValueListenableBuilder(
                        valueListenable: _sortBy,
                        builder: (context, value, snapshot) {
                          return GestureDetector(
                            onTap: () {
                              showModalBottomSheet(
                                  context: context,
                                  builder: (context) {
                                    return Container(
                                      height: 220,
                                      color: Colors.black,
                                      padding: const EdgeInsets.symmetric(
                                          horizontal: 20),
                                      child: Column(
                                        crossAxisAlignment:
                                            CrossAxisAlignment.start,
                                        children: [
                                          Row(
                                            mainAxisAlignment:
                                                MainAxisAlignment.center,
                                            children: [
                                              Icon(
                                                CupertinoIcons.minus,
                                                color: Colors.white
                                                    .withOpacity(0.8),
                                                size: 40,
                                              )
                                            ],
                                          ),
                                          const SizedBox(
                                            height: 6,
                                          ),
                                          const Text(
                                            "Sort",
                                            style: TextStyle(
                                              fontSize: 22,
                                              color: Colors.white,
                                              fontWeight: FontWeight.w600,
                                            ),
                                          ),
                                          const SizedBox(
                                            height: 15,
                                          ),
                                          GestureDetector(
                                            onTap: () async {
                                              _sortBy.value = 0;
                                              await fetchData(
                                                  _tabBarController.index);
                                              Navigator.pop(context);
                                            },
                                            child: Row(
                                              children: [
                                                const Text(
                                                  "Newest",
                                                  style: TextStyle(
                                                    fontSize: 16,
                                                    color: Colors.white,
                                                    fontWeight: FontWeight.w400,
                                                  ),
                                                ),
                                                const SizedBox(
                                                  width: 12,
                                                ),
                                                if (value == 0)
                                                  const Icon(
                                                    CupertinoIcons
                                                        .checkmark_alt_circle_fill,
                                                    color: Colors.white,
                                                  )
                                              ],
                                            ),
                                          ),
                                          const SizedBox(
                                            height: 15,
                                          ),
                                          GestureDetector(
                                            onTap: () async {
                                              _sortBy.value = 1;
                                              await fetchData(
                                                  _tabBarController.index);
                                              Navigator.pop(context);
                                            },
                                            child: Row(
                                              children: [
                                                const Text(
                                                  "Oldest",
                                                  style: TextStyle(
                                                    fontSize: 16,
                                                    color: Colors.white,
                                                    fontWeight: FontWeight.w400,
                                                  ),
                                                ),
                                                const SizedBox(
                                                  width: 12,
                                                ),
                                                if (value == 1)
                                                  const Icon(
                                                    CupertinoIcons
                                                        .checkmark_alt_circle_fill,
                                                    color: Colors.white,
                                                  )
                                              ],
                                            ),
                                          ),
                                          const SizedBox(
                                            height: 18,
                                          ),
                                        ],
                                      ),
                                    );
                                  });
                            },
                            child: Container(
                              padding: EdgeInsets.only(
                                  top: Constant.dimension_4,
                                  left: Constant.dimension_4),
                              child: Row(
                                children: [
                                  Icon(
                                    value == 0
                                        ? CupertinoIcons.sort_down
                                        : CupertinoIcons.sort_up,
                                    color: Constant.colour_low_black,
                                    size: 20,
                                  ),
                                  Text(
                                    value == 0 ? "Newest" : "Oldest",
                                    style: TextStyle(
                                        color: Constant.colour_low_black,
                                        fontSize: Constant.font_size_3,
                                        fontWeight: Constant.font_weight_light),
                                  ),
                                ],
                              ),
                            ),
                          );
                        },
                      ),
                    ],
                  ),
                  SizedBox(
                    height: Constant.dimension_14,
                  ),
                  StreamBuilder(
                      stream: _changeStream.stream,
                      builder: (context, snapshot) {
                        fetchByStatus();
                        return ValueListenableBuilder(
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
                              if (_orders.value == null ||
                                  _orders.value!.isEmpty) {
                                return Text(
                                  "No order found",
                                  style: TextStyle(
                                      color: Theme.of(context).primaryColorDark,
                                      fontSize: Constant.font_size_4,
                                      fontWeight:
                                          Constant.font_weight_heading2),
                                );
                              } else {
                                return Column(
                                  children: List.generate(
                                    _orders.value!.length,
                                    (index) => OrderItem(
                                      order: _orders.value![index],
                                      stream: _changeStream,
                                    ),
                                  ),
                                );
                              }
                            }
                          },
                        );
                      })
                ],
              ),
            ),
          ),
          Positioned(
            height: 36,
            top: 0,
            left: 0,
            right: 0,
            child: TabBar(
              padding: EdgeInsets.only(bottom: Constant.dimension_8),
              controller: _tabBarController,
              // isScrollable: true,
              indicatorColor: Theme.of(context).primaryColorDark,
              tabs: [
                Text(
                  "Preparing",
                  style: TextStyle(
                      color: Theme.of(context).primaryColorDark,
                      fontSize: Constant.font_size_3,
                      fontWeight: Constant.font_weight_nomal),
                ),
                Text(
                  "Delivery",
                  style: TextStyle(
                      color: Theme.of(context).primaryColorDark,
                      fontSize: Constant.font_size_3,
                      fontWeight: Constant.font_weight_nomal),
                ),
                Text(
                  "Done",
                  style: TextStyle(
                      color: Theme.of(context).primaryColorDark,
                      fontSize: Constant.font_size_3,
                      fontWeight: Constant.font_weight_nomal),
                ),
                Text(
                  "Cancelled",
                  style: TextStyle(
                      color: Theme.of(context).primaryColorDark,
                      fontSize: Constant.font_size_3,
                      fontWeight: Constant.font_weight_nomal),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }
}
