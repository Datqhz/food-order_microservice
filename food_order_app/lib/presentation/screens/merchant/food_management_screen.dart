import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_spinkit/flutter_spinkit.dart';
import 'package:food_order_app/core/constant.dart';
import 'package:food_order_app/core/global_val.dart';
import 'package:food_order_app/core/jwt_decode.dart';
import 'package:food_order_app/core/stream/change_stream.dart';
import 'package:food_order_app/data/models/food.dart';
import 'package:food_order_app/presentation/widgets/food_management_item.dart';
import 'package:food_order_app/repositories/food_repository.dart';

class FoodManagementScreen extends StatefulWidget {
  const FoodManagementScreen({super.key});

  @override
  State<FoodManagementScreen> createState() => _FoodManagementScreenState();
}

class _FoodManagementScreenState extends State<FoodManagementScreen> {
  ValueNotifier<List<Food>?> _foods = ValueNotifier(null);
  final _isLoading = ValueNotifier(false);
  final ChangeStream _stream = ChangeStream();

  Future<void> fetchData() async {
    _isLoading.value = true;
    _foods.value = await FoodRepository().getAllFoodsByMerchantId(
        JWTHelper.getCurrentUid(GlobalVariable.loginResponse!.accessToken),
        context);
    _isLoading.value = false;
  }

  void removeItem(int foodId) {
    var list = _foods.value;
    for (var i = 0; i < list!.length; i++) {
      if (list[i].id == foodId) {
        list.removeAt(i);
        break;
      }
    }
    _foods.value = list;
    _stream.notifyChange();
  }

  void updateItem(Food food) {
    var list = _foods.value;
    for (var i = 0; i < list!.length; i++) {
      if (list[i].id == food.id) {
        list.replaceRange(i, i + 1, [food]);
        break;
      }
    }
    _foods.value = list;
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
              "Foods",
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
                    if (_foods.value == null) {
                      return Text(
                        "No food found",
                        style: TextStyle(
                            color: Theme.of(context).primaryColorDark,
                            fontSize: Constant.font_size_4,
                            fontWeight: Constant.font_weight_heading2),
                      );
                    } else {
                      return Column(
                        children: List.generate(
                            _foods.value!.length,
                            (index) => FoodManagementItem(
                                food: _foods.value![index],
                                update: updateItem,
                                delete: removeItem)),
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
