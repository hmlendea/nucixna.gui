# Security Policy

This policy defines supported-release coverage and private, coordinated reporting for vulnerabilities caused by NuciXNA.Gui. The library has no built-in network service, persistence, authentication, or telemetry, but it executes inside consuming MonoGame applications and depends on MonoGame and NuciXNA packages.

## 📑 Table of Contents

- [Supported Versions](#supported-versions)
- [Reporting a Vulnerability](#reporting-a-vulnerability)
- [Scope](#scope)
- [Disclosure Policy](#disclosure-policy)
- [Safe Harbour](#safe-harbour)
- [Recognition](#recognition)

## 🛡️ Supported Versions

Use this table to indicate which project versions currently receive security maintenance.

| Version | Distribution Channel | Supported |
|---------|--------------------|-----------|
| Latest version | NuGet.org | ✅ |
| Latest version | GitHub Releases | ✅ |
| Latest version | Unofficial third-party distribution channels | ❌ |
| Preceding versions | Any distribution channel | ❌ |

## 🚨 Reporting a Vulnerability

Please do not disclose suspected vulnerabilities publicly before maintainers have had an opportunity to validate and remediate them.

To report a vulnerability:
- [GitHub Security Advisories](https://github.com/hmlendea/nucixna.gui/security/advisories)
- Contact the maintainers directly

## 📌 Scope

The subsequent report categories are in scope for this repository:
- Unintended code execution, information exposure, or trust-boundary violation caused by the library's reflection, lifecycle, content, input, or event handling
- Supply-chain vulnerabilities in declared dependencies with a demonstrated NuciXNA.Gui execution path

The subsequent categories are out of scope unless explicitly stated to the contrary:
- Consuming-application logic, configuration, content, or data handling that does not originate in this library
- Vulnerabilities solely in MonoGame, NuciXNA dependencies, NuGet infrastructure, or the .NET runtime without a repository-specific cause or mitigation
- Availability issues caused solely by a host deliberately supplying unbounded control trees, text, textures, or event-handler work
- Network-service issues, because this library opens no ports and sends no requests

## 📢 Disclosure Policy

This project follows coordinated disclosure:
1. Vulnerabilities are investigated privately.
2. A remediation plan is prepared and validated.
3. Public disclosure is published after a fix, mitigation, or agreed risk decision is available.
4. Credit is attributed in accordance with reporter preference and project policy.

## 🧾 Safe Harbour

If your research is conducted in good faith, confined to authorised scope, and disclosed responsibly, the maintainers will not pursue action for policy-compliant activity.

## 🙏 Recognition

We appreciate responsible disclosure. Reporters who desire public attribution may be acknowledged in release notes, advisories, or a dedicated acknowledgements section.