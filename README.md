# Assignment: Course Sign-up System

## Architectural overview:

Course Signup: 
1. simple Form on frontend that sends a post request to /api/{version}/courses/{courseId}/sign-up
with student information on body 
2. CourseSignUp.Api receives requests and send a message to NEW_SIGN_UPS topic on message (saves to database signup info to db?)
3. CourseSignUp.SignupProcessor reads from NEW_SIGN_UPS topic, checks if user is already signed up (removes/ignores if already signed) and checks if course is already maxed out
4. After deciding if user should be signed sends a message to PROCESSED_SIGN_UPS topic with result, course identifier and student information (maybe send a COURSE_CLOSED when last opening is consumed)
5. CourseSignUp.NotificationsSender reads from PROCESSED_SIGN_UPS topic and sends email alerting the user if they were signed up or not


Aggregation and querying:
1. CourseSignUp.StudentInfoAggregator reads batches of PROCESSED_SIGN_UPS (time-based depending on domain defined query demand) using azure functions and calculates maximum and minimum age of batch
2. Queries read from batch results to determine maximum and minimum of time slices
3. CourseSignUp.StudentInfoAggregator reads from COURSE_CLOSED and stores final max and minumum count from batches
4. Queries are made using get at endpoint /api/{version}/courses/{courseId}/statistics?start=<start_date>&end=<end_date>


