## Amazoon

An Amazon clone. This clone automatically simulates a warehouse in real-time. For example, when an order is placed through our webpage, robots will physically move around the warehouse, gather the order's items and place them into a truck, all without any human interaction. Synchronization is achieved through mutual exclusion and a database that holds live information about orders and items. Completed as a term project for a Systems Software Engineering course. Written in C# with an ASP.NET MVC webpage, MongoDB database and WinForms as a graphical display for the warehouse. <br>

### Features 
* __Order Placement through __Amazoon__ website__
  * Beautiful clien-interface
  * Order sent to database
* __Order fulfillment__ 📦
  * Automatic detection of new orders 
  * Robots gather items around warehouse
  * Drop off items into truck
  * Truck (_if carrying sufficient weight_) leaves warehouse
* __Restocking__ 🚛
  * Truck with new items arrives
  * Robots unload truck, place new items into warehouse
* __Database updated to reflect live status of orders and items__
  * e.g. Available, Purchases, Loaded, Shipped for items

## Demo 


https://user-images.githubusercontent.com/71570400/147901336-a1b36cf9-9526-4697-a71f-630c49163f35.mp4

