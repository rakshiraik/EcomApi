This solution includes three projects:

1.EcomApi: This is the entry point to the application where you can find all the controllers.

2.EcomApi.services :Where the business/data access logics are defined

3.EcomApi.Db : This is the SQL Server database project. When any database modification is done, it should be updated via schema compare. When we host in a new environment, it should be published so that a new database is created.



There is one api Post endpoint (https://localhost:7208/api/Customer) 

which accepts the data in the following formate
{ 
	"user": "bob@aol.com",
	"customerId": "C34454"
}

And it returns the details of the most recent order in JSON like this
{
	"customer": {
		"firstName": "Bob",
		"lastName": "Marshal"
			},
	"order": {
		"orderNumber": 456
		"orderDate": "28-Oct-2023",
		"deliveryAddress": "1A Uppingham Gate, Uppingham, LE15 9NY",
		"orderItems": [
			{ "product": "Tennis Ball", "quantity": 2, "priceEach": 30 },
			{ "product": "Tennis Gear", "quantity": 1, "priceEach": 120 },
			{ "product": "Tennis Racket", "quantity": 1, "priceEach": 75 }
		],
		"deliveryExpected": "04-May-2021"
	}
}