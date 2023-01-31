create table weekdays (
  Id serial primary key,
  WeekDay varchar(50) NOT NULL
);

create table employees (
  Id serial primary key,
  FirstName varchar(50) NOT NULL

);

create table workschedule (
  Employees_id int NOT NULL,
  WeekDay_id int NOT NULL,
  foreign key(Employees_id) references Employees(id),
  foreign key(WeekDay_id) references WeekDay(id),


);