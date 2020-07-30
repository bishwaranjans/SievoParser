# SievoParser.Console
The SievoParser.Console application has been designed in-line with SOA and Domain Driven Design(DDD) to provide end user a console application to process tsv file report and providing simulated output in real-time and dynamic environment.

The application is supporting the following command line arguments (only the first one is mandatory). The arguments are case in-sensitive:

1. --file <path>             full path to the input file
2. --sortByStartDate         sort results by column "Start date" in ascending order
3. --project <project id>    filter results by column "Project"

## Implementation Requirement
The following requirements define the program functionality
and refer to the data sample below:

1. Input data is tab-separated UTF-8 text with a header row.

2. It only includes the columns listed below.

3. Dates (Start date) should conform to the format yyyy-mm-dd hh:mm:ss.sss

4. Money (Savings amount) values should conform be numbers with a point as the decimal separator

5. Columns "Savings amount" and "Currency" can have missing values denoted
as NULL. Those should be printed as empty strings.

6. Column "Complexity" has a certain set of values (Simple, Moderate, Hazardous).
The program should report an error if a source value differs from those three
but keep in mind that more options (e.g. "VeryHigh") can be added in the future.

7. The output should also have a header line.

8. Lines that are empty or start with comment mark # are skipped.

9. Order (but not names) of columns might be changed in future.

10. In case of an invalid source value (in a date, money or Complexity column) a descriptive error message should be printed to console and the program terminated.

  ![alt text](https://github.com/bishwaranjans/SievoParser/blob/master/Documentation/SampleInput.PNG)

## Solution Architecture

DDD approach has been used for designing the architecture of the solution by clearly segregating the each responsibility with clear structure.
 - **SievoParser.Console** : It is user interface of our solution signifying starting of the application and further processing. This is the entry block of our program. This accepts the arguments, parsed it and propagate for further operation.
 - **SievoParser.Domain** : Responsible for representing concepts of the business, information about the business situation, and business rules. State that reflects the business situation is controlled and used here, even though the technical details of storing it are delegated to the infrastructure. This layer is the heart of our solution.
 - **SievoParser.Infrastructure** : Responsible for how the data that is initially held in domain entities (in memory) or another persistent store. It contains all our parsing logic along with validation.
 - **SievoParser.Tests** : Responsible for mirroring the structure of the code under test.
 
 ![alt text](https://github.com/bishwaranjans/SievoParser/blob/master/Documentation/DependenciesGraph.png)
 
 ## Design Patterns
 
The main focus during the development was to use composition over inheritance. Henceforth, **Facade** and **Abstract Factory** design patterns have been incorporated to design the application. The primary focus was to accommodate multiple parser into the application. Currently it is supporting TSV parsing and later on it provides the extensibility to support any other parsing like CSV or EXcel etc. Basic SOLID design patterns has been followed wherever possible. 

 ## Coding Guidelines
 The Microsoft recommended coding guidelines is used.

## Libraries Used
In order to avail the flexibility provided by many 3rd party libraries for parsing, below libraries are used in this application. They are simple to configure and provides a clean efficient way of implementation. 
1. **CsvHelper** - https://joshclose.github.io/CsvHelper/
2. **CommandLineParser** - https://github.com/commandlineparser/commandline

 ## Configuration
 As per the requirement, all the application settings are being retrieved from **App.config** file. The allowed **Complexity** values can be configured here.
