# Bowling API

Azure Function to calculate the score of a bowling game

## How To Run

1. Download [.Net Core 2.1.* SDK](https://dotnet.microsoft.com/download/dotnet-core/2.1) since Azure Functions do not support .Net Core 2.2 yet
2. Run 
	- Visual Studio 2017 (tested)
		- Run app by setting `Bowling.Function.csproj` as startup project and pressing "play" button
		- Use `Test Explorer` to run Unit Tests
	- VS Code (untested)
		- Follow these [instructions ](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local)
		- This is a bit more involved, but is feasible.

## Example

- Input Example: `23X4SXXXXXXX23` (entered as parameter in the URL)
    - Roll: `0-9`
    - Spare: `S`
    - Strike: `X`
