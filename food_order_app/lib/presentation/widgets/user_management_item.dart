import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:food_order_app/core/constant.dart';
import 'package:food_order_app/data/models/user.dart';

class UserManagementItem extends StatelessWidget {
  UserManagementItem({super.key, required this.user});
  User user;

  @override
  Widget build(BuildContext context) {
    return Container(
      margin: EdgeInsets.only(bottom: Constant.dimension_14),
      height: 80,
      child: Row(
        children: [
          // merchant avatar
          Container(
            height: 80,
            width: 80,
            decoration: BoxDecoration(
              image: DecorationImage(
                  image: NetworkImage(user.avatar), fit: BoxFit.cover),
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
                // display name
                Text(
                  user.displayName,
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
                  user.phoneNumber,
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
                const SizedBox(
                  width: 4,
                ),
              ],
            ),
          ),
          Row(
            children: [
              Icon(
                CupertinoIcons.circle_fill,
                color:
                    user.isActive ? Constant.colour_green : Constant.colour_red,
                size: Constant.dimension_12,
              ),
              SizedBox(
                width: Constant.dimension_4,
              ),
              Text(
                user.isActive ? "Active" : "Deleted",
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
