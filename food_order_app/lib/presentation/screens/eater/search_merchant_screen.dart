import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_spinkit/flutter_spinkit.dart';
import 'package:food_order_app/core/constant.dart';
import 'package:food_order_app/data/models/dto/merchant_with_paging.dart';
import 'package:food_order_app/presentation/widgets/merchant_item.dart';
import 'package:food_order_app/repositories/user_repository.dart';

class SearchMerchantScreen extends StatefulWidget {
  const SearchMerchantScreen({super.key});

  @override
  State<SearchMerchantScreen> createState() => _SearchMerchantScreenState();
}

class _SearchMerchantScreenState extends State<SearchMerchantScreen> {
  final _regexController = TextEditingController();
  final ValueNotifier<MerchantsWithPaging?> _merchants = ValueNotifier(null);
  final ValueNotifier<bool> _isLoading = ValueNotifier(false);

  Future<void> _onSearch(String keyword) async {
    _isLoading.value = true;
    var merchants = await UserRepository()
        .searchMerchantsByName(keyword, page: 1, maxPerPage: 1, context);
    if (merchants != null) {
      _merchants.value = merchants;
    }
    _isLoading.value = false;
  }

  Future<void> _onMore(String keyword) async {
    var currentPage = _merchants.value!.paging.pageNumber;
    if (currentPage < _merchants.value!.paging.totalPages) {
      var merchants = await UserRepository().searchMerchantsByName(
          keyword, page: ++currentPage, maxPerPage: 1, context);
      if (merchants != null) {
        var temp = _merchants.value!.clone();
        temp.users = temp.users.followedBy(merchants.users).toList();
        temp.paging = merchants.paging;
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
              hintText: "ex.Abc,...",
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
            valueListenable: _merchants,
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
                    if (value.users.isEmpty) {
                      return Center(
                        child: Text(
                          "No merchant found",
                          style: TextStyle(
                              color: Theme.of(context).primaryColorDark,
                              fontSize: Constant.font_size_4,
                              fontWeight: Constant.font_weight_heading2),
                        ),
                      );
                    }
                    return Column(
                      children: [
                        Expanded(
                          child: ListView.builder(
                            itemCount: _merchants.value!.users.length,
                            itemBuilder: (context, index) =>
                                MerchantItem(merchant: value.users[index]),
                          ),
                        ),
                        if (value.paging.pageNumber !=
                            value.paging.totalPages) ...[
                          GestureDetector(
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
