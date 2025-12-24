# Contributing to Ascorbic

Thanks for your interest in the Ascorbic library! 💞

When you make a contribution to Ascorbic, either by [creating an issue report](https://github.com/aridityteam/ascorbic/issues/new)
or [submitting a pull request](https://github.com/aridityteam/ascorbic/compare). We (The Aridity Team) want to be able to use your
contribution to improve the library for other people, and for other active Aridity projects.

We welcome contributions of all kinds — code, documentation, bug reports, ideas, and feedback. Every contribution helps make Ascorbic better.

## Reporting issues

Before opening a new issue, please:

- Provide clear steps to reproduce the problem
- Include relevant environmental details:
	- .NET runtime version
	- Current operating system
	- Active target framework(s)
- Include code snippets, or stack traces when possible

The more clearer the issue is, the better. Clear descriptions, and more contexts help us resolve the problem faster.

## Submitting PRs (pull requests)

When making new PRs, follow these steps to help keep things smooth:

1. Fork the repository (obviously)
2. Create a new branch for your change
3. Make your changes
4. Ensure all unit tests pass
5. Submit a pull request with a clear description

Please explain what your change does and why it’s needed.

## Code style & guidelines

When contributing to the source code, please make sure that your submission follows these guidelines:

- Keep APIs simple, predictable, and consistant
- Prefer clarity over cleverness. Clarity is always better than cleverness
- Write XML documentation for public APIs (you can write documentations for private/internal APIs)
- Follow existing naming conventions
- Avoid unnecessary breaking changes
- Keep implementations cross-platform and cross-framework when possible
- When adding a new feature, please include unit tests when possible

### Threading & asynchronous utilities

Ascorbic provides a set of threading, and asynchronous utilities. When contributing in this area:

- Avoid blocks/deadlocks on asynchronous code
- Use cancellation tokens where appropriate
- Ensure thread-safety is well documented
- Prefer deterministic behavior over timing-based logic

## Community and Conduct

The Aridity Team are striving to maintain a friendly, respectful, and inclusive environment.

- Be kind, be friendly, and be constructive
- Assume good intentions
- Try to provide helpful feedback
- Respect different experience levels

## Questions?

Got any questions? You can always open an issue, or start an discussion in the [Discussions](https://github.com/aridityteam/ascorbic/discussions) tab!
