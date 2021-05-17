# Assignment: Course Sign-up System

## Architectural overview:

### Characteristcs
The proposed solution was made with three main architecture characteristics in mind: Scalability, Testability and Performance. Each characteristic is achieved through the following:

Scalability:
I use Azure functions to ensure that the software can scale on demand. I would also use kubernetes for the api project. As a decoupling mechanism, I use messaging services and topics to enable several different functions being called from the same event and to avoid having too many responsabilities in a single function.

Testability:
To enable a project that is easy to test, I separated the concerns in the layers Domain, Applications (api and message processors) and Infraestructure. Each layer depends on the others through abstractions that can be mocked out to test.

Performance:
The problem at hand was divided in smaller pieces to be handled by different machines (and in some cases different times) to enable parallelized work through modularization.

### Diagram

see `docs/Architecture/overview.drawio`

## Solution summary
### Course Signup: 
1. simple Form on frontend that sends a post request to /api/v1/courses/{courseId}/sign-up
with student information on body 
2. `CourseSignUp.Api.CoursesController` receives requests and send a message to `NEW_SIGN_UPS` topic on message
3. Azure Function `CourseSignUp.MessageProcessors.ProcessSignUp` reads from `NEW_SIGN_UPS` topic, checks if user is already signed up (ignores if already signed) and checks if there are available seats
4. After deciding if user should be signed sends a message to `PROCESSED_SIGN_UPS` topic with result, course identifier and student information
5. `CourseSignUp.MessageProcessors.NotificationsSender` reads from `PROCESSED_SIGN_UPS` topic and sends email alerting the user if they were signed up or not


### Aggregation and querying:
1. `CourseSignUp.MessageProcessors.BatchAcceptedStudents` reads messages from `PROCESSED_SIGN_UPS` filtered by Student acceptance and batches then in distributed cache for later use 
2. Azure Function `CourseSignUp.MessageProcessors.StudentInfoAggregator`  batches of `PROCESSED_SIGN_UPS` (with time-based schedules based on business defined query demand) and process than in time slices per course
3. Queries are made using get at endpoint /api/v1/courses/{courseId}/statistics?start={start_date}&end={end_date} 
4. Queries read from batch results to determine maximum and minimum of time slices


## Test Strategy

To make sure the solution works we would need to unit test the domain services CoursesServices and StatisticsServices. Then we would need to make sure that all atomic repository and service operations work properly (`ICoursesRepository.ConsumeAvailableSeat` and `IDistributedCache.ConsumeAllStored`). Then we'd have to unit test all Azure functions

## Tools

- Moq
- Azure Functions
- Azure Service Bus

I would also use Azure Kubernetes Service to deploy the ASP.NET API, use MongoDB to store courses and lecturers data and use Redis for distributed caching

## Next Steps

- Implement repositories and external service integration
- Try to switch Azure Service Bus for kafka to use its message storing
- Add more logging and exception handling
- Add domain validation to course creation
- Add a student remove function for lecturers and students themselves
- add more statistics. E.g. a signup date aggregation to check at which time courses get more sign ups

## Notes

To aggregate information I used batches dispatched from message topics. Another interesting approach would be to send the course id to a `NEEDS_STATS_UPDATE` queue and read from this queue every five minutes (or some other time) and aggregate the results. The main advantage of this would be to remove the need for a distributed cache and the main disadvantages would be increased complexity to get all applicable messages (for that time slice) and having to aggregate over all student infomation at every aggregation. 
