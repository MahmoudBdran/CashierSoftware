--select * from(SELECT row_number() over(order by CONVERT (date, saleDate ,103) desc) AS num_row, CatSerial,CatName FROM SalesHistory ) as productlinenumber where num_row between 1 and 30;
--row_number() over(order by CONVERT (date, saleDate ,103) desc) AS num_row
--select ROW_NUMBER() over(order by CONVERT (date, saleDate ,103) desc) as Id_s ,CatSerial , CatName from SalesHistory ;
select * from (select row_number() over(order by CONVERT (date, saleDate ,103)) AS num_row , CatSerial,CatName,CatQuantity,BillNumber,
Price,saleTime,CONVERT (datetime, saleDate ,103) as saleDate,BasePrice,Discount,FinalPrice,CustomerPay,Rest,
Merchant from SalesHistory sh join sellingBillInfo sb on sh.BillNumber = sb.SellingBillnumber
) as sales where num_row between 1 and 20 ORDER BY saleDate desc, saleTime desc;
--select * from (select row_number() over(order by CONVERT (date, saleDate ,103) desc) AS num_row , CatSerial,CatName,CatQuantity,BillNumber,Price,saleTime,saleDate,BasePrice,Discount,FinalPrice,CustomerPay,Rest,Merchant  from SalesHistory sh join sellingBillInfo sb on sh.BillNumber = sb.SellingBillnumber ) as sales where saleDate = '24/10/2022' ORDER BY saleDate desc, saleTime desc ;
--select CatSerial,CatName,CatQuantity,BillNumber,Price,saleTime,CONVERT (date, saleDate ,103) as saleDate,BasePrice,Discount,FinalPrice,CustomerPay,Rest,Merchant from SalesHistory sh join sellingBillInfo sb on sh.BillNumber = sb.SellingBillnumber ORDER BY saleDate desc, saleTime desc;