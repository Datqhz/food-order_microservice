import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_spinkit/flutter_spinkit.dart';
import 'package:food_order_app/core/constant.dart';
import 'package:food_order_app/core/global_val.dart';
import 'package:food_order_app/core/jwt_decode.dart';
import 'package:food_order_app/core/snackbar.dart';
import 'package:food_order_app/core/stream/change_stream.dart';
import 'package:food_order_app/data/models/food.dart';
import 'package:food_order_app/data/requests/create_food_request.dart';
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
  final _nameController = TextEditingController();
  final _priceController = TextEditingController();
  final _describeController = TextEditingController();

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
          left: Constant.padding_horizontal_2,
          right: Constant.padding_horizontal_2),
      child: SingleChildScrollView(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                Text(
                  "Foods",
                  style: TextStyle(
                      color: Theme.of(context).primaryColorDark,
                      fontSize: Constant.font_size_4,
                      fontWeight: Constant.font_weight_heading2),
                ),
                IconButton(
                    onPressed: () async {
                      showDialog(
                        context: context,
                        builder: (context) {
                          return Dialog(
                            elevation: 0,
                            backgroundColor: Colors.white,
                            child: Container(
                              padding: const EdgeInsets.symmetric(
                                  horizontal: 20, vertical: 12),
                              height: 500,
                              child: Column(
                                mainAxisAlignment: MainAxisAlignment.start,
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: [
                                  const SizedBox(
                                    height: 20,
                                  ),
                                  const Text(
                                    "Update food",
                                    style: const TextStyle(
                                        fontSize: 18,
                                        fontWeight: FontWeight.w600,
                                        color: Colors.black),
                                  ),
                                  const SizedBox(
                                    height: 16,
                                  ),
                                  TextFormField(
                                    controller: _nameController,
                                    decoration: InputDecoration(
                                      enabledBorder: OutlineInputBorder(
                                        borderSide: BorderSide(
                                            color: Theme.of(context)
                                                .primaryColorDark,
                                            width: 1),
                                      ),
                                      focusedBorder: OutlineInputBorder(
                                        borderSide: BorderSide(
                                            color: Constant.colour_blue,
                                            width: 1),
                                      ),
                                      hintText: "Name",
                                      hintStyle: TextStyle(
                                          color: Constant.colour_grey,
                                          fontSize: Constant.font_size_2,
                                          fontWeight:
                                              Constant.font_weight_nomal),
                                      labelText: "Name",
                                      labelStyle: TextStyle(
                                          color: Constant.colour_grey,
                                          fontWeight:
                                              Constant.font_weight_nomal),
                                    ),
                                  ),
                                  const SizedBox(
                                    height: 16,
                                  ),
                                  TextFormField(
                                    controller: _describeController,
                                    decoration: InputDecoration(
                                      enabledBorder: OutlineInputBorder(
                                        borderSide: BorderSide(
                                            color: Theme.of(context)
                                                .primaryColorDark,
                                            width: 1),
                                      ),
                                      focusedBorder: OutlineInputBorder(
                                        borderSide: BorderSide(
                                            color: Constant.colour_blue,
                                            width: 1),
                                      ),
                                      hintText: "Describe",
                                      hintStyle: TextStyle(
                                          color: Constant.colour_grey,
                                          fontSize: Constant.font_size_2,
                                          fontWeight:
                                              Constant.font_weight_nomal),
                                      labelText: "Describe",
                                      labelStyle: TextStyle(
                                          color: Constant.colour_grey,
                                          fontWeight:
                                              Constant.font_weight_nomal),
                                    ),
                                  ),
                                  const SizedBox(
                                    height: 16,
                                  ),
                                  TextFormField(
                                    controller: _priceController,
                                    keyboardType: TextInputType.number,
                                    decoration: InputDecoration(
                                      enabledBorder: OutlineInputBorder(
                                        borderSide: BorderSide(
                                            color: Theme.of(context)
                                                .primaryColorDark,
                                            width: 1),
                                      ),
                                      focusedBorder: OutlineInputBorder(
                                        borderSide: BorderSide(
                                            color: Constant.colour_blue,
                                            width: 1),
                                      ),
                                      hintText: "Price",
                                      hintStyle: TextStyle(
                                          color: Constant.colour_grey,
                                          fontSize: Constant.font_size_2,
                                          fontWeight:
                                              Constant.font_weight_nomal),
                                      labelText: "Price",
                                      labelStyle: TextStyle(
                                          color: Constant.colour_grey,
                                          fontWeight:
                                              Constant.font_weight_nomal),
                                    ),
                                  ),
                                  const SizedBox(
                                    height: 16,
                                  ),
                                  const Expanded(
                                      child: SizedBox(
                                    height: 1,
                                  )),
                                  Row(
                                    mainAxisAlignment: MainAxisAlignment.end,
                                    children: [
                                      TextButton(
                                        onPressed: () async {
                                          Navigator.pop(context);
                                        },
                                        style: TextButton.styleFrom(
                                          foregroundColor:
                                              Colors.black.withOpacity(0.6),
                                          padding: const EdgeInsets.symmetric(
                                              horizontal: 16, vertical: 10),
                                          textStyle: TextStyle(
                                              fontSize: 16,
                                              fontWeight: FontWeight.w500,
                                              color: Colors.black
                                                  .withOpacity(0.6)),
                                          backgroundColor: Colors.transparent,
                                        ),
                                        child: const Text(
                                          "CANCEL",
                                        ),
                                      ),
                                      const SizedBox(
                                        height: 40,
                                      ),
                                      TextButton(
                                        onPressed: () async {
                                          final name =
                                              _nameController.text.trim();
                                          final describe =
                                              _describeController.text.trim();
                                          final price = double.parse(
                                              _priceController.text.trim());
                                          var result = await FoodRepository()
                                              .create(
                                                  CreateFoodRequest(
                                                      name: name,
                                                      price: price,
                                                      describe: describe,
                                                      imageUrl: "aaaaa",
                                                      userId: GlobalVariable
                                                          .currentUser!.id),
                                                  context);
                                          if (result) {
                                            await fetchData();
                                            Navigator.pop(context);
                                            showSnackBar(
                                                context, "Food was created");
                                            _nameController.text = "";
                                            _priceController.text = "";
                                            _describeController.text = "";
                                          }
                                        },
                                        style: TextButton.styleFrom(
                                          foregroundColor: Colors.black,
                                          padding: const EdgeInsets.symmetric(
                                              horizontal: 16, vertical: 10),
                                          textStyle: const TextStyle(
                                              fontSize: 16,
                                              fontWeight: FontWeight.w500,
                                              color: Color.fromRGBO(
                                                  15, 40, 232, 1)),
                                          backgroundColor: Colors.transparent,
                                        ),
                                        child: const Text(
                                          "CREATE",
                                        ),
                                      )
                                    ],
                                  )
                                ],
                              ),
                            ),
                          );
                        },
                      );
                    },
                    icon: Icon(
                      CupertinoIcons.add,
                      color: Theme.of(context).primaryColorDark,
                    ))
              ],
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
