select * from Consume
select * from ConsumeType
select * from DailyConsume

--�������ͣ�ʱ���ȡ���ѽ��--
declare @startDate datetime
declare @endDate datetime
set @startDate='2013-2-1'
set @endDate='2013-2-28'
select ConsumeType.description as '��������',SUM(Consume.amount) as '���ѽ��' from Consume 
inner join ConsumeType
on Consume.typeID=ConsumeType.typeID
where Consume.dailyID in
(
select dailyID from DailyConsume 
where date between @startDate and @endDate
)
group by ConsumeType.description,ConsumeType.typeID
order by ConsumeType.typeID asc

--�ݸ�ʱ���ȡ������--
select SUM(amount) as '���ѽ��' from DailyConsume 
where date between @startDate and @endDate

--��ȡһ�����ѽ��
declare @testDate datetime
declare @dailyID int 
set @testDate='2013-2-8'
select @dailyID=dailyID from DailyConsume where date=@testDate
select SUM(amount) from Consume where dailyID=@dailyID 


