# Security Policy

NuciXNA.Gui is a .NET GUI library for MonoGame/XNA applications with no network connectivity, external dependencies, or attack surface for remote exploitation. This policy covers vulnerability reporting for the library itself.

## 📑 Table of Contents

- [Supported Versions](#supported-versions)
- [Reporting a Vulnerability](#reporting-a-vulnerability)
- [Scope](#scope)
- [Disclosure Policy](#disclosure-policy)

## 🛡️ Supported Versions

Use this table to indicate which project versions currently receive security maintenance.

| Version | Distribution Channel | Supported |
|---------|--------------------|-----------|
| Latest version | NuGet.org | ✅ |
| Latest version | GitHub Releases | ✅ |
| Preceding versions | Any distribution channel | ❌ |

## 🚨 Reporting a Vulnerability

Please do not disclose suspected vulnerabilities publicly before maintainers have had an opportunity to validate and remediate them.

To report a vulnerability:
- [GitHub Security Advisories](https://github.com/hmlendea/NuciXNA.Gui/security/advisories)
- Contact the maintainers directly

## 📌 Scope

The subsequent report categories are in scope for this repository:
- Memory safety issues in the library code (buffer overflows, use-after-free, etc.)
- Deserialization vulnerabilities in any data parsing
- Supply chain vulnerabilities in declared dependencies

The subsequent categories are out of scope unless explicitly stated to the contrary:
- Vulnerabilities in consuming applications that use NuciXNA.Gui
- Vulnerabilities in MonoGame, XNA, or .NET runtime
- Denial-of-service via resource exhaustion in consuming applications
- Issues requiring network access (NuciXNA.Gui has no network capabilities)

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