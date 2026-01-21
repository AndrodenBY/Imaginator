# Imaginator

Version: #2

Imaginator is a console app that renders images as ASCII in terminal. Runs locally.
*Prerequisites*: .NET version on 20.01.2025 and some images stored on your computer. With some small changes, it can run on older versions

### Versions
Version #1: Can render local images
Version #2: Can render local and images from the web

![Preview of app](Proof_Of_Work.png)

---
### Usage
Run the console app and insert path to your file, then it will render this image using ASCII characters. Supports almost any image file

---
### Constants from `Constants.cs`
- `AsciiSymbols` - characters to draw the picture. I have commmented one of the sets, it you want to have something else
- `HeightAspectDivisor` - fixes the image's vertical stretch so the ASCII doesn't look tall or squished
- `Channels` - channels to average red, green and blue
- `MaxByteValue` - max channel value
- `IndexOffset` - adjustment to map brightness correctly
- `ErrorMessage` - default message when somethign goes wrong

---
## Contributing
> [!Tip] To contribute you need:
> 1. Fork repository and create feature branch
> 2. Make your changes with understandable commits
> 3. Open a pull request describing your changes
> But honestly, it's just my personal project I did out of boredom.

---
## License
See `licence.md` for the terms
