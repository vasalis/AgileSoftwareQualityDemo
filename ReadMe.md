# Agile Software Code Quality - Testing

## Steps of the demo

* Create a .Net Core API Project
* Add a Test Project 
* Add a Unit test for the WeatherForecast Controller
* Locally run tests
* Add solution to git (hub) source control
* Create a pipeline (GitHub) action, to run build and Unit tests in each code commit
* Add a new Controller (Students) that will have CRUD operations with a database (CosmosDB)
* Create a test for that (unit/integration)
* Locally run tests
* Run tests with pipeline
* Deploy to an environment - show issue at hand
* Add Application Insights / Troubleshot issue
* Load test with JMeter
* Change code / make it slow

### Notes
Many of those step can be seen from the git repo history.

## Various Resources

* Testing in .Net Core, Best Practices
https://docs.microsoft.com/en-us/dotnet/core/testing/
https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices

* Setup GitHub Action for publishing to Azure App Service
https://docs.microsoft.com/en-us/azure/app-service/deploy-github-actions?tabs=applevel

* Start with Application Insights - for .Net core
https://docs.microsoft.com/en-us/azure/azure-monitor/learn/dotnetcore-quick-start

* JMeter download
https://jmeter.apache.org/download_jmeter.cgi
