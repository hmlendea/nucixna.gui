# Privacy and Personal Data

NuciXNA.Gui is a .NET GUI library for MonoGame/XNA applications. It is a self-contained library with no network connectivity, telemetry, or data collection capabilities.

**Information reviewed:** 2026-10-13

## 📑 Table of Contents

- [What This Document Covers](#what-this-document-covers)
- [Data We Handle](#data-we-handle)
- [Processing and Use](#processing-and-use)
- [Storage, Retention, and Deletion](#storage-retention-and-deletion)
- [External Processing and Integrations](#external-processing-and-integrations)
- [Data Protection and Security](#data-protection-and-security)
- [Document Changes](#document-changes)
- [Contact](#contact)

## 🔎 What This Document Covers

This document describes how NuciXNA.Gui handles personal data. NuciXNA.Gui is a .NET class library that provides GUI controls (buttons, menus, text, images, tooltips) and screen management for MonoGame/XNA games and applications. It has no network capabilities, no telemetry, no crash reporting, and no external service integrations.

## 📥 Data We Handle

### Data Provided to the Application

NuciXNA.Gui does not request, collect, or process any personal data from users, administrators, or connected systems.

### Data Generated or Collected by the Application

NuciXNA.Gui does not generate, collect, or store any personal data automatically. It does not produce logs, telemetry, crash reports, or usage analytics.

### Data Received from Integrations

NuciXNA.Gui has no built-in integrations with external services and does not receive data from third parties.

## 🧭 Processing and Use

NuciXNA.Gui processes only the data explicitly passed to it by the consuming application at runtime (e.g., text strings for display, texture references for images, layout coordinates). This data exists only in memory during application execution and is not persisted, transmitted, or analyzed by the library.

## 🗄️ Storage, Retention, and Deletion

NuciXNA.Gui does not persist any data to disk, databases, caches, or backups. All data exists only in the consuming application's memory for the duration of the application session. The consuming application controls all data lifecycle.

## 🔗 External Processing and Integrations

NuciXNA.Gui has no built-in external data transfer, network connectivity, or third-party service integrations.

| Service or integration | Purpose | Data involved | Configuration or documentation |
|-----------------------|---------|---------------|--------------------------------|
| None | N/A | N/A | N/A |

## 🛡️ Data Protection and Security

NuciXNA.Gui is a library with no attack surface for data exfiltration. It does not:
- Open network ports or make HTTP requests
- Read or write files outside the consuming application's control
- Execute code from untrusted sources
- Store secrets, credentials, or tokens

The consuming application is responsible for its own security posture, including any data it passes to NuciXNA.Gui controls.

## 🔄 Document Changes

Update this document if NuciXNA.Gui adds network capabilities, telemetry, external integrations, or data persistence features. The current version is published at `PRIVACY.md` in the repository root.

## 📬 Contact

For questions about NuciXNA.Gui data handling, contact the project maintainers via the repository issue tracker. NuciXNA.Gui does not handle personal data directly; any data concerns should be directed to the consuming application's operator.