create table news_letter(
 email_id varchar(50) 
);

create table city(
 postal_code int NOT NULL PRIMARY KEY,
 city_name varchar(50) NOT NULL
);

create table address(
address_id int NOT NULL PRIMARY KEY,
street_name varchar(50) NOT NULL,
flat_no varchar(50) NOT NULL,
postal_code int NOT NULL FOREIGN KEY REFERENCES city(postal_code)
);

create table customer(
customer_id int NOT NULL PRIMARY KEY,
first_name varchar(20) NOT NULL,
last_name varchar(20) NOT NULL,
email_id varchar(50) NOT NULL UNIQUE,
mobile_number int NOT NULL UNIQUE,
password nvarchar(50) NOT NULL,
address_id int NOT NULL FOREIGN KEY REFERENCES address(address_id)
);

create table payments(
order_id int NOT NULL PRIMARY KEY,
transaction_id int NOT NULL,
card_number int NOT NULL,
expirydate int NOT NULL,
cvv int NOT NULL,
amount float NOT NULL,
promo_code nvarchar(50) NOT NULL
);

create table refund(
invoice_id int NOT NULL PRIMARY KEY,
refund_amount int NOT NULL
);

create table invoice(
customer_id int NOT NULL PRIMARY KEY,
invoice_id int NOT NULL FOREIGN KEY REFERENCES refund(invoice_id),
order_id int NOT NULL FOREIGN KEY REFERENCES payments(order_id)
);

create table bookaservice(
customer_id int NOT NULL PRIMARY KEY,
order_id int NOT NULL FOREIGN KEY REFERENCES payments(order_id),
address_id int NOT NULL FOREIGN KEY REFERENCES address(address_id),
service_date int,
service_time int,
service_hours int,
extra_service varchar(50),
comments varchar(100),
);

create table cancel_order(
order_id int NOT NULL PRIMARY KEY,
description varchar(100)
);

create table reschedule_order(
order_id int NOT NULL PRIMARY KEY,
new_date int NOT NULL,
new_time int NOT NULL,
description varchar(100)
);

create table blocked_user(
customer_id int NOT NULL PRIMARY KEY,
order_id int NOT NULL FOREIGN KEY REFERENCES payments(order_id),
isFavourite bit,
isBlocked bit
);

create table service_provider(
helper_id int NOT NULL PRIMARY KEY,
first_name varchar(20) NOT NULL,
last_name varchar(20) NOT NULL,
email_id varchar(50) NOT NULL UNIQUE,
mobile_number int NOT NULL UNIQUE,
password nvarchar(50) NOT NULL,
address_id int NOT NULL FOREIGN KEY REFERENCES address(address_id)
);

