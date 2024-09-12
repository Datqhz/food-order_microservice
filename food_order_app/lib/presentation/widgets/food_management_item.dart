import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:food_order_app/core/constant.dart';
import 'package:food_order_app/core/snackbar.dart';
import 'package:food_order_app/data/models/food.dart';
import 'package:food_order_app/data/requests/update_food_request.dart';
import 'package:food_order_app/repositories/food_repository.dart';
import 'package:intl/intl.dart';

class FoodManagementItem extends StatefulWidget {
  FoodManagementItem(
      {super.key,
      required this.food,
      required this.update,
      required this.delete});

  Food food;
  Function update;
  Function delete;

  @override
  State<FoodManagementItem> createState() => _FoodManagementItemState();
}

class _FoodManagementItemState extends State<FoodManagementItem> {
  final _nameController = TextEditingController();
  final _describeController = TextEditingController();
  final _priceController = TextEditingController();

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
        Column(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          children: [
            GestureDetector(
              onTap: () async {
                showDialog(
                  context: context,
                  builder: (context) {
                    return Dialog(
                      elevation: 0,
                      backgroundColor: Colors.white,
                      child: Container(
                        padding: const EdgeInsets.symmetric(
                            horizontal: 20, vertical: 12),
                        height: 500,
                        child: Column(
                          mainAxisAlignment: MainAxisAlignment.start,
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            const SizedBox(
                              height: 20,
                            ),
                            const Text(
                              "Update food",
                              style: const TextStyle(
                                  fontSize: 18,
                                  fontWeight: FontWeight.w600,
                                  color: Colors.black),
                            ),
                            const SizedBox(
                              height: 16,
                            ),
                            TextFormField(
                              controller: _nameController,
                              decoration: InputDecoration(
                                enabledBorder: OutlineInputBorder(
                                  borderSide: BorderSide(
                                      color: Theme.of(context).primaryColorDark,
                                      width: 1),
                                ),
                                focusedBorder: OutlineInputBorder(
                                  borderSide: BorderSide(
                                      color: Constant.colour_blue, width: 1),
                                ),
                                hintText: "Name",
                                hintStyle: TextStyle(
                                    color: Constant.colour_grey,
                                    fontSize: Constant.font_size_2,
                                    fontWeight: Constant.font_weight_nomal),
                                labelText: "Name",
                                labelStyle: TextStyle(
                                    color: Constant.colour_grey,
                                    fontWeight: Constant.font_weight_nomal),
                              ),
                            ),
                            const SizedBox(
                              height: 16,
                            ),
                            TextFormField(
                              controller: _describeController,
                              decoration: InputDecoration(
                                enabledBorder: OutlineInputBorder(
                                  borderSide: BorderSide(
                                      color: Theme.of(context).primaryColorDark,
                                      width: 1),
                                ),
                                focusedBorder: OutlineInputBorder(
                                  borderSide: BorderSide(
                                      color: Constant.colour_blue, width: 1),
                                ),
                                hintText: "Describe",
                                hintStyle: TextStyle(
                                    color: Constant.colour_grey,
                                    fontSize: Constant.font_size_2,
                                    fontWeight: Constant.font_weight_nomal),
                                labelText: "Describe",
                                labelStyle: TextStyle(
                                    color: Constant.colour_grey,
                                    fontWeight: Constant.font_weight_nomal),
                              ),
                            ),
                            const SizedBox(
                              height: 16,
                            ),
                            TextFormField(
                              controller: _priceController,
                              keyboardType: TextInputType.number,
                              decoration: InputDecoration(
                                enabledBorder: OutlineInputBorder(
                                  borderSide: BorderSide(
                                      color: Theme.of(context).primaryColorDark,
                                      width: 1),
                                ),
                                focusedBorder: OutlineInputBorder(
                                  borderSide: BorderSide(
                                      color: Constant.colour_blue, width: 1),
                                ),
                                hintText: "Price",
                                hintStyle: TextStyle(
                                    color: Constant.colour_grey,
                                    fontSize: Constant.font_size_2,
                                    fontWeight: Constant.font_weight_nomal),
                                labelText: "Price",
                                labelStyle: TextStyle(
                                    color: Constant.colour_grey,
                                    fontWeight: Constant.font_weight_nomal),
                              ),
                            ),
                            const SizedBox(
                              height: 16,
                            ),
                            const Expanded(
                                child: SizedBox(
                              height: 1,
                            )),
                            Row(
                              mainAxisAlignment: MainAxisAlignment.end,
                              children: [
                                TextButton(
                                  onPressed: () async {
                                    Navigator.pop(context);
                                  },
                                  style: TextButton.styleFrom(
                                    foregroundColor:
                                        Colors.black.withOpacity(0.6),
                                    padding: const EdgeInsets.symmetric(
                                        horizontal: 16, vertical: 10),
                                    textStyle: TextStyle(
                                        fontSize: 16,
                                        fontWeight: FontWeight.w500,
                                        color: Colors.black.withOpacity(0.6)),
                                    backgroundColor: Colors.transparent,
                                  ),
                                  child: const Text(
                                    "CANCEL",
                                  ),
                                ),
                                const SizedBox(
                                  height: 40,
                                ),
                                TextButton(
                                  onPressed: () async {
                                    final name = _nameController.text.trim();
                                    final describe =
                                        _describeController.text.trim();
                                    final price = double.parse(
                                        _priceController.text.trim());
                                    var result = await FoodRepository().update(
                                        UpdateFoodRequest(
                                            id: widget.food.id,
                                            name: name,
                                            price: price,
                                            describe: describe,
                                            imageUrl: "string"),
                                        context);
                                    if (result) {
                                      var tempFood = widget.food;
                                      tempFood.name = name;
                                      tempFood.price = price;
                                      tempFood.describe = describe;
                                      widget.update(tempFood);
                                      Navigator.pop(context);
                                      showSnackBar(context, "Food was updated");
                                    }
                                  },
                                  style: TextButton.styleFrom(
                                    foregroundColor: Colors.black,
                                    padding: const EdgeInsets.symmetric(
                                        horizontal: 16, vertical: 10),
                                    textStyle: const TextStyle(
                                        fontSize: 16,
                                        fontWeight: FontWeight.w500,
                                        color: Color.fromRGBO(15, 40, 232, 1)),
                                    backgroundColor: Colors.transparent,
                                  ),
                                  child: const Text(
                                    "UPDATE",
                                  ),
                                )
                              ],
                            )
                          ],
                        ),
                      ),
                    );
                  },
                );
              },
              child: const Icon(
                CupertinoIcons.pencil,
                color: Colors.black,
                size: 24,
              ),
            ),
            const SizedBox(
              height: 6,
            ),
            GestureDetector(
              onTap: () async {
                showDialog(
                  context: context,
                  builder: (context) {
                    return Dialog(
                      elevation: 0,
                      backgroundColor: Colors.white,
                      child: Container(
                        padding: const EdgeInsets.symmetric(
                            horizontal: 20, vertical: 12),
                        height: 180,
                        child: Column(
                          mainAxisAlignment: MainAxisAlignment.start,
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            const SizedBox(
                              height: 20,
                            ),
                            const Text(
                              "Delete food",
                              style: const TextStyle(
                                  fontSize: 18,
                                  fontWeight: FontWeight.w600,
                                  color: Colors.black),
                            ),
                            const SizedBox(
                              height: 16,
                            ),
                            Text(
                              "Do you want remove '${widget.food.name}'",
                              style: const TextStyle(
                                  fontSize: 16,
                                  fontWeight: FontWeight.w400,
                                  color: Colors.black),
                            ),
                            const Expanded(
                                child: SizedBox(
                              height: 1,
                            )),
                            Row(
                              mainAxisAlignment: MainAxisAlignment.end,
                              children: [
                                TextButton(
                                  onPressed: () async {
                                    Navigator.pop(context);
                                  },
                                  style: TextButton.styleFrom(
                                    foregroundColor:
                                        Colors.black.withOpacity(0.6),
                                    padding: const EdgeInsets.symmetric(
                                        horizontal: 16, vertical: 10),
                                    textStyle: TextStyle(
                                        fontSize: 16,
                                        fontWeight: FontWeight.w500,
                                        color: Colors.black.withOpacity(0.6)),
                                    backgroundColor: Colors.transparent,
                                  ),
                                  child: const Text(
                                    "CANCEL",
                                  ),
                                ),
                                const SizedBox(
                                  height: 40,
                                ),
                                TextButton(
                                  onPressed: () async {
                                    var result = await FoodRepository()
                                        .deleteFood(widget.food.id, context);
                                    if (result) {
                                      widget.delete(widget.food.id);
                                      showSnackBar(context, "Food was deleted");
                                    }
                                    Navigator.pop(context);
                                  },
                                  style: TextButton.styleFrom(
                                    foregroundColor: Colors.black,
                                    padding: const EdgeInsets.symmetric(
                                        horizontal: 16, vertical: 10),
                                    textStyle: const TextStyle(
                                        fontSize: 16,
                                        fontWeight: FontWeight.w500,
                                        color: Color.fromRGBO(15, 40, 232, 1)),
                                    backgroundColor: Colors.transparent,
                                  ),
                                  child: const Text(
                                    "YES",
                                  ),
                                )
                              ],
                            )
                          ],
                        ),
                      ),
                    );
                  },
                );
              },
              child: const Icon(
                CupertinoIcons.trash,
                color: Colors.red,
                size: 24,
              ),
            ),
          ],
        )
      ]),
    );
  }
}
