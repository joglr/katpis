# Katpis - A Kattis CLI
Katpis is a command-line interface for [kattis.com](https://www.kattis.com/)

Katpis allow you to fetch sample files, test your program, and submit your solution, all within your favorite commmand-line.

It is inspired by the official, but barebones CLI at [github.com/Kattis/kattis-cli](https://github.com/Kattis/kattis-cli)

![katpis submit](https://i.gyazo.com/d593615497aaa9a93966bbdfb9b3e946.gif)

## Supported languages
- Java

We are working on support for:
- C#
- Python

 ... and others - feel free to contribute!

## Installation
### Windows
Currently, we don't automatically build installers, so for now installation steps are as follows:

1. Install [.NET Core SDK](https://dotnet.microsoft.com/download)

1. Clone this repository

1. Add the root of this repository to your path enviroment variables - e.g. `C:\Users\myuser\somefolder\katpis`

Now you can run the command `katpis` anywhere!

### Mac/Linux
We will provide installation instructions for these platforms in the near future. You might be able to get it to work using the instructions for Windows.

## Available commands
### katpis submit

Submits a `.java` file from the current directory to Tattis with a matching name to a Kattis problem shortname.

#### Usage
    katpis submit <filename-with-extension>

#### Example
    katpis submit Hello.java

### katpis test
Tests any matching `.in` and `.ans` files on any `.java` problem in the current directory.

#### Usage
    katpis test

### katpis fetch
Fetches any available sample files, given the shortname of any Kattis problem.

#### Usage
    katpis fetch <kattis-problem-shortname>
#### Example

For the problem "Quality-Adjusted Life-Year" at [open.kattis.com/problems/qaly](open.kattis.com/problems/qaly), run 

    katpis fetch qaly
    
You should now see the newly downloaded files in your current directory, if any sample files were available.

### katpis template

Generates a new file from a template with basic input parsing.

#### Usage

    katpis template <filename-with-extension>

#### Example

    katpis template Hello.java
    
The above command will generate a `.java` file to the current directory.

## Development

### Useful commands

To run katpis using .NET Core SDK, the following command can be used:

```
dotnet run [args]
```

You can pass any arguments, e.g. sub command and its arguments, after `dotnet run`

To rebuild and restart the program, every time the source code changes, use the following command:

```
dotnet watch run [args]
```
