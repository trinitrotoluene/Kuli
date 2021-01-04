# About

> Der Kuli
>
> *Short for __ball-point pen__*

This tool generates static websites from markdown and liquid templates.

# Usage

Kuli uses the following folders by default in order to import your content:

```
/
  /Assets
    - my-css-file.css
  /Content
    - index.md
    - page1.md
  /Templates
    - index.liquid
    - page.liquid
```

These folders can be freely configured in `config.yml`

```yaml
Logging:
  # Configures the logging level, turn this down to debug issues.
  Level: Verbose # Verbose, Debug, Information, Warning, Fatal

# Relative to project root
Dirs:
  Fragments: Content
  Templates: Templates
  Output: Public
  Assets: Assets
  AssetsOutput: Assets

# Variables available to templates globally
Site:
  Title: My Cool Title
```