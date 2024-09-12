import 'package:flutter/material.dart';
import 'package:food_order_app/data/models/food.dart';
import 'package:intl/intl.dart';

class FoodSearchItem extends StatefulWidget {
  FoodSearchItem({super.key, required this.food});

  Food food;

  @override
  State<FoodSearchItem> createState() => _FoodSearchItemState();
}

class _FoodSearchItemState extends State<FoodSearchItem> {
  @override
  Widget build(BuildContext context) {
    return Container(
      height: 100,
      padding: const EdgeInsets.only(bottom: 8, top: 12),
      decoration: const BoxDecoration(
        border: Border(
          bottom: BorderSide(
            color: Color.fromRGBO(159, 159, 159, 1),
            width: 1,
          ),
        ),
      ),
      child: Row(children: [
        // food image
        Expanded(
          child: GestureDetector(
            onTap: () {},
            child: Row(
              children: [
                Container(
                  height: 80,
                  width: 80,
                  decoration: BoxDecoration(
                    image: const DecorationImage(
                        image: AssetImage("assets/images/store_avatar.jpg"),
                        fit: BoxFit.cover),
                    color: Colors.black,
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
                        widget.food.name,
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
                        NumberFormat.currency(locale: 'vi_VN', symbol: '₫')
                            .format(widget.food.price),
                        textAlign: TextAlign.center,
                        maxLines: 1,
                        style: const TextStyle(
                          fontSize: 14.0,
                          fontWeight: FontWeight.w300,
                          height: 1.2,
                          color: Color.fromRGBO(49, 49, 49, 1),
                          overflow: TextOverflow.ellipsis,
                        ),
                      )
                    ],
                  ),
                ),
                const SizedBox(
                  width: 8,
                )
              ],
            ),
          ),
        ),
      ]),
    );
  }
}
