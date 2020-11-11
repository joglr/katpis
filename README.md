# Katpis

A command-line interface for kattis.com

Currently only supports java, we are working on support for c#, python and others.

## Windows usage

Clone this repository

Add the root of this folder to your path enviroment variables e.g.: `C:\\Users\myuser\somefolder\katpis`

Now you can run the command `katpis` anywhere!

# Avalible commands

## Submit
`submit <filename-with-extension>`

Submits a .java file from the current directory to kattis with a matching name to a kattis problem shortname.

Example:
For the problem "Take Two Stones" at open.kattis.com/problems/twostones, run `katpis submit Twostones.java`

## Test
`test`

Tests any matching .in and .ans files on any .java problem in the current directory.

## Fetch
`fetch <kattis-problem-shortname>`

Fetches any available sample files given the shortname of any kattis problem.

Example: For the problem "Quality-Adjusted Life-Year" at open.kattis.com/problems/qaly, run `katpis fetch qaly`, and if there is sample files avalible, they will be downloaded and placed in the current directory.

## Template
`template <filename-with-extension>`

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
