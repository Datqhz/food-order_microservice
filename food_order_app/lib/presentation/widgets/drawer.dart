import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:food_order_app/core/global_val.dart';
import 'package:food_order_app/core/provider/login_state_provider.dart';
import 'package:food_order_app/core/utils/string_format.dart';
import 'package:provider/provider.dart';

class MyDrawer extends StatefulWidget {
  const MyDrawer({super.key});

  @override
  State<MyDrawer> createState() => _MyDrawerState();
}

class _MyDrawerState extends State<MyDrawer> {
  Widget accountItem(dynamic currentUser) {
    return Row(
      children: [
        Container(
          width: 50,
          height: 50,
          decoration: BoxDecoration(
              border: Border.all(color: Colors.black, width: 2),
              borderRadius: BorderRadius.circular(50)),
          child: CircleAvatar(
            backgroundColor: Colors.white,
            radius: 50,
            backgroundImage: NetworkImage(GlobalVariable.currentUser!.avatar),
          ),
        ),
        const SizedBox(
          width: 12,
        ),
        Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              currentUser.displayName,
              style: const TextStyle(
                  fontWeight: FontWeight.w600,
                  fontSize: 16,
                  color: Colors.white),
            ),
            const SizedBox(
              height: 6,
            ),
            Text(
              GlobalVariable.currentUser!.role.toLowerCase(),
              style: const TextStyle(
                  fontWeight: FontWeight.w400,
                  fontSize: 13,
                  color: Color.fromRGBO(170, 184, 194, 1)),
            ),
          ],
        )
      ],
    );
  }

  @override
  Widget build(BuildContext context) {
    return Drawer(
      backgroundColor: Colors.white,
      child: ListView(
        // Important: Remove any padding from the ListView.
        padding: EdgeInsets.zero,
        children: [
          const SizedBox(
            height: 40,
          ),
          Container(
            margin: const EdgeInsets.fromLTRB(16, 6, 16, 8),
            padding: const EdgeInsets.only(bottom: 22),
            decoration: const BoxDecoration(
              border: Border(
                  bottom: BorderSide(
                      width: 0.46, color: Color.fromRGBO(59, 59, 59, 1))),
              color: Colors.white,
            ),
            child: Column(
              mainAxisSize: MainAxisSize.min,
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                const SizedBox(
                  height: 12,
                ),
                Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  crossAxisAlignment: CrossAxisAlignment.center,
                  children: [
                    Container(
                      width: 50,
                      height: 50,
                      decoration: BoxDecoration(
                          border: Border.all(color: Colors.black, width: 1),
                          borderRadius: BorderRadius.circular(50)),
                      child: CircleAvatar(
                        backgroundColor: Colors.white,
                        radius: 50,
                        backgroundImage:
                            NetworkImage(GlobalVariable.currentUser!.avatar),
                      ),
                    ),
                    GestureDetector(
                      onTap: () {
                        showModalBottomSheet(
                            context: context,
                            builder: (context) {
                              return Container(
                                height: 220,
                                color: Colors.black,
                                padding:
                                    const EdgeInsets.symmetric(horizontal: 20),
                                child: Column(
                                  crossAxisAlignment: CrossAxisAlignment.start,
                                  children: [
                                    Row(
                                      mainAxisAlignment:
                                          MainAxisAlignment.center,
                                      children: [
                                        Icon(
                                          CupertinoIcons.minus,
                                          color: Colors.white.withOpacity(0.8),
                                          size: 40,
                                        )
                                      ],
                                    ),
                                    const SizedBox(
                                      height: 6,
                                    ),
                                    const Text(
                                      "Accounts",
                                      style: TextStyle(
                                        fontSize: 22,
                                        color: Colors.white,
                                        fontWeight: FontWeight.w600,
                                      ),
                                    ),
                                    const SizedBox(
                                      height: 15,
                                    ),
                                    accountItem(GlobalVariable.currentUser),
                                    const SizedBox(
                                      height: 18,
                                    ),
                                    // sign out button
                                    TextButton(
                                      onPressed: () async {
                                        GlobalVariable.currentUser = null;
                                        GlobalVariable.loginResponse = null;
                                        Provider.of<LoginStateProvider>(context,
                                                listen: false)
                                            .logout();
                                        Navigator.pop(context);
                                      },
                                      style: TextButton.styleFrom(
                                          minimumSize:
                                              const Size(double.infinity, 46),
                                          backgroundColor: Colors.black,
                                          shape: RoundedRectangleBorder(
                                              borderRadius:
                                                  BorderRadius.circular(25),
                                              side: const BorderSide(
                                                  color: Colors.white,
                                                  width: 1))),
                                      child: const Text(
                                        "Sign out",
                                        style: TextStyle(
                                            fontWeight: FontWeight.w600,
                                            fontSize: 18,
                                            color: Colors.red),
                                      ),
                                    )
                                  ],
                                ),
                              );
                            });
                      },
                      child: Container(
                          padding: const EdgeInsets.all(3),
                          decoration: BoxDecoration(
                              borderRadius: BorderRadius.circular(12),
                              border:
                                  Border.all(color: Colors.black, width: 1)),
                          child: const Icon(
                            CupertinoIcons.ellipsis_vertical,
                            size: 14,
                            color: Colors.black,
                          )),
                    ),
                  ],
                ),
                const SizedBox(
                  height: 8,
                ),
                Text(
                  GlobalVariable.currentUser!.displayName,
                  style: const TextStyle(
                      fontWeight: FontWeight.w600,
                      fontSize: 16,
                      color: Colors.black),
                ),
                const SizedBox(
                  height: 6,
                ),
                Text(
                  capitalize(GlobalVariable.currentUser!.role),
                  style: const TextStyle(
                      fontWeight: FontWeight.w400,
                      fontSize: 13,
                      color: Colors.black),
                ),
                const SizedBox(
                  height: 12,
                ),
              ],
            ),
          )
        ],
      ),
    );
  }
}
