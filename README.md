# SuperFormula Demo InsurancePolicy API

## Overview
This is a Demo InsurancePolicy API Solution in .NET Core 6. The project structure is based on 'Clean Architecture'. I have tried to touch all aspects of the problem, keeping in view the original purpose of this exercise. 

This project uses following major 3rd party libraries: 
 - [RabbitMQ.Client](https://www.rabbitmq.com/dotnet.html) for managing Email messages in Queue.
 - [MediatR](https://github.com/jbogard/MediatR) for loose couping and managing Query and Commands from Controllers.
 - [Fluent Validation](https://fluentvalidation.net/) for DTO/ViewModel validations.
 - [Auto Mapper](https://automapper.org/) for DTO/ViewModel mapping to Entities and vice versa.
 - [Serilog](https://serilog.net/) for Logging.
 - [Moq](https://github.com/moq/moq4) for Mocking DB Context and other services.
 - [xUnit](https://github.com/moq/moq4) for Unit Testing.

 Here is the Solution Structure
 - SF.IY.InsurancePolicy.DempAPI => Main API Project to entertain REST API Requests and it is also hosting a Background service to consume RabbitMQ messages. Ideally, this background service should had been hosted in a separate solution but for this demo, I have added it into the main Project. 
 - SF.IP.Domain => To manage the core domain of the application. 
 - SF.IP.Application => To manage the main Application business logic, Command, Queries, UseCases, Interfaces, DTOs/ViewModels  
 - SF.IP.Infrastructure => To provide external services implementation like DB, Message Queue, State Regulatory Service etc.
 - SF.IP.Tests => To have some basic Unit and Integration Tests

## Implemented Solution
 The solution is based on Clean Architecture principles, following CQRS Pattern. We have one Command (Write operation => Create Insurance Policy) and two Queries (Read operations GetPolicyById & License Number). The solution is configured to use both InMemory and actual Microsoft SQL Server database with Entity Framework Core 6. 
 Once Insurance Policy has been created successfully then a Domain Event is fired (published), which is handled(subscribed) and sent to two RabbitMQ Queues. One queue for 'Accounting' and the other for 'State Regulator Notification'. For the sake of brevity, I have only implemented one RabbitMQ consumer, which is listening for Accounting Queue messages. In the interest of time, this consumer is implemented as a 'BackgroundService' hosted in the same API project. Once the message is received/consumed, then we perform desired operation/logic over it. I think, ideally, these kinds of operations should be managed by external 'processors', and these can be serverless functions (Lambdas) or Microservices.
 The State Regulation Validation service has been implemented in 'SF.IP.Infrastructure' project. For unit testing, I have added some predictable behavior in it. And instead of randomly approving or rejecting insurance policy. It decides based on the policy holder's first name.
 Regarding, the State Regulation Validation service, being a synchronous operation, I will discuss it with my PO/Client and propose to create a Policy with 'Pending' status and once we get approval or rejection from an asynchronous operation then notify the User and UI/Dashboard.

 API also provides Query/Read operations, one can get Policies by providing a valid License Number with some optional params (sorting order and inclusion of expired policies). Users can also get a single Policy by providing its Id and LicenseNumber (though I am not sure why do we need a License Number here ?)
 For validation, I am using the FLuent Validation library. And for managing US Address validation, one option could have been to use some online API to check it. But I have taken a relatively simple route. And have seeded sample US State, City & ZipCode data into a DB. And validate the US address from there.

## API Consumption - Onboarding
 To consume Create Insurance Policy API, please send a POST call to this Endpoint 'api/IP/CreatePolicy'
 Sample Payload is 
 ```
{
  "requestId": "123",
  "insurancePolicy": {
    "id": "",
    "effectiveDate": "2022-05-08T02:18:15.396Z",
    "firstName": "imran",
    "lastName": "khan",
    "licenseNumber": "D6101-40706-60905",
    "address": {"street" : "one street" , "City": "las vegas", "State": "nevada", "ZipCode":"89144"},
    "premiumPrice": { "price": 50000, "currency": "$"},
    "expirationDate": "2024-03-08T02:18:15.396Z",
		"VehicleDetail": {
      "id": "",
      "year": 1996,
      "model": "EagleEyes",
      "manufacturer": "Honda",
     "name": "Civic - EgaleEyes"
    }
  }
}
 ```

 - LicenseNumber is validated using thie RegEx https://regex101.com/r/iW31BV/2/
 
 To get all Policies by LicenseNumber, please send a GET request to this Endpoint '/api/IP/PolicyByLicenseNumber?LicenseNumber=D6101-40706-60901'
 You can add some optional params i.e. 
- SortAscByVehicleRegisterationYear=true
- IncludeExpiredPolicies=true

To get Policy by Id (here I am assuming Id as PK of Policy Entity), please send a GET request to this Endpoint 'api/IP/PolicyById?PolicyId=3fa85f64-5717-4562-b3fc-2c963f66afa6'

## How to Run the Solution
Either use Visual Studio Docker Compose Tools to run the whole solution. Or you can run the solution separately in VS Code or Visual Studio and run RabbitMQ Docker Container by using this command:
  
  ```
   docker run -it --rm --name local-iy-rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
  ```
AppSettings file is configured to connect to RabbitMQ on 'localhost'. You can change its value or can also provide equivalent Environment variables against AppSettings variables as done in Docker Compose file.
If you want to use real SQL Server, then you need to provide its connection string in AppSettings. Or set "UseInMemoryDatabase": true in AppSettings, to use InMemory database.

## State Regulation Validation Method Improvement
 I think once Insurance Policy has been created, then we should fire a Domain Event that will ingest/push it to a Message Queue. And the separate team working on it, implements a Microservice to perform the required operation/bsuiness logic in relation to State regulations there. And once they are done, then they should call main application API (Webhook/Callback URL), and then our application should notify the User and UI/Dashboard of the result.

## Transfering Code Ownership to a new Engineering Team
  I would suggest that team to get them selves familier (if they are not already) with Clean Architecture and CQRS pattern. Then we can have a formal domain and project transfer session, where we can have detailed project walkthrough and any queries they have.
  Regarding productionizing the project, I think first and foremost, application logging needs to go to Cloud Watch. This will help setting up alerts and monitoring mechanism. We can also setup some Slack alerts for critical application events.
  For PII data protection, there can be muli level protection. At tarnsfer level, we can define Lambda@Edge to setup field-level encryption of sensitive data. Or we can encrypt our complete payload. At rest we can setup auto encryption & decryption at database level. There are other AWS services too, which can help us resolve this requirenment like AWS DataBrew.

## AWS Deployment Options
There are three main parts of this application and I am proposing following AWS Services for them 
- RabbitMQ Message Queue: I think we should use AWS SQS for it. We need to make small adjustments in code for it. Another option here could be to use Cloudamqp to use RabbitMQ as a Service.
- Main Project/API: This project is already running on Docker Container, so for high availability and fault tolerance, we can deploy it on AWS ECS Fargate with Application Load Balancer.
- RDBMS: As a best practice, we should not use Docker for containerizng our Databse. And If we have MS SQL Server license, then we can use AWS RDS for SQL Server. Otherwise we can use AWS Aurora DB, either MySQL or PostgreSQL. But for this we have to make some code adjsutments.
For Logging and Monitoring we should use AWS Cloud Watch. And for DB credentials we can use AWS Secrets Manager.