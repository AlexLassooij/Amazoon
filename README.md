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

<figure class="video_container">
  <iframe src="https://drive.google.com/file/d/1qtEt5il-Hq3AcaEy18Q0n5I7TauLVjf1/view?usp=sharing" frameborder="0" allowfullscreen="true"> </iframe>
</figure>
