import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:food_order_app/core/constant.dart';
import 'package:food_order_app/core/global_val.dart';
import 'package:food_order_app/data/models/order.dart';
import 'package:food_order_app/presentation/screens/order_detail_screen.dart';

class OrderItem extends StatelessWidget {
  OrderItem({super.key, required this.order});

  Order order;

  String getStatusAsString() {
    switch (order.orderStatus) {
      case 2:
        return "Preparing";
      case 3:
        return "Delivery";
      case 4:
        return "Received";
      case 5:
        return "Cancelled";
      default:
        return "Initialize";
    }
  }

  @override
  Widget build(BuildContext context) {
    return GestureDetector(
      onTap: () {
        Navigator.push(
            context,
            MaterialPageRoute(
                builder: (context) => OrderDetailScreen(
                      orderId: order.id,
                    )));
      },
      child: Row(
        children: [
          Icon(
            CupertinoIcons.cube_box,
            color: Theme.of(context).primaryColorDark,
            size: Constant.dimension_32,
          ),
          SizedBox(
            width: Constant.dimension_12,
          ),
          Text(
            GlobalVariable.currentUser!.role == "EATER"
                ? order.merchant!.displayName
                : order.eater!.displayName,
            style: TextStyle(
              color: Theme.of(context).primaryColorDark,
              fontSize: Constant.font_size_3,
              fontWeight: Constant.font_weight_nomal,
            ),
          ),
          const Expanded(child: SizedBox()),
          Row(
            children: [
              Icon(
                CupertinoIcons.circle_fill,
                color: Constant.colour_red,
                size: Constant.dimension_12,
              ),
              SizedBox(
                width: Constant.dimension_4,
              ),
              Text(
                getStatusAsString(),
                style: TextStyle(
                  color: Theme.of(context).primaryColorDark,
                  fontSize: Constant.font_size_3,
                  fontWeight: Constant.font_weight_nomal,
                ),
              ),
            ],
          )
        ],
      ),
    );
  }
}
