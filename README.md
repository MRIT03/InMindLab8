# Hello Elie !

This is the modified submission of Lab 8. I put all the files in one place.

# RabbitMq

## Sender

To implement the communication between both microservices. The main server acted as the publisher and created an exchange with the name "CourseExchange" and prepared it for whenever it needed to send messages. The code for this can be found in the services of the application layer. I decided to go with a private constructor and instantiate the object with an async factory. This was done because many parts of the code needed to be awaited, and this method made sure the async parts were handled properly. The Message was serialized and sent as JSON, this made sese as the object can be easily deserialized and read into an Entity on the receiver's end.

## Receiver 

From the receiving side, I implemented a simple hosted service that makes sure that the receiving microservice is listenting in on the "CourseExchange" and has an active consumer on it. Once the publisher publishes a message, the consumer intercepts it, uses JSON to deserialize it and changes and converts it into an entity, the entity is then sent to a handler which will create the course inside the database using the DbContext and some predefined repositories. 

# Multi Tenancy

## Persistence Layer

The following database was created:


![image](https://github.com/user-attachments/assets/69d97034-23ab-4eb0-a484-7b991ca9bfc4)


The database was first codded as entities in dotnet, and it was later translated into PostgreSql through the entity framework. To do so, the entities were declared in the Entities folder as part of the Domain section.

The dataase was then divided into several schemas. The student schema containing the student table. The admin schema contains the admin table. Finally, the teacher schema contains the teacher table and the teacher_course table. The course table and the enrollement tables were left public as it made sense to be accessible by different tenants.

In the Persistence layer, I installed the Entity Framework and declared it as a dependency, then I defined the DBContext as UmcContext.

Finally, to allow the movement of objects between the Persistence layer and other layers I used the Repository pattern.

## API Layer

To enforce the multi tenancy, I used different controllers and different endpoints. The idea being that the student branch would be on the student controller, the admin branch on the admin controller and the teacher branch on the techer controller. Each Branch had access to its schema only, except for the admins which had read access to the two other scehmas. 

To enforce proper access to each endpoint, I implemented authentication and authorization, where the user would need to prove their role using a JWT Token before accessing the controller.




