# Crm-RequestInspector

[![Build status](https://ci.appveyor.com/api/projects/status/3tcohcdgah20agkv/branch/master?svg=true)](https://ci.appveyor.com/project/shanec-/crm-requestinspector/branch/master)

Request Inspector for Dynamics CRM. It logs the raw requests being made to Dynamics CRM via the Organization Service.

Inspired by the blog post by Lucas Alexander: 
[http://alexanderdevelopment.net/post/2013/02/21/accessing-raw-soap-requests-responses-from-dynamics-crm-web-services-in-c/](http://alexanderdevelopment.net/post/2013/02/21/accessing-raw-soap-requests-responses-from-dynamics-crm-web-services-in-c/)


## Usage

Perform the following steps in order to add a new operation:

1. Create a a new class inherited from the `CrmRequestInspector.OperationBase` base class and give it an approrpriate name.
2. Implement the abstract method `Execute()` with the operation.
3. Compile the project. 
4. When ready to execute, run the CrmRequestInspector by passing the newly created class name as the `-o` parameter:
        `CrmRequestInspector.exe -o {your class name}`
5. Examine the logs in the `Log` folder in order to get the raw request.


## Command Line Arguments

- `--help`  Get a summary of available commands.
- `-o`      Required. List of operations to be executed. Multiple operations can be chanined together.
- `-r`      When this flag is provided, the application logs the error but attempts to resume to the next operation. 

        `CrmRequestInspector.exe -o Operation1 Operation2 Operation3 -r`