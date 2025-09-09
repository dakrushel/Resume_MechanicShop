README

Welcome to the Mechanic Shop application, From Team 2
(Chloe, Denver, Zack)
CPRG 211 E - OOP 2
Final Project
Completed on 2024-04-15

This application is designed to run FULL SCREEN on a WINDOWS machine.

The Mechanic Shop application is designed to facilitate an efficient 
and systematic workflow for a Service Advisor in an automotive Mechanic shop.

The Customer View allows the user to View and search through existing customers, 
Add new customers, View and edit customer details, and delete customers from the 
system. (Please not that a customer with active appointments or Repair Orders in 
the system cannot be deleted until those orders are processed through, preventing 
errors) Additionally on this page the user can manage the customer's vehicle 
collection, adding or removing vehicles as needed. (Vehicles with existing orders 
also cannot be deleted) This page also provides input validation for all entry 
fields, with visual indicators to help the user ensure that all information is 
correct.

The Appointments View allows the user to manage, search through, and view/edit/delete
appointments in the system. For convenience, there are two appointment lists, 
one showing all upcoming appointments, and another showing appointments that are 
expired, helping the user to easily see any customers that may need to be contacted 
to reschedule. From this page you can take down information about the problem from 
the customer, as well as choose an appointment date, or reschedule an existing 
appointment. You can also add jobs to the appointment, which allows you to see an 
estimated total of the work to be done. When your customer shows up for their 
appointment, you can then convert this into a Repair Order.

The Repair Orders Page adds some additional functionality to the orders, this is 
for vehicles that have been checked in and are on the property being repaired or 
waiting to be repaired. Here you may still edit the problem description, as well 
as add or remove jobs. Additionally, you will see a list of available technicians 
who aren’t currently assigned to a repair order. You may assign a technician to 
an order here. An Order may also be invoiced from this page, but only if it has 
jobs AND a technician assigned to it (preventing errors). 

Invoicing: When and order is closed you will find the customer's invoice in the 
Resources/Raw/Invoices folder. The filename will be 
(Date Invoiced)_(Customer Name)_(Order#). The invoice is meant to be a customer 
record of the work performed and has the total amount due so that the customer may 
go to the register and pay their bill.

Shop Management: The final view of the application is where a user may add, view, 
edit, and delete technicians from the system. Just like customers, all input is 
validated, and a tech cannot be deleted if they are assigned to a repair order. 
From this screen you may also update the shop's hourly service rate, and the cost 
of shop supplies. Both are used to calculate estimates, and generate amount owed 
on invoices. 

Thank you for using the Mechanic Shop Application, we hope we've made your business 
more efficient and less stressful to manage. Please contact us again should you 
need additional functionality in the future, and we would be happy to scale this 
application up for you!

- Chloe, Denver & Zack (Team 2)
