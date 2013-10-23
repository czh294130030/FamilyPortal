use FamilyDB
go

--create table UserInfo
create table UserInfo
(
	userID int primary key identity(1,1),
	account nvarchar(50),
	userName nvarchar(50),
	password nvarchar(50)
)

--insert UserInfo
insert into UserInfo(account,userName,password) values('chad','chad.cao','870602');
insert into UserInfo(account,userName,password) values('catherine','catherine.yu','870902');

--select UserInfo
select * from UserInfo

--create table ConsumeType
create table ConsumeType
(
	typeID int primary key identity(1,1),
	[description] nvarchar(500)
)

--insert ConsumeType
insert into ConsumeType([description]) values('food');
insert into ConsumeType([description]) values('clothing');
insert into ConsumeType([description]) values('shelter');
insert into ConsumeType([description]) values('transportation');
insert into ConsumeType([description]) values('others');

--select ConsumeType
select * from ConsumeType

--create table DailyConsume
create table DailyConsume
(
	dailyID int primary key identity(1,1),
	amount decimal(18,2),
	[date] date
)
--select DailyConsume
select * from DailyConsume

--create table Consume
create table Consume
(
	consumeID int primary key identity(1,1),
	[description] nvarchar(500),
	amount decimal(18,2),
	typeID int,
	dailyID int
)
--select Consume
select * from Consume

--create table BankCard
create table BankCard
(
	cardID int primary key identity(1,1),
	cardNO nvarchar(50),
	amount decimal(18,2),
	dateFrom date,
	dateTo date,
	userID int,
	cardTypeID int,
	bankID int,
	cityID int,
	isUsing bit

)
--select BankCard
select * from BankCard

--create table ParaInfo
create table ParaInfo
(
	infoID int primary key identity(1,1),
	[description] nvarchar(50) 
)
--insert ParaInfo
insert into ParaInfo([description])values('card type');
insert into ParaInfo([description])values('bank');
insert into ParaInfo([description])values('city');

--select ParaInfo
select * from ParaInfo

--create table ParaDetail
create table ParaDetail
(
	detailID int primary key identity(1,1),
	[description] nvarchar(50),
	infoID int
)

--insert ParaDetail
insert into ParaDetail([description],infoID) values('bank card',1);
insert into ParaDetail([description],infoID) values('bank book',1);
insert into ParaDetail([description],infoID) values('Bank of China',2);
insert into ParaDetail([description],infoID) values('Industrial and Commercial Bank of China',2);
insert into ParaDetail([description],infoID) values('Agricultural Bank of China',2);
insert into ParaDetail([description],infoID) values('Bank of Communications',2);
insert into ParaDetail([description],infoID) values('China Construction Bank',2);
insert into ParaDetail([description],infoID) values('China Citic Bank',2);
insert into ParaDetail([description],infoID) values('China Merchants Bank',2);
insert into ParaDetail([description],infoID) values('Bank of JiangSu',2);
insert into ParaDetail([description],infoID) values('Others',2);
insert into ParaDetail([description],infoID) values('suzhou',3);
insert into ParaDetail([description],infoID) values('jiaxing',3);
insert into ParaDetail([description],infoID) values('yaan',3);
--select ParaDetail
select * from ParaDetail

