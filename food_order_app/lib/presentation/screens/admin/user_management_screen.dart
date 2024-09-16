import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter_spinkit/flutter_spinkit.dart';
import 'package:food_order_app/core/constant.dart';
import 'package:food_order_app/core/stream/change_stream.dart';
import 'package:food_order_app/data/models/user.dart';
import 'package:food_order_app/presentation/widgets/user_management_item.dart';
import 'package:food_order_app/repositories/user_repository.dart';

class UserManagementScreen extends StatefulWidget {
  const UserManagementScreen({super.key});

  @override
  State<UserManagementScreen> createState() => _UserManagementScreenState();
}

class _UserManagementScreenState extends State<UserManagementScreen>
    with SingleTickerProviderStateMixin {
  final ValueNotifier<List<User>?> _users = ValueNotifier(null);
  final _isLoading = ValueNotifier(false);
  final _sortBy = ValueNotifier(3);
  late TabController _tabBarController;
  final _changeStream = ChangeStream();

  Future<void> fetchData() async {
    _isLoading.value = true;
    _users.value = await UserRepository()
        .getAllUsers(_tabBarController.index, _sortBy.value, context);
    _isLoading.value = false;
  }

  @override
  void initState() {
    // TODO: implement initState
    super.initState();
    _tabBarController = TabController(length: 2, vsync: this);
    _tabBarController.addListener(() async {
      fetchData();
    });
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
                      ValueListenableBuilder(
                          valueListenable: _isLoading,
                          builder: (context, value, child) {
                            return Text(
                              _tabBarController.index == 0
                                  ? "Eaters"
                                  : "Merchants",
                              style: TextStyle(
                                  color: Theme.of(context).primaryColorDark,
                                  fontSize: Constant.font_size_4,
                                  fontWeight: Constant.font_weight_heading2),
                            );
                          }),
                      const Expanded(child: SizedBox()),
                      ValueListenableBuilder(
                        valueListenable: _sortBy,
                        builder: (context, value, snapshot) {
                          return GestureDetector(
                            onTap: () {
                              if (value == 3) {
                                _sortBy.value = 2;
                              } else {
                                _sortBy.value = 3;
                              }
                              fetchData();
                            },
                            child: Container(
                              padding: EdgeInsets.only(
                                  top: Constant.dimension_4,
                                  left: Constant.dimension_4),
                              child: Icon(
                                value == 3
                                    ? CupertinoIcons.sort_down
                                    : CupertinoIcons.sort_up,
                                color: Constant.colour_low_black,
                                size: 20,
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
                        fetchData();
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
                              if (_users.value == null ||
                                  _users.value!.isEmpty) {
                                return Text(
                                  "No user found",
                                  style: TextStyle(
                                      color: Theme.of(context).primaryColorDark,
                                      fontSize: Constant.font_size_4,
                                      fontWeight:
                                          Constant.font_weight_heading2),
                                );
                              } else {
                                return Column(
                                  children: List.generate(
                                    _users.value!.length,
                                    (index) => UserManagementItem(
                                      user: _users.value![index],
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
                  "Eater",
                  style: TextStyle(
                      color: Theme.of(context).primaryColorDark,
                      fontSize: Constant.font_size_3,
                      fontWeight: Constant.font_weight_nomal),
                ),
                Text(
                  "Merchant",
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
