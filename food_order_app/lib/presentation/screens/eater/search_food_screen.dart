import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_spinkit/flutter_spinkit.dart';
import 'package:food_order_app/core/constant.dart';
import 'package:food_order_app/data/models/dto/food_with_paging.dart';
import 'package:food_order_app/presentation/widgets/food_search_item.dart';
import 'package:food_order_app/repositories/food_repository.dart';

class SearchFoodScreen extends StatefulWidget {
  const SearchFoodScreen({super.key});

  @override
  State<SearchFoodScreen> createState() => _SearchFoodScreenState();
}

class _SearchFoodScreenState extends State<SearchFoodScreen> {
  final _regexController = TextEditingController();
  final ValueNotifier<FoodsWithPaging?> _foods = ValueNotifier(null);
  final ValueNotifier<bool> _isLoading = ValueNotifier(false);

  Future<void> _onSearch(String keyword) async {
    _isLoading.value = true;
    var foods = await FoodRepository().searchFoodsByName(keyword, 1, context);
    if (foods != null) {
      _foods.value = foods;
    }
    _isLoading.value = false;
  }

  Future<void> _onMore(String keyword) async {
    var currentPage = _foods.value!.paging.pageNumber;

    if (currentPage < _foods.value!.paging.totalPages) {
      var foods = await FoodRepository()
          .searchFoodsByName(keyword, ++currentPage, context);
      if (foods != null) {
        var temp = _foods.value!.clone();
        temp.foods = temp.foods.followedBy(foods.foods).toList();
        temp.paging = foods.paging;
        _foods.value = temp;
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        backgroundColor: Theme.of(context).primaryColorLight,
        title: SizedBox(
          height: 30,
          width: double.infinity,
          child: TextFormField(
            controller: _regexController,
            decoration: InputDecoration(
              hintText: "ex.Phở, bún,...",
              hintStyle: TextStyle(
                  color: Constant.colour_grey,
                  fontWeight: Constant.font_weight_light),
              suffix: IconButton(
                onPressed: () {
                  var keyword = _regexController.text.trim();
                  if (keyword.isNotEmpty) {
                    _onSearch(keyword);
                  }
                },
                icon: const Icon(CupertinoIcons.search),
              ),
            ),
          ),
        ),
      ),
      body: SafeArea(
        child: Container(
          padding:
              EdgeInsets.symmetric(horizontal: Constant.padding_horizontal_3),
          width: MediaQuery.of(context).size.width,
          height: MediaQuery.of(context).size.height,
          color: Theme.of(context).primaryColorLight,
          child: ValueListenableBuilder(
            valueListenable: _foods,
            builder: (context, value, child) {
              return ValueListenableBuilder(
                valueListenable: _isLoading,
                builder: (context, isLoadingValue, child) {
                  if (isLoadingValue) {
                    return Center(
                      child: SpinKitCircle(
                        color: Theme.of(context).primaryColorDark,
                        size: Constant.dimension_50,
                      ),
                    );
                  }
                  if (value != null) {
                    if (value.foods.isEmpty) {
                      return Center(
                        child: Text(
                          "No food found",
                          style: TextStyle(
                              color: Theme.of(context).primaryColorDark,
                              fontSize: Constant.font_size_4,
                              fontWeight: Constant.font_weight_heading2),
                        ),
                      );
                    }
                    return ListView(
                      children: [
                        ...List.generate(
                          value.foods.length,
                          (index) => FoodSearchItem(food: value.foods[index]),
                        ),
                        if (value.paging.pageNumber !=
                            value.paging.totalPages) ...[
                          GestureDetector(
                            onTap: () async {
                              var keyword = _regexController.text.trim();
                              if (keyword.isNotEmpty) {
                                _onMore(keyword);
                              }
                            },
                            child: Padding(
                              padding: EdgeInsets.symmetric(
                                  horizontal: Constant.padding_horizontal_4),
                              child: Text(
                                "See more",
                                style: TextStyle(
                                    color: Constant.colour_blue,
                                    fontSize: Constant.font_size_3,
                                    fontWeight: Constant.font_weight_nomal),
                              ),
                            ),
                          )
                        ]
                      ],
                    );
                  }
                  return const SizedBox();
                },
              );
            },
          ),
        ),
      ),
    );
  }
}
