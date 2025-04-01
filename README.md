# `Better.Spectre.Console.CLI`

A drop-in replacement for Spectre.Console.Cli containing upstream bug fixes and new features.

<br />

## Enhancements

As the current Spectre.Console CLI maintainer, I'm well-positioned to address upstream issues quickly, but the lack of a second active maintainer has caused my PRs to remain unmerged for longer than I'd like. Hence, the creation of Better.Spectre.Console.CLI. 

My personal goal is to complete 15 - 20 enhancements each year, although I remain committed to Spectre.Console and my fellow maintainers whom I will support with the integration back into the main repository.

<br />


## Features

Better.Spectre.Console.CLI includes the following upstream bug fixes and new features, which Spectre.Console.Cli 0.49.1 does not contain.

Description | Issue | PR
--- | --- | --- 
Made trimming of TestConsole output by CommandAppTester user-configurable  | [#1738](https://github.com/spectreconsole/spectre.console/issues/1738) | [#1739](https://github.com/spectreconsole/spectre.console/pull/1739)
Added support for conditional trimming of trailing periods from argument and option help descriptions | [#1729](https://github.com/spectreconsole/spectre.console/issues/1729) | [#1740](https://github.com/spectreconsole/spectre.console/pull/1740)

<br />


## Installation
Install via NuGet:

```bash
dotnet add package Better.Spectre.Console.CLI
```

<br />


## Usage
Replace `Spectre.Console.Cli` with `Better.Spectre.Console.CLI` in your project.

The official [Spectre.Console](https://spectreconsole.net/) website contains extensive documentation.

<br />


## Releases

See [CHANGELOG](CHANGELOG.md) or [Releases](https://github.com/FrankRay78/Better.Spectre.Console.CLI/releases) for the release history.

<br />


## License
Distributed under the MIT license. See `LICENSE` for more information.

<br />


## Contact
Frank Ray - [LinkedIn](https://www.linkedin.com/in/frankray/) - [Better Software UK](https://bettersoftware.uk)

Project Link: [https://github.com/FrankRay78/Better.Spectre.Console.CLI](https://github.com/FrankRay78/Better.Spectre.Console.CLI)
