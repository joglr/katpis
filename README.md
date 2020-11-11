# Katpis
Katpis is a command-line interface for [kattis.com](https://www.kattis.com/developers/)

## Supported languages
- Java

We are working on support for:
- C#
- Python

 ... and others - feel free to contribute!

## Installation
### Windows
Currently we don't automaticly build installers, so for now installation steps are as follows:

1. Install [.NET Core SDK](https://dotnet.microsoft.com/download)

1. Clone this repository

1. Add the root of this folder to your path enviroment variables - e.g. `C:\Users\myuser\somefolder\katpis`

Now you can run the command `katpis` anywhere!

### Mac/Linux
We will provide installation instructions for these platforms in the near future. You might be able to get it to work using the instructions for Windows.

## Available commands
### katpis submit
This command is still work in progress

#### Usage
    katpis submit <filename-with-extension>

#### Example
    katpis submit hello.java

### katpis test
Looks for a file in the current directory with a `.java` file extension, runs it with any `.in` files as standard input, and compares the output with any `.ans` files in the directory.

#### Usage
    katpis test

### katpis fetch
Attemps to download sample files for a given problem. If available, they will be downloaded to the current directory.

#### Usage
    katpis fetch <kattis-problem-shortname>
**Example**: 

For the problem "Hello World!" at [open.kattis.com/problems/hello](https://open.kattis.com/problems/hello), run 

    katpis fetch hello
    
You should now see the newly downloaded files in your current directory.

### katpis template

Generate a new file from a template with basic input parsing.

#### Usage

    katpis template <filename-with-extension>

#### Example

    katpis template Hello.java
    
The above command will generate a `.java` file to the current directory.

## Development

### Useful commands

To run katpis using .NET Core SDK, the following command can be used:

```
dotnet run
```

You can pass any arguments, e.g. sub command and its arguments, after `dotnet run`

To rebuild and restart the program every time the source code changes, use the following command:

```
dotnet watch run
```

To interactively stage changes to git, use the following command:

```
 git add -p
```
