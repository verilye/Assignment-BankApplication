# Web Development Technologies Assignment 2

## About me
Name: Connor Logan
StudentNumber: s3768929
Github Repo: https://github.com/rmit-wdt-sp2-2023/s3768929-a2.git 

## Note on Validation
As this assignment was completed on a tight deadline, I made the decision
to handle validation mostly on the backend as is specified in the assignment
criteria. The app wont let you insert or modify bad data, but it handles errors
in an ugly way sometimes. 

## Design Choices
In an attempt to adhere to best practices for a webapp of this ilk I tried
to implement the repository deisgn pattern across my entire project. 
I used razor pages and bootstrap 5 for handling front end stuff as I originally
assumed those were mandatory for the assignment specs. 

## Note on Unit Tests
I unit tested all the methods and classes that have notably complex logic. 
I used theories notably in HomeRepositoryTests and LoginControllerTests.
I tried to use the repository pattern throughout my whole project, so chose not to
unit test many intermediary repository methods as they provide no logic other than
passing a value to data access and returning that value back. In addition,
most of the data access operations are incredibly similar and simple, so I would
argue there is very little point to testing them other than an arbitrary code coverage
percentage.


