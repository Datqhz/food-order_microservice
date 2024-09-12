import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_spinkit/flutter_spinkit.dart';
import 'package:food_order_app/core/constant.dart';
import 'package:food_order_app/core/global_val.dart';
import 'package:food_order_app/data/models/food.dart';
import 'package:food_order_app/data/models/order.dart';
import 'package:food_order_app/data/models/order_detail.dart';
import 'package:food_order_app/data/models/user.dart';
import 'package:food_order_app/data/requests/get_order_by_userId_request.dart';
import 'package:food_order_app/data/requests/modify_orderd_detail_request.dart';
import 'package:food_order_app/presentation/screens/eater/prepare_order_screen.dart';
import 'package:food_order_app/presentation/widgets/food_item.dart';
import 'package:food_order_app/repositories/food_repository.dart';
import 'package:food_order_app/repositories/order_detail_repository.dart';
import 'package:food_order_app/repositories/order_repository.dart';

class MerchantDetailScreen extends StatefulWidget {
  MerchantDetailScreen({super.key, required this.merchant});

  User merchant;

  @override
  State<MerchantDetailScreen> createState() => _MerchantDetailScreenState();
}

class _MerchantDetailScreenState extends State<MerchantDetailScreen> {
  final ValueNotifier<List<Food>?> _foods = ValueNotifier(null);
  final ValueNotifier<List<OrderDetail>?> _orderDetails = ValueNotifier(null);
  final ValueNotifier<Order?> _order = ValueNotifier(null);
  final ValueNotifier<bool> _isLoading = ValueNotifier(true);
  final ValueNotifier<List<GlobalKey<FoodItemState>>> _foodItemStates =
      ValueNotifier([]);

  Future<void> _fetchData() async {
    _isLoading.value = true;
    _foods.value = await FoodRepository()
        .getAllFoodsByMerchantId(widget.merchant.id, context);
    _order.value = await OrderRepository().getOrderByEaterAndMerchant(
        GetOrderByUseridRequest(
            eaterId: GlobalVariable.currentUser!.id,
            merchantId: widget.merchant.id),
        context);
    if (_order.value != null) {
      _orderDetails.value = await OrderDetailRepository()
          .getAllOrderDetailByOrderId(_order.value!.id, context);
    }
    _isLoading.value = false;
  }

  OrderDetail? _getDetail(int foodId) {
    for (OrderDetail detail in _orderDetails.value!) {
      if (detail.food.id == foodId) {
        return detail;
      }
    }
    return null;
  }

  List<Widget> _loadFoods() {
    List<Widget> widgets = [];
    List<GlobalKey<FoodItemState>> states = [];
    for (var food in _foods.value!) {
      var key = GlobalKey<FoodItemState>();
      states.add(key);
      widgets.add(
        FoodItem(
          key: key,
          food: food,
          detail: _getDetail(food.id),
        ),
      );
    }
    _foodItemStates.value = states;
    return widgets;
  }

  bool _isOrderEmpty() {
    for (var item in _foodItemStates.value) {
      if (item.currentState!.getQuantity() != 0) {
        return false;
      }
    }
    return true;
  }

  List<ModifyOrderdDetailRequest> _getAllModified() {
    List<ModifyOrderdDetailRequest> details = [];
    for (var i = 0; i < _foodItemStates.value.length; i++) {
      var detail = _foodItemStates.value[i].currentState!.getOrderDetail();
      var quantity = _foodItemStates.value[i].currentState!.getQuantity();
      if (detail != null) {
        if (quantity == 0) {
          details.add(ModifyOrderdDetailRequest(
              orderDetailId: detail.id,
              quantity: quantity,
              price: _foods.value![i].price!,
              feature: 3));
        } else if (quantity != detail.quantity) {
          details.add(ModifyOrderdDetailRequest(
              orderDetailId: detail.id,
              quantity: quantity,
              price: _foods.value![i].price!,
              feature: 2));
        }
      } else {
        if (quantity != 0) {
          details.add(ModifyOrderdDetailRequest(
              orderId: _order.value!.id,
              foodId: _foods.value![i].id,
              quantity: quantity,
              price: _foods.value![i].price!,
              feature: 1));
        }
      }
    }
    print(details);
    return details;
  }

  @override
  void initState() {
    // TODO: implement initState
    _fetchData();
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Theme.of(context).primaryColorLight,
      body: SafeArea(
        child: Container(
          width: MediaQuery.of(context).size.width,
          height: MediaQuery.of(context).size.height,
          child: Stack(
            children: [
              SingleChildScrollView(
                child: Column(
                  children: [
                    Image(
                      height: 200,
                      width: MediaQuery.of(context).size.width,
                      image: const AssetImage("assets/images/store_avatar.jpg"),
                      fit: BoxFit.cover,
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
                                  size: Constant.dimension_50),
                            );
                          } else {
                            if (_foods.value!.isNotEmpty) {
                              return Padding(
                                padding: EdgeInsets.symmetric(
                                    horizontal: Constant.padding_horizontal_2),
                                child: GridView.count(
                                    primary: false,
                                    physics:
                                        const NeverScrollableScrollPhysics(),
                                    shrinkWrap: true,
                                    crossAxisSpacing: 12,
                                    mainAxisSpacing: 12,
                                    crossAxisCount: 2,
                                    childAspectRatio: 0.9,
                                    children: _loadFoods()),
                              );
                            } else {
                              return Text(
                                "No food found",
                                style: TextStyle(
                                    fontSize: Constant.font_size_4,
                                    fontWeight: Constant.font_weight_heading2,
                                    color: Theme.of(context).primaryColorDark),
                              );
                            }
                          }
                        }),
                    SizedBox(
                      height: Constant.dimension_14,
                    )
                  ],
                ),
              ),
              Positioned(
                top: 0,
                left: 0,
                right: 0,
                height: 50,
                child: Padding(
                  padding: EdgeInsets.symmetric(
                      horizontal: Constant.padding_horizontal_2),
                  child: Row(
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: [
                      GestureDetector(
                        onTap: () {
                          Navigator.pop(context);
                        },
                        child: Container(
                          padding: EdgeInsets.all(Constant.dimension_4),
                          decoration: BoxDecoration(
                              color: Constant.colour_low_white,
                              borderRadius: BorderRadius.circular(
                                  Constant.dimension_100)),
                          child: const Icon(
                            CupertinoIcons.arrow_left,
                            size: 22,
                            color: Colors.black,
                          ),
                        ),
                      ),
                      SizedBox(
                        width: Constant.dimension_12,
                      ),
                      Text(
                        widget.merchant.displayName,
                        style: TextStyle(
                            color: Theme.of(context).primaryColorLight,
                            fontSize: Constant.font_size_3,
                            fontWeight: Constant.font_weight_heading2),
                      )
                    ],
                  ),
                ),
              ),
              Positioned(
                bottom: 60,
                right: 20,
                child: GestureDetector(
                  onTap: () async {
                    if (!_isOrderEmpty()) {
                      var modifies = _getAllModified();
                      var result = true;
                      if (modifies.isNotEmpty) {
                        result = await OrderDetailRepository()
                            .modifyMultipleOrderDetails(modifies, context);
                      }
                      if (result) {
                        var isOrder = await Navigator.push(
                            context,
                            MaterialPageRoute(
                                builder: (context) => PrepareOrderScreen(
                                      orderId: _order.value!.id,
                                    )));
                        print(isOrder);
                        if (isOrder) {
                          await _fetchData();
                        }
                      }
                    }
                  },
                  child: Container(
                    height: 60,
                    width: 60,
                    decoration: BoxDecoration(
                        color: Colors.black,
                        borderRadius: BorderRadius.circular(50)),
                    child: const Icon(
                      CupertinoIcons.news,
                      color: Colors.white,
                      size: 30,
                    ),
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
