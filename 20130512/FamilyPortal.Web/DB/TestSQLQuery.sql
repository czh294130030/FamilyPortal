select * from Consume
select * from ConsumeType
select * from DailyConsume

--根据类型，时间获取消费金额--
declare @startDate datetime
declare @endDate datetime
set @startDate='2013-2-1'
set @endDate='2013-2-28'
select ConsumeType.description as '消费类型',SUM(Consume.amount) as '消费金额' from Consume 
inner join ConsumeType
on Consume.typeID=ConsumeType.typeID
where Consume.dailyID in
(
select dailyID from DailyConsume 
where date between @startDate and @endDate
)
group by ConsumeType.description,ConsumeType.typeID
order by ConsumeType.typeID asc

--据跟时间获取总消费--
select SUM(amount) as '消费金额' from DailyConsume 
where date between @startDate and @endDate

--获取一天消费金额
declare @testDate datetime
declare @dailyID int 
set @testDate='2013-2-8'
select @dailyID=dailyID from DailyConsume where date=@testDate
select SUM(amount) from Consume where dailyID=@dailyID 


