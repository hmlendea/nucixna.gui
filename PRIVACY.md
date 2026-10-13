# Privacy and Personal Data

This document covers the NuciXNA.Gui in-process .NET library and its verified data handling. The library requests and collects no personal data automatically, but a consuming application may supply runtime UI values that contain personal data; the host controls those values and their lifecycle.

**Information reviewed:** 2026-10-13

## 📑 Table of Contents

- [What This Document Covers](#what-this-document-covers)
- [Data We Handle](#data-we-handle)
	- [Data Provided to the Application](#data-provided-to-the-application)
	- [Data Generated or Collected by the Application](#data-generated-or-collected-by-the-application)
	- [Data Received from Integrations](#data-received-from-integrations)
- [Processing and Use](#processing-and-use)
- [Storage, Retention, and Deletion](#storage-retention-and-deletion)
- [External Processing and Integrations](#external-processing-and-integrations)
- [Data Protection and Security](#data-protection-and-security)
- [Document Changes](#document-changes)
- [Contact](#contact)

## 🔎 What This Document Covers

This document describes how NuciXNA.Gui at https://github.com/hmlendea/nucixna.gui handles personal data. It covers the library behaviour and verified integrations described below. NuciXNA.Gui provides GUI controls and screen management inside a consuming MonoGame/NuciXNA application; it has no built-in network transfer, telemetry, crash reporting, persistence, logging, or external service integration.

## 📥 Data We Handle

### Data Provided to the Application

- NuciXNA.Gui requests no personal data itself. A consuming application can pass text, selection values, content identifiers, constructor parameters, and event data at runtime; those values may contain personal data depending on the host.

### Data Generated or Collected by the Application

- NuciXNA.Gui generates or collects no personal data automatically and produces no built-in logs, telemetry, crash reports, or usage analytics.

### Data Received from Integrations

- NuciXNA.Gui has no built-in external service integration and receives no personal data from third parties.

## 🧭 Processing and Use

The application processes the data described above for these verified functions:
- UI display and interaction - host-provided text, selections, layout, colours, content identifiers, and constructor parameters.
- Input and lifecycle event delivery - NuciXNA.Input event arguments and host event handlers.

These values exist only in process memory from this library's perspective and are not persisted, transmitted, logged, or analysed by NuciXNA.Gui.

## 🗄️ Storage, Retention, and Deletion

NuciXNA.Gui stores no data in files, databases, external caches, logs, or backups. Library-owned state remains in the consuming process until controls/screens/cursors are unloaded, disposed, dereferenced, or the process ends. The consuming application controls any separate persistence, deletion, logs, crash dumps, screenshots, and backups.

## 🔗 External Processing and Integrations

NuciXNA.Gui has no built-in external data transfer. MonoGame and the declared NuciXNA packages execute in process to provide graphics, input, and primitive types; this repository does not send UI values to them as remote processors.

| Service or integration | Purpose | Data involved | Configuration or documentation |
|-----------------------|---------|---------------|--------------------------------|
| None | N/A | N/A | N/A |

## 🛡️ Data Protection and Security

NuciXNA.Gui provides no authentication, encryption, sandbox, or data-loss-prevention boundary. It does not:
- Open network ports or make HTTP requests
- Read or write files outside the consuming application's control
- Execute code from untrusted sources
- Store secrets, credentials, or tokens

The consuming application is responsible for updates, content/package provenance, process access, event handlers, crash dumps, screenshots, and any data it passes to NuciXNA.Gui controls. See [SECURITY.md](./SECURITY.md) for vulnerability reporting; no absolute security guarantee is made.

## 🔄 Document Changes

Update this document if NuciXNA.Gui adds network capabilities, telemetry, external integrations, or data persistence features. The current version is published at `PRIVACY.md` in the repository root.

## 📬 Contact

For questions about application data handling, contact the project maintainers. For a consuming application, contact its operator unless this project explicitly handles the request. Include enough information to identify the library data flow; do not send passwords, access tokens, personal data, or other secrets.