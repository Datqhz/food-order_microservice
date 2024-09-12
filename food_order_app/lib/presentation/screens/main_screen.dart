import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:food_order_app/core/constant.dart';
import 'package:food_order_app/core/global_val.dart';
import 'package:food_order_app/presentation/screens/eater/home_screen.dart';
import 'package:food_order_app/presentation/screens/eater/search_food_screen.dart';
import 'package:food_order_app/presentation/screens/eater/search_merchant_screen.dart';
import 'package:food_order_app/presentation/screens/merchant/food_management_screen.dart';
import 'package:food_order_app/presentation/screens/order_management_screen.dart';
import 'package:food_order_app/presentation/screens/profile.dart';
import 'package:food_order_app/presentation/widgets/drawer.dart';

class MainScreen extends StatefulWidget {
  const MainScreen({super.key});

  @override
  State<MainScreen> createState() => _MainScreenState();
}

class _MainScreenState extends State<MainScreen> {
  final _bottomIndex = ValueNotifier(0);

  Widget _handleNavigationBottomBar() {
    switch (_bottomIndex.value) {
      case 0:
        return const HomeScreen();
      case 1:
        return const OrderManagementScreen();
      case 2:
        return ProfileScreen();
      default:
        return const FoodManagementScreen();
    }
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
              Container(
                color: Theme.of(context).primaryColorLight,
                child: ValueListenableBuilder(
                  valueListenable: _bottomIndex,
                  builder: (context, value, child) {
                    return _handleNavigationBottomBar();
                  },
                ),
              ),
              ValueListenableBuilder(
                valueListenable: _bottomIndex,
                builder: (context, value, child) {
                  if (value != 2) {
                    return Positioned(
                      top: 0,
                      left: 0,
                      right: 0,
                      height: 50,
                      child: Builder(
                        builder: (context) => Container(
                          padding: EdgeInsets.symmetric(
                              horizontal: Constant.padding_horizontal_2),
                          child: Row(
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            children: [
                              GestureDetector(
                                onTap: () {
                                  Scaffold.of(context).openDrawer();
                                },
                                child: const Icon(
                                  CupertinoIcons.bars,
                                  size: 22,
                                  color: Colors.black,
                                ),
                              ),
                              if (GlobalVariable.currentUser!.role
                                          .toLowerCase() ==
                                      'eater' &&
                                  value == 0) ...[
                                GestureDetector(
                                  onTap: () {
                                    Navigator.push(
                                      context,
                                      MaterialPageRoute(
                                        builder: (context) =>
                                            const SearchMerchantScreen(),
                                      ),
                                    );
                                  },
                                  child: Padding(
                                    padding:
                                        EdgeInsets.all(Constant.dimension_8),
                                    child: const Icon(
                                      CupertinoIcons.search,
                                      size: 22,
                                      color: Colors.black,
                                    ),
                                  ),
                                ),
                                GestureDetector(
                                  onTap: () {
                                    Navigator.push(
                                        context,
                                        MaterialPageRoute(
                                            builder: (context) =>
                                                const SearchFoodScreen()));
                                  },
                                  child: Padding(
                                    padding:
                                        EdgeInsets.all(Constant.dimension_8),
                                    child: const Icon(
                                      CupertinoIcons.search,
                                      size: 22,
                                      color: Colors.black,
                                    ),
                                  ),
                                ),
                              ]
                            ],
                          ),
                        ),
                      ),
                    );
                  }
                  return const SizedBox();
                },
              )
            ],
          ),
        ),
      ),
      bottomNavigationBar: BottomAppBar(
        color: Theme.of(context).primaryColorDark,
        child: Row(
          mainAxisAlignment: MainAxisAlignment.spaceAround,
          children: [
            IconButton(
              onPressed: () {
                _bottomIndex.value = 0;
              },
              icon: Icon(
                CupertinoIcons.home,
                color: Theme.of(context).primaryColorLight,
              ),
            ),
            if (GlobalVariable.currentUser!.role == "MERCHANT") ...[
              IconButton(
                onPressed: () {
                  _bottomIndex.value = 3;
                },
                icon: Icon(
                  CupertinoIcons.archivebox,
                  color: Theme.of(context).primaryColorLight,
                ),
              ),
            ],
            IconButton(
              onPressed: () {
                _bottomIndex.value = 1;
              },
              icon: Icon(
                CupertinoIcons.layers_alt,
                color: Theme.of(context).primaryColorLight,
              ),
            ),
            IconButton(
              onPressed: () {
                _bottomIndex.value = 2;
              },
              icon: Icon(
                CupertinoIcons.person,
                color: Theme.of(context).primaryColorLight,
              ),
            ),
          ],
        ),
      ),
      drawer: const MyDrawer(),
    );
  }
}
