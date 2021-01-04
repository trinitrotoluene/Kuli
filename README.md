# Über

> Der Kuli
>
> *Kurzwort für __Kugelschreiber__*

Dieses Tool generiert statische Webseiten von Markdown und Liquid-Templates.

# Nutzung

Kuli nutzt standartmäßig folgende Ordner, um deine Inhalte zu importieren:

```
/
  /Assets
    - meine-css-datei.css
  /Content
    - index.md
    - seite1.md
  /Templates
    - index.liquid
    - seite.liquid
```

Diese Ordner können frei in der `config.yml` Datei geändert werden.

```yaml
Logging:
  # Konfiguriert das Logging-Level, herunterstellen um mögliche Probleme zu erfassen.
  Level: Verbose # Verbose, Debug, Information, Warning, Fatal

# Relativ zu dem Projektordner
Dirs:
  Fragments: Content
  Templates: Templates
  Output: Public
  Assets: Assets
  AssetsOutput: Assets

# Variablen die in allen Templates verfügbar sind
Site:
  Title: Mein Cooler Titel
```