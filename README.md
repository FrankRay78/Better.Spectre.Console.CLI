# Better.Spectre.Console.CLI

Better.Spectre.Console.CLI is a drop-in replacement for Spectre.Console.Cli that contains upstream bug fixes and new features. 

As the current Spectre.Console CLI maintainer, I'm well-positioned to address upstream issues quickly, but the lack of a second active maintainer has caused my PRs to remain unmerged for longer than I'd like. Hence the creation of Better.Spectre.Console.CLI. 

I do remain committed to Spectre.Console and my fellow maintainers, and I will support the integration of these enhancements into the main repository.

<br />


## Installation
Install via NuGet:

```bash
dotnet add package Better.Spectre.Console.CLI
```

<br />


## Usage
Replace `Spectre.Console.Cli` with `Better.Spectre.Console.CLI` in your project.

For full documentation, refer to the [Spectre.Console](https://github.com/spectresystems/spectre.console) repository.

<br />


## Releases

### Version 1.0.1

Upstream bug fixes not included in the latest official `Spectre.Console.Cli` package:

- Made trimming of `TestConsole` output by `CommandAppTester` user-configurable  
  *(Upstream Issue: [#1738](https://github.com/spectreconsole/spectre.console/issues/1738), PR: [#1739](https://github.com/spectreconsole/spectre.console/pull/1739))*

- Added support for conditional trimming of trailing periods from argument and option help descriptions  
  *(Upstream Issue: [#1729](https://github.com/spectreconsole/spectre.console/issues/1729), PR: [#1740](https://github.com/spectreconsole/spectre.console/pull/1740))*

#### Dependencies

- `Spectre.Console` NuGet package: **v0.49.1**

<br />


## License
Distributed under the MIT license. See `LICENSE` for more information.
