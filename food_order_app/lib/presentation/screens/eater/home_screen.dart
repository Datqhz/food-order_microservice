import 'package:flutter/material.dart';
import 'package:flutter_spinkit/flutter_spinkit.dart';
import 'package:food_order_app/core/constant.dart';
import 'package:food_order_app/data/models/user.dart';
import 'package:food_order_app/presentation/screens/eater/merchant_detail_screen.dart';
import 'package:food_order_app/presentation/widgets/merchant_item.dart';
import 'package:food_order_app/repositories/user_repository.dart';

class HomeScreen extends StatefulWidget {
  const HomeScreen({super.key});

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  @override
  void initState() {
    super.initState();
  }

  @override
  void dispose() {
    // TODO: implement dispose
    super.dispose();
  }

  Future<List<User>?> fetchData() async {
    return await UserRepository().getAllMerchants(context);
  }

  List<Widget> _showMerchants(List<User> merchants) {
    var result = <Widget>[];
    for (var merchant in merchants) {
      result.add(MerchantItem(merchant: merchant));
    }
    return result;
  }

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: EdgeInsets.only(
        top: Constant.padding_verticle_4,
        left: Constant.padding_horizontal_2,
        right: Constant.padding_horizontal_2,
      ),
      child: SingleChildScrollView(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              "All merchants",
              style: TextStyle(
                  color: Theme.of(context).primaryColorDark,
                  fontSize: Constant.font_size_5,
                  fontWeight: Constant.font_weight_heading2),
            ),
            SizedBox(
              height: Constant.dimension_12,
            ),
            FutureBuilder(
                future: fetchData(),
                builder: (context, snapshot) {
                  if (snapshot.connectionState == ConnectionState.done) {
                    if (snapshot.hasData) {
                      return Column(
                          children:
                              List.generate(snapshot.data!.length, (index) {
                        return GestureDetector(
                          onTap: () => {
                            Navigator.push(
                              context,
                              MaterialPageRoute(
                                builder: (context) => MerchantDetailScreen(
                                  merchant: snapshot.data![index],
                                ),
                              ),
                            )
                          },
                          child: Padding(
                            padding: const EdgeInsets.only(bottom: 8),
                            child: MerchantItem(
                              merchant: snapshot.data![index],
                            ),
                          ),
                        );
                      }));
                    }
                    return Text(
                      "Can't find any merchant",
                      style: TextStyle(
                          color: Constant.colour_low_black,
                          fontSize: Constant.font_size_3,
                          fontWeight: Constant.font_weight_nomal),
                    );
                  } else {
                    return Center(
                        child: SpinKitCircle(
                      color: Theme.of(context).primaryColorDark,
                      size: 50,
                    ));
                  }
                })
          ],
        ),
      ),
    );
  }
}
