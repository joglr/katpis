# Katpis

A command-line interface for kattis.com

Currently only supports java, we are working on support for c#, python and others.

## Windows usage

Clone this repository

Add the root of this folder to your path enviroment variables e.g.: `C:\\Users\myuser\somefolder\katpis`

Now you can run the command `katpis` anywhere!

## Avalible commands

### `submit <filename-with-extension>`

This command is a work in progress

Example: `katpis submit hello.java`

### `test`

Finds a .java file in the current directory with a .java file extension, runs it with any .in files, and compares the output with any .ans files in the directory.

### `fetch <kattis-problem-shortname>`

Example: For the problem "Hello World!" at open.kattis.com/problems/hello

Run `katpis fetch hello`, and if there is sample files avalible, they will be downloaded to the current directory.

### `template <filename-with-extension>`

Example: `katpis template Hello.java`, will generate a java file with some parsing template content, to the current directory.

## Development

### Useful commands:

```
dotnet watch run
```

```
dotnet run
```

```
 git add -p
```
