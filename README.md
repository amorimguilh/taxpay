# == TaxPay Technical Evaluation ==

* Create a simple Blazor project that displays a list of Source bank accounts and Target Tax Accounts (each have a Name and a Balance)
There should be a UI to move money from a source account to a target tax account.
* When money is transferred, the bank account's balance diminishes while the tax account increases.
* The Blazor should only be the UI and send REST requests to a simple .NET backend.
* Error checking is done on the backend.
* Write a minimum of 5 tests on the back-end, and 5 tests on the front-end.
* Create a docker-compose file so when executing `docker-compose up`, the front-end starts on port 3001 and back-end on 5001, and the solution is ready to be used by typing http://localhost:3001 in browser
* Use an In-Memory database with Entity Framework so we don't have to create DB.
* Privately share github with `pocharlebois`

BE
    * Error checking is done on the backend.
    * When money is transferred, the bank account's balance diminishes while the tax account increases.

FE
    * Create a simple Blazor project that displays a list of Source bank accounts and Target Tax Accounts (each have a Name and a Balance)
    * The Blazor should only be the UI and send REST requests to a simple .NET backend.

Minimum of 5 Unit tests for backend and frontent

CQRS
UI with logo an colors of taxpay
In MemoryDB
Lock the account

Section of how it can be improved
Diagrams
Database Schema
    
