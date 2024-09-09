import 'package:flutter/material.dart';
import 'package:food_order_app/core/constant.dart';

void showSnackBar(BuildContext context, String text) {
  ScaffoldMessenger.of(context).showSnackBar(
    SnackBar(
      elevation: 0,
      width: MediaQuery.of(context).size.width,
      backgroundColor: Colors.transparent,
      behavior: SnackBarBehavior.floating,
      content: Row(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          Container(
              padding: EdgeInsets.symmetric(
                  horizontal: Constant.padding_horizontal_2,
                  vertical: Constant.padding_verticle_1),
              decoration: BoxDecoration(
                color: Constant.colour_low_black,
                borderRadius: BorderRadius.circular(Constant.dimension_14),
              ),
              child: Text(
                text,
                style: TextStyle(color: Theme.of(context).primaryColorLight),
              )),
        ],
      ),
      duration: const Duration(seconds: 3),
    ),
  );
}
