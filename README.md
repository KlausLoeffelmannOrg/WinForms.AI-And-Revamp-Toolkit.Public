![Intro Title](docs\Images\ReadMeTitle.png)

# Welcome to the WinForms AI-and-Revamp-Toolkit!

**Bringing the simplicity of WinForms to AI development - what VBA was for Office in the 90s, this toolkit aspires to be for AI applications based on WinForms Design-simplicity today.**

## The three pillars of the WinForms AI-and-Revamp-Toolkit

### 1. The Prerequisites for Vibe Coding

Embrace the future of development with a toolkit designed for the Microsoft
"Vibe Coding" vision. The included "Chatty" application serves as both a
learning platform for effective prompting and a template for your own AI tools.

![Screenshot, showing how to activate Copilot Edits via the icon](docs/Images/WACT 0-3 Chatty.gif)

Developers need to develop a feel for prompting and understand what different
models can achieve. "Chatty" enables exactly this by:

- Allowing quick reconfiguration to use different AI models
- Supporting the creation of "Personalities" and small Agents for experimentation
- Providing an open system where you can see how everything works together
- Serving as a practical playground to build prompting skills

Experiment, learn, and build production-ready applications all in the familiar WinForms environment.

### 2. Rapid AI Application Development

Transform how you build AI-powered applications with our drag-and-drop
components. The toolkit provides a simplified architecture with the central
AIServicesComponent and AIAdapter layer, allowing you to switch between
Anthropic, OpenAI, and other providers effortlessly. No more wrestling with
changing abstraction layers or complex setups - just drag, drop, and connect to
your preferred AI model.

### 3. Rich Interactive Components

Build sophisticated AI interactions without complex coding:

- **BlazorHybrid Chat Control**: Fully-featured dialog renderer with support for structured data via Meta-Tags
- **AI-Aware Controls**: From intelligent text completion to natural language date input
- **Template-Based Prompting**: Define structured prompts even with minimal prompting experience
- **Automatic Code Extraction**: Receive parsed code blocks as events without additional processing

## Why This Toolkit?

The AI ecosystem has become increasingly complex and confusing - even for
experienced developers. This toolkit addresses that challenge by returning to
what WinForms does best: Rapid Application Development with visual tools.

- **Complete Application Infrastructure**: Full WinFormsApplication/IHost infrastructure that enables BlazorHybrid components, dependency injection, and modern service configuration
- **Beyond App.Config**: Replace traditional configuration with AppSettings.json for more flexibility
- **Modern Fluent API**: WinForms-specialized Use... Fluent-methods for easy setup
- **Project Templates**: Custom .NET project templates for an effortless start
- **Simplified Architecture**: One consistent approach for interacting with AI services
- **Real-Time Structured Data**: Receive typed data from LLMs as it streams in
- **Modern UI Components**: FluentControls for contemporary interfaces
- **Batch Processing**: Handle large workloads with cascaded templates (coming soon)
- **Function Calling**: Seamless AI function integration (in development)

## Using the NuGets out-of-the-box or Building from Source

The WinForms AI-and-Revamp-Toolkit is designed for:

- **.NET 9+** compatibility
- **Windows 22000+** systems

Our NuGet packages are available on NuGet.org with the prefix "CToolkit..." for easy integration.

Getting started is simple with our included project templates.

```
// Installation instructions will go here
```

Build the toolkit easily from the command line:

- `build` - Standard build
- `build -clean` - Clean and rebuild
- `build -pack` - Create NuGet packages

## API Reference

(coming new soon.)

## Showcase

The toolkit includes a fully-functional chat application that demonstrates the
components in action. Use it as a learning tool for effective prompting or as a
template for your own applications.

## Continuous Integration Status

(coming soon.)

GitHub Actions handle building, testing, SemVer versioning, and NuGet package
publishing.

## Community

This toolkit has been under development for 10 months with the goal of
democratizing AI application development for the WinForms community. Join us in
building the next generation of desktop AI tools!

[Community links will go here]
