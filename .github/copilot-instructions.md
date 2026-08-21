# Git Commit Instructions

## 1. Formatting Rules
- Use the imperative, present tense for the description (e.g., "add feature", NOT "added feature" or "adds feature").
- Do not end the description line with a period or full stop.
- The entire title line must never exceed **50 characters**.
- Extended description body MUST encase referenced source code tokens or file names with backticks (e.g. `ClassName`).

### A. Strict Type Overrides (File-Based Priority)
Before evaluating the content of a file diff, you MUST look at the file extensions and paths. Apply these strict type overrides regardless of what features are being discussed inside the text:
- **Documentation Only**: If the diff only contains `.md`, `.txt`, or user manual files, the type MUST be treated as documentation changes.

## 2. Body Content Requirements
- Separate the title line from the body block using exactly one blank line.
- The body must provide a clear explanation of *why* the change was made and *what* it achieves structurally.
- Detail individual file or utility modifications using a clean, punchy bulleted list.
- Do NOT use full path descriptions when referring to changes, instead use the minimum required name for identification.

## 3. Meta File Handling
- Unity `.meta` files are critical. If a `.meta` file is part of the staging area, do not ignore it. 

## 4. Code Style Language
- For changes within C# scripts, align the description verbs with Unity-centric operations where applicable (e.g., "implement awake cycle", "optimise update loop", "expose serialized field").
