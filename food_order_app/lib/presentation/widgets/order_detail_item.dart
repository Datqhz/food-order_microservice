import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:food_order_app/core/constant.dart';
import 'package:food_order_app/data/models/order_detail.dart';
import 'package:intl/intl.dart';

class OrderDetailItem extends StatelessWidget {
  OrderDetailItem({super.key, required this.detail});
  OrderDetail detail;

  @override
  Widget build(BuildContext context) {
    return Container(
        padding: EdgeInsets.only(
          bottom: Constant.padding_verticle_1,
        ),
        child: Row(
          children: [
            // food image
            Container(
              height: 80,
              width: 80,
              decoration: BoxDecoration(
                image: const DecorationImage(
                    image: AssetImage("assets/images/store_avatar.jpg"),
                    fit: BoxFit.cover),
                color: Theme.of(context).primaryColorDark,
                borderRadius: BorderRadius.circular(6),
              ),
            ),
            const SizedBox(
              width: 12,
            ),
            Expanded(
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  // food name
                  Text(
                    detail.food.name,
                    textAlign: TextAlign.center,
                    maxLines: 1,
                    style: const TextStyle(
                      fontSize: 16.0,
                      fontWeight: FontWeight.w600,
                      height: 1.2,
                      color: Color.fromRGBO(49, 49, 49, 1),
                      overflow: TextOverflow.ellipsis,
                    ),
                  ),
                  const SizedBox(
                    height: 6,
                  ),
                  Text(
                    '${NumberFormat.currency(locale: 'vi_VN', symbol: '₫').format(detail.price)}  x${detail.quantity}',
                    textAlign: TextAlign.center,
                    maxLines: 1,
                    style: const TextStyle(
                      fontSize: 14.0,
                      fontWeight: FontWeight.w300,
                      height: 1.2,
                      color: Color.fromRGBO(49, 49, 49, 1),
                      overflow: TextOverflow.ellipsis,
                    ),
                  ),
                ],
              ),
            ),
          ],
        ));
  }
}
