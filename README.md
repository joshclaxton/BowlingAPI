[![Build status](https://joshclaxton.visualstudio.com/Root/_apis/build/status/BowlingAPI)](https://joshclaxton.visualstudio.com/Root/_build/latest?definitionId=2)

# Bowling API

Azure Function to calculate the score of a bowling game

## How To Run

1. Download [.Net Core 2.1.* SDK](https://dotnet.microsoft.com/download/dotnet-core/2.1) since Azure Functions do not support .Net Core 2.2 yet
2. Run 
	- Visual Studio 2017
		- Run app by setting `Bowling.Function.csproj` as startup project and pressing "play" button
		- Use `Test Explorer` to run Unit Tests

## CI/CD Through Azure DevOps

PR Requests must build and successfully pass all unit tests to be merged to master. Once in master, the function is continuously pushed to the live example.

## Live Example

- Input Example: `23X4SXXXXXXX23` (entered as parameter in the URL)
    - Roll: `0-9`
    - Spare: `S`
    - Strike: `X`

Hosted version can be found at https://bowlingapi.azurewebsites.net/api/CalculateBowlingScore/23XXXXXXXXX23
