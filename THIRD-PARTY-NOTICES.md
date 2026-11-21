# Third-party software

Tangles' MIT licence applies to its own code, documentation and puzzle packs.
It does not replace the terms of the components listed below.

## Web player

The static website includes .NET 11's browser WebAssembly runtime and NuGet
libraries resolved by the pinned projects. Full licence texts, runtime notices,
package versions and credits are in [third-party/web](third-party/web/).
The web player links to those same files through its footer.

- Bolero 0.25.65, Loic Denuziere / IntelliFactory: Apache License 2.0.
  Original licence: https://github.com/fsbolero/Bolero/blob/master/LICENSE.md
- Elmish 4.0.1: Apache License 2.0.
  Original licence: https://github.com/elmish/elmish/blob/master/LICENSE.md
- FSharp.SystemTextJson 0.19.13, Loic Denuziere: MIT.
  Original licence: https://github.com/Tarmil/FSharp.SystemTextJson/blob/v0.19.13/LICENSE
- FSharp.Core 11.0.101 and Microsoft .NET/ASP.NET libraries: MIT, with
  additional component notices in their supplied THIRD-PARTY-NOTICES files.

The resolved dependency inventory includes supporting build-time components;
it does not claim that every listed component executes in the browser.

## QL player

The QL release bundles the unmodified SMSQ/E-specific Turbo Toolkit 3.44 and
TurboPTR TPTR_EXT 5.12. Their complete original distribution archives accompany
the runtime copies, including documentation, credits and available terms.

Turbo Toolkit is copyright Charles T Dillon, Simon N Goodwin and Gerry Jackson,
with additions by David Gilham, Mark Knight and George Gwilt. Its supplied
TurboTK.txt section 18 permits giving it away with Turbo-compiled programs and
requires retaining that text and the associated files. The SMSQ/E-only runtime
is published as ttk; it is byte-identical to turbo_sms.code in the archive.

TurboPTR is by George Gwilt. TPTR_EXT is byte-identical to tptr.ext in the
original tptrp20.zip freeware distribution. The original overview, description
and remaining distribution files are included in turboptr_zip. TurboPTR's own
terms are separate from Tangles' MIT licence.

Official distribution page: https://sinclairql.net/djw/turbo/index.html

Pinned originals:

| Archive | Source | SHA-256 |
|---|---|---|
| turbo-toolkit.zip | https://sinclairql.net/djw/turbo/trbot14.zip | 4590458257ebfd2690dfc9581acdf098e62b546b361fb34bebdde993c98f3e27 |
| turboptr.zip | https://sinclairql.net/djw/turbo/tptrp20.zip | 930d4f9740d0e1d4d03abea4386f502aee49b0447f0410e06a1f6b75c6d6f51c |

The release does not bundle SMSQ/E, an emulator, Java, the SSB translator,
the Turbo compiler, a QDOS-aware unzip tool or Windows development tools.
