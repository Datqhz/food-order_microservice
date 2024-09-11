import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:food_order_app/core/constant.dart';
import 'package:food_order_app/data/models/food.dart';
import 'package:food_order_app/data/models/order_detail.dart';
import 'package:intl/intl.dart';

class FoodItem extends StatefulWidget {
  FoodItem({super.key, required this.food, this.detail});
  Food food;
  OrderDetail? detail;
  @override
  State<FoodItem> createState() => FoodItemState();
}

class FoodItemState extends State<FoodItem> {
  final _quantity = ValueNotifier(0);

  int getQuantity() => _quantity.value;

  OrderDetail? getOrderDetail() => widget.detail;

  @override
  void initState() {
    if (widget.detail != null) {
      _quantity.value = widget.detail!.quantity;
    }
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return LayoutBuilder(builder: (context, constraints) {
      return Container(
        clipBehavior: Clip.antiAlias,
        decoration: BoxDecoration(
          borderRadius: BorderRadius.circular(12),
          color: Constant.colour_low_grey,
        ),
        height: 190,
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // food image
            const Image(
              height: 100,
              width: double.infinity,
              image: AssetImage("assets/images/store_avatar.jpg"),
              fit: BoxFit.cover,
            ),
            const SizedBox(
              height: 8,
            ),
            // food name
            Container(
              padding: const EdgeInsets.symmetric(horizontal: 10),
              width: constraints.maxWidth - 10,
              child: Row(
                children: [
                  Flexible(
                    flex: 1,
                    child: Text(
                      widget.food.name,
                      textAlign: TextAlign.center,
                      maxLines: 1,
                      style: const TextStyle(
                        fontSize: 14.0,
                        fontWeight: FontWeight.w500,
                        height: 1.2,
                        color: Color.fromRGBO(49, 49, 49, 1),
                        overflow: TextOverflow.ellipsis,
                      ),
                    ),
                  ),
                  ValueListenableBuilder(
                      valueListenable: _quantity,
                      builder: (context, value, child) {
                        if (value != 0) {
                          return Text(
                            ' - Selected: $value',
                            textAlign: TextAlign.left,
                            maxLines: 1,
                            style: const TextStyle(
                              fontSize: 14.0,
                              fontWeight: FontWeight.w300,
                              height: 1.2,
                              color: Color.fromRGBO(49, 49, 49, 1),
                              overflow: TextOverflow.ellipsis,
                            ),
                          );
                        } else {
                          return const SizedBox();
                        }
                      }),
                ],
              ),
            ),
            const Expanded(child: SizedBox()),
            // price
            Padding(
              padding: EdgeInsets.symmetric(
                  horizontal: Constant.padding_horizontal_1),
              child: Row(
                crossAxisAlignment: CrossAxisAlignment.end,
                mainAxisAlignment: MainAxisAlignment.start,
                children: [
                  Expanded(
                    child: Text(
                      NumberFormat.currency(locale: 'vi_VN', symbol: '₫')
                          .format(widget.food.price),
                      textAlign: TextAlign.start,
                      maxLines: 1,
                      style: const TextStyle(
                        fontSize: 14.0,
                        fontWeight: FontWeight.w600,
                        height: 1.2,
                        color: Color.fromRGBO(49, 49, 49, 1),
                        overflow: TextOverflow.ellipsis,
                      ),
                    ),
                  ),
                  GestureDetector(
                    onTap: () {
                      if (_quantity.value > 0) {
                        _quantity.value = _quantity.value - 1;
                      }
                    },
                    child: Container(
                      height: 30,
                      width: 30,
                      decoration: BoxDecoration(
                        color: Constant.colour_low_black,
                        borderRadius:
                            BorderRadius.circular(Constant.dimension_100),
                      ),
                      child: Icon(
                        CupertinoIcons.minus,
                        size: Constant.dimension_14,
                        color: Theme.of(context).primaryColorLight,
                      ),
                    ),
                  ),
                  SizedBox(
                    width: Constant.dimension_8,
                  ),
                  GestureDetector(
                    onTap: () {
                      _quantity.value = _quantity.value + 1;
                    },
                    child: Container(
                      height: 30,
                      width: 30,
                      decoration: BoxDecoration(
                        color: Constant.colour_low_black,
                        borderRadius:
                            BorderRadius.circular(Constant.dimension_100),
                      ),
                      child: Icon(
                        CupertinoIcons.add,
                        size: Constant.dimension_14,
                        color: Theme.of(context).primaryColorLight,
                      ),
                    ),
                  )
                ],
              ),
            ),
            const SizedBox(
              height: 12,
            ),
          ],
        ),
      );
    });
  }
}
