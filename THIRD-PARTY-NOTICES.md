# Third-Party Notices

WinClock includes or redistributes the third-party components listed below.
Each component is licensed under its own terms, reproduced here to satisfy their notice requirements.

本專案使用或隨附下列第三方元件，各元件依其授權條款提供，以下附上授權聲明。

| Component | Version | License | Distributed with WinClock | Source |
|:--|:--|:--|:--|:--|
| NAudio | 2.2.1 | MIT | Yes (bundled in exe) | https://github.com/naudio/NAudio |
| .NET Runtime / Windows Desktop (WPF, Windows Forms) | 10.0 | MIT | Yes (self-contained release build) | https://github.com/dotnet/runtime, https://github.com/dotnet/wpf, https://github.com/dotnet/winforms |
| Nerdbank.GitVersioning | 3.6.143 | MIT | No (build-time only) | https://github.com/dotnet/Nerdbank.GitVersioning |

---

## NAudio

```text
Copyright 2020 Mark Heath

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
```

## .NET Runtime, WPF, Windows Forms

The self-contained release build includes the .NET runtime and Windows Desktop libraries.
Their full third-party notices are published at https://github.com/dotnet/runtime/blob/main/THIRD-PARTY-NOTICES.TXT.

```text
The MIT License (MIT)

Copyright (c) .NET Foundation and Contributors

All rights reserved.

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
```

## Nerdbank.GitVersioning

Used only at build time to compute version numbers; not included in the distributed program.
Licensed under the MIT License, Copyright (c) .NET Foundation and Contributors.

## Icons and sounds

All icons (`src/Images/`) and alarm sounds (`src/Sounds/`) are original works created for WinClock and are covered by the project's own license.
